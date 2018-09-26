using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using PharmaTab.Fragments;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Content;
using System.Text;
using Android.Util;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Android;
using Android.Support.V4.Content;
using Plugin.SecureStorage;

namespace PharmaTab
{
    [Activity(Label = "PharmaTrack", MainLauncher = true, LaunchMode = Android.Content.PM.LaunchMode.SingleTop, Icon = "@drawable/icon")]
    public class LoginActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            //CrossSecureStorage.Current.SetValue("SessionToken", "admin");
            //CrossSecureStorage.Current.SetValue("passwordToken", "admin");


            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Loginlayout);
            // Create your application here
            

            Button connexion = FindViewById<Button>(Resource.Id.buttonco);
            EditText username = FindViewById<EditText>(Resource.Id.idmatr);
            EditText password = FindViewById<EditText>(Resource.Id.idmdp);


            connexion.Click += (s, e) =>
            {
                var sessionToken = CrossSecureStorage.Current.GetValue("SessionToken");
                var passwordToken = CrossSecureStorage.Current.GetValue("passwordToken");

                if (username.Text == sessionToken && password.Text == passwordToken)
                {
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