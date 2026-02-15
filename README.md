🌐 [English](README.md) | [日本語](README.ja.md)

⬇️ [Download latest Installer](releases/latest)

## ocrFileVeriProc

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
In such cases, you can proceed by clicking **"More info"** 
and then selecting **"Run anyway"**.

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
|mr|Marathi|मराठी|
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
|Lang Code|Language|4.0 traineddata|
|----|----|----|
|afr|Afrikaans|afr.traineddata|
|amh|Amharic|amh.traineddaa|
|ara|Arabic|ara.traineddata|
|asm|Assamese|asm.traineddata|
|aze|Azerbaijani|aze.traineddata|
|aze_cyrl|Azerbaijani - Cyrillic|aze_cyrl.traineddata|
|bel|Belarusian|bel.traineddata|
|ben|Bengali|ben.traineddata|
|bod|Tibetan|bod.traineddata|
|bos|Bosnian|bos.traineddata|
|bul|Bulgarian|bul.traineddata|
|cat|Catalan; Valencian|cat.traineddata|
|ceb|Cebuano|ceb.traineddata|
|ces|Czech|ces.traineddata|
|chi_sim|Chinese - Simplified|chi_sim.traineddata|
|chi_tra|Chinese - Traditional|chi_tra.traineddata|
|chr|Cherokee|chr.traineddata|
|cym|Welsh|cym.traineddata|
|dan|Danish|dan.traineddata|
|deu|German|deu.traineddata|
|dzo|Dzongkha|dzo.traineddata|
|ell|Greek, Modern (1453-)|ell.traineddata|
|eng|English|eng.traineddata|
|enm|English, Middle (1100-1500)|enm.traineddata|
|epo|Esperanto|epo.traineddata|
|est|Estonian|est.traineddata|
|eus|Basque|eus.traineddata|
|fas|Persian|fas.traineddata|
|fin|Finnish|fin.traineddata|
|fra|French|fra.traineddata|
|frk|German Fraktur|frk.traineddata|
|frm|French, Middle (ca. 1400-1600)|frm.traineddata|
|gle|Irish|gle.traineddata|
|glg|Galician|glg.traineddata|
|grc|Greek, Ancient (-1453)|grc.traineddata|
|guj|Gujarati|guj.traineddata|
|hat|Haitian; Haitian Creole|hat.traineddata|
|heb|Hebrew|heb.traineddata|
|hin|Hindi|hin.traineddata|
|hrv|Croatian|hrv.traineddata|
|hun|Hungarian|hun.traineddata|
|iku|Inuktitut|iku.traineddata|
|ind|Indonesian|ind.traineddata|
|isl|Icelandic|isl.traineddata|
|ita|Italian|ita.traineddata|
|ita_old|Italian - Old|ita_old.traineddata|
|jav|Javanese|jav.traineddata|
|jpn|Japanese|jpn.traineddata|
|kan|Kannada|kan.traineddata|
|kat|Georgian|kat.traineddata|
|kat_old|Georgian - Old|kat_old.traineddata|
|kaz|Kazakh|kaz.traineddata|
|khm|Central Khmer|khm.traineddata|
|kir|Kirghiz; Kyrgyz|kir.traineddata|
|kor|Korean|kor.traineddata|
|kur|Kurdish|kur.traineddata|
|lao|Lao|lao.traineddata|
|lat|Latin|lat.traineddata|
|lav|Latvian|lav.traineddata|
|lit|Lithuanian|lit.traineddata|
|mal|Malayalam|mal.traineddata|
|mar|Marathi|mar.traineddata|
|mkd|Macedonian|mkd.traineddata|
|mlt|Maltese|mlt.traineddata|
|msa|Malay|msa.traineddata|
|mya|Burmese|mya.traineddata|
|nep|Nepali|nep.traineddata|
|nld|Dutch; Flemish|nld.traineddata|
|nor|Norwegian|nor.traineddata|
|ori|Oriya|ori.traineddata|
|pan|Panjabi; Punjabi|pan.traineddata|
|pol|Polish|pol.traineddata|
|por|Portuguese|por.traineddata|
|pus|Pushto; Pashto|pus.traineddata|
|ron|Romanian; Moldavian; Moldovan|ron.traineddata|
|rus|Russian|rus.traineddata|
|san|Sanskrit|san.traineddata|
|sin|Sinhala; Sinhalese|sin.traineddata|
|slk|Slovak|slk.traineddata|
|slv|Slovenian|slv.traineddata|
|spa|Spanish; Castilian|spa.traineddata|
|spa_old|Spanish; Castilian - Old|spa_old.traineddata|
|sqi|Albanian|sqi.traineddata|
|srp|Serbian|srp.traineddata|
|srp_latn|Serbian - Latin|srp_latn.traineddata|
|swa|Swahili|swa.traineddata|
|swe|Swedish|swe.traineddata|
|syr|Syriac|syr.traineddata|
|tam|Tamil|tam.traineddata|
|tel|Telugu|tel.traineddata|
|tgk|Tajik|tgk.traineddata|
|tgl|Tagalog|tgl.traineddata|
|tha|Thai|tha.traineddata|
|tir|Tigrinya|tir.traineddata|
|tur|Turkish|tur.traineddata|
|uig|Uighur; Uyghur|uig.traineddata|
|ukr|Ukrainian|ukr.traineddata|
|urd|Urdu|urd.traineddata|
|uzb|Uzbek|uzb.traineddata|
|uzb_cyrl|Uzbek - Cyrillic|uzb_cyrl.traineddata|
|vie|Vietnamese|vie.traineddata|
|yid|Yiddish|yid.traineddata|

## License
Proprietary License  
For details, please refer to `documents/Lisence.txt`.

## Author
minipoisson
