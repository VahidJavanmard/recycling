using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace SimpleMapDemo
{
    public class ManageDrawer : Android.Support.V7.App.ActionBarDrawerToggle
    {
        private AppCompatActivity activity;
        private int openRes;
        private int closeRes;
        public ManageDrawer(AppCompatActivity activity, DrawerLayout drawerLayout, int openDrawerContentDescRes, int closeDrawerContentDescRes) : base(activity, drawerLayout, openDrawerContentDescRes, closeDrawerContentDescRes)
        {
            this.activity = activity;
            this.openRes = openDrawerContentDescRes;
            this.closeRes = closeDrawerContentDescRes;
        }

        public override void OnDrawerOpened(View drawerView)
        {
            int type = (int)drawerView.Tag;
            if (type == 0)
            {
                activity.SupportActionBar.SetTitle(openRes);
                base.OnDrawerOpened(drawerView);
            }

        }

        public override void OnDrawerClosed(View drawerView)
        {
            int type = (int)drawerView.Tag;
            if (type == 0)
            {
                activity.SupportActionBar.SetTitle(closeRes);
                base.OnDrawerClosed(drawerView);
            }

        }

        public override void OnDrawerSlide(View drawerView, float slideOffset)
        {
            int type = (int)drawerView.Tag;
            if (type == 0)
            {
                base.OnDrawerSlide(drawerView, slideOffset);
            }

        }
    }
}