using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using System;
using Android.Util;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using Android.Content;
using Android.Content.PM;





namespace PharamaStock
{
    [Activity(Label = "PharamaStock", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        //TextView _dateDisplay;
        //Button _dateSelectButton;
        string fileName = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "Pharmastock_" + DateTime.Now.ToString("ddMMyyy") + ".csv";



        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());

            // Set our view from the "main" layout resource
            //SetContentView(Resource.Layout.activity_main);
            LinearLayout view = new LinearLayout(this)
            

            {
                Orientation = Orientation.Vertical
            };

            this.SetContentView(view);


            //affiche le numéro du patient

            TextView numPatient = new TextView(this)
            {
                Text = "Patient n° : "
                
            };
            view.AddView(numPatient);
            EditText patient = new EditText(this)
            {
                InputType = Android.Text.InputTypes.ClassNumber
            };
            patient.SetSingleLine(true);
            view.AddView(patient);
            this.SetContentView(view);

            //affiche le coe gef
            TextView codeGef = new TextView(this)
            {
                Text = "Code GEF : "
            };
            view.AddView(codeGef);
            EditText gef = new EditText(this)
            {
                InputType = Android.Text.InputTypes.ClassNumber

            };
            gef.SetSingleLine(true);
            view.AddView(gef);
            this.SetContentView(view);

            //affiche le numéro du lot
            TextView numLot = new TextView(this)
            {
                Text = "Lot n° : "
            };
            view.AddView(numLot);
            EditText lot = new EditText(this)
            {
                 InputType = Android.Text.InputTypes.ClassNumber

             };
            lot.SetSingleLine(true);
            view.AddView(lot);
            this.SetContentView(view);

            //affiche la quantité délivrée
            TextView quantiteDelivree = new TextView(this)
            {
                Text = "Quantité : "
            };
            view.AddView(quantiteDelivree);
            EditText quantite = new EditText(this)
            {
                 InputType = Android.Text.InputTypes.ClassNumber

             };
            quantite.SetSingleLine(true);
            view.AddView(quantite);
            this.SetContentView(view);

            //affiche la date de délivrence à la date du jour
            TextView date_display = new TextView(this)
            {
                Text = "Délivré le : "

            };
            view.AddView(date_display);
            TextView date = new TextView(this)
            {
                Text = DateTime.Now.ToLongDateString(),


            };
            view.AddView(date);
            DatePicker datepick = new DatePicker(this)
            {
                Visibility = Android.Views.ViewStates.Gone
            };
            view.AddView(datepick);

            date.Click += (s, e) =>
            {
                datepick.Visibility = Android.Views.ViewStates.Visible;
            };
            
            datepick.DateChanged += (s, e) =>
            {
                date.Text = datepick.DateTime.ToLongDateString();
                datepick.Visibility = Android.Views.ViewStates.Gone;
            };

            //enregistre les données récoltées dans un fichier 
            Button Enregistrer = new Button(this)
            {
                Text = "Enregistrer"

            };
            view.AddView(Enregistrer);

            Enregistrer.Click += (s, e) =>
            {
                //Création du fichier CSV
                if(!string.IsNullOrEmpty(patient.Text) && !string.IsNullOrEmpty(gef.Text) && !string.IsNullOrEmpty(lot.Text) && !string.IsNullOrEmpty(quantite.Text) && !string.IsNullOrEmpty(date.Text))
                CreateCSV(patient.Text, gef.Text, lot.Text, quantite.Text, date.Text);

                //Vide les champs d'entrée
                quantite.Text = "";
                lot.Text = "";
                gef.Text = "";
                patient.Text = "";

            };
            this.SetContentView(view);

            //envoie la liste par email
            Button Envoyer = new Button(this)
            {
                Text = "Envoyer"
            };
            Envoyer.Click += (s, e) =>
            {
                 

                try
                {
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com",587);
                    mail.From = new MailAddress("jolyrudypro@gmail.com");
                    mail.To.Add("jolyrudy@msn.com");
                    mail.Subject = "Document CSV";
                    mail.Body = "Veuillez trouver ci joint le document récapitalif de la journée";
                    System.Net.Mail.Attachment pj;
                    pj = new Attachment(fileName);
                    mail.Attachments.Add(pj);
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("jolyrudypro@gmail.com", "joru59120");
                    SmtpServer.EnableSsl = true;
                    ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
                    {
                        return true;
                    };
                    SmtpServer.Send(mail);
                    Toast.MakeText(Application.Context, "Mail envoyé", ToastLength.Short).Show();
                }
                catch (Exception ex)
                {

                    Toast.MakeText(Application.Context, ex.ToString(), ToastLength.Long);
                }

                   
               
            };
            view.AddView(Envoyer);
            this.SetContentView(view);

            //Affiche l'historique de l'ensemble des fichiers
            Button Historique = new Button(this)
            {
                Text = "Historique"

            };
            view.AddView(Historique);


            TextView fichierstxt = new TextView(this)
            {
                Text = ""
            };
            view.AddView(fichierstxt);
            this.SetContentView(view);


            Historique.Click += (s, e) =>
            {
                Intent historiqueActivity = new Intent(this, typeof(Historique));
                StartActivity(historiqueActivity);

            };



            Button scanButton = new Button(this)
            {
                Text = "Scanner"
            };
            view.AddView(scanButton);
            scanButton.Click += async(sender, args) =>
            {
               

            };


        }
        string directory = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "Pharmastock";
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


        //Méthode de création du fichier CSV
        public void CreateCSV(string numpat, string codeGEF, string lotnum, string quant, string date)
        {

            //Création d'un dossier
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                Toast.MakeText(Application.Context, "Dossier Pharmastock créé", ToastLength.Short).Show();

            }
            //Nom du fichier + Location
            string fileName = directory + Java.IO.File.Separator + "Pharmastock_" +DateTime.Now.ToString("ddMMyyy") + ".csv";

            //Ligne à ajouter lors de l'enregistrement. Reprend les entrées des champs EditText
            var newline = string.Format("{0};{1};{2};{3};{4}", numpat, codeGEF, lotnum, quant, date);

            //Si le fichier n'existe pas, créer les entêtes et aller à la ligne. 
            if (!File.Exists(fileName))
            {
                string header = "Patient n° :" + ";" + "code GEF :" + ";" + "Lot n° :" + ";" + "Quantité :" + ";" + "Délivré le :";
                File.WriteAllText(fileName, header, Encoding.UTF8);       // Création de la ligne + Encodage pour les caractères spéciaux
                File.AppendAllText(fileName, System.Environment.NewLine); // Aller à la ligne
            }
            File.AppendAllText(fileName, newline + System.Environment.NewLine); // Ajout de la ligne contenant les champs
            Toast.MakeText(Application.Context, "Données enregistrées", ToastLength.Short).Show();
        }


        public class DatePickerFragment : DialogFragment,
                                  DatePickerDialog.IOnDateSetListener
        {
            // TAG can be any string of your choice.
            public static readonly string TAG = "X:" + typeof(DatePickerFragment).Name.ToUpper();

            // Initialize this value to prevent NullReferenceExceptions.
            Action<DateTime> _dateSelectedHandler = delegate { };

            public static DatePickerFragment NewInstance(Action<DateTime> onDateSelected)
            {
                DatePickerFragment frag = new DatePickerFragment();
                frag._dateSelectedHandler = onDateSelected;
                return frag;
            }

            public override Dialog OnCreateDialog(Bundle savedInstanceState)
            {
                DateTime currently = DateTime.Now;
                DatePickerDialog dialog = new DatePickerDialog(Activity,
                                                               this,
                                                               currently.Year,
                                                               currently.Month - 1,
                                                               currently.Day);
                return dialog;
            }

            public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
            {
                // Note: monthOfYear is a value between 0 and 11, not 1 and 12!
                DateTime selectedDate = new DateTime(year, monthOfYear + 1, dayOfMonth);
                Log.Debug(TAG, selectedDate.ToLongDateString());
                _dateSelectedHandler(selectedDate);
            }
        }
        





    }
}