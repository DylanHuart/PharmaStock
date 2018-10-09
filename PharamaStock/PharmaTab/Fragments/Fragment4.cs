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
    public class Fragment4 : Android.Support.V4.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static Fragment4 NewInstance()
        {
            var frag4 = new Fragment4 { Arguments = new Bundle() };
            return frag4;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fragment4, null);

            //Chemin du fichier xml
            string path = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "PharmastockXML" + Java.IO.File.Separator+ "Config.xml";

            Button modif = view.FindViewById<Button>(Resource.Id.btnModif);
            Button supp = view.FindViewById<Button>(Resource.Id.btnSuppr);
            EditText pwd = view.FindViewById<EditText>(Resource.Id.idpwd);
            ListView list = view.FindViewById<ListView>(Resource.Id.listView1);

            //Contenu des balises user du fichier xml chargé dans l'adapter 
            var users = XML.ListeUser(path);
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(Application.Context, Android.Resource.Layout.SimpleListItemActivated1, users);
            list.Adapter = adapter;
           
            //Rafraîchit la listview
            void Refresh()
            {
                users.Clear();
                adapter.Clear();
                users = XML.ListeUser(path);
                adapter.AddAll(users);
                list.Adapter = adapter;
            }

            //Bouton modifier
            modif.Click += (s, e) =>
            {
                try
                {
                    var positem = list.CheckedItemPosition;
                    var item = adapter.GetItem(positem).ToString(); 
                    XML.EcritXml(path,item,pwd.Text);
                    Toast.MakeText(Application.Context, "Mot de passe modifié avec succès", ToastLength.Long).Show();
                    Refresh();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(Application.Context, ex.Message, ToastLength.Long).Show();
                }
            };

            //Bouton supprimer avec alerte
            supp.Click += (s, e) =>
            {
                try
                {
                    var positem = list.CheckedItemPosition;
                    var item = adapter.GetItem(positem).ToString();
                    AlertDialog.Builder alert = new AlertDialog.Builder(this.Context);
                    alert.SetTitle("Suppression");
                    alert.SetMessage("Voulez-vous vraiment supprimer l'utilisateur " + item + "?");
                    alert.SetPositiveButton("Supprimer", (senderAlert, args) =>
                    {
                        XML.DeleteXml(path, item);
                        Toast.MakeText(Application.Context, "Utilisateur supprimé avec succès", ToastLength.Long).Show();
                        Refresh();
                    });

                    alert.SetNegativeButton("Annuler", (senderAlert, args) =>
                    {
                        Toast.MakeText(Application.Context, "Annulé !", ToastLength.Short).Show();
                    });

                    Dialog dialog = alert.Create();
                    dialog.Show();
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