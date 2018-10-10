using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using ZXing.Mobile;

namespace PharmaTab.Fragments
{
    public class Fragment3 : Android.Support.V4.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static Fragment3 NewInstance()
        {
            var frag3 = new Fragment3 { Arguments = new Bundle() };
            return frag3;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fragment3, null);

            //Chemin du fichier xml
            string path = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "PharmastockXML"  + Java.IO.File.Separator + "Config.xml";

            EditText matricule = view.FindViewById<EditText>(Resource.Id.idmatr);
            EditText mdp = view.FindViewById<EditText>(Resource.Id.idmdp);
            Button enr = view.FindViewById<Button>(Resource.Id.buttonuser);
            ImageButton scan = view.FindViewById<ImageButton>(Resource.Id.btnscan);

            scan.Click += async (s, e) =>
            {
                mdp.Text = await Scan();
            };
            //Bouton enregistrer
            enr.Click += (s, e) =>
            {
                try
                {
                    if (!string.IsNullOrEmpty(matricule.Text) && !string.IsNullOrEmpty(mdp.Text))
                    {
                        XML.CreateUser(path, matricule.Text, mdp.Text);
                        Toast.MakeText(Application.Context, string.Format("Utilisateur {0} créé", matricule.Text), ToastLength.Long).Show();

                    }

                    else
                        Toast.MakeText(Application.Context, "Veuillez remplir les champs", ToastLength.Long).Show();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(Application.Context, ex.Message, ToastLength.Long).Show();
                }
            };
            return view;
        }

        async Task<string> Scan()
        {
            MobileBarcodeScanner scanner;
            MobileBarcodeScanner.Initialize(Activity.Application);

            var options = new MobileBarcodeScanningOptions
            {
                AutoRotate = false,
                UseFrontCameraIfAvailable = false,
                DelayBetweenContinuousScans = 1500,
            };

            scanner = new MobileBarcodeScanner()
            {
                TopText = "Mot de passe"
            };

            var result = await scanner.Scan(options);
            Android.Media.Stream str = Android.Media.Stream.Music;
            ToneGenerator tg = new ToneGenerator(str, 100);
            tg.StartTone(Tone.PropAck);

            if (result == null)
            {
                return "";
            }

            return result.Text;
        }
    }
}