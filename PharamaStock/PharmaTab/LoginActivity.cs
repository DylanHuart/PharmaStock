using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Plugin.SecureStorage;
using Android.Views;

namespace PharmaTab
{
    [Activity(Label = "PharmaTrack", MainLauncher = true, LaunchMode = Android.Content.PM.LaunchMode.SingleTop, Icon = "@drawable/icon")]
    public class LoginActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            //On appelle les classe du using plugin.SecureStorage
            CrossSecureStorage.Current.SetValue("AdminToken", "admin");
            CrossSecureStorage.Current.SetValue("AdmpwdToken", "admin");

            CrossSecureStorage.Current.SetValue("SessionToken", "01682286");
            CrossSecureStorage.Current.SetValue("passwordToken", "recette0");

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Loginlayout);
           
            
            //On créer des variables en appelant les ID de LoginLayout.axml            
            ImageButton connexion = FindViewById<ImageButton>(Resource.Id.buttonco);
            EditText username = FindViewById<EditText>(Resource.Id.idmatr);
            EditText password = FindViewById<EditText>(Resource.Id.idmdp);

            //On créer l'évenment password du Edit Text de LoginLayout.axml
            password.KeyPress += (object sender, View.KeyEventArgs e) => {
                e.Handled = false;
                if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                {
                    e.Handled = true;
                    connexion.PerformClick();
                }
            };
            //On créer l'évenement connexion de l'image button de LoginLayout.axml
            connexion.Click += (s, e) =>
            {
                var AdminToken = CrossSecureStorage.Current.GetValue("AdminToken");
                var AdmpwdToken = CrossSecureStorage.Current.GetValue("AdmpwdToken");

                var sessionToken = CrossSecureStorage.Current.GetValue("SessionToken");
                var passwordToken = CrossSecureStorage.Current.GetValue("passwordToken");

                if (username.Text == sessionToken || username.Text == AdminToken && password.Text == passwordToken || password.Text == AdmpwdToken)
                {
                    if(username.Text == "admin")
                    {
                        Settings.Adminstate = "admin";
                    }
                    else
                    {
                        Settings.Adminstate = "";
                    }
                    Settings.Username = username.Text;
                    Toast.MakeText(Application.Context, "Connexion réussie !", ToastLength.Long);
                    StartActivity(typeof(MainActivity));
                }
                else
                {
                    Toast.MakeText(Application.Context, "Echec de la connexion, vérifiez vos identifiants !", ToastLength.Long);
                }
            };
        }
    }
}