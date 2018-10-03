using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing.Mobile;

namespace PharmaTab.Fragments
{
    /// <summary>
    /// Mode automatique:
    /// Cette partie va permettre d'enregistrer les informations
    /// relev�es par scanner automatiquement
    /// </summary>
    public class Fragment2 : Android.Support.V4.App.Fragment
    {
        //Variables
        string toptext = "";
        string resultscan = "";

        //Chemin d'acc�s au fichier
        string fileName = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "Pharmastock" + Java.IO.File.Separator + "Pharmastock_" + DateTime.Now.ToString("ddMMyyy") + ".csv";
        string directory = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "Pharmastock";

        //cr�ation des variables des EditText de fragment1.axml
        EditText patient = new EditText(Application.Context);
        EditText gef = new EditText(Application.Context);
        EditText lot = new EditText(Application.Context);
        EditText quantite = new EditText(Application.Context);
        EditText date = new EditText(Application.Context);
        EditText matricule = new EditText(Application.Context);

        //cr�ation des variables des ImageButton de fragment1.axml
        ImageButton suivant = new ImageButton(Application.Context);
        ImageButton savebt = new ImageButton(Application.Context);
        ImageButton raz = new ImageButton(Application.Context); 
        
        //On instancie le fragment 2
        public static Fragment2 NewInstance()
        {
            var frag2 = new Fragment2 { Arguments = new Bundle() };
            return frag2;
        }
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fragment2, null);
            var tabs = Activity.FindViewById<TabLayout>(Resource.Id.tabs);

            matricule = new EditText(this.Context);
            matricule.Text = Settings.Username;

            //cr�ation des variables des ImageButton de fragment2.axml
            suivant = view.FindViewById<ImageButton>(Resource.Id.buttonnext2);
            savebt = view.FindViewById<ImageButton>(Resource.Id.buttonenr2);
            raz = view.FindViewById<ImageButton>(Resource.Id.buttonreset);
            //cr�ation des variables des EditText de fragment2.axml
            patient = view.FindViewById<EditText>(Resource.Id.numpat2);
            gef = view.FindViewById<EditText>(Resource.Id.codgef2);
            lot = view.FindViewById<EditText>(Resource.Id.numlot2);
            quantite = view.FindViewById<EditText>(Resource.Id.qtedel2);
            date = view.FindViewById<EditText>(Resource.Id.datedel2);
        
            //M�thode enregistrer
            savebt.Click += async (s, e) =>
            {
                if (!string.IsNullOrEmpty(patient.Text) && !string.IsNullOrEmpty(gef.Text) && !string.IsNullOrEmpty(lot.Text) && !string.IsNullOrEmpty(quantite.Text) && !string.IsNullOrEmpty(date.Text))
                    CreateCSV(patient.Text, gef.Text, lot.Text, quantite.Text, date.Text, matricule.Text);
                else
                    Toast.MakeText(Application.Context, "Veuillez remplir les champs", ToastLength.Short).Show();

                //Vide les champs d'entr�e                       
                quantite.Text = "";
                lot.Text = "";
                gef.Text = "";
                patient.Text = "";

                toptext = "N� du patient";
                Task<string> task = Scan();
                patient.Text = await task;
            };

            //M�thode suivant
            suivant.Click += async (s, e) =>
            {
                if (!string.IsNullOrEmpty(patient.Text) && !string.IsNullOrEmpty(gef.Text) && !string.IsNullOrEmpty(lot.Text) && !string.IsNullOrEmpty(quantite.Text) && !string.IsNullOrEmpty(date.Text))
                    CreateCSV(patient.Text, gef.Text, lot.Text, quantite.Text, date.Text, matricule.Text);
                else
                    Toast.MakeText(Application.Context, "Veuillez remplir les champs", ToastLength.Short).Show();

                //Vide les champs d'entr�e sauf celui patient
                quantite.Text = "";
                lot.Text = "";
                gef.Text = "";

                toptext = "N� du patient";
                Task<string> task = Scan();
                patient.Text = await task;
            };

           //M�thode selection date de d�livrance
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
            //M�thode de s�lection en mode automatique
            tabs.TabSelected += async (s, e) =>
            {

               var tab = e.Tab;
               var text = tab.Text;
               if (text == "Auto")
               {
                   toptext = "N� du patient";
                   Task<string> task = Scan();
                   patient.Text = await task;
                }
             };
            //lecture code barre suivant apr�s �criture du code barre pr�c�dent
            patient.AfterTextChanged += async (s, e) =>
            {
                toptext = "Code GEF";
                Task<string> task = Scan();
                gef.Text = await task;
            };


            gef.AfterTextChanged += async (s, e) =>
            {
                toptext = "Num�ro de lot";
                Task<string> task = Scan();
                lot.Text = await task;
            };
            //Enregistrement sur csv apr�s derni�re lecture
            //lot.TextChanged += (s, e) =>
            //{
            //        if (!string.IsNullOrEmpty(patient.Text) && !string.IsNullOrEmpty(gef.Text) && !string.IsNullOrEmpty(lot.Text) && !string.IsNullOrEmpty(quantite.Text))
            //            CreateCSV(patient.Text, gef.Text, lot.Text, quantite.Text, DateTime.Now.Date.ToString("dd/MM/yyyy"), Settings.Username);
            //};

            return view;
        }

        //M�thode de cr�ation du fichier CSV
        public void CreateCSV(string numpat, string codeGEF, string lotnum, string quant, string date, string matricule)
        {
            //Cr�ation d'un dossier
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                Toast.MakeText(Application.Context, "Dossier Pharmastock cr�� � la racine du stockage", ToastLength.Short).Show();
            }

            //Ligne � ajouter lors de l'enregistrement. Reprend les entr�es des champs EditText
            var newline = string.Format("{0};{1};{2};{3};{4};{5}", numpat, codeGEF, lotnum, quant, date, matricule);
            var geftext = string.Format(codeGEF);
            var lot = string.Format(lotnum);
            var patient = string.Format(numpat);
            bool newlinetrue = true;

            //Si le fichier n'existe pas, cr�er les ent�tes et aller � la ligne. 
            if (!File.Exists(fileName))
            {
                string header = "Patient n� :" + ";" + "code GEF :" + ";" + "Lot n� :" + ";" + "Quantit� :" + ";" + "D�livr� le :" + ";" + "Matricule :";
                File.WriteAllText(fileName, header, Encoding.UTF8);       // Cr�ation de la ligne + Encodage pour les caract�res sp�ciaux
                File.AppendAllText(fileName, System.Environment.NewLine); // Aller � la ligne
                Toast.MakeText(Application.Context, "Nouveau fichier cr�� pour la date du jour", ToastLength.Short).Show();
            }

            //Cr�ation d'un tableau qui �vite les doublons
            string[] lines = File.ReadLines(fileName).ToArray<string>();
            for (int i = 1; i < lines.Length; i++)
            {
                List<string> listItems = lines[i].Split(';').ToList();


                if (listItems[0] == patient && listItems[1] == geftext && listItems[2] == lot)
                {
                    Toast.MakeText(Application.Context, "Cette ligne existe d�j�", ToastLength.Short).Show();
                    newlinetrue = false;


                    //Vide les champs d'entr�e sauf celui patient
                    toptext = "Code GEF";
                    Task<string> task = Scan();
                    gef.Text = task.Result;
                    break;
                }
            }
            if (newlinetrue)
            {
                File.AppendAllText(fileName, newline + System.Environment.NewLine); // Ajout de la ligne contenant les champs
                Toast.MakeText(Application.Context, "Donn�es enregistr�es", ToastLength.Short).Show();
            }
        }

        //M�thode de cr�ation du scanner
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