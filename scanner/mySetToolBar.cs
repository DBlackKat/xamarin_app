using Android.App;
using Android.Widget;
using Android.OS;
using Android.Net;
using System.Net;
using System.Net.Http;
using Android.Content;
using System.Collections.Generic;
using System.Json;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System;
using SQLite;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Views;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace scanner
{
    class mySetToolBar : ActionBarActivity
    {
        private SupportToolbar refToolBar;
        private Context context;
        private Activity activity;
        private DrawerLayout drawerLayout;
        private ListView leftDrawer;
        private ActionBarDrawerToggle DrawerToggle;
        private ArrayAdapter<String> drawerAdapter;
        private string[] index = { "���y", "�K�[���e", "�j�M", "�U�����" };
        

        public mySetToolBar(Activity _activity, ref SupportToolbar _toolBar,Context _context)
        {
            activity = _activity;
            refToolBar = _toolBar;
            context = _context;

            refToolBar.SetLogo(Resource.Mipmap.Icon);
            //Set DrawerToggle
            drawerLayout = activity.FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            leftDrawer = activity.FindViewById<ListView>(Resource.Id.left_drawer);
            DrawerToggle = new ActionBarDrawerToggle(
                activity,                         //Host Activity
                drawerLayout,               //DrawerLayout
                Resource.String.openDrawer,
                Resource.String.closeDrawer
                );
            DrawerToggle.SyncState();
            drawerLayout.SetDrawerListener(DrawerToggle);

            //Create Index to Add in the drawer            
            drawerAdapter = new ArrayAdapter<String>(activity, Android.Resource.Layout.SimpleListItem1, index);
            leftDrawer.SetAdapter(drawerAdapter);
            leftDrawer.ItemClick += leftDrawer_ItemClick;


        }

        /********This Area is for the ckick event for toolbar and its button*******/
        //Any icon on the toolbar was "clicked"
        public void DrawerToggleEvent(ref IMenuItem item)
        {
            DrawerToggle.OnOptionsItemSelected(item);
        }

        private void leftDrawer_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if (index[e.Position] == "���y")
            {
                buttonEvent.scanEvent(sender, e);
            }
            else if (index[e.Position] == "�K�[���e")
            {
                var intent = new Intent(activity, typeof(InputNewUser)).SetFlags(ActivityFlags.NewTask);
                context.StartActivity(intent);
            }

            else if (index[e.Position] == "�j�M")
            {
                var intent = new Intent(activity, typeof(Search)).SetFlags(ActivityFlags.NewTask);
                context.StartActivity(intent);
            }

            else if (index[e.Position] == "�U�����")
            {
                buttonEvent.testEvent(sender, e);
            }
        }


        /********This Area is for the ckick event for toolbar and its button*******/

    }

    public class buttonEvent
    {
        private static void nullfunc(object sender, EventArgs e) { }
        public delegate void ButtonEvent(object sender, EventArgs e);
        public  static ButtonEvent scanEvent;
        public static ButtonEvent testEvent;
        public buttonEvent(ButtonEvent _s,ButtonEvent _t)
        {
            scanEvent = _s;
            testEvent = _t;
        }
    }
}