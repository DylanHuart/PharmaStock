using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Widget;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace PharmaTab
{
    [Activity(Label = "Historique")]
    
    public class Historique : Activity
    {
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            LinearLayout view = new LinearLayout(this)
            {
                Orientation = Orientation.Vertical
            };



            string directory = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "Pharmastock";
            string[] fichiers = Directory.GetFiles(directory);
            List<string> listefichiers = new List<string>();
            listefichiers = fichiers.OfType<string>().ToList();




            ListView liste = new ListView(this)
            {
              
            };
            view.AddView(liste);

            

            liste.ItemClick += (s, e) =>
            {

                Java.IO.File file = new Java.IO.File(e.ToString());
                Intent intent = new Intent();
                intent.AddFlags(ActivityFlags.NewTask);
                intent.SetAction(Intent.ActionView);
                string type = getMIMEType(file);
                intent.SetDataAndType(Uri.FromFile(file), type);
                StartActivity(intent);
            };

            this.SetContentView(view);

        }

        private string getMIMEType(Java.IO.File file)
        {
            string type = "*/*";
            string fName = file.Name;
            int dotIndex = fName.LastIndexOf(".");
            if (dotIndex < 0)
            {
                return type;
            }
            // get the file extension
            string end = fName.Substring(dotIndex, fName.Length).ToLower();
            if (end == "") return type;
            //from MIME_MapTable to get the respond type  
            for (int i = 0; i < MIME_MapTable.Length; i++)
            {
                if (end.Equals(MIME_MapTable[i, 0]))
                    type = MIME_MapTable[i, 1];
            }
            return type;
        }
        public string[,] MIME_MapTable = new string[,] {

            {".3gp",    "video/3gpp"},
            {".apk",    "application/vnd.android.package-archive"},
            {".asf",    "video/x-ms-asf"},
            {".avi",    "video/x-msvideo"},
            {".bin",    "application/octet-stream"},
            {".bmp",      "image/bmp"},
            {".c",        "text/plain"},
            {".class",    "application/octet-stream"},
            {".conf",    "text/plain"},
            {".cpp",    "text/plain"},
            {".doc",    "application/msword"},
            {".exe",    "application/octet-stream"},
            {".gif",    "image/gif"},
            {".gtar",    "application/x-gtar"},
            {".gz",        "application/x-gzip"},
            {".h",        "text/plain"},
            {".htm",    "text/html"},
            {".html",    "text/html"},
            {".jar",    "application/java-archive"},
            {".java",    "text/plain"},
            {".jpeg",    "image/jpeg"},
            {".jpg",    "image/jpeg"},
            {".js",        "application/x-javascript"},
            {".log",    "text/plain"},
            {".m3u",    "audio/x-mpegurl"},
            {".m4a",    "audio/mp4a-latm"},
            {".m4b",    "audio/mp4a-latm"},
            {".m4p",    "audio/mp4a-latm"},
            {".m4u",    "video/vnd.mpegurl"},
            {".m4v",    "video/x-m4v"},
            {".mov",    "video/quicktime"},
            {".mp2",    "audio/x-mpeg"},
            {".mp3",    "audio/x-mpeg"},
            {".mp4",    "video/mp4"},
            {".mpc",    "application/vnd.mpohun.certificate"},
            {".mpe",    "video/mpeg"},
            {".mpeg",    "video/mpeg"},
            {".mpg",    "video/mpeg"},
            {".mpg4",    "video/mp4"},
            {".mpga",    "audio/mpeg"},
            {".msg",    "application/vnd.ms-outlook"},
            {".ogg",    "audio/ogg"},
            {".pdf",    "application/pdf"},
            {".png",    "image/png"},
            {".pps",    "application/vnd.ms-powerpoint"},
            {".ppt",    "application/vnd.ms-powerpoint"},
            {".prop",    "text/plain"},
            {".rar",    "application/x-rar-compressed"},
            {".rc",        "text/plain"},
            {".rmvb",    "audio/x-pn-realaudio"},
            {".rtf",    "application/rtf"},
            {".sh",        "text/plain"},
            {".tar",    "application/x-tar"},
            {".tgz",    "application/x-compressed"},
            {".txt",    "text/plain"},
            {".wav",    "audio/x-wav"},
            {".wma",    "audio/x-ms-wma"},
            {".wmv",    "audio/x-ms-wmv"},
            {".wps",    "application/vnd.ms-works"},  
            //{".xml",    "text/xml"},  
            {".xml",    "text/plain"},
            {".z",        "application/x-compress"},
            {".zip",    "application/zip"},
            {"",        "*/*"}

    };
    }
}