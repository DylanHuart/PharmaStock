using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;
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
            //List<string> listefichiers = new List<string>();
            //listefichiers = fichiers.OfType<string>().ToList();
            ArrayAdapter<string> fichiersAdapter;
            fichiersAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, fichiers);

            //TextView details = new TextView(this)
            //{
            //    Text = "Patient n° :" + ";" + "code GEF :" + ";" + "Lot n° :" + ";" + "Quantité :" + ";" + "Délivré le :"
            //};
            ListView listehisto = new ListView(this);
            view.AddView(listehisto);
            listehisto.SetAdapter(fichiersAdapter);

            listehisto.ItemClick += Listehisto_ItemClick;

            void Listehisto_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
            {
                var item = fichiersAdapter.GetItem(e.Position);
                File.Open(item, FileMode.Open);
            }

            this.SetContentView(view);

        }


    }
}