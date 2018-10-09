using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
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

        //Chemin d'acc�s au fichier
        string fileName = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "Pharmastock" + Java.IO.File.Separator + "Pharmastock_" + DateTime.Now.ToString("ddMMyyy") + ".csv";
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fragment1, null);

            //cr�ation des variables des EditText de fragment1.axml
            EditText patient = view.FindViewById<EditText>(Resource.Id.numpat);
            EditText gef = view.FindViewById<EditText>(Resource.Id.codgef);
            EditText lot = view.FindViewById<EditText>(Resource.Id.numlot);
            EditText quantite = view.FindViewById<EditText>(Resource.Id.qtedel);
            EditText date = view.FindViewById<EditText>(Resource.Id.datedel);
            EditText matricule = new EditText(this.Context);

            quantite.Text = "1";
            //le matricule reste celui indiqu� en page de connexion
            matricule.Text = Settings.Username;

            //cr�ation des variables des ImageButton de fragment1.axml
            Button savebt = view.FindViewById<Button>(Resource.Id.buttonenr);
            Button historique = view.FindViewById<Button>(Resource.Id.buttonhist);
            ImageButton scan1 = view.FindViewById<ImageButton>(Resource.Id.button1);
            ImageButton scan2 = view.FindViewById<ImageButton>(Resource.Id.button2);
            ImageButton scan3 = view.FindViewById<ImageButton>(Resource.Id.button3);
            ImageButton settings = view.FindViewById<ImageButton>(Resource.Id.buttonsettings);
            Button suivant = view.FindViewById<Button>(Resource.Id.buttonnext);
            Button raz = view.FindViewById<Button>(Resource.Id.buttonreset);

            //Evenement d'acces aux pages
            //if (Settings.Adminstate == "admin")
            //{
            //    settings.Visibility = ViewStates.Visible;
            //}
            //else
            //{
                settings.Visibility = ViewStates.Invisible;
            //}

            date.Click += (s, e) =>
            {
                DatePickerDialog datepick = new DatePickerDialog(this.Context, AlertDialog.ThemeDeviceDefaultLight, OnDateSet, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                datepick.DatePicker.DateTime = DateTime.Today;
                datepick.Show();

            };
            //Evenements d'affichage du scanner lors des clics sur les boutons
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

            
            savebt.Click += Button_Click;       //Evenement bouton "Enregistrer"
            suivant.Click += Button_Click;      //Evenement bouton "Suivant"
            historique.Click += Button_Click;   //Evenement bouton "Historique"
            raz.Click += Button_Click;          //Evenement bouton "RAZ"

            //M�thode d'affichage dans les zones de texte par le biais du scanner
            async Task Scan(object s, EventArgs e)
            {
                ImageButton btn = (ImageButton)s;
                var toptext = "";
                switch (btn.Id) //Changer le toptext selon le bouton cliqu�
                {
                    case Resource.Id.button1:
                        toptext = "N� du patient";
                        break;
                    case Resource.Id.button2:
                        toptext = "Code GEF";
                        break;
                    case Resource.Id.button3:
                        toptext = "N� du lot";
                        break;
                }

                MobileBarcodeScanner.Initialize(Activity.Application);
                var options = new MobileBarcodeScanningOptions
                {
                    //Options de l'appareil photo : pas de rotation, pas de cam�ra frontale
                    AutoRotate = false,
                    UseFrontCameraIfAvailable = false,
                    TryHarder = true
                };
                scanner = new MobileBarcodeScanner()
                {
                    TopText = toptext //Valeur du toptext
                };

                //Cette variable attend un scan pour obtenir la valeur lue dans le code barre
                var result = await scanner.Scan(options);
                
                Android.Media.Stream str = Android.Media.Stream.Music;
                ToneGenerator tg = new ToneGenerator(str, 100);
                tg.StartTone(Tone.PropAck);

                if (result == null)
                {
                    return;
                }
                else
                {
                    switch (btn.Id) //Les champs de texte prennent la valeur lue par le scan
                    {
                        case Resource.Id.button1:
                            patient.Text = result.Text;
                            break;
                        case Resource.Id.button2:
                            gef.Text = result.Text;
                            break;
                        case Resource.Id.button3:
                            lot.Text = result.Text;
                            break;
                    }
                }

                return;
            }
            
            //M�thode dd fonction des boutons
            void Button_Click(object sender, EventArgs e)
            {
                Button btn = (Button)sender;
                switch (btn.Id)
                {
                    //// Affiche un calendrier en dialogue pour y s�lectionner la date
                    //case Resource.Id.button5:
                    //    DatePickerDialog datepick = new DatePickerDialog(this.Context, AlertDialog.ThemeDeviceDefaultLight, OnDateSet, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                    //    datepick.DatePicker.DateTime = DateTime.Today;
                    //    datepick.Show();
                    //    break;

                    //Enregistre les champs textes sous format CSV
                    case Resource.Id.buttonenr:
                        if (!string.IsNullOrEmpty(patient.Text) && !string.IsNullOrEmpty(gef.Text) && !string.IsNullOrEmpty(lot.Text) && !string.IsNullOrEmpty(quantite.Text) && !string.IsNullOrEmpty(date.Text) || quantite.Text != "0")
                        {
                            if (CreateCSV(patient.Text, gef.Text, lot.Text, quantite.Text, date.Text, matricule.Text))
                            {
                                Toast.MakeText(Application.Context, "Cette ligne existe d�j�", ToastLength.Short).Show();
                            }
                        }
                        else
                            Toast.MakeText(Application.Context, "Veuillez remplir les champs", ToastLength.Short).Show();

                        //Vide les champs d'entr�e                       
                        quantite.Text = "1";
                        lot.Text = "";
                        gef.Text = "";
                        patient.Text = "";
                        break;

                    //Enregistre les champs textes sous format CSV, mais garde le num�ro du patient
                    case Resource.Id.buttonnext:
                        if (!string.IsNullOrEmpty(patient.Text) && !string.IsNullOrEmpty(gef.Text) && !string.IsNullOrEmpty(lot.Text) && !string.IsNullOrEmpty(quantite.Text) && !string.IsNullOrEmpty(date.Text) || quantite.Text != "0")
                        {
                            if (CreateCSV(patient.Text, gef.Text, lot.Text, quantite.Text, date.Text, matricule.Text))
                            {
                                Toast.MakeText(Application.Context, "Cette ligne existe d�j�", ToastLength.Short).Show();
                            }
                        }
                        else
                            Toast.MakeText(Application.Context, "Veuillez remplir les champs", ToastLength.Short).Show();

                        //Vide les champs d'entr�e sauf celui patient
                        quantite.Text = "1";
                        lot.Text = "";
                        gef.Text = "";
                        gef.RequestFocus();
                        break;

                    //Affiche l'historique
                    case Resource.Id.buttonhist:
                        Intent historiqueActivity = new Intent(this.Context, typeof(Historique));
                        StartActivity(historiqueActivity);
                        break;

                    //Vide tous les champs textes selon la r�ponse � l'alerte
                    case Resource.Id.buttonreset:
                        if (!string.IsNullOrEmpty(patient.Text) || !string.IsNullOrEmpty(gef.Text) || !string.IsNullOrEmpty(lot.Text) || !string.IsNullOrEmpty(quantite.Text) || quantite.Text != "0" || !string.IsNullOrEmpty(date.Text))
                        {
                            Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this.Context);
                            AlertDialog alert = dialog.Create();
                            alert.SetTitle("Attention");
                            alert.SetMessage("Les champs de texte seront vid�s");
                            alert.SetButton("OK", (c, ev) =>
                            {
                                //Vide les champs
                                date.Text = "";
                                quantite.Text = "";
                                lot.Text = "";
                                gef.Text = "";
                                patient.Text = "";
                                Toast.MakeText(Application.Context, "Champs r�initialis�s", ToastLength.Short).Show();
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

            return view;

            //La valeur du champ texte date est la valeur choisie sur le calendrier
            void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
            {
                date.Text = e.Date.ToLongDateString();
            }
        }
        //Nom du dossier + chemin
        string directory = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "Pharmastock";

        //M�thode de cr�ation du fichier CSV
        public bool CreateCSV(string numpat, string codeGEF, string lotnum, string quant, string date, string matricule)
        {
            //Cr�ation d'un dossier
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                Toast.MakeText(Application.Context, "Dossier Pharmastock cr�� � la racine du stockage", ToastLength.Short).Show();
            }

            //Lignes � ajouter lors de l'enregistrement. Reprend les entr�es des champs EditText
            var newline = string.Format("{0};{1};{2};{3};{4};{5}", numpat, codeGEF, lotnum, quant, date, matricule);
            var geftext = string.Format(codeGEF);
            var lot = string.Format(lotnum);
            var patient = string.Format(numpat);

            //Si le fichier n'existe pas, cr�er les ent�tes et aller � la ligne. 
            if (!File.Exists(fileName))
            {
                string header = "Patient n� :" + ";" + "code GEF :" + ";" + "Lot n� :" + ";" + "Quantit� :" + ";" + "D�livr� le :" + ";" + "Matricule :";
                File.WriteAllText(fileName, header, System.Text.Encoding.UTF8);       // Cr�ation de la ligne + Encodage pour les caract�res sp�ciaux
                File.AppendAllText(fileName, System.Environment.NewLine); // Aller � la ligne
                Toast.MakeText(Application.Context, "Nouveau fichier cr�� pour la date du jour", ToastLength.Short).Show();
            }

            //Cr�ation d'un tableau qui �vite les doublons
            string[] lines = File.ReadLines(fileName).ToArray<string>();
            List<string> listItems = new List<string>();
            for (int i = 1; i < lines.Length; i++)
            {
                listItems = lines[i].Split(';').ToList();
                //Si la condition retourne true reprend le la cr�ation du csv a partir du code GEF en gardant le code patient
                if (listItems[0] == patient && listItems[1] == geftext && listItems[2] == lot)
                {
                    return true;
                }

            }
            File.AppendAllText(fileName, newline + System.Environment.NewLine); // Ajout de la ligne contenant les champs
            Toast.MakeText(Application.Context, "Donn�es enregistr�es", ToastLength.Short).Show();
            return false;
        }


    }
}
