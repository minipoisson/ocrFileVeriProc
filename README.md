# ocrFileVeriProc

![GitHub release (latest by date)](https://img.shields.io/github/v/release/minipoisson/ocrFileVeriProc)
![GitHub](https://img.shields.io/github/license/minipoisson/ocrFileVeriProc)
![Language](https://img.shields.io/badge/language-C%23-blue)
![Platform](https://img.shields.io/badge/platform-Windows-lightgrey)

🌐 [English](README.md) | [日本語](README.ja.md)

⬇️ [Download latest Installer](https://github.com/minipoisson/ocrFileVeriProc/releases/latest)

`ocrFileVeriProc` is a Windows application for OCR file verification and processing.
It performs OCR on image files (PNG, JPG, JPEG, BMP) 
and automates file verification, moving, or deletion based on specified keywords.
The tool utilizes Tesseract OCR and OpenCV, 
supporting multiple languages, parallel processing, and advanced image preprocessing (deskew, binarization).

## Main Features
- Text extraction from image files using OCR
- File verification, moving, or deletion based on keyword matching
- Verification mode (judgment by keyword match/mismatch)
- Move mode (move to specified folder by keyword match/mismatch)
- Delete mode (delete by keyword match/mismatch)
- Image deskew (binarization and skew correction)
- Acceleration by parallel processing
- Multi-language UI support

## How to Use
1. Specify the target image folder
2. Select the processing mode (verify, move, delete)
3. Specify the destination folder if needed
4. Enter keywords (multiple lines = OR, multiple keywords in a line separated by spaces = AND)
5. Click the [Start] button to execute
![Screenshot](capture.png)

## Requirements
- Windows 10/11
- .NET Framework 4.8
- Tesseract OCR (included)
- OpenCVSharp (included)

## Installation Note
Since this is an unsigned application from an individual developer, 
Windows SmartScreen may display a "Windows protected your PC" warning 
when you run the installer.
In such cases, you can proceed by clicking **"More info"** and then selecting **"Run anyway"**.

## UI Languages
|Code|English Name|Native Name|
|----|------------|-----------|
|ar|Arabic|العربية|
|bn|Bengali|বাংলা|
|de|German|Deutsch|
|en|English|English|
|es|Spanish|Español|
|fa|Persian|فارسی|
|fr|French|Français|
|hi|Hindi|हिन्दी|
|id|Indonesian|BahasaIndonesia|
|it|Italian|Italiano|
|ja|Japanese|日本語|
|jv|Javanese|BasaJawa|
|ko|Korean|한국어|
|mr|Marathi|मラティ語|
|ms|Malay|BahasaMelayu|
|pa|Punjabi|ਪੰਜਾਬੀ|
|pcm|Nigerian Pidgin|NigerianPidgin|
|pt|Portuguese|Português|
|ru|Russian|Русский|
|sw|Swahili|Kiswahili|
|ta|Tamil|தமிழ்|
|te|Telugu|తెలుగు|
|th|Thai|ไทย|
|tr|Turkish|Türkçe|
|uk|Ukrainian|Українська|
|ur|Urdu|اردو|
|vi|Vietnamese|TiếngViệt|
|zh_Hans|Chinese (Simplified)|简体中文|
|zh_Hant|Chinese (Traditional)|繁體中文|

## OCR Languages
(OCR Languages table omitted for brevity, keeping your original list)

## License
MIT License  
For details, please refer to the [LICENSE](LICENSE) file.

## Author
minipoisson
