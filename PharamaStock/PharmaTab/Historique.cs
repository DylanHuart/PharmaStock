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
            SetContentView(Resource.Layout.Historique);

           
            ListView lst = FindViewById<ListView>(Resource.Id.listHisto);

            string directory = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "Pharmastock";
            string[] fichiers = Directory.GetFiles(directory);

            ArrayAdapter<string> fichiersAdapter;
            fichiersAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, fichiers);
            lst.SetAdapter(fichiersAdapter);
        }
    }
}