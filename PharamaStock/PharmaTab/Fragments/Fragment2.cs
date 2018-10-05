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
    /// relevées par scanner automatiquement
    /// </summary>
    public class Fragment2 : Android.Support.V4.App.Fragment
    {
        //Variables
        string toptext = "";

        //Chemin d'accès au fichier
        string fileName = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "Pharmastock" + Java.IO.File.Separator + "Pharmastock_" + DateTime.Now.ToString("ddMMyyy") + ".csv";
        string directory = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "Pharmastock";

        Task<string> task;

        EditText patient = new EditText(Application.Context);
        EditText gef = new EditText(Application.Context);
        EditText lot = new EditText(Application.Context);
        EditText quantite = new EditText(Application.Context);
        EditText date = new EditText(Application.Context);
        EditText matricule = new EditText(Application.Context);

        //création des variables des ImageButton de fragment1.axml
        ImageButton suivant = new ImageButton(Application.Context);
        ImageButton savebt = new ImageButton(Application.Context);
        ImageButton raz = new ImageButton(Application.Context);
        ImageButton plusLot = new ImageButton(Application.Context);

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

            //création des variables des ImageButton de fragment2.axml
            suivant = view.FindViewById<ImageButton>(Resource.Id.buttonnext2);
            savebt = view.FindViewById<ImageButton>(Resource.Id.buttonenr2);
            raz = view.FindViewById<ImageButton>(Resource.Id.buttonreset);
            //création des variables des EditText de fragment2.axml
            patient = view.FindViewById<EditText>(Resource.Id.numpat2);
            gef = view.FindViewById<EditText>(Resource.Id.codgef2);
            lot = view.FindViewById<EditText>(Resource.Id.numlot2);
            plusLot = view.FindViewById<ImageButton>(Resource.Id.buttonplusLot);
            quantite = view.FindViewById<EditText>(Resource.Id.qtedel2);
            date = view.FindViewById<EditText>(Resource.Id.datedel2);
            matricule = new EditText(this.Context);
            matricule.Text = Settings.Username;
            quantite.Text = "1";
           

            savebt.Click += async (s, e) =>
            {
                if (!string.IsNullOrEmpty(patient.Text) && !string.IsNullOrEmpty(gef.Text) && !string.IsNullOrEmpty(lot.Text) && !string.IsNullOrEmpty(quantite.Text) && !string.IsNullOrEmpty(date.Text) || quantite.Text != "0")
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

                toptext = "N° du patient";
                task = Scan();
                patient.Text = await task;
            };
            //Méthode raz
            raz.Click += (s, e) =>
            {
                if (!string.IsNullOrEmpty(patient.Text) || !string.IsNullOrEmpty(gef.Text) || !string.IsNullOrEmpty(lot.Text) || !string.IsNullOrEmpty(quantite.Text) || quantite.Text != "0" || !string.IsNullOrEmpty(date.Text))
                {
                    Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this.Context);
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

            plusLot.Click += async (s, e) =>
            {
                if (!string.IsNullOrEmpty(patient.Text) && !string.IsNullOrEmpty(gef.Text) && !string.IsNullOrEmpty(lot.Text) && !string.IsNullOrEmpty(quantite.Text) && !string.IsNullOrEmpty(date.Text) || quantite.Text != "0")
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

                toptext = "Numéro de lot";
                Task<string> task = Scan();
                lot.Text = await task;
            };

            //Méthode suivant
            suivant.Click += async (s, e) =>
            {
                if (!string.IsNullOrEmpty(patient.Text) && !string.IsNullOrEmpty(gef.Text) && !string.IsNullOrEmpty(lot.Text) && !string.IsNullOrEmpty(quantite.Text) && !string.IsNullOrEmpty(date.Text) || quantite.Text != "0")
                {
                    if (CreateCSV(patient.Text, gef.Text, lot.Text, quantite.Text, date.Text, matricule.Text))
                    {
                        Toast.MakeText(Application.Context, "Cette ligne existe déjà", ToastLength.Short).Show();
                        


                    }
                }
                else
                    Toast.MakeText(Application.Context, "Veuillez remplir les champs", ToastLength.Short).Show();
                //Vide les champs d'entrée sauf celui patient
                quantite.Text = "1";

                toptext = "Code GEF";
                Task<string> task = Scan();
                gef.Text = await task;
            };

            //Méthode selection date de délivrance
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
            //Méthode de sélection en mode automatique
            tabs.TabSelected += async (s, e) =>
            {

               var tab = e.Tab;
               var text = tab.Text;
               if (text == "Auto")
               {
                   toptext = "N° du patient";
                   
                   task = Scan();
                   patient.Text = await task;
                }
             };
            //lecture code barre suivant aprés écriture du code barre précèdent
            patient.AfterTextChanged += async (s, e) =>
            {
                //Vide les champs d'entrée sauf celui patient
                toptext = "Code GEF";
                task = Scan();
                gef.Text = await task;
            };


            gef.AfterTextChanged += async (s, e) =>
            {
                toptext = "Numéro de lot";
                task = Scan();
                lot.Text = await task;
            };
            
            return view;
        }

        //Méthode de création du fichier CSV
        public bool CreateCSV(string numpat, string codeGEF, string lotnum, string quant, string date, string matricule)
        {
            //Création d'un dossier
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                Toast.MakeText(Application.Context, "Dossier Pharmastock créé à la racine du stockage", ToastLength.Short).Show();
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
                File.WriteAllText(fileName, header, Encoding.UTF8);       // Création de la ligne + Encodage pour les caractères spéciaux
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

        //Méthode de création du scanner
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