using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using SQLite;
using System.Collections.Generic;
using Xamarin.Android;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Views;
namespace scanner
{
    [Activity(Label = "scanner", MainLauncher = true, Icon = "@mipmap/icon", Theme = "@style/MyTheme")]
    public class Search : ActionBarActivity
    {
        private mySetToolBar setToolBar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Search);
            var sqlLiteFilePath = GetFileStreamPath("") + "/db_user.db";
			var Main = FindViewById<TextView>(Resource.Id.text_toolbar_title);

            //set toolbar
            SupportToolbar Toolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            SetSupportActionBar(Toolbar);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            setToolBar = new mySetToolBar(this, ref Toolbar,ApplicationContext);

            // back to main page

			//setup list view 
			ListView listView = FindViewById<ListView>(Resource.Id.listViewMain);
			Main.Click += delegate
            {
                var main = new Intent(this, typeof(MainActivity));
                StartActivity(main);
            };
            var find = FindViewById<EditText>(Resource.Id.searchBar);
            find.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) =>
            {
                try
                {
					string queryID = e.Text.ToString();
					string buffer;
					var db = new SQLiteConnection(sqlLiteFilePath);

                    int num = 0;
					List<queryResult> results = new List<queryResult>();
					var query = db.Table<Student>().Where(v => v.stuID.Contains(queryID));
                    foreach (var stu in query)
                    {
                        if (num >= 50)
                            break;
						if (stu.sex == "male")
							buffer = "男";
						else if (stu.sex == "female")
							buffer = "女";
						else
							buffer = "無資料";
						results.Add(new queryResult { Title = stu.stuID + " " + stu.name, StuInfo = buffer });	
						num++;
                    }
					listView.Adapter = new studentListAdapter(this, results);
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
                }
            };
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            setToolBar.DrawerToggleEvent(ref item);
            return base.OnOptionsItemSelected(item);
        }
    }
}

