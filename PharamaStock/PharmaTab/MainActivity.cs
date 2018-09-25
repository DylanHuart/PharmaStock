using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using PharmaTab.Fragments;

using Android.Support.Design.Widget;
using Android.Support.V7.App;
using System.Text;
using System.IO;
using Android.Util;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Android.Content.PM;
using Android;
using Android.Support.V4.Content;

namespace PharmaTab
{
    [Activity(Label = "@string/app_name", MainLauncher = true, LaunchMode = Android.Content.PM.LaunchMode.SingleTop, Icon = "@drawable/icon")]

  
    public class MainActivity : AppCompatActivity
    {

        string fileName = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "PharmaStock" + Java.IO.File.Separator + "Pharmastock_" + DateTime.Now.ToString("ddMMyyy") + ".csv";
        ViewPager pager;
        TabsAdapter adapter;

        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);
            SetContentView(Resource.Layout.main);
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            if (toolbar != null)
            {
                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(false);
                SupportActionBar.SetHomeButtonEnabled(false);
            }

            // Afficher une boîte de dialogue pour accorder l'autorisation
            var permission = Manifest.Permission.WriteExternalStorage;
            if (ContextCompat.CheckSelfPermission(this, permission) != Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.WriteExternalStorage }, 0);
                Toast.MakeText(Application.Context, "Permission accordée", ToastLength.Short).Show();

            }

            adapter = new TabsAdapter(this, SupportFragmentManager);
            pager = FindViewById<ViewPager>(Resource.Id.pager);
            var tabs = FindViewById<TabLayout>(Resource.Id.tabs);
            pager.Adapter = adapter;
            
            tabs.SetupWithViewPager(pager);
            pager.OffscreenPageLimit = 3;


            SetContentView(Resource.Layout.fragment1);

            EditText patient = FindViewById<EditText>(Resource.Id.numpat);
            EditText gef = FindViewById<EditText>(Resource.Id.codgef);
            EditText lot = FindViewById<EditText>(Resource.Id.numlot);
            EditText quantite = FindViewById<EditText>(Resource.Id.qtedel);
            EditText date = FindViewById<EditText>(Resource.Id.datedel);
           // DatePicker datepick = FindViewById<DatePicker>(Resource.Id.datePicker1);
            Button savebt = FindViewById<Button>(Resource.Id.buttonenr);
            Button selectdate = FindViewById<Button>(Resource.Id.button5);
            Button send = FindViewById<Button>(Resource.Id.buttonenv);
            Button historique = FindViewById<Button>(Resource.Id.buttonhist);

            //datepick.Visibility = Android.Views.ViewStates.Gone;

            selectdate.Click += Button_Click;
            savebt.Click += Button_Click;
            send.Click += Button_Click;
            historique.Click += Button_Click;

            void Button_Click(object sender, EventArgs e)
            {
                Button btn = (Button)sender;
                switch (btn.Id)
                {
                    case Resource.Id.button5: //select date
                        //datepick.Visibility = ViewStates.Visible;
                        break;
                    case Resource.Id.buttonenr:     //enregistrer csv
                                                    //Création du fichier CSV
                        if (!string.IsNullOrEmpty(patient.Text) && !string.IsNullOrEmpty(gef.Text) && !string.IsNullOrEmpty(lot.Text) && !string.IsNullOrEmpty(quantite.Text) && !string.IsNullOrEmpty(date.Text))
                            CreateCSV(patient.Text, gef.Text, lot.Text, quantite.Text, date.Text);

                        //Vide les champs d'entrée
                        quantite.Text = "";
                        lot.Text = "";
                        gef.Text = "";
                        patient.Text = "";
                        break;
                    case Resource.Id.buttonenv: //envoyer mail
                        try
                        {
                            MailMessage mail = new MailMessage();
                            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
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
                            ServicePointManager.ServerCertificateValidationCallback = delegate (object sende, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
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
                        break;
                    case Resource.Id.buttonhist:    //historique
                        //Intent historiqueActivity = new Intent(this, typeof(historique));
                        //StartActivity(historiqueActivity);
                        Toast.MakeText(Application.Context, "Historique", ToastLength.Long);
                        break;
                }
            }

            //datepick.DateChanged += (s, e) =>
            //{
            //    date.Text = datepick.DateTime.ToLongDateString();
            //    datepick.Visibility = Android.Views.ViewStates.Gone;
            //};
        }



        //Lieu de stockage
        string directory = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "Pharmastock";

        //Méthode de création du fichier CSV
        public void CreateCSV(string numpat, string codeGEF, string lotnum, string quant, string date)
        {
            //Création d'un dossier
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                Toast.MakeText(Application.Context, "Dossier Pharmastock créé", ToastLength.Long).Show();
            }
            
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

        public class DatePickerFragment : Android.App.DialogFragment,
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
        
        class TabsAdapter : FragmentStatePagerAdapter
        {
            string[] titles;

            public override int Count
            {
                get
                {
                    return titles.Length;
                }
            }

            public TabsAdapter(Context context, Android.Support.V4.App.FragmentManager fm) : base(fm)
            {
                titles = context.Resources.GetTextArray(Resource.Array.sections);
            }

            public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
            {
                return new Java.Lang.String(titles[position]);
            }

            public override Android.Support.V4.App.Fragment GetItem(int position)
            {
                switch (position)
                {
                    case 0:
                        return Fragment1.NewInstance();
                    case 1:
                        return Fragment2.NewInstance();
                }
                return null;
            }

            public override int GetItemPosition(Java.Lang.Object frag)
            {
                return PositionNone;
            }
        }
    }
}

