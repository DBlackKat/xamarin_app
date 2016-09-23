using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using SQLite;
using Xamarin.Android;
namespace scanner
{
	[Activity(Label = "Search")]
	public class Search : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Search);
			ActionBar.Hide(); //hides the bloody ugly top menu bar
			var sqlLiteFilePath = GetFileStreamPath("") + "/db_user.db";
			var Main = FindViewById<Button>(Resource.Id.mainButton);
			// back to main page
			Main.Click += delegate
			{
				var main = new Intent(this, typeof(MainActivity));
				StartActivity(main);
			};
			var find = FindViewById<EditText>(Resource.Id.searchBar);
			var db = new SQLiteConnection(sqlLiteFilePath);
			find.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) =>
			{
				try
				{
					var container = FindViewById<LinearLayout>(Resource.Id.dataContainer);
					container.RemoveAllViews();
					string queryID = e.Text.ToString();
					int num = 0;
					var query = db.Table<Student>().Where(v => v.stuID.Contains(queryID));
					foreach (var stu in query)
					{
						if (num >= 30)
							break;
						var txtView = new TextView(this);
						if(stu.sex == "male")
							txtView.Text = stu.stuID + " | " +"男 | "+ stu.name ;
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
	}
}

