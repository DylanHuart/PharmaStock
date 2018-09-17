using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;

namespace PharamaStock
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
          
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            LinearLayout view = new LinearLayout(this)
            {
                Orientation = Orientation.Vertical
            };
            this.SetContentView(view);

            TextView pat = new TextView(this)
            {
                Text = "Numéro du Patient"
            };
            view.AddView(pat);


            EditText edit_pat = new EditText(this)
            {
               
            };
            edit_pat.SetSingleLine(true);
            view.AddView(edit_pat);

            TextView UF = new TextView(this)
            {
                Text = "Numéro du Patient"
            };


            EditText edit_UF = new EditText(this)
            {

            };
            edit_UF.SetSingleLine(true);
            view.AddView(edit_UF);

            view.AddView(UF);

            TextView gef = new TextView(this)
            {
                Text = "Numéro GEF"
            };
            view.AddView(gef);


            EditText edit_gef = new EditText(this)
            {

            };
            edit_gef.SetSingleLine(true);
            view.AddView(edit_gef);

        }
    }
}