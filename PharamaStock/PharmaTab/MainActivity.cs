using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using PharmaTab.Fragments;

using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Content;
using System.Text;
using System.IO;
using Android.Util;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Android;
using Android.Support.V4.Content;

namespace PharmaTab
{
    [Activity(Label = "@string/app_name", MainLauncher = true, LaunchMode = Android.Content.PM.LaunchMode.SingleTop, Icon = "@drawable/icon")]

  
    public class MainActivity : AppCompatActivity
    {

       
        ViewPager pager;
        TabsAdapter adapter;
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.main);
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            if (toolbar != null)
            {
                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(false);
                SupportActionBar.SetHomeButtonEnabled(false);
            }

            // Affiche une boîte de dialogue pour accorder l'autorisation d'accès au stockage
            var permissionSto = Manifest.Permission.WriteExternalStorage;
            var permissionCam = Manifest.Permission.Camera;

            if (ContextCompat.CheckSelfPermission(this, permissionSto) != Android.Content.PM.Permission.Granted || ContextCompat.CheckSelfPermission(this, permissionCam) != Android.Content.PM.Permission.Granted)
                ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.WriteExternalStorage, Manifest.Permission.Camera, Manifest.Permission.ReadExternalStorage }, 0);

            adapter = new TabsAdapter(this, SupportFragmentManager);
            pager = FindViewById<ViewPager>(Resource.Id.pager);
            var tabs = FindViewById<TabLayout>(Resource.Id.tabs);
            pager.Adapter = adapter;
            
            tabs.SetupWithViewPager(pager);
            pager.OffscreenPageLimit = 3;

            
        }
        
        class TabsAdapter : FragmentStatePagerAdapter
        {
            string[] titles;

            public override int Count
            {
                get
                {
                    return titles.Length;
                }
            }

            public TabsAdapter(Context context, Android.Support.V4.App.FragmentManager fm) : base(fm)
            {
                titles = context.Resources.GetTextArray(Resource.Array.sections);
            }

            public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
            {
                return new Java.Lang.String(titles[position]);
            }

            public override Android.Support.V4.App.Fragment GetItem(int position)
            {
                switch (position)
                {
                    case 0:
                        return Fragment1.NewInstance();
                    case 1:
                        return Fragment2.NewInstance();
                }
                return null;
            }

            public override int GetItemPosition(Java.Lang.Object frag)
            {
                return PositionNone;
            }
        }
    }
}

