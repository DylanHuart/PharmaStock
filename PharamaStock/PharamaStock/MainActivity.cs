using Android.App;
using Android.OS;
using Android.Views;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using Android.Util;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;

namespace PharamaStock
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]

    public class MainActivity : AppCompatActivity
    {
        //TextView _dateDisplay;
        //Button _dateSelectButton;


        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //SetContentView(Resource.Layout.activity_main);

            // Set our view from the "main" layout resource
            //SetContentView(Resource.Layout.activity_main);
            LinearLayout view = new LinearLayout(this)
            {
                Orientation = Orientation.Vertical
            };

            //affiche le numéro du patient
            this.SetContentView(view);
            TextView numPatient = new TextView(this)
            {
                Text = "Numéro du patient : "
            };
            view.AddView(numPatient);
            EditText patient = new EditText(this)
            {

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

            };
            gef.SetSingleLine(true);
            view.AddView(gef);
            this.SetContentView(view);

            //affiche le numéro du lot
            TextView numLot = new TextView(this)
            {
                Text = "Lot numéro : "
            };
            view.AddView(numLot);
            EditText lot = new EditText(this)
            {

            };
            patient.SetSingleLine(true);
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

            };
            quantite.SetSingleLine(true);
            view.AddView(quantite);
            this.SetContentView(view);

            //affiche la date de délivrance à la date du jour
            TextView date_display = new TextView(this)
            {
                Text = "date"

            };
            view.AddView(date_display);

            TextView date = new TextView(this)
            {
                Text = DateTime.Now.ToLongDateString()

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
            Enregistrer.Click += (s, e) =>
            {
                if (!string.IsNullOrEmpty(patient.Text) && !string.IsNullOrEmpty(gef.Text) && !string.IsNullOrEmpty(lot.Text) && !string.IsNullOrEmpty(quantite.Text) && !string.IsNullOrEmpty(date.Text))
                    CreateCSV(patient.Text, gef.Text, lot.Text, quantite.Text, date.Text);


            };

            view.AddView(Enregistrer);
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
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                    mail.From = new MailAddress("from address here");
                    mail.To.Add("to adress here");
                    mail.Subject = "Message Subject";
                    mail.Body = "Message Body";
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("username", "password");
                    SmtpServer.EnableSsl = true;
                    ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors) {
                        return true;
                    };
                    SmtpServer.Send(mail);
                    Toast.MakeText(Application.Context, "Mail Send Sucessufully", ToastLength.Short).Show();
                }

                catch (Exception ex)
                {
                    Toast.MakeText(Application.Context, ex.ToString(), ToastLength.Long);
                }
            };
            view.AddView(Envoyer);
            this.SetContentView(view);


        }

        public void CreateCSV(string numpat, string codeGEF, string lotnum, string quant, string date)
        {
            var fileName = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "Pharmastock_"+DateTime.Now.ToString("ddMMyyy") + ".csv";

            var csv = new StringBuilder();

            var newline = string.Format("{0},{1},{2},{3},{4}", numpat, codeGEF, lotnum, quant, date);

            csv.AppendLine(newline);

            File.WriteAllText(fileName, csv.ToString());
            //using (var fs = new FileStream(fileName, FileMode.OpenOrCreate))
            //{
            //    //Write your file here

            //    fs.Write()
            //}


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