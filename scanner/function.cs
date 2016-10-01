using System;
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
using SQLite;
using Newtonsoft.Json;

namespace scanner
{
	public class function
	{

		public async Task<string> parseData(string url)
		{
			string TARGETURL = url;
			//Toast.MakeText(this, TARGETURL, ToastLength.Short).Show();
			var handler = new HttpClientHandler();
			//Toast.MakeText(this, "handler gen pass", ToastLength.Short).Show();
			// ... Use HttpClient.            
			var client = new HttpClient(handler);
			//Toast.MakeText(this, "client gen pass", ToastLength.Short).Show();
			string result = null;
			var byteArray = Encoding.ASCII.GetBytes("theblackcat102:AF51FAF9BE961002");
			//Toast.MakeText(this, "encode gen pass", ToastLength.Short).Show();
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
			//Toast.MakeText(this, "auth gen pass", ToastLength.Short).Show();
			//Console.WriteLine("Successful!\n");
			try
			{
				HttpResponseMessage ServerResponse = await client.GetAsync(TARGETURL);
				HttpContent content = ServerResponse.Content;
				result = await content.ReadAsStringAsync();
				// ... Check Status Code                                
			}
			catch (HttpRequestException except)
			{
				Console.WriteLine( "client get failed:" + except.Message);
			}
			return result;
		}

		public void fillingDB(string result,string dbPath)
		{
			if (result != null && result.Length >= 50)
			{

				try
				{
					var db = new SQLiteConnection(dbPath);
					Console.WriteLine("try and catch passed\n");
					var students = Newtonsoft.Json.Linq.JObject.Parse(result);
					foreach (var student in students["students"])
					{
						byte[] utf8bytes = Encoding.UTF8.GetBytes((string)student["name"]);
						string name = Encoding.UTF8.GetString(utf8bytes, 0, utf8bytes.Length);
						var stu = new Student { name = name, stuID = (string)student["stuID"], pay = (string)student["pay"], sex = (string)student["sex"] };
						db.Insert(stu);
						Console.WriteLine("inserted\n");
					}
				}
				catch (SQLiteException ex)
				{
					Console.WriteLine("Sqlite exploded");
					Console.WriteLine(ex.Message);
				}
			}
		}
		public async Task<JsonValue> FetchStudentAsync(string url)
		{
			// Create an HTTP web request using the URL:
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new System.Uri(url));
			request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("theblackcat102:AF51FAF9BE961002"));
			request.ContentType = "application/json";
			request.Method = "GET";
			// Send the request to the server and wait for the response:
			using (WebResponse response = await request.GetResponseAsync())
			{
				//Get a stream representation of the HTTP web response:
				using (Stream stream = response.GetResponseStream())
				{
					// Use this stream to build a JSON document object:
					JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
					//string reponse = "Response: " + jsonDoc.ToString();
					//debug purpose
					//Toast.MakeText(this, reponse, ToastLength.Long).Show();
					// Return the JSON document:
					return jsonDoc;
				}
			}
			//private CredentialCache GetCredential(string url)
			//{
			//	ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
			//	CredentialCache credentialCache = new CredentialCache();
			//	credentialCache.Add(new System.Uri(url), "Basic", new NetworkCredential("python", "python123"));
			//	return credentialCache;
			//}
		}
		public bool GotPay(JsonValue json)
		{
			JsonValue Pay = json["task"];
			bool payment = Pay["pay"];
			return payment;
		}
		private string insertDB(string name, string id, string pay, string sex,string dbPath)
		{
			try
			{
				var db = new SQLiteConnection(dbPath);
				var stu = new Student { name = name, stuID = id, pay = pay, sex = sex };
				db.Insert(stu);
				return "success";
			}
			catch (SQLiteException ex)
			{
				return ex.Message;
			}
		}

		public string createDB(string dbPath)
		{
			try
			{
				var db = new SQLiteConnection(dbPath);
				db.CreateTable<Student>();
				db.Close();
				return "db create success";
			}
			catch (SQLiteException ex)
			{
				return ex.Message;
			}
		}


	}
}

