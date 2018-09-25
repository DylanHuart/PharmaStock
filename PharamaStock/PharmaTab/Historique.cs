using Android.App;
using Android.OS;
using Android.Widget;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace PharmaTab
{
    [Activity(Label = "Historique")]
    public class Historique : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            LinearLayout view = new LinearLayout(this)
            {
                Orientation = Orientation.Vertical
            };



            string directory = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "Pharmastock";
            string[] fichiers = Directory.GetFiles(directory);
            List<string> listefichiers = new List<string>();
            listefichiers = fichiers.OfType<string>().ToList();




            ListView liste = new ListView(this)
            {

            };
            view.AddView(liste);

            this.SetContentView(view);

        }

    }
}