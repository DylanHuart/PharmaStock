using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Support.V4.Content;
using System.IO;
using Android.Views;
using System.Linq;

namespace PharmaTab.Fragments
{
    public class Fragment3 : Android.Support.V4.App.Fragment
    {

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        //On instancie le fragment3 qui sert a afficher la page Historique
        public static Fragment3 NewInstance()
        {
            var frag3 = new Fragment3 { Arguments = new Bundle() };
            return frag3;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fragment3, null);

            //On créer la variable qui va donner la direction des fichiers dans le stockage interne de l'appareil
            string directory = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "Pharmastock";

            //On créer un tableau qui contient les chemins d'accès aux fichiers du dossier
            List<string> fichiers = Directory.GetFiles(directory).ToList();

            //On créer une liste qui va afficher une ligne personnalisée pour chaque éléments du tableau
            List<string> fichierstxt = new List<string>();
            foreach (var item in fichiers)
                fichierstxt.Add("Fichier du " + File.GetCreationTime(item));

            //On met en place les "adapters" qui prennent en charge les éléments du tableau et de la liste
            ArrayAdapter<string> fichiersAdapter = new ArrayAdapter<string>(this.Context, Android.Resource.Layout.SimpleListItemActivated1, fichiers);
            ArrayAdapter<string> fichierstxtAdapter = new ArrayAdapter<string>(this.Context, Android.Resource.Layout.SimpleListItemActivated1, fichierstxt);

            //On appelle la liste view présente dans fragment3.axml
            ListView listehisto = view.FindViewById<ListView>(Resource.Id.listehisto);

            //On associe l'adapter à la liste view
            listehisto.Adapter = fichierstxtAdapter;

            // Valeur par défaut des items en false
            for (int i = 0; i < listehisto.Count; i++)
                listehisto.SetItemChecked(i, false);



            //Méthode pour rafraîchir la liste et déselectionner les éléments
            void ClearList()
            {
                for (int i = 0; i < listehisto.Count; i++)
                    listehisto.SetItemChecked(i, false);
                fichierstxtAdapter.Clear();
                fichierstxt.Clear();
                fichiers.Clear();
                fichiers = Directory.GetFiles(directory).ToList();

                foreach (var item in fichiers)
                    fichierstxt.Add("Fichier du " + File.GetCreationTime(item));

                fichierstxtAdapter.AddAll(fichierstxt);
            }


            //On appelle les boutons ouvrir et supprimer de fragment3.axml et on déclare la méthode utilisée lors de l'évement Click
            Button btnOuvrir = view.FindViewById<Button>(Resource.Id.button2);
            btnOuvrir.Click += BtnOuvrir_Click;
            Button btnSuppr = view.FindViewById<Button>(Resource.Id.button1);
            btnSuppr.Click += BtnSuppr_Click;



            //On créer la méthode qui va ouvrir le fichier sélectionné avec Excel
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
                    intent.SetDataAndType(FileProvider.GetUriForFile(this.Context, this.Activity.PackageName + ".fileprovider", file), "text/csv");// "application/vnd.ms-excel");
                    StartActivity(intent);
                }
                else
                {
                    Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this.Context);
                    AlertDialog alert = dialog.Create();
                    alert.SetTitle("Attention");
                    alert.SetMessage("Veuillez sélectionner un fichier à ouvrir");
                    alert.SetButton("OK", (c, ev) =>
                    {
                    });
                    alert.Show();
                }
            }




            //On créer une méthode qui va supprimer le ou les fichiers sélectionnés
            void BtnSuppr_Click(object sender, EventArgs e)
            {
                if (listehisto.CheckedItemCount > 0)
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

                        Toast.MakeText(this.Context, "Supprimé !", ToastLength.Short).Show();
                        ClearList();
                    });

                    alert.SetNegativeButton("Annuler", (senderAlert, args) =>
                    {
                        Toast.MakeText(this.Context, "Annulé !", ToastLength.Short).Show();
                    });

                    Dialog dialog = alert.Create();
                    dialog.Show();
                }
                else
                {
                    Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this.Context);
                    AlertDialog alert = dialog.Create();
                    alert.SetTitle("Attention");
                    alert.SetMessage("Veuillez sélectionner un ou plusieurs fichiers à supprimer");
                    alert.SetButton("OK", (c, ev) =>
                    {
                    });
                    alert.Show();
                }
            }


            // MODIFICATION

            Button modif = view.FindViewById<Button>(Resource.Id.btnModif);
            modif.Click += Modif_Click;

            void Modif_Click(object sender, EventArgs e)
            {
                var position = listehisto.CheckedItemPositions;
                string path = fichiersAdapter.GetItem(position.IndexOfValue(true)).ToString();

                using (var reader = new StreamReader(path))
                {
                    List<string> listA = new List<string>();
                    List<string> listB = new List<string>();
                    List<string> listC = new List<string>();
                    List<string> listD = new List<string>();
                    List<string> listE = new List<string>();
                    List<string> listF = new List<string>();


                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(';');

                        listA.Add(values[0]);
                        listB.Add(values[1]);
                        listC.Add(values[2]);
                        listD.Add(values[3]);
                        listE.Add(values[4]);
                        listF.Add(values[5]);

                    }
                }


            }






            return view;
        }


    }
}