using Android.App;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using ZXing;
using ZXing.Mobile;

namespace PharmaTab.Fragments
{
    public class Fragment1 : Android.Support.V4.App.Fragment
    {
        string fileName = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "Pharmastock_" + DateTime.Now.ToString("ddMMyyy") + ".csv";
        

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


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fragment1, null);
            
            EditText patient = view.FindViewById<EditText>(Resource.Id.numpat);
            EditText gef = view.FindViewById<EditText>(Resource.Id.codgef);
            EditText lot = view.FindViewById<EditText>(Resource.Id.numlot);
            EditText quantite = view.FindViewById<EditText>(Resource.Id.qtedel);
            EditText date = view.FindViewById<EditText>(Resource.Id.datedel);

            Button savebt = view.FindViewById<Button>(Resource.Id.buttonenr);
            Button selectdate = view.FindViewById<Button>(Resource.Id.button5);
            Button send = view.FindViewById<Button>(Resource.Id.buttonenv);
            Button historique = view.FindViewById<Button>(Resource.Id.buttonhist);
            Button scan1 = view.FindViewById<Button>(Resource.Id.button1);
            Button scan2 = view.FindViewById<Button>(Resource.Id.button2);
            Button scan3 = view.FindViewById<Button>(Resource.Id.button3);
            Button scan4 = view.FindViewById<Button>(Resource.Id.button4);



            MobileBarcodeScanner scanner;

            scan1.Click += async (s, e) =>
            {
                MobileBarcodeScanner.Initialize(Activity.Application);

                scanner = new MobileBarcodeScanner();

                var result = await scanner.Scan();

                if (result == null)
                {
                    return;
                }

                else
                {
                    patient.Text = result.Text;
                }
            };
            scan2.Click += openscan;
            scan3.Click += openscan;
            scan4.Click += openscan;
            
            selectdate.Click += Button_Click;
            savebt.Click += Button_Click;
            send.Click += Button_Click;
            historique.Click += Button_Click;


            void openscan(object sender,EventArgs e)
            {
                Toast.MakeText(this.Context,"Coin", ToastLength.Long);
            }
            void Button_Click(object sender, EventArgs e)
            {
                Button btn = (Button)sender;
                switch (btn.Id)
                {
                    case Resource.Id.button5: //select date
                        DatePickerDialog datepick = new DatePickerDialog(this.Context, OnDateSet, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        datepick.DatePicker.MinDate = DateTime.Today.Millisecond;
                        datepick.Show();
                        break;
                    case Resource.Id.buttonenr:     //enregistrer csv
                                                    //Cr�ation du fichier CSV
                        if (!string.IsNullOrEmpty(patient.Text) && !string.IsNullOrEmpty(gef.Text) && !string.IsNullOrEmpty(lot.Text) && !string.IsNullOrEmpty(quantite.Text) && !string.IsNullOrEmpty(date.Text))
                            CreateCSV(patient.Text, gef.Text, lot.Text, quantite.Text, date.Text);

                        //Vide les champs d'entr�e
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
                            mail.Body = "Veuillez trouver ci joint le document r�capitalif de la journ�e";
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
                            Toast.MakeText(Application.Context, "Mail envoy�", ToastLength.Short).Show();
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

            
            return view;

            void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
            {
                date.Text = e.Date.ToLongDateString();
            }
        }
        string directory = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "Pharmastock";

        //M�thode de cr�ation du fichier CSV
        public void CreateCSV(string numpat, string codeGEF, string lotnum, string quant, string date)
        {

            //Cr�ation d'un dossier
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                Toast.MakeText(Application.Context, "Dossier Pharmastock cr��", ToastLength.Short).Show();

            }
            //Nom du fichier + Location

            //Ligne � ajouter lors de l'enregistrement. Reprend les entr�es des champs EditText
            var newline = string.Format("{0};{1};{2};{3};{4}", numpat, codeGEF, lotnum, quant, date);

            //Si le fichier n'existe pas, cr�er les ent�tes et aller � la ligne. 
            if (!File.Exists(fileName))
            {
                string header = "Patient n� :" + ";" + "code GEF :" + ";" + "Lot n� :" + ";" + "Quantit� :" + ";" + "D�livr� le :";
                File.WriteAllText(fileName, header, Encoding.UTF8);       // Cr�ation de la ligne + Encodage pour les caract�res sp�ciaux
                File.AppendAllText(fileName, System.Environment.NewLine); // Aller � la ligne
            }
            File.AppendAllText(fileName, newline + System.Environment.NewLine); // Ajout de la ligne contenant les champs
            Toast.MakeText(Application.Context, "Donn�es enregistr�es", ToastLength.Short).Show();
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

    }
}