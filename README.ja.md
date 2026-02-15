# ocrFileVeriProc

![GitHub release (latest by date)](https://img.shields.io/github/v/release/minipoisson/ocrFileVeriProc)
![GitHub](https://img.shields.io/github/license/minipoisson/ocrFileVeriProc)
![言語](https://img.shields.io/badge/%E8%A8%80%E8%AA%9E-C%23-blue)
![プラットフォーム](https://img.shields.io/badge/%E3%83%97%E3%83%A9%E3%83%83%E3%83%88%E3%83%95%E3%82%A9%E3%83%BC%E3%83%A0-Windows-lightgrey)

⬇️ [最新版インストーラーのダウンロード](https://github.com/minipoisson/ocrFileVeriProc/releases/latest)

`ocrFileVeriProc` OCRファイル検証・処理ツールは、
画像ファイル（PNG, JPG, JPEG, BMP）に対してOCR処理を行い、
指定したキーワードに基づいてファイルの検証・移動・削除を自動化する
Windowsアプリケーションです。
Tesseract OCRとOpenCVを利用し、複数言語・並列処理・
高度な画像前処理（Deskew, 二値化）に対応しています。

## 主な機能
- OCRによる画像ファイルのテキスト抽出
- キーワード一致によるファイルの検証・移動・削除
- 検証モード（キーワード一致/不一致で判定）
- 移動モード（キーワード一致/不一致で指定フォルダへ移動）
- 削除モード（キーワード一致/不一致で削除）
- 画像のDeskew（二値化・傾き補正）処理
- 並列処理による高速化
- 多言語UI対応（日本語含む）

## 使い方
1. 対象画像フォルダを指定
2. 処理モード（検証・移動・削除）を選択
3. 必要に応じて移動先フォルダを指定
4. キーワードを入力（複数行可(OR)、行内にスペース区切りで複数キーワード指定可(AND)）
5. [開始]ボタンで処理を実行
![画面キャプチャ](capture.png)

## 必要環境
- Windows 10/11
- .NET Framework 4.8
- Tesseract OCR（同封）
- OpenCVSharp（同封）

## インストール時の注意
本ソフトは個人開発の未署名アプリであるため、
実行時に Windows SmartScreen によって
「Windows によって PC が保護されました」という警告が表示される場合があります。
その場合は、**「詳細情報」** をクリックした後に表示される
**「実行」** ボタンを押すことでインストール・起動が可能です。

## UI言語
(UI Languages table omitted for brevity, keeping your original list)

## スキャン言語
(Scan Languages table omitted for brevity, keeping your original list)

## ライセンス
MIT ライセンス  
詳細は [License.ja.txt](https://github.com/minipoisson/ocrFileVeriProc/blob/master/ocrFileVeriProc/documents/License.ja.txt) をご参照ください。

## 作者
minipoisson