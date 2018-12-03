using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PharmaTab
{
    [Activity(Label = "Historique")]
    
    public class Historique : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Historique);

            string directory = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "Pharmatrack";

            //On crée un tableau qui contient les chemins d'accès aux fichiers du dossier
            List<string> fichiers = new List<string>();
            try
            {
                fichiers = Directory.GetFiles(directory).ToList();
            }
            catch (Exception)
            { }

            //On crée une liste qui va afficher une ligne personnalisée pour chaque éléments du tableau
            List<string> fichierstxt = new List<string>();
            foreach (var item in fichiers)
                fichierstxt.Add("Fichier du " + item.Substring(44, 2) + "/" + item.Substring(46, 2) + "/" + item.Substring(50, 2));
            
            //On met en place les "adapters" qui prennent en charge les éléments du tableau et de la liste
            ArrayAdapter<string> fichiersAdapter = new ArrayAdapter<string>(this.ApplicationContext, Android.Resource.Layout.SimpleListItemActivated1, fichiers);
            ArrayAdapter<string> fichierstxtAdapter = new ArrayAdapter<string>(this.ApplicationContext, Android.Resource.Layout.SimpleListItemActivated1, fichierstxt);

            //On appelle la liste view présente dans fragment3.axml
            ListView listehisto = FindViewById<ListView>(Resource.Id.listehisto);

            //On associe l'adapter à la liste view
            listehisto.Adapter = fichierstxtAdapter;

            // Valeur par défaut des items en false
            for (int i = 0; i < listehisto.Count; i++)
                listehisto.SetItemChecked(i, false);



            //Méthode pour rafraîchir la liste et déselectionner les éléments
            void RefreshList()
            {
                for (int i = 0; i < listehisto.Count; i++)
                    listehisto.SetItemChecked(i, false);

                fichierstxtAdapter.Clear();
                fichierstxt.Clear();
                fichiers.Clear();
                fichiers = Directory.GetFiles(directory).ToList();

                foreach (var item in fichiers)
                    fichierstxt.Add("Fichier du " + item.Substring(44, 2) + "/" + item.Substring(46, 2) + "/" + item.Substring(48, 2));

                fichierstxtAdapter.AddAll(fichierstxt);
            }


            //On appelle les boutons ouvrir et supprimer de fragment3.axml et on déclare la méthode utilisée lors de l'évement Click
            Button btnOuvrir = FindViewById<Button>(Resource.Id.button2);
            btnOuvrir.Click += BtnOuvrir_Click;
            Button btnSuppr = FindViewById<Button>(Resource.Id.button1);
            btnSuppr.Click += BtnSuppr_Click;
            Button btedit = FindViewById<Button>(Resource.Id.buttonmodifier);

            btedit.Click += (s, e) =>
            {
                if (listehisto.CheckedItemCount > 0)
                {
                    var position = listehisto.CheckedItemPositions;
                    for (int i = 0; i < listehisto.Count; i++)
                    {
                        if (position.ValueAt(i) == true)
                        {
                            Settings.FilePath = fichiers[i];
                        }
                    }
                    
                }
                     
                StartActivity(typeof(EditActivity));
            };

            //On crée la méthode qui va ouvrir le fichier sélectionné avec Excel
            void BtnOuvrir_Click(object sender, EventArgs e)
            {
                if (listehisto.CheckedItemCount == 1)
                {
                    var position = listehisto.CheckedItemPositions;

                    Java.IO.File file = new Java.IO.File(fichiersAdapter.GetItem(position.IndexOfValue(true)));
                    Intent intent = new Intent();
                    intent.AddFlags(ActivityFlags.NewTask);
                    intent.AddFlags(ActivityFlags.GrantReadUriPermission);
                    intent.SetAction(Intent.ActionView);
                    intent.SetDataAndType(FileProvider.GetUriForFile(Application.Context,  PackageName+ ".fileprovider", file), "text/csv");
                    StartActivity(intent);
                }
                else
                    Toast.MakeText(Application.Context, "Veuillez sélectionner un fichier à ouvrir", ToastLength.Short).Show();
            }




            //On crée une méthode qui va supprimer le ou les fichiers sélectionnés
            void BtnSuppr_Click(object sender, EventArgs e)
            {
                if (listehisto.CheckedItemCount > 0)
                {
                    var position = listehisto.CheckedItemPositions;
                    Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
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
                        RefreshList();
                        Toast.MakeText(Application.Context, "Supprimé !", ToastLength.Short).Show();
                    });

                    alert.SetNegativeButton("Annuler", (senderAlert, args) =>
                    {
                        Toast.MakeText(Application.Context, "Annulé !", ToastLength.Short).Show();
                    });

                    Dialog dialog = alert.Create();
                    dialog.Show();
                }
                else
                    Toast.MakeText(Application.Context, "Veuillez sélectionner un ou plusieurs fichiers à supprimer.", ToastLength.Long).Show();
            }
        }
      
    }
}