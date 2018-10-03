using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ZXing.Mobile;

namespace PharmaTab.Fragments
{
    /// <summary>
    /// Mode automatique:
    /// Cette partie va permettre d'enregistrer les informations
    /// relevées par scanner automatiquement
    /// </summary>
    public class Fragment2 : Android.Support.V4.App.Fragment
    {
        string toptext = "";
        string fileName = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "Pharmastock" + Java.IO.File.Separator + "Pharmastock_" + DateTime.Now.ToString("ddMMyyy") + ".csv";

        EditText patient = new EditText(Application.Context);
        EditText gef = new EditText(Application.Context);
        EditText lot = new EditText(Application.Context);
        EditText quantite = new EditText(Application.Context);
        EditText date = new EditText(Application.Context);
        EditText matricule = new EditText(Application.Context);

        ImageButton suivant = new ImageButton(Application.Context);
        ImageButton savebt = new ImageButton(Application.Context);
        ImageButton raz = new ImageButton(Application.Context); 
        public static Fragment2 NewInstance()
        {
            var frag2 = new Fragment2 { Arguments = new Bundle() };
            return frag2;
        }

        string directory = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "Pharmastock";

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fragment2, null);

             suivant = view.FindViewById<ImageButton>(Resource.Id.buttonnext2);
             savebt = view.FindViewById<ImageButton>(Resource.Id.buttonenr2);
            raz = view.FindViewById<ImageButton>(Resource.Id.buttonreset);

            patient = view.FindViewById<EditText>(Resource.Id.numpat2);
            gef = view.FindViewById<EditText>(Resource.Id.codgef2);
            lot = view.FindViewById<EditText>(Resource.Id.numlot2);
            quantite = view.FindViewById<EditText>(Resource.Id.qtedel2);
            date = view.FindViewById<EditText>(Resource.Id.datedel2);
            matricule = new EditText(this.Context);
            matricule.Text = Settings.Username;

            var tabs = Activity.FindViewById<TabLayout>(Resource.Id.tabs);

            savebt.Click += async (s, e) =>
            {
                if (!string.IsNullOrEmpty(patient.Text) && !string.IsNullOrEmpty(gef.Text) && !string.IsNullOrEmpty(lot.Text) && !string.IsNullOrEmpty(quantite.Text) && !string.IsNullOrEmpty(date.Text))
                    CreateCSV(patient.Text, gef.Text, lot.Text, quantite.Text, date.Text, matricule.Text);
                else
                    Toast.MakeText(Application.Context, "Veuillez remplir les champs", ToastLength.Short).Show();

                //Vide les champs d'entrée                       
                quantite.Text = "";
                lot.Text = "";
                gef.Text = "";
                patient.Text = "";

                toptext = "N° du patient";
                Task<string> task = Scan();
                patient.Text = await task;
            };

            suivant.Click += async (s, e) =>
            {
                if (!string.IsNullOrEmpty(patient.Text) && !string.IsNullOrEmpty(gef.Text) && !string.IsNullOrEmpty(lot.Text) && !string.IsNullOrEmpty(quantite.Text) && !string.IsNullOrEmpty(date.Text))
                    CreateCSV(patient.Text, gef.Text, lot.Text, quantite.Text, date.Text, matricule.Text);
                else
                    Toast.MakeText(Application.Context, "Veuillez remplir les champs", ToastLength.Short).Show();

                //Vide les champs d'entrée sauf celui patient
                quantite.Text = "";
                lot.Text = "";
                gef.Text = "";

                toptext = "N° du patient";
                Task<string> task = Scan();
                patient.Text = await task;
            };

            date.Click += (s, e) =>
            {
                DatePickerDialog datepick = new DatePickerDialog(this.Context, AlertDialog.ThemeDeviceDefaultLight, OnDateSet, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                datepick.DatePicker.DateTime = DateTime.Today;
                datepick.Show();
            };

            void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
            {
                date.Text = e.Date.ToLongDateString();
            }

            tabs.TabSelected += async (s, e) =>
            {

               var tab = e.Tab;
               var text = tab.Text;
               if (text == "Auto")
               {
                   toptext = "N° du patient";
                   Task<string> task = Scan();
                   patient.Text = await task;
                }
             };

            patient.AfterTextChanged += async (s, e) =>
            {
                toptext = "Code GEF";
                Task<string> task = Scan();
                gef.Text = await task;
            };


            gef.AfterTextChanged += async (s, e) =>
            {
                toptext = "Numéro de lot";
                Task<string> task = Scan();
                lot.Text = await task;
            };

            lot.TextChanged += (s, e) =>
            {
                    if (!string.IsNullOrEmpty(patient.Text) && !string.IsNullOrEmpty(gef.Text) && !string.IsNullOrEmpty(lot.Text) && !string.IsNullOrEmpty(quantite.Text))
                        CreateCSV(patient.Text, gef.Text, lot.Text, quantite.Text, DateTime.Now.Date.ToString("dd/MM/yyyy"), Settings.Username);
            };

            return view;
        }


        //string directory = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "Pharmastock";

        //Méthode de création du fichier CSV
        public void CreateCSV(string numpat, string codeGEF, string lotnum, string quant, string date, string matricule)
        {
            //Création d'un dossier
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                Toast.MakeText(Application.Context, "Dossier Pharmastock créé", ToastLength.Short).Show();
            }

            //Ligne à ajouter lors de l'enregistrement. Reprend les entrées des champs EditText
            var newline = string.Format("{0};{1};{2};{3};{4};{5}", numpat, codeGEF, lotnum, quant, date, matricule);

            //Si le fichier n'existe pas, créer les entêtes et aller à la ligne. 
            if (!File.Exists(fileName))
            {
                string header = "Patient n° :" + ";" + "code GEF :" + ";" + "Lot n° :" + ";" + "Quantité :" + ";" + "Délivré le :" + ";" + "Matricule :";
                File.WriteAllText(fileName, header, Encoding.UTF8);       // Création de la ligne + Encodage pour les caractères spéciaux
                File.AppendAllText(fileName, System.Environment.NewLine); // Aller à la ligne
            }
            File.AppendAllText(fileName, newline + System.Environment.NewLine); // Ajout de la ligne contenant les champs
            Toast.MakeText(Application.Context, "Données enregistrées", ToastLength.Short).Show();
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
                CancelButtonText = "Annuler",
                FlashButtonText = "Flash",
                TopText = toptext
            };

            var result = await scanner.Scan(options);

            if (result == null)
            {
                return "";
            }

            return result.Text;
        }
    }
}