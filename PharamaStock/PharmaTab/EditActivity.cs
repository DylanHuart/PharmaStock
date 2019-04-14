using Android.App;
using Android.OS;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PharmaTab
{
    [Activity(Label = "EditActivity")]
    public class EditActivity : Activity
    {
        EditText patient = new EditText(Application.Context);
        EditText gef = new EditText(Application.Context);
        EditText lot = new EditText(Application.Context);
        EditText quantite = new EditText(Application.Context);
        EditText date = new EditText(Application.Context);

        TextView pages = new TextView(Application.Context);
        ImageButton scan1 = new ImageButton(Application.Context);
        ImageButton scan2 = new ImageButton(Application.Context);
        ImageButton scan3 = new ImageButton(Application.Context);

        Button enregistrer = new Button(Application.Context);
        Button suivant = new Button(Application.Context);
        Button prev = new Button(Application.Context);
        Button supprimer = new Button(Application.Context);
        Button raz = new Button(Application.Context);

        string[] lines;
        int totalpages = 0;
        List<string> listItems = new List<string>();
        int ligne = 1;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.editlayout);
            
            patient = FindViewById<EditText>(Resource.Id.numpat3);
             gef = FindViewById<EditText>(Resource.Id.codgef3);
             lot = FindViewById<EditText>(Resource.Id.numlot3);
             quantite = FindViewById<EditText>(Resource.Id.qtedel3);
             date = FindViewById<EditText>(Resource.Id.datedel3);

            pages = FindViewById<TextView>(Resource.Id.textpages);

             enregistrer = FindViewById<Button>(Resource.Id.buttonenr3);
             suivant = FindViewById<Button>(Resource.Id.buttonnext3);
             prev = FindViewById<Button>(Resource.Id.buttonprev3);
             supprimer = FindViewById<Button>(Resource.Id.delbt1);

            raz = FindViewById<Button>(Resource.Id.buttonreset2);

            FillandRefresh();

           
            date.Click += (s, e) =>
            {
                DatePickerDialog datepick = new DatePickerDialog(this, AlertDialog.ThemeDeviceDefaultLight, OnDateSet, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                datepick.DatePicker.DateTime = DateTime.Today;
                datepick.Show();

            };
            raz.Click += (s, e) =>
            {
                if (!string.IsNullOrEmpty(patient.Text) || !string.IsNullOrEmpty(gef.Text) || !string.IsNullOrEmpty(lot.Text) || !string.IsNullOrEmpty(quantite.Text) || quantite.Text != "0" || !string.IsNullOrEmpty(date.Text))
                {
                    Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                    AlertDialog alert = dialog.Create();
                    alert.SetTitle("Attention");
                    alert.SetMessage("Les champs de texte seront vidés");
                    alert.SetButton("OK", (c, ev) =>
                    {
                        //Vide les champs
                        date.Text = "";
                        quantite.Text = "";
                        lot.Text = "";
                        gef.Text = "";
                        patient.Text = "";
                        Toast.MakeText(Application.Context, "Champs réinitialisés", ToastLength.Short).Show();
                    });
                    alert.SetButton2("Annuler", (c, ev) =>
                    {
                        //Ne rien faire
                    });
                    alert.Show();
                }


            };

            suivant.Click += (s, e) =>
            {
                if(ligne<lines.Count()-1)
                {
                    ligne++;
                    pages.Text = "Page " + ligne.ToString() + " sur " + totalpages.ToString();
                    fillform(listItems = lines[ligne].Split(';').ToList());
                }
            };

            prev.Click += (s, e) =>
            {
                if (ligne > 1)
                {
                    ligne--;
                    pages.Text = "Page " + ligne.ToString() + " sur " + totalpages.ToString();
                    fillform(listItems = lines[ligne].Split(';').ToList());
                }
            };

            enregistrer.Click += (s, e) =>
            {
                var newline = string.Format("{0};{1};{2};{3};{4};{5}", patient.Text, gef.Text, lot.Text, quantite.Text, date.Text, Settings.Username);
                ReadReplace(ligne, newline);
                FillandRefresh();
            };

            supprimer.Click += (s, e) =>
            {
                Readdelete(ligne);
                FillandRefresh();
            };
            // Create your application here
        }

        public void FillandRefresh()
        {
             lines = File.ReadLines(Settings.FilePath).ToArray<string>();
            totalpages = lines.Count() - 1;
             listItems = lines[ligne].Split(';').ToList();
            pages.Text = "Page " + ligne.ToString() + " sur " + totalpages.ToString();
            fillform(listItems = lines[ligne].Split(';').ToList());
        }

        void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            date.Text = e.Date.ToLongDateString();
        }
        //Méthode d'affichage dans les zones de texte par le biais du scanner
       

        public static void Readdelete(int ligne)
        {

            string[] lines = File.ReadLines(Settings.FilePath).ToArray<string>();
            
            using (StreamWriter file = new StreamWriter(Settings.FilePath))
            {
                for (int j = 0; j < lines.Count(); j++)
                {
                    if(j!=ligne)
                    {
                        file.WriteLine(lines[j]);
                    }
                }
            }

            //File.WriteAllText(Settings.FilePath, string.Join(";",lines), System.Text.Encoding.UTF8);       // Création de la ligne + Encodage pour les caractères spéciaux

            Toast.MakeText(Application.Context, "Données enregistrées", ToastLength.Short).Show();

        }

        public static void ReadReplace(int ligne,string newline)
        {

            string[] lines = File.ReadLines(Settings.FilePath).ToArray<string>();
            for (int i = 0; i < lines.Count(); i++)
            {
                if (ligne == i)
                {
                    lines[i] = newline;
                }
            }
            using (StreamWriter file = new StreamWriter(Settings.FilePath))
            {
                for(int j = 0; j<lines.Count();j++)
                {
                    file.WriteLine(lines[j]);
                }
            }
            
            //File.WriteAllText(Settings.FilePath, string.Join(";",lines), System.Text.Encoding.UTF8);       // Création de la ligne + Encodage pour les caractères spéciaux
            
            Toast.MakeText(Application.Context, "Données enregistrées", ToastLength.Short).Show();

        }

        public void fillform(List<string> items)
        {
            patient.Text = items[0];
            gef.Text = items[1];
            lot.Text = items[2];
            quantite.Text = items[3];
            date.Text = items[4];
        }
    }
}