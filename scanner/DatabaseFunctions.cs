using System;
using SQLite;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace scanner
{
	public class DatabaseFunctions
	{

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
		public void fillingDB(string result, string dbPath)
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
					}
				}
				catch (SQLiteException ex)
				{
					Console.WriteLine("Sqlite exploded");
					Console.WriteLine(ex.Message);
				}
			}
		}
		public string insertDB(string name, string id, string pay, string sex, string dbPath)
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
	}
}
