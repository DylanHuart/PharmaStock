using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using System;
using System.IO;

namespace PharmaTab
{
    [Activity(Label = "PharmaTrack", MainLauncher = true, Theme = "@style/Theme.Splash", NoHistory = true)]
    public class SplashScreen : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }
        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();

            SimulateStartup();
        }

        void SimulateStartup()
        {
            var permissionSto = Manifest.Permission.WriteExternalStorage;
            var permissionCam = Manifest.Permission.Camera;
            do
            {
                if (ContextCompat.CheckSelfPermission(this, permissionSto) != Android.Content.PM.Permission.Granted || ContextCompat.CheckSelfPermission(this, permissionCam) != Android.Content.PM.Permission.Granted)
                    ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.WriteExternalStorage, Manifest.Permission.ReadExternalStorage }, 0);

            } while (ContextCompat.CheckSelfPermission(this, permissionSto) != Android.Content.PM.Permission.Granted && ContextCompat.CheckSelfPermission(this, permissionCam) != Android.Content.PM.Permission.Granted);
            // Affiche une boîte de dialogue pour accorder l'autorisation d'accès au stockage
            
            string path = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "PharmatrackXML";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (!File.Exists(path + Java.IO.File.Separator + "Config.xml"))
                XML.CreateXml(path+ Java.IO.File.Separator + "Config.xml");

            //if (Directory.Exists(Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "Pharmatrack"))
            //{
            //    var files = Directory.GetFiles(Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "Pharmatrack");
            //    if (files.Length > 0)
            //    {
            //        foreach (var file in files)
            //        {
            //            var datefile = file.Substring(44,8);
            //            var jour = datefile.Substring(0, 2);
            //            var mois = datefile.Substring(2, 2);
            //            var annee = datefile.Substring(4);

            //            if(annee == DateTime.Now.Year.ToString())
            //            {
            //                if(mois == DateTime.Now.Month.ToString())
            //                {
            //                    if(Convert.ToInt32(jour) < DateTime.Now.Day - 15)
            //                    {
            //                        File.Delete(file);
            //                    }
            //                }
            //                else
            //                {
            //                    File.Delete(file);
            //                }
            //            }
            //            else
            //            {
            //                File.Delete(file);
            //            }
            //        }
            //    }
            //}
            StartActivity(new Intent(Application.Context, typeof(LoginActivity)));
        }
    }
} 
