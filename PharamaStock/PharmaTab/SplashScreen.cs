using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
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
            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }

        async void SimulateStartup()
        {
            string path = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "PharmastockXML";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (!File.Exists(path + Java.IO.File.Separator + "Config.xml"))
                XML.CreateXml(path+ Java.IO.File.Separator + "Config.xml");
            await Task.Delay(2000); // Simulate a bit of startup work.
            StartActivity(new Intent(Application.Context, typeof(LoginActivity)));
        }
    }
} 