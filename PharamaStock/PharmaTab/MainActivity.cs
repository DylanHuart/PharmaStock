using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using Symbol.XamarinEMDK;
using Symbol.XamarinEMDK.Barcode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PharmaTab
{
    [Activity(Label = "@string/app_name", MainLauncher = false, LaunchMode = Android.Content.PM.LaunchMode.SingleTop, Icon = "@drawable/icon")]


    public class MainActivity : AppCompatActivity, EMDKManager.IEMDKListener
    {
        EMDKManager emdkManager = null;
        BarcodeManager barcodeManager = null;
        Scanner scanner = null;

        private TextView statusView = null;

        string fileName = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "Pharmatrack" + Java.IO.File.Separator + "Pharmatrack_" + DateTime.Now.ToString("ddMMyyy") + ".csv";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.main);


            //création des variables des EditText de fragment1.axml
            EditText patient = FindViewById<EditText>(Resource.Id.numpat);
            EditText gef = FindViewById<EditText>(Resource.Id.codgef);
            EditText lot = FindViewById<EditText>(Resource.Id.numlot);
            EditText quantite = FindViewById<EditText>(Resource.Id.qtedel);
            EditText date = FindViewById<EditText>(Resource.Id.datedel);
            EditText matricule = new EditText(this);
            statusView = FindViewById<TextView>(Resource.Id.statusView);


            EMDKResults results = EMDKManager.GetEMDKManager(Android.App.Application.Context, this);

            if (results.StatusCode != EMDKResults.STATUS_CODE.Success)
            {
                statusView.Text = "Status: EMDKManager object creation failed ...";
            }
            else
            {
                statusView.Text = "Status: EMDKManager object creation succeeded ...";
            }


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

            savebt.Click += Button_Click;       //Evenement bouton "Enregistrer"
            suivant.Click += Button_Click;      //Evenement bouton "Suivant"
            historique.Click += Button_Click;   //Evenement bouton "Historique"
            raz.Click += Button_Click;          //Evenement bouton "RAZ"

            //Méthode dd fonction des boutons
            void Button_Click(object sender, EventArgs e)
            {
                Button btn = (Button)sender;
                switch (btn.Id)
                {
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

        protected override void OnResume()
        {
            base.OnResume();
            InitScanner();
        }


        protected override void OnPause()
        {
            base.OnPause();
            DeinitScanner();
        }



        protected override void OnDestroy()
        {
            base.OnDestroy();

            //Clean up the emdkManager
            if (emdkManager != null)
            {
                //EMDK: Release the EMDK manager object
                emdkManager.Release();
                emdkManager = null;
            }
        }

        void EMDKManager.IEMDKListener.OnOpened(EMDKManager emdkManager)
        {
            this.emdkManager = emdkManager;

            InitScanner();
        }

        void EMDKManager.IEMDKListener.OnClosed()
        {
            if (emdkManager != null)
            {
                emdkManager.Release();
                emdkManager = null;
            }
        }

        void scanner_Data(object sender, Scanner.DataEventArgs e)
        {

        }


        void scanner_Status(object sender, Scanner.StatusEventArgs e)
        {

        }
        void InitScanner()
        {
            if (emdkManager != null)
            {

                if (barcodeManager == null)
                {
                    try
                    {

                        //Get the feature object such as BarcodeManager object for accessing the feature.
                        barcodeManager = (BarcodeManager)emdkManager.GetInstance(EMDKManager.FEATURE_TYPE.Barcode);

                        scanner = barcodeManager.GetDevice(BarcodeManager.DeviceIdentifier.Default);

                        if (scanner != null)
                        {

                            //Attahch the Data Event handler to get the data callbacks.
                            scanner.Data += scanner_Data;

                            //Attach Scanner Status Event to get the status callbacks.
                            scanner.Status += scanner_Status;

                            scanner.Enable();

                            //EMDK: Configure the scanner settings
                            ScannerConfig config = scanner.GetConfig();
                            config.SkipOnUnsupported = ScannerConfig.SkipOnUnSupported.None;
                            config.ScanParams.DecodeLEDFeedback = true;
                            config.ScanParams.DecodeLEDTime = 75;
                            config.
                            // Set beam timer for camera"
                            config.ReaderParams.ReaderSpecific.CameraSpecific.BeamTimer = 4000;
                            //Set beam timer for imager
                            config.ReaderParams.ReaderSpecific.ImagerSpecific.BeamTimer = 4000;
                            //Set beam timer for laser
                            config.ReaderParams.ReaderSpecific.LaserSpecific.BeamTimer = 4000;
                            //config.ReaderParams.ReaderSpecific.ImagerSpecific.BeamTimer = 4000;
                            //config.ReaderParams.ReaderSpecific.ImagerSpecific.ch
                            //config.DecoderParams.Code39.Enabled = true;
                            //config.DecoderParams.Code128.Enabled = false;

                            scanner.SetConfig(config);

                        }
                        else
                        {
                            //displayStatus("Failed to enable scanner.\n");
                        }
                    }
                    catch (ScannerException e)
                    {
                        Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
                        Android.App.AlertDialog alert = dialog.Create();
                        alert.SetTitle("Erreur");
                        alert.SetMessage("Error: " + e.Message);
                        alert.SetButton("OK", (c, ev) =>
                        {
                            alert.Hide();
                        });
                        alert.Show();
                    }
                    catch (Exception ex)
                    {
                        Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
                        Android.App.AlertDialog alert = dialog.Create();
                        alert.SetTitle("Erreur");
                        alert.SetMessage("Error: " + ex.Message);
                        alert.SetButton("OK", (c, ev) =>
                        {
                            alert.Hide();
                        });
                        alert.Show();
                    }
                }
            }
        }

        void DeinitScanner()
        {
            if (emdkManager != null)
            {

                if (scanner != null)
                {
                    try
                    {

                        scanner.Data -= scanner_Data;
                        scanner.Status -= scanner_Status;

                        scanner.Disable();


                    }
                    catch (ScannerException e)
                    {
                        Log.Debug(this.Class.SimpleName, "Exception:" + e.Result.Description);
                    }
                }

                if (barcodeManager != null)
                {
                    emdkManager.Release(EMDKManager.FEATURE_TYPE.Barcode);
                }
                barcodeManager = null;
                scanner = null;
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

