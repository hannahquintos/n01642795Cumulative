using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using n01642795Cumulative1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text.RegularExpressions;

namespace n01642795Cumulative1.Controllers
{
    // A WebAPI controller which allows you to access information about teachers
    public class TeacherDataController : ApiController
    {
        // SchoolDbContext class to access the database (Models/SchoolDbContext.cs)
        private SchoolDbContext School = new SchoolDbContext();


        /// <summary>
        ///     Returns a list of teachers from the teachers table of the school database. If there is a search input, returns a list of teachers related to the search.
        /// </summary>
        /// <param name="SearchKey"> An optional parameter (null if not provided) of the user's search input (as a string) </param>
        /// <returns>
        ///     A list of Teacher objects that relate to the SearchKey with their properties (teacher objects = rows in teacher table, properties = columns in teacher table for a specific row)
        /// </returns>
        /// <example>
        ///     GET api/TeacherData/ListTeachers --> [Teacher Object{properties}, Teacher Object{properties}, ...] 
        ///     GET api/TeacherData/ListTeachers/5 --> [Teacher Object{...EmployeeNumber:"T385"... }, Teacher Object{...EmployeeNumber:"T505"... }]
        /// </example>
        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]
        public IEnumerable <Teacher> ListTeachers(string SearchKey=null)
        {
            // create a connection
            MySqlConnection Conn = SchoolDbContext.AccessDatabase();

            // open connection
            Conn.Open();

            // create command/query
            MySqlCommand cmd = Conn.CreateCommand();

            // SQL query
            // use SearchKey parameter and match with data in database using WHERE and LIKE
            // allow search of first name, last name, and employee number
            // use lower to convert both the search key input and the data in database to all lowercase - this way the search is not case sensitive
            cmd.CommandText = "Select * from Teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or lower(concat(teacherfname, ' ', teacherlname)) like lower(@key) or lower(employeenumber) like lower(@key)";

            // sanitize the search key input! - to prevent MySQL injection attacks
            // @key in SQL query is replaced by %SearchKey%
            cmd.Parameters.AddWithValue("key", "%" + SearchKey + "%");
            cmd.Prepare();

            // get result set of query
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            // set up list of teachers
            List<Teacher> Teachers = new List<Teacher>();

            // loop through results of query
            while(ResultSet.Read())
            {
                // get properties
                int TeacherId = (int)ResultSet["teacherId"];
                string TeacherFname = (string)ResultSet["teacherfname"];
                string TeacherLname = (string)ResultSet["teacherlname"];
                string EmployeeNumber = (string)ResultSet["employeenumber"];
                DateTime HireDate = (DateTime)ResultSet["hiredate"];
                decimal Salary = (decimal)ResultSet["salary"];

                // create new teacher object (Models/Teacher.cs)
                Teacher NewTeacher = new Teacher();

                // set properties of new teacher object to values retrieved from database
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.EmployeeNumber = EmployeeNumber;
                NewTeacher.HireDate = HireDate.ToString("D"); // converted date to long date format
                NewTeacher.Salary = Salary;

                // add this new teacher to list of teachers
                Teachers.Add(NewTeacher);
            }

            // close connection
            Conn.Close();

            // return list of all teachers
            return Teachers;
        }

        /// <summary>
        ///     Recieves a teacher id and returns the corresponding teacher and their information for that id
        /// </summary>
        /// <param name="id"> The teacher id (as an integer) </param>
        /// <returns>
        ///     A teacher object and its properties
        /// </returns>
        /// <example>
        ///     GET api/TeacherData/FindTeacher/3 --> [Teacher Object{...TeacherFname:Linda, TeacherId:3... }]
        ///     GET api/TeacherData/FindTeacher/5 --> [Teacher Object{...TeacherFname:Jessica, TeacherId:5... }]
        /// </example>
        [HttpGet]
        public Teacher FindTeacher(int id)
        {
            // create new teacher object
            Teacher NewTeacher = new Teacher();

            // create a connection
            MySqlConnection Conn = SchoolDbContext.AccessDatabase();

            // open connection
            Conn.Open();

            // create command/query
            MySqlCommand cmd = Conn.CreateCommand();

            // SQL query
            // get the teacher that has a matching teacherid to the input id
            cmd.CommandText = "Select * from Teachers where teacherid = @id";

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            // get result set of query
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                // get properties
                int TeacherId = (int)ResultSet["teacherId"];
                string TeacherFname = (string)ResultSet["teacherfname"];
                string TeacherLname = (string)ResultSet["teacherlname"];
                string EmployeeNumber = (string)ResultSet["employeenumber"];
                DateTime HireDate = (DateTime)ResultSet["hiredate"];
                decimal Salary = (decimal)ResultSet["salary"];

                // set properties of new teacher object to values retrieved from database
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.EmployeeNumber = EmployeeNumber;
                NewTeacher.HireDate = HireDate.ToString("D");
                NewTeacher.Salary = Salary;
            }

            // close connection
            Conn.Close();

            // return a single teacher object
            return NewTeacher;
        }

        /// <summary>
        ///     Recieves a teacher id and deletes the corresponding teacher from the database. Maintains refential integrity by also deleting any classes pointing to the teacher.
        /// </summary>
        /// <param name="id"> The teacher id (as an integer) </param>
        /// <example>
        ///     POST: /api/TeacherData/DeleteTeacher/2
        /// </example>

        [HttpPost]
        public void DeleteTeacher(int id)
        {
            // create a connection
            MySqlConnection Conn = SchoolDbContext.AccessDatabase();

            // open connection
            Conn.Open();

            // to maintain referential integrity - delete query for classes pointing to teacher being deleted
            // create command/query
            MySqlCommand cmd1 = Conn.CreateCommand();

            // SQL query
            cmd1.CommandText = "Delete from classes where teacherid=@id";
            cmd1.Parameters.AddWithValue("@id", id);
            cmd1.Prepare();

            // execute non select query
            cmd1.ExecuteNonQuery();

            // create command/query
            MySqlCommand cmd2 = Conn.CreateCommand();

            // SQL query
            cmd2.CommandText = "Delete from teachers where teacherid=@id";
            cmd2.Parameters.AddWithValue("@id", id);
            cmd2.Prepare();

            // execute non select query
            cmd2.ExecuteNonQuery();

            // close connection
            Conn.Close();
        }

        /// <summary>
        ///     Adds a new teacher to the database.
        /// </summary>
        /// <param name="NewTeacher"> The teacher object with properties that match coloumns of teacher table in the database </param>
        /// <example>
        ///    POST: /api/TeacherData/AddTeacher
        /// </example>
        [HttpPost]
        public void AddTeacher([FromBody]Teacher NewTeacher) // added [FromBody] to make sure AddTeacher method is supported by Web API
        {
            // server side validation - ensure there is no missing information before adding to the database
            string salaryExpression = @"^-?\d+(\.\d+)?$"; // regex expression to check if input salary is in decimal format
            if (NewTeacher.TeacherFname == "" || NewTeacher.TeacherLname == "" || NewTeacher.EmployeeNumber == "" || NewTeacher.HireDate == "" || Regex.IsMatch(NewTeacher.Salary.ToString(), salaryExpression) == false) return;

            // create a connection
            MySqlConnection Conn = SchoolDbContext.AccessDatabase();

            // open connection
            Conn.Open();

            // create command/query
            MySqlCommand cmd = Conn.CreateCommand();

            // SQL query
            cmd.CommandText = "Insert into teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) values (@TeacherFname, @TeacherLname, @EmployeeNumber, @HireDate, @Salary)";
            
            cmd.Parameters.AddWithValue("@TeacherFname", NewTeacher.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", NewTeacher.TeacherLname);
            cmd.Parameters.AddWithValue("@EmployeeNumber", NewTeacher.EmployeeNumber);
            cmd.Parameters.AddWithValue("@HireDate", NewTeacher.HireDate);
            cmd.Parameters.AddWithValue("@Salary", NewTeacher.Salary);

            cmd.Prepare();

            // execute non select query
            cmd.ExecuteNonQuery();

            // close connection
            Conn.Close();

        }

    }

}
