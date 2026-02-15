#nullable enable
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Tesseract;

namespace ocrFileVeriProc
{
    public partial class FormMain : Form
    {
        private const string OCR_PARAMS_INI = "ocr_params.ini";
        private const string TESSDATA_DIR = "tessdata";
        private const string DOCUMENTS_DIR = "documents";
        private const string EXPLORE_EXE = "explorer.exe";
        private const string DT_FORMAT = "yyyyMMdd_HHmmss";
        private const int TIME_LEN = 18; // "DTyyyyMMdd_HHmmss\n"
        private const string KEPT_RES = "KEPT\n";
        private const string KEPT_TXT = "Kept";
        private const string VERI_RES = "VERI\n";
        private const string VERI_TXT = "Verified";
        private const string MOVE_RES = "MOVE\n";
        private const string MOVE_TXT = "Moved";
        private const string DELE_RES = "DELE\n";
        private const string DELE_TXT = "Deleted";
        private const string ERR__RES = "ERR: ";
        private const int RES_LEN = 5;
        private const string DEBUG_STR = "DEBUG";
        private enum ProcMode { VeriCont, MoveCont, DeleCont, VeriNotCont, MoveNotCont, DeleNotCont };

        private static readonly object s_engineInitLock = new object();
        private CancellationTokenSource? _cts;
        private readonly ConcurrentDictionary<string, string>
            r_fileUpdOcrStrByName = new ConcurrentDictionary<string, string>();
        public static OcrParams Cfg { get; private set; } = new OcrParams();
        private bool _isLoaded = false;
        private int _completedCount = 0;
        public FormMain()
        {
            if (Properties.Settings.Default.UILang == "")
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentCulture;
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CurrentUICulture;
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Properties.Settings.Default.UILang);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Properties.Settings.Default.UILang);
            }
            InitializeComponent();
            var cfgPath = Path.Combine(Application.StartupPath, OCR_PARAMS_INI);
            Cfg = OcrParams.LoadFromFile(cfgPath);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.WindowSize.Width > 0)
            {
                this.Size = Properties.Settings.Default.WindowSize;
                this.Location = Properties.Settings.Default.WindowLocation;
            }
            btnOr.Text = Messages.ArrowUp + "\n" + Messages.Or + "\n" + Messages.ArrowDn;
            txtBoxFolderPathImg.Text = Properties.Settings.Default.FolderPathImg;
            txtBoxFolderPathDst.Text = Properties.Settings.Default.FolderPathDst;
            btnOpenDst.Enabled = Directory.Exists(txtBoxFolderPathDst.Text);
            // 処理モードコンボボックス初期化
            cmbProcMode.SelectedIndex = Properties.Settings.Default.procMode;
            // UI言語コンボボックス初期化
            ResourceManager rmUILang = UILangs.ResourceManager;
            ResourceSet uiLangsSet = rmUILang.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            Dictionary<string, string> uiLangsDic = uiLangsSet.Cast<DictionaryEntry>()
                .ToDictionary(
                    // リソース名には - が使えず、 _ に置換してあるので戻す
                    de => (de.Key.ToString() ?? "").Replace('_', '-'),
                    de => de.Value.ToString() ?? "");
            foreach (string key in uiLangsDic.Keys.OrderBy(k => k).ToList())
            {
                cmbBoxLang.Items.Add(uiLangsDic[key]);
                if (key == Properties.Settings.Default.UILang)
                    cmbBoxLang.SelectedIndex = cmbBoxLang.Items.Count - 1;
            }
            _isLoaded = true;
            // Cfg.errorMsgが空でない場合はダイアログ表示
            if (!string.IsNullOrEmpty(Cfg.ErrorMessage))
            {
                MessageBox.Show(this, Cfg.ErrorMessage, OCR_PARAMS_INI,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } // FormMain_Load
        private void FormMain_Resize(object sender, EventArgs e)
        {
            // 最小化されたときは何もしない
            if (this.WindowState == FormWindowState.Minimized) return;
            // 最大化されたときは何もしない
            if (this.WindowState == FormWindowState.Maximized) return;
            // 画面外に出た場合は画面内に移動
            Rectangle screenRect = Screen.FromControl(this).WorkingArea;
            if (this.Right > screenRect.Right)
                this.Left = screenRect.Right - this.Width;
            if (this.Bottom > screenRect.Bottom)
                this.Top = screenRect.Bottom - this.Height;
            if (this.Left < screenRect.Left)
                this.Left = screenRect.Left;
            if (this.Top < screenRect.Top)
                this.Top = screenRect.Top;
            // UI要素の再配置
            linkLabelDoc.Left = btnAbout.Left - linkLabelDoc.Width - 6;
            btnStart.Left = btnCancel.Right + 6;
            btnStart.Width = this.ClientSize.Width - btnStart.Left - 6;
            txtBoxResults.Height = btnOpenDst.Bottom - txtBoxResults.Top;
            txtBoxFolderPathImg.Width = btnBrowseImg.Left - 6 - txtBoxFolderPathImg.Left;
            txtBoxFolderPathDst.Width = btnBrowseDst.Left - 6 - txtBoxFolderPathDst.Left;
            cmbBoxLang.Left = lblLang.Right + 6;
            cmbBoxLang.Width = btnBrowseImg.Right - cmbBoxLang.Left;
        } // FormMain_Resize
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_cts != null && !_cts.IsCancellationRequested)
            {
                _cts.Cancel(); // 中止要求
            }
            Properties.Settings.Default.WindowLocation = this.Location;
            Properties.Settings.Default.WindowSize = this.Size;
            Properties.Settings.Default.Save();
        } // FormMain_FormClosing

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            btnCancel.Enabled = false;     // 連打防止
            _cts?.Cancel();                // 中止要求
        } // btnCancel_Click

        private async void BtnStart_Click(object sender, EventArgs e)
        {
            // キーワード欄の空白チェック（全角・半角とも）
            bool hasKeyword = txtBoxKeywords.Lines.Any(line => Regex.IsMatch(line, @"[^\s　]"));
            if (!hasKeyword)
            {
                MessageBox.Show(this, Messages.KeywordsRequired);
                txtBoxResults.Text = Messages.KeywordsRequired;
                return;
            }
            // フォルダ存在チェック
            string imgFolderPath = txtBoxFolderPathImg.Text;
            if (!Directory.Exists(imgFolderPath))
            {
                MessageBox.Show(this, Messages.FolderNotExistsImg);
                txtBoxResults.Text = Messages.FolderNotExistsImg;
                return;
            }
            // 処理モードと行先フォルダ
            ProcMode procMode = (ProcMode)cmbProcMode.SelectedIndex;
            string folderPathDst = txtBoxFolderPathDst.Text;
            if (procMode == ProcMode.MoveCont || procMode == ProcMode.MoveNotCont)
            {
                if (!Directory.Exists(folderPathDst))
                {
                    MessageBox.Show(this, Messages.FolderNotExistsDst);
                    txtBoxResults.Text = Messages.FolderNotExistsDst;
                    return;
                }
                if (txtBoxFolderPathImg.Text == txtBoxFolderPathDst.Text)
                {
                    MessageBox.Show(this, Messages.BothFoldersAreSame);
                    txtBoxResults.Text = Messages.BothFoldersAreSame;
                    return;
                }
            }
            // ☑debug なら DEBUGフォルダ作成
            string? debugDirPath = chkBDebug.Checked 
                ? Path.Combine(imgFolderPath, DEBUG_STR) : null;
            if (null != debugDirPath && !Directory.Exists(debugDirPath))
                try
                {
                    Directory.CreateDirectory(debugDirPath);
                }
                catch(Exception ex)
                {
                    string strMsg = string.Format(Messages.UnexpectedError, 
                        ex.Message + "\n" + ex.StackTrace);
                    MessageBox.Show(this, strMsg);
                    txtBoxResults.Text = strMsg;
                    return;
                }
            // tessdataフォルダ存在チェック
            string tessDataPath = Path.Combine(Application.StartupPath, TESSDATA_DIR);
            if (!Directory.Exists(tessDataPath))
            {
                MessageBox.Show(this, "tessdata folder is not found.");
                return;
            }

            btnStart.Enabled = false;           // 二重起動防止
            btnCancel.Enabled = true;
            txtBoxResults.Text = btnStart.Text;
            Cursor = Cursors.WaitCursor;

            string[] imageFiles = Directory.GetFiles(imgFolderPath, "*.*")
                                           .Where(f => Regex.IsMatch(f,
                                           @"\.(png|jpg|jpeg|bmp)$", 
                                           RegexOptions.IgnoreCase))
                                           .ToArray();
            // imageFilesに含まれていないkeyを削除
            foreach (var key in r_fileUpdOcrStrByName.Keys.ToArray())
            {
                if (!imageFiles.Contains(key))
                {
                    r_fileUpdOcrStrByName.TryRemove(key, out _);
                }
            }
            // 画像ファイルが1つもなければ終了
            int totalCount = imageFiles.Length;
            if (totalCount == 0)
            {
                MessageBox.Show(this, Messages.NoImageFiles);
                txtBoxResults.Text = Messages.NoImageFiles;
                btnStart.Enabled = true;
                btnCancel.Enabled = false;
                Cursor = Cursors.Default;
                return;
            }
            // 進捗管理初期化
            _completedCount = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = totalCount;
            progressBar1.Value = 0;
            txtBoxResults.Text = string.Format(Messages.Processing,
                progressBar1.Value, progressBar1.Maximum);
            DateTime startTime = DateTime.Now;

            string[] keywordLines = txtBoxKeywords.Lines
                .Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
            string[][] keywordsLines = txtBoxKeywords.Lines
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line => {
                    return line.Split(new char[] { ' ', '　' },
                        StringSplitOptions.RemoveEmptyEntries);
                })
                .ToArray();
            var startProcFileTimeDic = new ConcurrentDictionary<string, DateTime>();
            var endProcFileTimeDic = new ConcurrentDictionary<string, DateTime>();
            var procFileResultStrDic = new ConcurrentDictionary<string, string>();

            _cts = new CancellationTokenSource();    // ★ トークン源を作成
            var token = _cts.Token;
            bool wasCancelledThisRun = false;

            timerProgress.Start();
            Environment.SetEnvironmentVariable("OMP_THREAD_LIMIT", Cfg.OpenMpThreads.ToString());
            Environment.SetEnvironmentVariable("OMP_NUM_THREADS", Cfg.OpenMpThreads.ToString());
            // 並列処理
            try
            {
                await Task.Run(() =>
                {
                    var options = new ParallelOptions
                    {
                        // 外側並列は控えめに（2〜4推奨）
                        MaxDegreeOfParallelism = Math.Max(1, Math.Min(
                            Cfg.MaxDegreeOfParallelism, Environment.ProcessorCount - 1)),
                        CancellationToken = token
                    };

                    var errors = new ConcurrentBag<string>();

                    // ★ 各ワーカースレッド専用の Engine を、初回アクセス時に作る
                    var threadLocalEngine = new ThreadLocal<TesseractEngine>(() =>
                    {
                        // ★ 初期化だけは直列化
                        lock (s_engineInitLock)
                        {
                            var engine = new TesseractEngine(tessDataPath, Cfg.Languages, EngineMode.Default);
                            engine.SetVariable("user_defined_dpi", Cfg.UserDefinedDpi.ToString());
                            if (Enum.TryParse<PageSegMode>(Cfg.PageSegMode, true, out var psm))
                                engine.DefaultPageSegMode = psm;
                            else
                                engine.DefaultPageSegMode = PageSegMode.Auto;
                            return engine;
                        }
                    }, /*trackAllValues:*/ true);
                    try // Parallel.ForEachの本体
                    {
                        Parallel.ForEach(imageFiles, options, filePath =>
                        {
                            token.ThrowIfCancellationRequested();
                            try // 個別ファイル処理の本体
                            {
                                startProcFileTimeDic[filePath] = DateTime.Now;

                                // ★ 各スレッド専用の Engine を取得
                                var engine = threadLocalEngine.Value 
                                    ?? throw new Exception("Tesseract Engine initialization failed.");
                                string resTimeOcrStr = ProcessImageFile(filePath, debugDirPath,
                                    procMode, folderPathDst, engine, keywordsLines, token);
                                string resStr = (RES_LEN <= resTimeOcrStr.Length)
                                    ? resTimeOcrStr.Substring(0, RES_LEN) : "";
                                string resTxt = resStr switch
                                {
                                    KEPT_RES => KEPT_TXT,
                                    VERI_RES => VERI_TXT,
                                    MOVE_RES => MOVE_TXT,
                                    DELE_RES => DELE_TXT,
                                    _ => resTimeOcrStr
                                };
                                procFileResultStrDic[filePath] = resTxt;
                                endProcFileTimeDic[filePath] = DateTime.Now;
                                // 更新日時＋OCR文字列をキャッシュ
                                if ((resStr == KEPT_RES || resStr == VERI_RES)
                                    && ((RES_LEN + TIME_LEN) <= resTimeOcrStr.Length))
                                {
                                    string timeOcrStr = resTimeOcrStr.Substring(RES_LEN);
                                    r_fileUpdOcrStrByName.AddOrUpdate(filePath, timeOcrStr,
                                        (key, oldValue) => timeOcrStr);
                                }
                            } 
                            catch (Exception ex)
                            {
                                var msg = ex.InnerException?.Message ?? ex.Message;
                                procFileResultStrDic[filePath] = ERR__RES + msg;
                                endProcFileTimeDic[filePath] = DateTime.Now;
                                errors.Add($"{Path.GetFileName(filePath)}\t{ex}\n{ex.InnerException}");
                            }
                            finally
                            {
                                Interlocked.Increment(ref _completedCount);
                            } // 個別ファイル処理の本体終わり
                        });
                    } // try (Parallel.ForEachの本体)
                    finally
                    {
                        // ★ 各スレッドの Engine を確実に破棄
                        foreach (var engine in threadLocalEngine.Values)
                            engine?.Dispose();
                        threadLocalEngine.Dispose();

                        if (!errors.IsEmpty)
                            File.WriteAllLines(Path.Combine(imgFolderPath, "Errors.txt"), errors);
                    } // Parallel.ForEachの本体終わり
                }); // await Task.Run終わり
            } // try（並列処理）
            catch (OperationCanceledException) // 中止時の後片付け
            {
                wasCancelledThisRun = true;
            }
            catch (Exception ex) // その他の例外
            {
                MessageBox.Show(this, string.Format(Messages.UnexpectedError, 
                    ex.Message + "\n" + ex.StackTrace));
            }
            finally
            {
                try
                {
                    // ログ書き出し
                    string logPath = Path.Combine(imgFolderPath, "ImageProcessLog.txt");
                    using (var writer = new StreamWriter(logPath, false, System.Text.Encoding.UTF8))
                    {
                        writer.WriteLine("File\tStart\tEnd\tDuration(ms)\tResult");
                        foreach (var file in imageFiles)
                        {
                            if (startProcFileTimeDic.TryGetValue(file, out var start) &&
                                endProcFileTimeDic.TryGetValue(file, out var end) &&
                                procFileResultStrDic.TryGetValue(file, out var result))
                            {
                                var duration = (end - start).TotalMilliseconds;
                                writer.WriteLine($"{Path.GetFileName(file)}\t"
                                    + $"{start:yyyy-MM-dd HH:mm:ss.fff}\t"
                                    + $"{end:yyyy-MM-dd HH:mm:ss.fff}\t"
                                    + $"{duration:F0}\t{result}");
                            }
                        }
                        string status = wasCancelledThisRun ? "Cancelled" : "Completed";
                        writer.WriteLine($"# Status: {status}");
                        writer.WriteLine($"# CancelRequested: {_cts?.IsCancellationRequested ?? false}, "
                            + $"Completed: {_completedCount}/{totalCount}");
                    }
                    _cts?.Dispose();
                    _cts = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, string.Format(Messages.UnexpectedError, 
                        ex.Message + "\n" + ex.StackTrace));
                }

                timerProgress.Stop();
                progressBar1.Value = Math.Min(_completedCount, progressBar1.Maximum);
                Cursor = Cursors.Default;
                btnStart.Enabled = true;
                btnCancel.Enabled = false;
            } // 並列処理終わり

            // 結果集計
            int deletedCount = procFileResultStrDic.Values.Count(v => v == DELE_TXT);
            int movedCount = procFileResultStrDic.Values.Count(v => v == MOVE_TXT);
            int verifiedCount = procFileResultStrDic.Values.Count(v => v == VERI_TXT);
            int keptCount = procFileResultStrDic.Values.Count(v => v == KEPT_TXT);
            int errorCount = procFileResultStrDic.Values.Count(v => v.StartsWith(ERR__RES));
            int elapsedSec = (int)(DateTime.Now - startTime).TotalSeconds;

            // メッセージ作成
            string messageStr = string.Format(Messages.Result0Total, totalCount) + "\r\n";
            switch (procMode)
            {
                case ProcMode.VeriCont:
                case ProcMode.VeriNotCont:
                    messageStr += string.Format(Messages.Result1Verified, verifiedCount) + "\r\n";
                    break;
                case ProcMode.MoveCont:
                case ProcMode.MoveNotCont:
                    messageStr += string.Format(Messages.Result1Moved, movedCount) + "\r\n";
                    break;
                case ProcMode.DeleCont:
                case ProcMode.DeleNotCont:
                    messageStr += string.Format(Messages.Result1Deleted, deletedCount) + "\r\n";
                    break;
            }
            messageStr += string.Format(Messages.Result2Kept, keptCount) + "\r\n"
                + string.Format(Messages.Result3Error, errorCount) + "\r\n"
                + string.Format(Messages.Result4Time, elapsedSec) + "\r\n"
                + ((_completedCount == totalCount) ? Messages.Finished : Messages.Cancelled);
            MessageBox.Show(this, messageStr);
            try
            {
                txtBoxResults.Multiline = true;
                txtBoxResults.Text = messageStr;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, string.Format(Messages.UnexpectedError, 
                    ex.Message + "\n" + ex.StackTrace));
            }
        } // btnStart_Click終わり

        private void BtnOpenImg_Click(object sender, EventArgs e)
        {
            string folderPath = txtBoxFolderPathImg.Text;
            if (Directory.Exists(folderPath))
                System.Diagnostics.Process.Start(EXPLORE_EXE, folderPath);
            else
            {
                MessageBox.Show(this, Messages.FolderNotExistsImg);
                BtnBrowseImg_Click(sender, e);
            }
        } // BtnOpenImg_Click
        private void BtnOpenDst_Click(object sender, EventArgs e)
        {
            string folderPath = txtBoxFolderPathDst.Text;
            if (Directory.Exists(folderPath))
                System.Diagnostics.Process.Start(EXPLORE_EXE, folderPath);
            else
            {
                MessageBox.Show(this, Messages.FolderNotExistsDst);
                BtnBrowseDst_Click(sender, e);
            }
        } // BtnOpenDst_Click
        private void CmbProcMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isLoaded) return;
            Properties.Settings.Default.procMode = cmbProcMode.SelectedIndex;
            Properties.Settings.Default.Save();
        } // CmbProcMode_SelectedIndexChanged
        private void BtnBrowseImg_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                if (fbd.SelectedPath == txtBoxFolderPathDst.Text)
                {
                    MessageBox.Show(this, Messages.BothFoldersAreSame);
                    return;
                }
                txtBoxFolderPathImg.Text = fbd.SelectedPath;
                Properties.Settings.Default.FolderPathImg = txtBoxFolderPathImg.Text;
            }
        } // BtnBrowseImg_Click
        private void BtnBrowseDst_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                if (fbd.SelectedPath == txtBoxFolderPathImg.Text)
                {
                    MessageBox.Show(this, Messages.BothFoldersAreSame);
                    return;
                }
                txtBoxFolderPathDst.Text = fbd.SelectedPath;
                btnOpenDst.Enabled = Directory.Exists(txtBoxFolderPathDst.Text);
                Properties.Settings.Default.FolderPathDst = txtBoxFolderPathDst.Text;
            }
        } // BtnBrowseDst_Click
        private void CmbLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isLoaded) return;
            if (cmbBoxLang.SelectedItem == null) return;
            string selectedLangName = cmbBoxLang.SelectedItem.ToString() ?? "";

            // 選択された言語名から言語コードを逆引き
            ResourceManager rmUILang = UILangs.ResourceManager;
            ResourceSet uiLangsSet = rmUILang.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            Dictionary<string, string> uiLangsRevDic = uiLangsSet.Cast<DictionaryEntry>()
                .ToDictionary(
                    de => de.Value.ToString() ?? "",
                    // リソース名に - が使えなくて _ に置換してあるのを戻す
                    de => (de.Key.ToString() ?? "").Replace('_', '-'));
            string selectedLangCode = uiLangsRevDic.ContainsKey(selectedLangName)
                ? uiLangsRevDic[selectedLangName] : "";
            // 言語コードが現在の設定と同じなら何もしない
            if (selectedLangCode == Properties.Settings.Default.UILang) return;
            // 言語コードを保存して再起動
            Properties.Settings.Default.UILang = selectedLangCode;
            Properties.Settings.Default.Save();
            Application.Restart();
        } // CmbLang_SelectedIndexChanged

        private void BtnAbout_Click(object sender, EventArgs e)
        {
            using var formAbout = new AboutBox1();
            formAbout.StartPosition = FormStartPosition.CenterParent;
            formAbout.ShowDialog(this);
        } // BtnAbout_Click

        private void LinkLabelDoc_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(EXPLORE_EXE,
                Path.Combine(Application.StartupPath, DOCUMENTS_DIR));
        } // LinkLabelDoc_LinkClicked
        private void TimerProgress_Tick(object sender, EventArgs e)
        {
            progressBar1.Value = _completedCount;
            txtBoxResults.Text = string.Format(Messages.Processing, progressBar1.Value, progressBar1.Maximum);
            progressBar1.Refresh();
            txtBoxResults.Refresh();
        } // TimerProgress_Tick

        private string ProcessImageFile(string filePath, string? debugDirPath,
            ProcMode procMode, string dstFolderPath,
            TesseractEngine engine, string[][] keywordsLines, CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                // 移動やDEBUGの際に使うファイル名はオリジナルと同じもの
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                // ファイル更新日時取得
                DateTime upd = File.GetLastWriteTime(filePath);
                string timeStr = "DT" + upd.ToString(DT_FORMAT) + "\n";
                string ocrText = "";
                // 以前のOCR結果がキャッシュにあれば取得
                if (r_fileUpdOcrStrByName.ContainsKey(filePath))
                {
                    string updOcrStr = r_fileUpdOcrStrByName[filePath];
                    if (updOcrStr != null)
                        if (timeStr.Length < updOcrStr.Length)
                            if (updOcrStr.Substring(0, TIME_LEN) == timeStr)
                                ocrText = updOcrStr.Substring(timeStr.Length);
                }
                // キャッシュに無かったら画像処理とOCRを実行
                if (ocrText == "")
                {
                    // 画像読み込み＋Deskew＋二値化
                    Bitmap processed;
                    using (var orgBmp = new Bitmap(filePath))
                    {
                        if (orgBmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb
                            || orgBmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
                        {   // そのまま処理
                            // 以下の処理を変更するときは★↓else 節↓★も同様に変更すること
                            using Mat srcBgr = BitmapToMat(orgBmp);
                            using Mat binaryBgr = DeskewThenAdaptiveBinarize(srcBgr, fileName, debugDirPath);
                            processed = MatToBitmap(binaryBgr);
                        }
                        else
                        {   // 24bppRgbに変換してから処理：Format24bppRgb か Format8bppIndexed 以外は非対応だから
                            using var convertedBmp = new Bitmap(orgBmp.Width, orgBmp.Height,
                                System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                            using var g = Graphics.FromImage(convertedBmp);
                            g.DrawImage(orgBmp, new Rectangle(0, 0, convertedBmp.Width, convertedBmp.Height));
                            // 以下の処理を変更するときは★↑if 節↑★も同様に変更すること
                            using Mat srcBgr = BitmapToMat(convertedBmp);
                            using Mat binaryBgr = DeskewThenAdaptiveBinarize(srcBgr, fileName, debugDirPath);
                            processed = MatToBitmap(binaryBgr);
                        }
                    }

                    // OCR実行
                    token.ThrowIfCancellationRequested();
                    using (processed)
                    {
                        using var ms = new MemoryStream();
                        processed.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        ms.Position = 0;
                        using var img = Pix.LoadFromMemory(ms.ToArray());
                        using var page = engine.Process(img);
                        ocrText = page.GetText();
                    }
                    // DEBUG用OCRテキスト保存
                    if (debugDirPath != null)
                    {
                        string ocrFile = Path.Combine(debugDirPath,
                            Path.GetFileNameWithoutExtension(filePath) + "_ocr.txt");
                        File.WriteAllText(ocrFile, ocrText);
                    }
                } else {
                    // キャッシュから取得したOCRテキストのDEBUG保存
                    if (debugDirPath != null)
                    {
                        string ocrFile = Path.Combine(debugDirPath,
                        Path.GetFileNameWithoutExtension(filePath) + "_ocr_cached.txt");
                        File.WriteAllText(ocrFile, ocrText);
                    }
                }
                // キーワードマッチ&削除
                token.ThrowIfCancellationRequested();
                bool isAnyLineMatched = keywordsLines.Any(
                    words => words.All(word => ocrText.Contains(word)));
                // 処理モード別の後処理
                if ((isAnyLineMatched && procMode == ProcMode.VeriCont)
                    || (!isAnyLineMatched && procMode == ProcMode.VeriNotCont))
                {
                    return VERI_RES + timeStr + ocrText;
                }
                if ((isAnyLineMatched && procMode == ProcMode.MoveCont)
                    || (!isAnyLineMatched && procMode == ProcMode.MoveNotCont))
                {
                    string filePathDst = Path.Combine(dstFolderPath, Path.GetFileName(filePath));
                    try
                    {
                        if (!Directory.Exists(dstFolderPath))
                            Directory.CreateDirectory(dstFolderPath);
                        if (File.Exists(filePathDst))
                            File.Delete(filePathDst);
                        File.Move(filePath, filePathDst);
                    }
                    catch (IOException) // まれな競合の際には再試行
                    {
                        Thread.Sleep(50);
                        if (File.Exists(filePathDst))
                            File.Delete(filePathDst);
                        File.Move(filePath, filePathDst);
                    }
                    return MOVE_RES;
                }
                if ((isAnyLineMatched && procMode == ProcMode.DeleCont)
                    || (!isAnyLineMatched && procMode == ProcMode.DeleNotCont))
                {
                    try
                    {
                        File.Delete(filePath);
                    }
                    catch (IOException) // まれな競合に再試行
                    {
                        Thread.Sleep(50);
                        File.Delete(filePath);
                    }
                    return DELE_RES;
                }
                return KEPT_RES + timeStr + ocrText;
            }
            catch (OperationCanceledException) { throw; }
            catch (Exception ex)
            {
                return ERR__RES + (ex.Message + "|" + ex.StackTrace).Replace("\n", "\\n");
            }
        } // ProcessImageFile終わり

        public static Mat DeskewThenAdaptiveBinarize(Mat srcBgr, string fileName, string? debugDirPath)
        {
            // 1) グレースケール
            using var grayBgr = new Mat();
            if (srcBgr.Channels() == 1)
                srcBgr.CopyTo(grayBgr); // 既にグレースケール
            else
                Cv2.CvtColor(srcBgr, grayBgr, ColorConversionCodes.BGR2GRAY);

            if (debugDirPath != null)
                Cv2.ImWrite(Path.Combine(debugDirPath, fileName + "_01_gray.png"), grayBgr);

            // 2) 傾き角推定（縮小・軽量処理）
            double angle = EstimateSkewAngle(grayBgr);

            // しきい値：小さすぎる傾きは無視
            if (Math.Abs(angle) >= Cfg.SkewSkipThresholdDeg)
            {
                using var rotated = RotateWithPadding(grayBgr, -angle); // deskewなので反対向きに回転
                if (debugDirPath != null)
                    Cv2.ImWrite(Path.Combine(debugDirPath,
                        fileName + "_02_" + angle.ToString("0.00") + "_rotated_gray.png"), rotated);

                // 3) 適応的二値化（Gaussian）
                var binaryBgr = new Mat();
                Cv2.AdaptiveThreshold(rotated, binaryBgr, 255,
                    AdaptiveThresholdTypes.GaussianC, ThresholdTypes.Binary,
                    Cfg.AdaptiveBlockSize, Cfg.AdaptiveC);

                if (debugDirPath != null)
                    Cv2.ImWrite(Path.Combine(debugDirPath, fileName + "_03_binary.png"), binaryBgr);
                return binaryBgr;
            }
            else
            {
                // 回転スキップしてそのまま二値化
                var binaryBgr = new Mat();
                Cv2.AdaptiveThreshold(grayBgr, binaryBgr, 255,
                    AdaptiveThresholdTypes.GaussianC, ThresholdTypes.Binary,
                    Cfg.AdaptiveBlockSize, Cfg.AdaptiveC);
                if (debugDirPath != null)
                    Cv2.ImWrite(Path.Combine(debugDirPath, fileName + "_03_binary.png"), binaryBgr);
                return binaryBgr;
            }
        } // DeskewThenAdaptiveBinarize終わり

        private static double EstimateSkewAngle(Mat gray)
        {
            // ダウンサンプル（高速化）。幅1200基準
            double scale = gray.Width > 1200 ? 1200.0 / gray.Width : 1.0;
            using var smallBgr = new Mat();
            if (scale < 1.0) Cv2.Resize(gray, smallBgr,
                new OpenCvSharp.Size(), scale, scale, InterpolationFlags.Area);
            else gray.CopyTo(smallBgr);

            // 1) エッジ＋HoughLinesP
            using var edges = new Mat();
            Cv2.GaussianBlur(smallBgr, edges, new OpenCvSharp.Size(3, 3), 0);
            Cv2.Canny(edges, edges, 50, 150);

            var lines = Cv2.HoughLinesP(
                edges,
                1, Math.PI / 180.0,
                threshold: 120,
                minLineLength: (int)(smallBgr.Width * 0.5),
                maxLineGap: 20);

            var angles = lines
                .Select(l =>
                {
                    double dx = l.P2.X - l.P1.X;
                    double dy = l.P2.Y - l.P1.Y;
                    double deg = Math.Atan2(dy, dx) * 180.0 / Math.PI; // [-180,180]
                                                                       // 水平に近い線だけ（±20°以内）
                    double win = Cfg.HoughNearHorizontalDeg;
                    return (Math.Abs(deg) <= win || Math.Abs(Math.Abs(deg) - 180.0) <= win)
                    ? NormalizeAngle(deg) : double.NaN;
                })
                .Where(d => !double.IsNaN(d))
                .ToArray();

            if (angles.Length >= 5)
            {
                Array.Sort(angles);
                double median = angles[angles.Length / 2];

                // ばらつき（中央値絶対偏差: MAD）
                double mad = angles.Select(x => Math.Abs(x - median))
                    .OrderBy(x => x).ElementAt(angles.Length / 2);
                if (mad > Cfg.MadThresholdDeg)
                    return SweepProjectionForSkew(smallBgr, Cfg.ProjectionSweepStartDeg,
                        Cfg.ProjectionSweepEndDeg, Cfg.ProjectionSweepStepDeg);

                return -median; // この角度で時計回りに傾ける必要がある
            }

            // 2) 投影プロファイル フォールバック（-5°〜+5°）
            return SweepProjectionForSkew(smallBgr, Cfg.ProjectionSweepStartDeg,
                Cfg.ProjectionSweepEndDeg, Cfg.ProjectionSweepStepDeg);
        } // EstimateSkewAngle終わり

        private static double NormalizeAngle(double deg)
        {
            // 角度を [-90, 90] に畳み込み（水平基準）
            if (deg > 90) deg -= 180;
            if (deg < -90) deg += 180;
            return deg;
        } // NormalizeAngle終わり

        private static double SweepProjectionForSkew(Mat smallGray,
            double startDeg, double endDeg, double stepDeg)
        {
            double bestAngle = 0.0;
            double bestScore = double.NegativeInfinity;

            for (double a = startDeg; a <= endDeg; a += stepDeg)
            {
                using var rotated = RotateWithPadding(smallGray, -a); // 仮回転
                // Otsuで簡易二値化（高速用）
                using var thr = new Mat();
                Cv2.Threshold(rotated, thr, 0, 255, ThresholdTypes.Otsu | ThresholdTypes.Binary);
                Cv2.BitwiseNot(thr, thr); // ← 黒画素を非ゼロに

                // 横方向の投影（各行のインク量の分散が大きいほど行がくっきり＝良い角度）
                var proj = new double[thr.Rows];
                for (int y = 0; y < thr.Rows; y++)
                {
                    var row = thr.Row(y);
                    proj[y] = Cv2.CountNonZero(row);
                }
                double mean = proj.Average();
                double var = proj.Select(v => (v - mean) * (v - mean)).Average();

                if (var > bestScore)
                {
                    bestScore = var;
                    bestAngle = a;
                }
            }
            return bestAngle;
        } // SweepProjectionForSkew終わり
        private static Mat RotateWithPadding(Mat srcGray, double angleDeg)
        {
            // 新しいキャンバスサイズを計算
            int w = srcGray.Width;
            int h = srcGray.Height;
            double rad = angleDeg * Math.PI / 180.0;
            double cos = Math.Abs(Math.Cos(rad));
            double sin = Math.Abs(Math.Sin(rad));
            int newW = (int)Math.Round(h * sin + w * cos);
            int newH = (int)Math.Round(h * cos + w * sin);

            // 回転行列＋平行移動（中心合わせ）
            var center = new OpenCvSharp.Point2f(w / 2f, h / 2f);
            var rot = Cv2.GetRotationMatrix2D(center, angleDeg, 1.0);
            // 余白分のシフト
            rot.Set(0, 2, rot.Get<double>(0, 2) + (newW - w) / 2.0);
            rot.Set(1, 2, rot.Get<double>(1, 2) + (newH - h) / 2.0);

            var dstBgr = new Mat(newH, newW, MatType.CV_8UC1);
            Cv2.WarpAffine(
                srcGray, dstBgr, rot, new OpenCvSharp.Size(newW, newH),
                InterpolationFlags.Cubic,
                BorderTypes.Constant,
                new Scalar(255)); // 白で埋める（紙の余白っぽく）

            return dstBgr;
        } // RotateWithPadding終わり

        public static Mat BitmapToMat(Bitmap bmp)
        {
            return OpenCvSharp.Extensions.BitmapConverter.ToMat(bmp);
        }

        public static Bitmap MatToBitmap(Mat mat)
        {
            return OpenCvSharp.Extensions.BitmapConverter.ToBitmap(mat);
        }

    }
}
