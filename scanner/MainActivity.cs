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
using Newtonsoft.Json;

namespace scanner
{
	[Activity(Label = "scanner", MainLauncher = true, Icon = "@mipmap/icon", Theme ="@style/MyTheme" )]

	public class MainActivity : ActionBarActivity
	{
		private async void BtnScan_Click(object sender, EventArgs e)
		{
			//var options = new ZXing.Mobile.MobileBarcodeScanningOptions();
			var scanner = new ZXing.Mobile.MobileBarcodeScanner();
           
			//options.PossibleFormats = new List<ZXing.BarcodeFormat>() {
			//	ZXing.BarcodeFormat.CODE_93, ZXing.BarcodeFormat.CODE_39
			//};

			var options = new ZXing.Mobile.MobileBarcodeScanningOptions()
			{
				TryHarder = true,
				PossibleFormats = new List<ZXing.BarcodeFormat>
				{
					ZXing.BarcodeFormat.CODE_39
				}
			};
			var result = await scanner.Scan(options);
			if (result != null)
			{
				function func = new function();
				string id;
				id = result.Text;
				id = id.Remove(0, 2);
				id = id.Remove(id.Length - 1);
				//Toast.MakeText(this, id, ToastLength.Long).Show();
				string url = "http://theblackcat102.nctu.me:5000/NCTU/api/v1.0/tasks/" + id.ToString();
				JsonValue pay = await func.FetchStudentAsync(url);
				if (func.GotPay(pay))
				{
					Toast.MakeText(this, id + ",有繳學聯會費喔", ToastLength.Long).Show();
				}
				else {
					Toast.MakeText(this, id + ",沒有繳學聯會費！！", ToastLength.Long).Show();
				}
			}
		}

		private async void testBtn_Click(object sender, EventArgs e)
		{
			function func = new function();
			var sqlLiteFilePath = GetFileStreamPath("") + "/db_user.db";
			if (File.Exists(sqlLiteFilePath))
			{
				File.Delete(sqlLiteFilePath);
			}
			string response = func.createDB(sqlLiteFilePath);
			Toast.MakeText(this, response, ToastLength.Short).Show();

			string result = await func.parseData("http://theblackcat102.nctu.me:5000/NCTU/api/v1.0/grade/108");

			// ... Display the result.
			ProgressDialog progress;
			progress = ProgressDialog.Show(this, "Loading", "Please Wait...", true);
			await Task.Factory.StartNew(
			() =>
			{
				if (result != null && result.Length >= 50)
				{
					string dbPath = GetFileStreamPath("") + "/db_user.db";
					func.fillingDB(result, dbPath); 
				}
			}).ContinueWith(
				t =>
				{
					if (progress != null)
					{
						progress.Hide();
					}

				}, TaskScheduler.FromCurrentSynchronizationContext()
			);
		}

        //private member needed for navigated drawer
        private DrawerLayout drawerLayout;
        private ListView leftDrawer;
        private ActionBarDrawerToggle DrawerToggle;
        private ArrayAdapter<String> drawerAdapter;
        private string[] index = { "掃描", "添加內容", "搜尋", "下載資料" };

        protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

            //Set Toolbar
            SupportToolbar Toolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            SetSupportActionBar(Toolbar);
            Toolbar.SetLogo(Resource.Mipmap.Icon);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            //Set DrawerToggle
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            leftDrawer = FindViewById<ListView>(Resource.Id.left_drawer);
            DrawerToggle = new ActionBarDrawerToggle(
                this,                         //Host Activity
                drawerLayout,               //DrawerLayout
                Resource.String.openDrawer,
                Resource.String.closeDrawer
                );
            DrawerToggle.SyncState();
            drawerLayout.SetDrawerListener(DrawerToggle);

            //Create Index to Add in the drawer            
            drawerAdapter = new ArrayAdapter<String>(this,Android.Resource.Layout.SimpleListItem1,index);
            leftDrawer.SetAdapter(drawerAdapter);
            leftDrawer.ItemClick += leftDrawer_ItemClick;
		}

        /********This Area is for the ckick event for toolbar and its button*******/
        //Any icon on the toolbar was "clicked"
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            DrawerToggle.OnOptionsItemSelected(item);
            return base.OnOptionsItemSelected(item);
        }

        private void leftDrawer_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if (index[e.Position] == "掃描")
            {
                BtnScan_Click(sender, e);
            }
            else if (index[e.Position] == "添加內容")
            {
                var intent = new Intent(this, typeof(InputNewUser));
                StartActivity(intent);
            }

            else if (index[e.Position] == "搜尋")
            {
                var searchLayout = new Intent(this, typeof(Search));
                StartActivity(searchLayout);
            }

            else if (index[e.Position] == "下載資料")
            {
                testBtn_Click(sender, e);
            }
        }

        /********This Area is for the ckick event for toolbar and its button*******/


	}
}


