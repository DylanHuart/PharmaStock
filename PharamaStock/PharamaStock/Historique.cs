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

namespace PharamaStock
{
    [Activity(Label = "Historique")]
    public class Historique : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            string directory = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "Pharmastock";
            string[] fichiers = Directory.GetFiles(directory);
            



            TextView liste = new TextView(this)
            {

            };

            liste.Click += (s, e) =>
            {
            for (int i = 0; i < fichiers.Length; i++)
                {
                    liste.Text += fichiers[i] + "\n";
                }
            };

        }

    }
}