using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using SQLite;
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
            var Main = FindViewById<ImageButton>(Resource.Id.mainButton);

            //set toolbar
            SupportToolbar Toolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            SetSupportActionBar(Toolbar);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            setToolBar = new mySetToolBar(this, ref Toolbar,ApplicationContext);

            // back to main page
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

					var db = new SQLiteConnection(sqlLiteFilePath);

					var container = FindViewById<LinearLayout>(Resource.Id.dataContainer);
                    container.RemoveAllViews();
                    
                    int num = 0;
                    var query = db.Table<Student>().Where(v => v.stuID.Contains(queryID));
                    foreach (var stu in query)
                    {
                        if (num >= 30)
                            break;
                        var txtView = new TextView(this);
                        if (stu.sex == "male")
                            txtView.Text = stu.stuID + " | " + "男 | " + stu.name;
                        else
                            txtView.Text = stu.stuID + " | " + "女 | " + stu.name;
                        container.AddView(txtView);
                        num++;
                    }
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

