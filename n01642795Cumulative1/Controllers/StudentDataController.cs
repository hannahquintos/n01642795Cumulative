using MySql.Data.MySqlClient;
using n01642795Cumulative1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace n01642795Cumulative1.Controllers
{
    // A WebAPI controller which allows you to access information about students
    public class StudentDataController : ApiController
    {
        // SchoolDbContext class to access the database (Models/SchoolDbContext.cs)
        private SchoolDbContext School = new SchoolDbContext();


        /// <summary>
        ///     Returns a list of students from the students table of the school database. If there is a search input, returns a list of students related to the search.
        /// </summary>
        /// <param name="SearchKey"> An optional parameter (null if not provided) of the user's search input (as a string) </param>
        /// <returns>
        ///     A list of Student objects that relate to the SearchKey with their properties (student objects = rows in student table, properties = columns in student table for a specific row)
        /// </returns>
        /// <example>
        ///     GET api/StudentData/ListStudents --> [Student Object{properties}, Student Object{properties}, ...]
        ///     GET api/StudentData/ListStudents/kim --> [Student Object{...StudentFname:"Kimberly"... }]
        /// </example>
        [HttpGet]
        [Route("api/StudentData/ListStudents/{SearchKey?}")]
        public IEnumerable<Student> ListStudents(string SearchKey = null)
        {
            // create a connection
            MySqlConnection Conn = SchoolDbContext.AccessDatabase();

            // open connection
            Conn.Open();

            // create command/query
            MySqlCommand cmd = Conn.CreateCommand();

            // SQL query
            // use SearchKey parameter and match with data in database using WHERE and LIKE
            // allow search of first name, last name, and student number
            // use lower to convert both the search key input and the data in database to all lowercase - this way the search is not case sensitive
            cmd.CommandText = "Select * from Students where lower(studentfname) like lower(@key) or lower(studentlname) like lower(@key) or lower(concat(studentfname, ' ', studentlname)) like lower(@key) or lower(studentnumber) like lower(@key)";

            // sanitize the search key input! - to prevent MySQL injection attacks
            // @key in SQL query is replaced by %SearchKey%
            cmd.Parameters.AddWithValue("key", "%" + SearchKey + "%");
            cmd.Prepare();

            // get result set of query
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            // set up list of students
            List<Student> Students = new List<Student>();

            // loop through results of query
            while (ResultSet.Read())
            {
                // get properties
                int StudentId = Convert.ToInt32(ResultSet["studentid"]);
                string StudentFname = (string)ResultSet["studentfname"];
                string StudentLname = (string)ResultSet["studentlname"];
                string StudentNumber = (string)ResultSet["studentnumber"];
                string EnrolDate = ResultSet["enroldate"].ToString();

                // create new student object (Models/Student.cs)
                Student NewStudent = new Student();

                // set properties of new student object to values retrieved from database
                NewStudent.StudentId = StudentId;
                NewStudent.StudentFname = StudentFname;
                NewStudent.StudentLname = StudentLname;
                NewStudent.StudentNumber = StudentNumber;
                NewStudent.EnrolDate = EnrolDate;

                // add this new student to list of students
                Students.Add(NewStudent);
            }

            // close connection
            Conn.Close();

            // return list of all students
            return Students;
        }

        /// <summary>
        ///     Recieves a student id and returns the corresponding student and their information for that id
        /// </summary>
        /// <param name="id"> The student id (as an integer) </param>
        /// <returns>
        ///     A student object and its properties
        /// </returns>
        /// <example>
        ///     GET api/StudentData/FindStudent/3 --> [Student Object{...StudentFname:Austin, StudentId:3... }]
        ///     GET api/StudentData/FindStudent/5 --> [Student Object{...StudentFname:Elizabeth, StudentId:5... }]
        /// </example>
        [HttpGet]
        public Student FindStudent(int id)
        {
            // create new student object
            Student NewStudent = new Student();

            // create a connection
            MySqlConnection Conn = SchoolDbContext.AccessDatabase();

            // open connection
            Conn.Open();

            // create command/query
            MySqlCommand cmd = Conn.CreateCommand();

            // SQL query
            // get the student that has a matching studentid to the input id
            cmd.CommandText = "Select * from Students where studentid = @id";

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            // get result set of query
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                // get properties
                int StudentId = Convert.ToInt32(ResultSet["studentid"]);
                string StudentFname = (string)ResultSet["studentfname"];
                string StudentLname = (string)ResultSet["studentlname"];
                string StudentNumber = (string)ResultSet["studentnumber"];
                DateTime EnrolDate = (DateTime)ResultSet["enroldate"];

                // set properties of new student object to values retrieved from database
                NewStudent.StudentId = StudentId;
                NewStudent.StudentFname = StudentFname;
                NewStudent.StudentLname = StudentLname;
                NewStudent.StudentNumber = StudentNumber;
                NewStudent.EnrolDate = EnrolDate.ToString("D");
            }

            // close connection
            Conn.Close();

            // return a single student object
            return NewStudent;
        }

        /// <summary>
        ///     Recieves a class id and returns a list of students who are enrolled in that corresponding class
        /// </summary>
        /// <param name="id">The class id (as an integer)</param>
        /// <returns>
        ///     A list of Student objects with their properties
        /// </returns>     
        /// <example>
        ///     GET api/StudentData/GetStudentsInClass/{id} --> [Student Object{properties}, Student Object{properties}, ...]
        /// </example>
        [HttpGet]
        public IEnumerable<Student> GetStudentsInClass(int id)
        {
            // set up list of students
            List<Student> Students = new List<Student>();

            // create a connection
            MySqlConnection Conn = SchoolDbContext.AccessDatabase();

            // open connection
            Conn.Open();

            // create command/query
            MySqlCommand cmd = Conn.CreateCommand();

            // SQL query
            // get students with a classid that matches the input id
            cmd.CommandText = "SELECT * FROM students JOIN studentsxclasses ON students.studentid = studentsxclasses.studentid JOIN classes ON studentsxclasses.classid = classes.classid WHERE classes.classid = @id";

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            // get result set of query
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                // get properties
                int StudentId = Convert.ToInt32(ResultSet["studentid"]);
                string StudentFname = (string)ResultSet["studentfname"];
                string StudentLname = (string)ResultSet["studentlname"];
                string StudentNumber = (string)ResultSet["studentnumber"];
                DateTime EnrolDate = (DateTime)ResultSet["enroldate"];

                // create new Student object (Models/Student.cs)
                Student NewStudent = new Student();

                // set properties of new Student object to values retrieved from database
                NewStudent.StudentId = StudentId;
                NewStudent.StudentFname = StudentFname;
                NewStudent.StudentLname = StudentLname;
                NewStudent.StudentNumber = StudentNumber;
                NewStudent.EnrolDate = EnrolDate.ToString("D");

                // add this new Student to list of students
                Students.Add(NewStudent);
            }

            // close connection
            Conn.Close();

            // return list of Student objects
            return Students;
        }
    }
}
