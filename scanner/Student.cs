using SQLite;

namespace scanner
{
	public class Student
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		[MaxLength(20)]
		public string name { get; set; }
		public string sex { get; set; }
		public string pay { get; set; }
		[MaxLength(7)]
		public string stuID { get; set; }
	}
	public class StudentAPI
	{
		public string name { get; set; }
		public string sex { get; set; }
		public string stuID { get; set; }
		public string pay { get; set; }
		public string course { get; set; }
	}
	//            a_dict = {'name':row[2],'sex':row[4],'stuID':row[1],'pay':pay,'course':row[5]}

}