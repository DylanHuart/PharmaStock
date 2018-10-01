using Android.OS;
using Android.Views;
using Android.Widget;
using ZXing.Mobile;
using Android.App;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Android.Support.Design.Widget;

namespace PharmaTab.Fragments
{
    /// <summary>
    /// Mode automatique:
    /// Cette partie va permettre d'enregistrer les informations
    /// relev�es par scanner automatiquement
    /// </summary>
    public class Fragment2 : Android.Support.V4.App.Fragment
    {
        string toptext = "";
        string fileName = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "Pharmastock" + Java.IO.File.Separator + "Pharmastock_" + DateTime.Now.ToString("ddMMyyy") + ".csv";

        TextView numpat = new TextView(Application.Context);
        TextView gef = new TextView(Application.Context);
        TextView qte = new TextView(Application.Context);
        TextView lot = new TextView(Application.Context);

        public static Fragment2 NewInstance()
        {
            var frag2 = new Fragment2 { Arguments = new Bundle() };
            return frag2;
        }
        
        public override void OnStart()
        {
            base.OnStart();
            var tabs = Activity.FindViewById<TabLayout>(Resource.Id.tabs);

            tabs.TabSelected += async (s, e) =>
            {              
                var tab = e.Tab;
                var text =tab.Text;

                if(text == "Auto")
                {
                    toptext = "N� du patient";
                    await Scan();
                }
            };

            numpat.AfterTextChanged += async (s, e) =>
            {
                toptext = "Code GEF";
                await Scan();
            };

            gef.AfterTextChanged += async (s, e) =>
            {
                toptext = "Quantit� d�livr�e";
                await Scan();
            };

            qte.AfterTextChanged += async (s, e) =>
            {
                toptext = "N� du lot";
                await Scan();
            };

            lot.TextChanged += (s, e) =>
            {
                if (!string.IsNullOrEmpty(numpat.Text) && !string.IsNullOrEmpty(gef.Text) && !string.IsNullOrEmpty(lot.Text) && !string.IsNullOrEmpty(qte.Text))
                    CreateCSV(numpat.Text, gef.Text, lot.Text, qte.Text, DateTime.Now.Date.ToString("dd/MM/yyyy"), Settings.Username);

                numpat.Text = numpat.Text; 
               
            };
        }

        string directory = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "Pharmastock";

        //M�thode de cr�ation du fichier CSV
        public void CreateCSV(string numpat, string codeGEF, string lotnum, string quant, string date, string matricule)
        {
            //Cr�ation d'un dossier
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                Toast.MakeText(Application.Context, "Dossier Pharmastock cr��", ToastLength.Short).Show();
            }
            //Nom du fichier + Location

            //Ligne � ajouter lors de l'enregistrement. Reprend les entr�es des champs EditText
            var newline = string.Format("{0};{1};{2};{3};{4};{5}", numpat, codeGEF, lotnum, quant, date, matricule);

            //Si le fichier n'existe pas, cr�er les ent�tes et aller � la ligne. 
            if (!File.Exists(fileName))
            {
                string header = "Patient n� :" + ";" + "code GEF :" + ";" + "Lot n� :" + ";" + "Quantit� :" + ";" + "D�livr� le :" + "Matricule :";
                File.WriteAllText(fileName, header, Encoding.UTF8);       // Cr�ation de la ligne + Encodage pour les caract�res sp�ciaux
                File.AppendAllText(fileName, System.Environment.NewLine); // Aller � la ligne
            }
            File.AppendAllText(fileName, newline + System.Environment.NewLine); // Ajout de la ligne contenant les champs
            Toast.MakeText(Application.Context, "Donn�es enregistr�es", ToastLength.Short).Show();
        }

        async Task Scan()
        {
            MobileBarcodeScanner scanner;
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
                switch (toptext)
                {
                    case "N� du patient":
                        numpat.Text = result.Text;
                        break;
                    case "Code GEF":
                        gef.Text = result.Text;
                        break;
                    case "Quantit� d�livr�e":
                        qte.Text = result.Text;
                        break;
                    case "N� du lot":
                        lot.Text = result.Text;
                        break;
                }
            }
            return;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fragment2, null);
            
            return view;
        }
    }
}