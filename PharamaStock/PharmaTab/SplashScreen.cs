using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using System;
using System.IO;
using System.Threading.Tasks;

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
            
            string path = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "PharmastockXML";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (!File.Exists(path + Java.IO.File.Separator + "Config.xml"))
                XML.CreateXml(path+ Java.IO.File.Separator + "Config.xml");
           
            StartActivity(new Intent(Application.Context, typeof(LoginActivity)));
        }
    }
} 