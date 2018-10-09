using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

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
            ImageButton enr = view.FindViewById<ImageButton>(Resource.Id.buttonuser);
            ImageButton scan = view.FindViewById<ImageButton>(Resource.Id.btnscan);

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
    }
}