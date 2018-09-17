using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using Android.Util;

namespace PharamaStock
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        TextView _dateDisplay;
        Button _dateSelectButton;


        
        protected override void OnCreate(Bundle savedInstanceState)
        {
          
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            _dateDisplay = FindViewById<TextView>(Resource.Id.date_display);
            _dateSelectButton = FindViewById<Button>(Resource.Id.date_select_button);
            _dateSelectButton.Click += DateSelect_OnClick;
            
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
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

            //EditText date = new EditText(this)
            //{

            //};
            //date.SetSingleLine(true);
            //view.AddView(date);
            //this.SetContentView(view);
            void DateSelect_OnClick(object sender, EventArgs eventArgs)
            {
                DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
                {
                    _dateDisplay.Text = time.ToLongDateString();
                });
                frag.Show(FragmentManager, DatePickerFragment.TAG);
            }
            base.OnCreate(savedInstanceState);

            //enregistre les données récoltées dans un fichier 
            Button Enregistrer = new Button(this)
            {
                Text = "Enregistrer"

            };

            view.AddView(Enregistrer);
            this.SetContentView(view);
        }
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