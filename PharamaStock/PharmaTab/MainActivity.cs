﻿using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Support.V7.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZXing.Mobile;
using Symbol.XamarinEMDK;
using Symbol.XamarinEMDK.Barcode;

namespace PharmaTab
{
    [Activity(Label = "@string/app_name", MainLauncher = false, LaunchMode = Android.Content.PM.LaunchMode.SingleTop, Icon = "@drawable/icon")]


    public class MainActivity : AppCompatActivity
    {
        EMDKManager manager = null;
        BarcodeManager barcodeManager = null;
        Scanner scanner = null;

        string fileName = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "Pharmatrack" + Java.IO.File.Separator + "Pharmatrack_" + DateTime.Now.ToString("ddMMyyy") + ".csv";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.main);

            barcodeManager = (BarcodeManager)manager.GetInstance(EMDKManager.FEATURE_TYPE.Barcode);
            scanner = barcodeManager.GetDevice(BarcodeManager.DeviceIdentifier.Default);

            //création des variables des EditText de fragment1.axml
            EditText patient = FindViewById<EditText>(Resource.Id.numpat);
            EditText gef = FindViewById<EditText>(Resource.Id.codgef);
            EditText lot = FindViewById<EditText>(Resource.Id.numlot);
            EditText quantite = FindViewById<EditText>(Resource.Id.qtedel);
            EditText date = FindViewById<EditText>(Resource.Id.datedel);
            EditText matricule = new EditText(this);

            patient.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(10) });
            quantite.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(5) });
            gef.TextChanged += (s, e) =>
            {
                if (gef.Text.Length > 30)
                {
                    gef.TextSize = 15;
                }
                else
                {
                    gef.TextSize = 19;
                }
            };

            lot.TextChanged += (s, e) =>
            {
                if (lot.Text.Length > 30)
                {
                    lot.TextSize = 15;
                }
                else
                {
                    lot.TextSize = 19;
                }
            };

            //le matricule reste celui indiqué en page de connexion
            matricule.Text = Settings.Username;

            //création des variables des ImageButton de fragment1.axml
            Button savebt = FindViewById<Button>(Resource.Id.buttonenr);
            Button historique = FindViewById<Button>(Resource.Id.buttonhist);
            
            Button settings = FindViewById<Button>(Resource.Id.buttonsettings);
            Button suivant = FindViewById<Button>(Resource.Id.buttonnext);
            Button raz = FindViewById<Button>(Resource.Id.buttonreset);

            //Evenement d'acces aux pages
            if (Settings.Adminstate == "admin")
            {
                settings.Visibility = ViewStates.Visible;
            }
            else
            {
                settings.Visibility = ViewStates.Invisible;
            }

            date.Click += (s, e) =>
            {
                DatePickerDialog datepick = new DatePickerDialog(this, Android.App.AlertDialog.ThemeDeviceDefaultLight, OnDateSet, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                datepick.DatePicker.DateTime = DateTime.Today;
                datepick.Show();

            };
            settings.Click += (s, e) =>
            {
                Intent userActivity = new Intent(this, typeof(UserActivity));
                StartActivity(userActivity);
            };

            scanner.Status += (s, e) =>
            {

            };
            //Evenements d'affichage du scanner lors des clics sur les boutons
            //MobileBarcodeScanner scanner;
            //scan1.Click += async (s, e) =>
            //{
            //    await Scan(s, e);
            //};
            //scan2.Click += async (s, e) =>
            //{
            //    await Scan(s, e);
            //};
            //scan3.Click += async (s, e) =>
            //{
            //    await Scan(s, e);

            //};


            savebt.Click += Button_Click;       //Evenement bouton "Enregistrer"
            suivant.Click += Button_Click;      //Evenement bouton "Suivant"
            historique.Click += Button_Click;   //Evenement bouton "Historique"
            raz.Click += Button_Click;          //Evenement bouton "RAZ"

            //Méthode d'affichage dans les zones de texte par le biais du scanner
            //async Task Scan(object s, EventArgs e)
            //{
            //    ImageButton btn = (ImageButton)s;
            //    var toptext = "";
            //    switch (btn.Id) //Changer le toptext selon le bouton cliqué
            //    {
            //        case Resource.Id.button1:
            //            toptext = "N° du patient";
            //            break;
            //        case Resource.Id.button2:
            //            toptext = "Code GEF";
            //            break;
            //        case Resource.Id.button3:
            //            toptext = "N° du lot";
            //            break;
            //    }

            //    MobileBarcodeScanner.Initialize(Activity.Application);
            //    var options = new MobileBarcodeScanningOptions
            //    {
            //        //Options de l'appareil photo : pas de rotation, pas de caméra frontale
            //        AutoRotate = false,
            //        UseFrontCameraIfAvailable = false
            //    };
            //    scanner = new MobileBarcodeScanner()
            //    {
            //        TopText = toptext //Valeur du toptext

            //    };

            //    ZXing.Result result = null;

            //    new Thread(new ThreadStart(delegate
            //    {
            //        while (result is null)
            //        {
            //            scanner.AutoFocus();
            //            Thread.Sleep(2000);
            //        }
            //    })).Start();
            //    //Cette variable attend un scan pour obtenir la valeur lue dans le code barre
            //    result = await scanner.Scan(options);

            //    Android.Media.Stream str = Android.Media.Stream.Music;
            //    ToneGenerator tg = new ToneGenerator(str, 100);


            //    if (result == null)
            //    {
            //        return;
            //    }
            //    else
            //    {
            //        tg.StartTone(Tone.PropAck);
            //        switch (btn.Id) //Les champs de texte prennent la valeur lue par le scan
            //        {
            //            case Resource.Id.button1:
            //                patient.Text = result.Text;
            //                break;
            //            case Resource.Id.button2:
            //                gef.Text = result.Text;
            //                break;
            //            case Resource.Id.button3:
            //                lot.Text = result.Text;
            //                break;
            //        }

            //    }

            //    return;
            //}

            //Méthode dd fonction des boutons
            void Button_Click(object sender, EventArgs e)
            {
                Button btn = (Button)sender;
                switch (btn.Id)
                {
                    //// Affiche un calendrier en dialogue pour y sélectionner la date
                    //case Resource.Id.button5:
                    //    DatePickerDialog datepick = new DatePickerDialog(this.Context, AlertDialog.ThemeDeviceDefaultLight, OnDateSet, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                    //    datepick.DatePicker.DateTime = DateTime.Today;
                    //    datepick.Show();
                    //    break;

                    //Enregistre les champs textes sous format CSV
                    case Resource.Id.buttonenr:
                        if (!string.IsNullOrEmpty(patient.Text) && !string.IsNullOrEmpty(gef.Text) && !string.IsNullOrEmpty(lot.Text) && !string.IsNullOrEmpty(quantite.Text) && (!string.IsNullOrEmpty(date.Text) || quantite.Text != "0"))
                        {
                            if (CreateCSV(patient.Text, gef.Text, lot.Text, quantite.Text, date.Text, matricule.Text))
                            {
                                Toast.MakeText(Application.Context, "Cette ligne existe déjà", ToastLength.Short).Show();
                            }
                        }
                        else
                            Toast.MakeText(Application.Context, "Veuillez remplir les champs", ToastLength.Short).Show();

                        //Vide les champs d'entrée                       
                        quantite.Text = "";
                        lot.Text = "";
                        gef.Text = "";
                        patient.Text = "";
                        break;

                    //Enregistre les champs textes sous format CSV, mais garde le numéro du patient
                    case Resource.Id.buttonnext:
                        if (!string.IsNullOrEmpty(patient.Text) && !string.IsNullOrEmpty(gef.Text) && !string.IsNullOrEmpty(lot.Text) && !string.IsNullOrEmpty(quantite.Text) && (!string.IsNullOrEmpty(date.Text) || quantite.Text != "0"))
                        {
                            if (CreateCSV(patient.Text, gef.Text, lot.Text, quantite.Text, date.Text, matricule.Text))
                            {
                                Toast.MakeText(Application.Context, "Cette ligne existe déjà", ToastLength.Short).Show();
                            }
                        }
                        else
                            Toast.MakeText(Application.Context, "Veuillez remplir les champs", ToastLength.Short).Show();

                        //Vide les champs d'entrée sauf celui patient
                        quantite.Text = "";
                        lot.Text = "";
                        gef.Text = "";
                        gef.RequestFocus();
                        break;

                    //Affiche l'historique
                    case Resource.Id.buttonhist:
                        Intent historiqueActivity = new Intent(this, typeof(Historique));
                        StartActivity(historiqueActivity);
                        break;

                    //Vide tous les champs textes selon la réponse à l'alerte
                    case Resource.Id.buttonreset:
                        if (!string.IsNullOrEmpty(patient.Text) || !string.IsNullOrEmpty(gef.Text) || !string.IsNullOrEmpty(lot.Text) || !string.IsNullOrEmpty(quantite.Text) || quantite.Text != "0" || !string.IsNullOrEmpty(date.Text))
                        {
                            Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
                            Android.App.AlertDialog alert = dialog.Create();
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

                        break;

                }
            }

            //La valeur du champ texte date est la valeur choisie sur le calendrier
            void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
            {
                date.Text = e.Date.ToLongDateString();
            }
            

           
        }

        string directory = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "Pharmatrack";

        //Méthode de création du fichier CSV
        public bool CreateCSV(string numpat, string codeGEF, string lotnum, string quant, string date, string matricule)
        {
            //Création d'un dossier
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                Toast.MakeText(Application.Context, "Dossier Pharmatrack créé à la racine du stockage", ToastLength.Short).Show();
            }

            //Lignes à ajouter lors de l'enregistrement. Reprend les entrées des champs EditText
            var newline = string.Format("{0};{1};{2};{3};{4};{5}", numpat, codeGEF, lotnum, quant, date, matricule);
            var geftext = string.Format(codeGEF);
            var lot = string.Format(lotnum);
            var patient = string.Format(numpat);

            //Si le fichier n'existe pas, créer les entêtes et aller à la ligne. 
            if (!File.Exists(fileName))
            {
                string header = "Patient n° :" + ";" + "code GEF :" + ";" + "Lot n° :" + ";" + "Quantité :" + ";" + "Délivré le :" + ";" + "Matricule :";
                File.WriteAllText(fileName, header, System.Text.Encoding.UTF8);       // Création de la ligne + Encodage pour les caractères spéciaux
                File.AppendAllText(fileName, System.Environment.NewLine); // Aller à la ligne
                Toast.MakeText(Application.Context, "Nouveau fichier créé pour la date du jour", ToastLength.Short).Show();
            }

            //Création d'un tableau qui évite les doublons
            string[] lines = File.ReadLines(fileName).ToArray<string>();
            List<string> listItems = new List<string>();
            for (int i = 1; i < lines.Length; i++)
            {
                listItems = lines[i].Split(';').ToList();
                //Si la condition retourne true reprend le la création du csv a partir du code GEF en gardant le code patient
                if (listItems[0] == patient && listItems[1] == geftext && listItems[2] == lot)
                {
                    return true;
                }

            }
            File.AppendAllText(fileName, newline + System.Environment.NewLine); // Ajout de la ligne contenant les champs
            Toast.MakeText(Application.Context, "Données enregistrées", ToastLength.Short).Show();
            return false;
        }
    }
}

