using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using System.Threading;
using System.Threading.Tasks;
using ZXing.Mobile;

namespace PharmaTab
{
    [Activity(Label = "PharmaTrack")] //, MainLauncher = true, LaunchMode = Android.Content.PM.LaunchMode.SingleTop, Icon = "@drawable/icon")]
    public class LoginActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            string path = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "PharmatrackXML" + Java.IO.File.Separator + "Config.xml";

            //On appelle les classe du using plugin.SecureStorage
           
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Loginlayout);
           
            
            //On créer des variables en appelant les ID de LoginLayout.axml            
            Button connexion = FindViewById<Button>(Resource.Id.buttonco);
            ImageButton mdpscan = FindViewById<ImageButton>(Resource.Id.btnPwd);
            EditText username = FindViewById<EditText>(Resource.Id.idmatr);
            EditText password = FindViewById<EditText>(Resource.Id.idmdp);

            username.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(8) });
            //On créer l'évenment password du Edit Text de LoginLayout.axml
            password.KeyPress += (object sender, View.KeyEventArgs e) => {
                e.Handled = false;
                if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                {
                    e.Handled = true;
                    connexion.PerformClick();
                }
            };

            

            mdpscan.Click += async (s, e) =>
            {
                Task<string> task = Scan();
                password.Text = await task;
                if (!string.IsNullOrEmpty(password.Text))
                {
                    connexion.PerformClick();
                }
            };

            
            
            async Task<string> Scan()
            {
                MobileBarcodeScanner scanner;
                MobileBarcodeScanner.Initialize(Application);

                var options = new MobileBarcodeScanningOptions
                {
                    AutoRotate = false,
                    UseFrontCameraIfAvailable = false,
                };

                scanner = new MobileBarcodeScanner()
                {
                    TopText = "Scannez le code barre de votre carte"
                };

                ZXing.Result result = null;

                new Thread(new ThreadStart(delegate
                {
                    while (result is null)
                    {
                        scanner.AutoFocus();
                        Thread.Sleep(2000);
                    }
                })).Start();

                result = await scanner.Scan(options);

                if (result == null)
                {
                    return "";
                }
                return result.Text;
            }

            //On créer l'évenement connexion de l'image button de LoginLayout.axml
            connexion.Click += (s, e) =>
            {
                //Lit le fichier XML pour voir si les identifiants sont valides
                var conn = XML.LitXml(path, username.Text, password.Text);
                
                if (conn)
                {
                    Settings.Username = username.Text;
                    if (username.Text == "admin")
                    {
                        Settings.Adminstate = "admin";
                    }
                    else
                    {
                        Settings.Adminstate = "";
                    }
                    Toast.MakeText(Application.Context, "Connexion réussie !", ToastLength.Short).Show();
                    StartActivity(typeof(MainActivity));
                }
                else
                    Toast.MakeText(Application.Context, "Identifiants invalides !", ToastLength.Short).Show();
                
            };
        }
    }
}