using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ZXing.Mobile;

namespace PharmaTab.Fragments
{
    public class Fragment1 : Android.Support.V4.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public static Fragment1 NewInstance()
        {
            var frag1 = new Fragment1 { Arguments = new Bundle() };
            return frag1;
        }
        string fileName = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "Pharmastock" + Java.IO.File.Separator + "Pharmastock_" + DateTime.Now.ToString("ddMMyyy") + ".csv";

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fragment1, null);
            //création des variables des EditText de fragment1.axml
            EditText patient = view.FindViewById<EditText>(Resource.Id.numpat);
            EditText gef = view.FindViewById<EditText>(Resource.Id.codgef);
            EditText lot = view.FindViewById<EditText>(Resource.Id.numlot);
            EditText quantite = view.FindViewById<EditText>(Resource.Id.qtedel);
            EditText date = view.FindViewById<EditText>(Resource.Id.datedel);
            EditText matricule = new EditText(this.Context);
            //le matricule reste celui indiqué en page de connection
            matricule.Text = Settings.Username;
            
            ImageButton savebt = view.FindViewById<ImageButton>(Resource.Id.buttonenr);
            ImageButton selectdate = view.FindViewById<ImageButton>(Resource.Id.button5);
            ImageButton historique = view.FindViewById<ImageButton>(Resource.Id.buttonhist);
            ImageButton scan1 = view.FindViewById<ImageButton>(Resource.Id.button1);
            ImageButton scan2 = view.FindViewById<ImageButton>(Resource.Id.button2);
            ImageButton scan3 = view.FindViewById<ImageButton>(Resource.Id.button3);
            ImageButton scan4 = view.FindViewById<ImageButton>(Resource.Id.button4);
            ImageButton settings = view.FindViewById<ImageButton>(Resource.Id.buttonsettings);
            ImageButton suivant = view.FindViewById<ImageButton>(Resource.Id.buttonnext);
            //Evenement d'acces aux pages
            if (Settings.Adminstate == "admin")
            {
                settings.Visibility = ViewStates.Visible;
            }
            else
            {
                settings.Visibility = ViewStates.Invisible;
            }
            //Evenements d'affichage du scanner
            MobileBarcodeScanner scanner;           
            scan1.Click += async (s, e) =>
            {
                await Scan(s, e);
            };
            scan2.Click += async (s, e) =>
            {
                await Scan(s, e);
            };
            scan3.Click += async (s, e) =>
            {
                await Scan(s, e);
            };
            scan4.Click += async (s, e) =>
            {
                await Scan(s, e);
            };

            selectdate.Click += Button_Click;//Evenement bouton "Date"
            savebt.Click += Button_Click;//Evenement bouton "Enregistrer"
            suivant.Click += Button_Click;//Evenement bouton "Suivant"
            historique.Click += Button_Click;//Evenement bouton "Historique"

            //Méthode d'affichage dans les zones de texte par le biais du scanner
            async Task Scan(object s,EventArgs e)
            {
                ImageButton btn = (ImageButton)s;
                var toptext = "";
                switch (btn.Id)
                {
                    case Resource.Id.button1:
                        toptext = "N° du patient";
                        break;
                    case Resource.Id.button2:
                        toptext = "Code GEF";
                        break;
                    case Resource.Id.button3:
                        toptext = "Quantité délivrée";
                        break;
                    case Resource.Id.button4:
                        toptext = "N° du lot";
                        break;
                }

                MobileBarcodeScanner.Initialize(Activity.Application);
                var options = new MobileBarcodeScanningOptions
                {
                    AutoRotate = false,
                    UseFrontCameraIfAvailable = false
                };
                scanner = new MobileBarcodeScanner()
                {
                    CancelButtonText = "Annuler",
                    FlashButtonText = "Flash",
                    TopText = toptext
                };

                var result = await scanner.Scan(options);

                
                if (result == null)
                {
                    return;
                }
                else
                {
                    switch (btn.Id)
                    {
                        case Resource.Id.button1:
                            patient.Text = result.Text;
                            break;
                        case Resource.Id.button2:
                            gef.Text = result.Text;
                            break;
                        case Resource.Id.button3:
                            quantite.Text = result.Text;
                            break;
                        case Resource.Id.button4:
                            lot.Text = result.Text;
                            break;
                    }
                }

                return;
            }

            
            //Méthode dd fonction des boutons
            void Button_Click(object sender, EventArgs e)
            {
                ImageButton btn = (ImageButton)sender;
                switch (btn.Id)
                {
                    case Resource.Id.button5: //select date
                        DatePickerDialog datepick = new DatePickerDialog(this.Context,AlertDialog.ThemeDeviceDefaultLight, OnDateSet, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                       
                        datepick.DatePicker.DateTime = DateTime.Today;
                        datepick.Show();
                        break;
                    case Resource.Id.buttonenr:     //enregistrer csv
                                                    //Création du fichier CSV
                        if (!string.IsNullOrEmpty(patient.Text) && !string.IsNullOrEmpty(gef.Text) && !string.IsNullOrEmpty(lot.Text) && !string.IsNullOrEmpty(quantite.Text) && !string.IsNullOrEmpty(date.Text))
                            CreateCSV(patient.Text, gef.Text, lot.Text, quantite.Text, date.Text,matricule.Text);

                        //Vide les champs d'entrée                       
                        quantite.Text = "";
                        lot.Text = "";
                        gef.Text = "";
                        patient.Text = "";
                        break;

                    case Resource.Id.buttonnext:  //suivant

                        if (!string.IsNullOrEmpty(patient.Text) && !string.IsNullOrEmpty(gef.Text) && !string.IsNullOrEmpty(lot.Text) && !string.IsNullOrEmpty(quantite.Text) && !string.IsNullOrEmpty(date.Text))
                            CreateCSV(patient.Text, gef.Text, lot.Text, quantite.Text, date.Text, matricule.Text);

                        //Vide les champs d'entrée sauf celui patient
                        quantite.Text = "";
                        lot.Text = "";
                        gef.Text = "";
                        //patient.Text = "";
                        break;

                    case Resource.Id.buttonhist:    //historique

                        Intent historiqueActivity = new Intent(this.Context, typeof(Historique));
                        StartActivity(historiqueActivity);
                        
                        Toast.MakeText(Application.Context, "Historique", ToastLength.Long);
                        break;
                }
            }
            
            return view;

            void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
            {
                date.Text = e.Date.ToLongDateString();
            }
        }
        string directory = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "Pharmastock";

        //Méthode de création du fichier CSV
        public void CreateCSV(string numpat, string codeGEF, string lotnum, string quant, string date,string matricule)
        {
            //Création d'un dossier
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                Toast.MakeText(Application.Context, "Dossier Pharmastock créé", ToastLength.Short).Show();
            }
            //Nom du fichier + Location

            //Ligne à ajouter lors de l'enregistrement. Reprend les entrées des champs EditText
            var newline = string.Format("{0};{1};{2};{3};{4};{5}", numpat, codeGEF, lotnum, quant, date,matricule);

            //Si le fichier n'existe pas, créer les entêtes et aller à la ligne. 
            if (!File.Exists(fileName))
            {
                string header = "Patient n° :" + ";" + "code GEF :" + ";" + "Lot n° :" + ";" + "Quantité :" + ";" + "Délivré le :" + "Matricule :";
                File.WriteAllText(fileName, header, Encoding.UTF8);       // Création de la ligne + Encodage pour les caractères spéciaux
                File.AppendAllText(fileName, System.Environment.NewLine); // Aller à la ligne
            }
            File.AppendAllText(fileName, newline + System.Environment.NewLine); // Ajout de la ligne contenant les champs
            Toast.MakeText(Application.Context, "Données enregistrées", ToastLength.Short).Show();
        }      
    }
}