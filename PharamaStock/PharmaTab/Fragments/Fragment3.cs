using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Widget;
using System;
using Android.Support.V4.Content;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace PharmaTab.Fragments
{
    public class Fragment3 : Android.Support.V4.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static Fragment3 NewInstance()
        {
            var frag3 = new Fragment3 { Arguments = new Bundle() };
            return frag3;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fragment3, null);

            // Create your application here
            LinearLayout vieww = new LinearLayout(this.Context)
            {
                Orientation = Orientation.Vertical
            };

            

            string directory = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "Pharmastock";
            string[] fichiers = Directory.GetFiles(directory);
            //List<string> listefichiers = new List<string>();
            //listefichiers = fichiers.OfType<string>().ToList();
            ArrayAdapter<string> fichiersAdapter;
            fichiersAdapter = new ArrayAdapter<string>(this.Context, Android.Resource.Layout.SimpleListItem1, fichiers);

            //TextView details = new TextView(this)
            //{
            //    Text = "Patient n° :" + ";" + "code GEF :" + ";" + "Lot n° :" + ";" + "Quantité :" + ";" + "Délivré le :"
            //};
            ListView listehisto = new ListView(this.Context);
            vieww.AddView(listehisto);
            listehisto.SetAdapter(fichiersAdapter);

            listehisto.ItemClick += Listehisto_ItemClick;

            void Listehisto_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
            {
                try
                {
                    Java.IO.File file = new Java.IO.File(fichiersAdapter.GetItem(e.Position));
                    Intent intent = new Intent();
                    intent.AddFlags(ActivityFlags.NewTask);
                    intent.AddFlags(ActivityFlags.GrantReadUriPermission);
                    intent.SetAction(Intent.ActionView);
                    //string type = getMIMEType(file);
                    intent.SetDataAndType(FileProvider.GetUriForFile(this.Context, this.Activity.PackageName + ".fileprovider", file), "text/csv");// "application/vnd.ms-excel");
                    StartActivity(intent);
                }
                catch (Exception ex)
                {
                    Toast.MakeText(Application.Context, ex.Message, ToastLength.Long);
                }
            };


            //this.SetContentView(vieww);

            return view;
        }
    }
}