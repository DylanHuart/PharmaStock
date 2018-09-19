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
using Android.Content;
using Android.Text;
using System.Collections.Generic;
using System.Linq;
using System.Collections.;

namespace PharamaStock
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




            ListView liste = new ListView(this);


            
           

            
            view.AddView(liste);

            this.SetContentView(view);

        }

    }
}