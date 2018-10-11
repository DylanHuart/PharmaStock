using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V4.View;
using Android.Support.V7.App;
using PharmaTab.Fragments;
using System;

namespace PharmaTab
{
    [Activity(Label = "@string/app_name", MainLauncher = false, LaunchMode = Android.Content.PM.LaunchMode.SingleTop, Icon = "@drawable/icon")]

  
    public class MainActivity : AppCompatActivity
    {
        public static bool Fragmentauto;
       
        ViewPager pager;
        TabsAdapter adapter;
        TabLayout tabs;
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            
        }

        protected override void OnCreate(Bundle bundle)
        {
            Fragmentauto = true;

            base.OnCreate(bundle);
            SetContentView(Resource.Layout.main);
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            if (toolbar != null)
            {
                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(false);
                SupportActionBar.SetHomeButtonEnabled(false);
            }

            

            adapter = new TabsAdapter(this, SupportFragmentManager);
            pager = FindViewById<ViewPager>(Resource.Id.pager);
            
            pager.Adapter = adapter;
            tabs = FindViewById<TabLayout>(Resource.Id.tabs);
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

