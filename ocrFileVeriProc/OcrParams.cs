using System;
using System.IO;
using System.Globalization;

public sealed class OcrParams
{
    // ===== 既定値（今の実装に合わせています） =====
    public string Languages { get; set; } = "jpn+eng";
    public int UserDefinedDpi { get; set; } = 300;
    public string PageSegMode { get; set; } = "Auto"; // Tesseract.PageSegMode

    public int MaxDegreeOfParallelism { get; set; } = 4; // 外側並列の上限
    public int OpenMpThreads { get; set; } = 1;          // Tesseract内部OpenMP

    // Deskew関連
    public double SkewSkipThresholdDeg { get; set; } = 0.3;
    public double MadThresholdDeg { get; set; } = 2.0;
    public double HoughNearHorizontalDeg { get; set; } = 20.0;
    public double ProjectionSweepStartDeg { get; set; } = -5.0;
    public double ProjectionSweepEndDeg { get; set; } = 5.0;
    public double ProjectionSweepStepDeg { get; set; } = 0.2;

    // 二値化
    public int AdaptiveBlockSize { get; set; } = 31; // 奇数
    public double AdaptiveC { get; set; } = 10.0;

    // エラー
    public string ErrorMessage { get; set; } = "";

    // ===== INI読み込み =====
    public static OcrParams LoadFromFile(string path)
    {
        var p = new OcrParams();
        if (!File.Exists(path)) return p;

        try
        {
            foreach (var raw in File.ReadAllLines(path))
            {
                var line = raw.Trim();
                if (line.Length == 0 || line.StartsWith("#") || line.StartsWith(";")) continue;
                // 行末コメント除去
                int cidx = line.IndexOfAny(new[] { '#', ';' });
                if (cidx >= 0) line = line.Substring(0, cidx).Trim();
                int eq = line.IndexOf('=');
                if (eq <= 0) continue;

                string key = line.Substring(0, eq).Trim().ToLowerInvariant();
                string val = line.Substring(eq + 1).Trim();

                // 汎用パーサ
                bool TryInt(out int v) =>
                    int.TryParse(val, NumberStyles.Integer, CultureInfo.InvariantCulture, out v)
                    || int.TryParse(val, NumberStyles.Integer, CultureInfo.CurrentCulture, out v);
                bool TryDouble(out double v)
                {
                    var s = val.Replace(',', '.');
                    return double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out v)
                           || double.TryParse(val, NumberStyles.Float, CultureInfo.CurrentCulture, out v);
                }

                switch (key)
                {
                    case "languages": p.Languages = val; break;
                    case "user_defined_dpi": if (TryInt(out var dpi)) p.UserDefinedDpi = dpi; break;
                    case "psm": p.PageSegMode = val; break;

                    case "max_degree_of_parallelism": if (TryInt(out var mdop)) p.MaxDegreeOfParallelism = mdop; break;
                    case "openmp_threads": if (TryInt(out var omp)) p.OpenMpThreads = omp; break;

                    case "skew_skip_threshold_deg": if (TryDouble(out var s1)) p.SkewSkipThresholdDeg = s1; break;
                    case "mad_threshold_deg": if (TryDouble(out var s2)) p.MadThresholdDeg = s2; break;
                    case "hough_near_horizontal_deg": if (TryDouble(out var s3)) p.HoughNearHorizontalDeg = s3; break;
                    case "projection_sweep_start_deg": if (TryDouble(out var s4)) p.ProjectionSweepStartDeg = s4; break;
                    case "projection_sweep_end_deg": if (TryDouble(out var s5)) p.ProjectionSweepEndDeg = s5; break;
                    case "projection_sweep_step_deg": if (TryDouble(out var s6)) p.ProjectionSweepStepDeg = s6; break;

                    case "adaptive_block_size": if (TryInt(out var bs)) p.AdaptiveBlockSize = (bs % 2 == 0) ? bs + 1 : bs; break;
                    case "adaptive_c": if (TryDouble(out var c)) p.AdaptiveC = c; break;
                }
            }
        }
        catch (Exception ex)
        {
            // 失敗時は既定値で返し、必要ならログやメッセージ表示を追加
            p.ErrorMessage += "OcrParams LoadFromFile error: " + ex.Message + "\n" + ex.StackTrace;
            System.Diagnostics.Debug.WriteLine($"OcrParams LoadFromFile error: {ex.Message}");
        }
        return p;
    }
}
