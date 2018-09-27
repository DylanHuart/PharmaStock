using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Widget;
using System;
using Android.Support.V4.Content;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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



            

            string directory = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "Pharmastock";
            string[] fichiers = Directory.GetFiles(directory);
            ArrayAdapter<string> fichiersAdapter;
            fichiersAdapter = new ArrayAdapter<string>(this.Context, Android.Resource.Layout.SimpleListItemActivated1, fichiers);


            List<string> fichierstxt = new List<string>();
            foreach (var item in fichiers)
                fichierstxt.Add("Fichier du "+ File.GetCreationTime(item));

            ArrayAdapter<string> fichierstxtAdapter = new ArrayAdapter<string>(this.Context, Android.Resource.Layout.SimpleListItemActivated1, fichierstxt);
               


            
            ListView listehisto = view.FindViewById<ListView>(Resource.Id.listehisto);
            listehisto.SetAdapter(fichierstxtAdapter);

            

            // Valeur par défaut des items en false
            for (int i = 0; i < listehisto.Count; i++)
                listehisto.SetItemChecked(i, false);
            

            Button btnOuvrir = view.FindViewById<Button>(Resource.Id.button2);
            Button btnSuppr = view.FindViewById<Button>(Resource.Id.button1);
            btnOuvrir.Click += BtnOuvrir_Click;
            btnSuppr.Click += BtnSuppr_Click;


            void BtnOuvrir_Click(object sender, EventArgs e)
            {

                try
                {

               var position = listehisto.CheckedItemPositions;

                    Java.IO.File file = new Java.IO.File(fichiersAdapter.GetItem(position.IndexOfValue(true)));
                    Intent intent = new Intent();
                    intent.AddFlags(ActivityFlags.NewTask);
                    intent.AddFlags(ActivityFlags.GrantReadUriPermission);
                    intent.SetAction(Intent.ActionView);
                    intent.SetDataAndType(FileProvider.GetUriForFile(this.Context, this.Activity.PackageName + ".fileprovider", file), "text/csv");// "application/vnd.ms-excel");
                    StartActivity(intent);
                }
                catch (Exception ex)
                {
                    Toast.MakeText(Application.Context, ex.Message, ToastLength.Long);
                }
            }




            void BtnSuppr_Click(object sender, EventArgs e)
            {
                var position = listehisto.CheckedItemPositions;
                AlertDialog.Builder alert = new AlertDialog.Builder(this.Context);
                alert.SetTitle("Suppression");
                alert.SetMessage("Voulez-vous vraiment supprimer ?");
                alert.SetPositiveButton("Supprimer", (senderAlert, args) =>
                {
                    for (int i = 0; i < listehisto.Count; i++)
                    {
                        if (position.ValueAt(i) == true)
                        {
                            File.Delete(fichiers[i]);
                        }

                    }
                    Intent historiqueActivity = new Intent(this.Context, typeof(Historique));
                    StartActivity(historiqueActivity);

                    Toast.MakeText(this.Context, "Supprimé !", ToastLength.Short).Show();

                });

                alert.SetNegativeButton("Annuler", (senderAlert, args) =>
                {
                    Toast.MakeText(this.Context, "Annulé !", ToastLength.Short).Show();
                });

                Dialog dialog = alert.Create();
                dialog.Show();
            }
            return view;
        }
    }
}