using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System.Data;
using SQLite;
using System.Json;
namespace scanner
{
	[Activity(Label = "InputNewUser")]
	public class InputNewUser : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.student);
			var sqlLiteFilePath = GetFileStreamPath("") + "/db_user.db";
			var name = FindViewById<EditText>(Resource.Id.studentName);
			var stuID = FindViewById<EditText>(Resource.Id.studentID);
			var payed = FindViewById<RadioButton>(Resource.Id.radioYes);
			var insert = FindViewById<Button>(Resource.Id.addUser);
			var Main = FindViewById<Button>(Resource.Id.backMain);
			string response;

			insert.Click += delegate {
				string payment = "false";
				if (payed.Checked)
					payment = "true";
				response = insertDB(sqlLiteFilePath, name.Text, stuID.Text, payment,"male");
				Toast.MakeText(this, response, ToastLength.Short).Show();                    
			};
			Main.Click += delegate {
				var main = new Intent(this, typeof(MainActivity));
				StartActivity(main);
			};
			// Create your application here
		}
		private async void postData()
		{
			
		}
		public string insertDB(string dbPath,string name,string id,string pay,string sex)
		{
			try
			{
				var db = new SQLiteConnection(dbPath);
				var stu = new Student { name = name, stuID = id, pay = pay, sex = sex};
				db.Insert(stu); 
				return "Insert successful";
			}
			catch (SQLiteException ex)
			{
				return ex.Message;
			}
		}
	}
}

