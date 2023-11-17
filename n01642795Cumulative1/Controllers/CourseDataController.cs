using MySql.Data.MySqlClient;
using n01642795Cumulative1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Numerics;
using System.Web.Http;

namespace n01642795Cumulative1.Controllers
{
    // A WebAPI controller which allows you to access information about classes
    public class CourseDataController : ApiController
    {
        // SchoolDbContext class to access the database (Models/SchoolDbContext.cs)
        private SchoolDbContext School = new SchoolDbContext();


        /// <summary>
        ///     Returns a list of classes from the classes table of the school. If there is a search input, returns a list of classes related to the search.
        /// </summary>
        /// <param name="SearchKey"> An optional parameter (null if not provided) of the user's search input (as a string) </param>
        /// <returns>
        ///     A list of Course objects that relate to the SearchKey with their properties (course objects = rows in class table, properties = columns in class table for a specific row)
        /// </returns>
        /// <example>
        ///     GET api/CourseData/ListCourses --> [Course Object{properties}, Course Object{properties}, ...]
        ///     GET api/CourseData/ListCourses/pro --> [Course Object{...ClassName:"Project Management"... }, Course Object{...ClassName:"Web Programming"... }]
        /// </example>
        [HttpGet]
        [Route("api/CourseData/ListCourses/{SearchKey?}")]
        public IEnumerable<Course> ListCourses(string SearchKey = null)
        {
            // create a connection
            MySqlConnection Conn = SchoolDbContext.AccessDatabase();

            // open connection
            Conn.Open();

            // create command/query
            MySqlCommand cmd = Conn.CreateCommand();

            // SQL query
            // use SearchKey parameter and match with data in database using WHERE and LIKE
            // allow search of class name and class code
            // use lower to convert both the search key input and the data in database to all lowercase - this way the search is not case sensitive
            cmd.CommandText = "Select * from Classes where lower(classname) like lower(@key) or lower(classcode) like lower(@key)";

            // sanitize the search key input! - to prevent MySQL injection attacks
            // @key in SQL query is replaced by %SearchKey%
            cmd.Parameters.AddWithValue("key", "%" + SearchKey + "%");
            cmd.Prepare();

            // get result set of query
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            // set up list of classes
            List<Course> Courses = new List<Course>();

            // loop through results of query
            while (ResultSet.Read())
            {
                // get properties
                int ClassId = (int)ResultSet["classId"];
                string ClassCode = (string)ResultSet["classcode"];
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string StartDate = ResultSet["startdate"].ToString();
                string FinishDate = ResultSet["finishdate"].ToString();
                string ClassName = ResultSet["classname"].ToString();

                // create new course object (Models/Course.cs)
                Course NewCourse= new Course();

                // set properties of new course object to values retrieved from database
                NewCourse.ClassId = ClassId;
                NewCourse.ClassCode = ClassCode;
                NewCourse.TeacherId = TeacherId;
                NewCourse.StartDate = StartDate;
                NewCourse.FinishDate = FinishDate;
                NewCourse.ClassName = ClassName;

                // add this new course to list of courses
                Courses.Add(NewCourse);
            }

            // close connection
            Conn.Close();

            // return list of all courses
            return Courses;
        }

        /// <summary>
        ///     Recieves a class id and returns the corresponding class and information for that id
        /// </summary>
        /// <param name="id"> The class id (as an integer) </param>
        /// <returns>
        ///     A course object and its properties
        /// </returns>
        /// <example>
        ///     GET api/CourseData/FindCourse/3 --> [Course Object{...ClassId:3, ClassName:"Web Programming"... }]
        ///     GET api/CourseData/FindCourse/5 --> [Course Object{...ClassId:5, ClassName:"Database Development"... }]
        /// </example>
        [HttpGet]
        public Course FindCourse(int id)
        {
            // create new Course object
            Course NewCourse = new Course();

            // create a connection
            MySqlConnection Conn = SchoolDbContext.AccessDatabase();

            // open connection
            Conn.Open();

            // create command/query
            MySqlCommand cmd = Conn.CreateCommand();

            // SQL query
            // get the class that has a matching classid to the input id
            cmd.CommandText = "Select * from Classes where classid = " + id;

            // get result set of query
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                // get properties
                int ClassId = (int)ResultSet["classId"];
                string ClassCode = (string)ResultSet["classcode"];
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                DateTime StartDate = (DateTime)ResultSet["startdate"];
                DateTime FinishDate = (DateTime)ResultSet["finishdate"];
                string ClassName = ResultSet["classname"].ToString();

                // set properties of new course object to values retrieved from database
                NewCourse.ClassId = ClassId;
                NewCourse.ClassCode = ClassCode;
                NewCourse.TeacherId = TeacherId;
                NewCourse.StartDate = StartDate.ToString("D");
                NewCourse.FinishDate = FinishDate.ToString("D");
                NewCourse.ClassName = ClassName;
            }

            // return a single course object
            return NewCourse;
        }

        /// <summary>
        ///     Recieves a teacher id and returns a list of classes of which the corresponding teacher teaches
        /// </summary>
        /// <param name="id">The teacher id (as an integer)</param>
        /// <returns>
        ///     A list of Course objects with their properties
        /// </returns>
        /// <example>
        ///     GET api/CourseData/GetCoursesTaughtByTeacher/1 --> [Course Object{ClassCode:"http5101", ... TeacherId:1... }]
        /// </example>
        [HttpGet]
        public IEnumerable<Course> GetCoursesTaughtByTeacher(int id)
        {
            // set up list of classes
            List<Course> Courses = new List<Course>();

            // create a connection
            MySqlConnection Conn = SchoolDbContext.AccessDatabase();

            // open connection
            Conn.Open();

            // create command/query
            MySqlCommand cmd = Conn.CreateCommand();

            // SQL query
            // get the classes with a teacherid that matches the input id
            cmd.CommandText = "Select * from Classes where teacherid = " + id;

            // get result set of query
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                // get properties
                int ClassId = (int)ResultSet["classId"];
                string ClassCode = (string)ResultSet["classcode"];
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                DateTime StartDate = (DateTime)ResultSet["startdate"];
                DateTime FinishDate = (DateTime)ResultSet["finishdate"];
                string ClassName = ResultSet["classname"].ToString();

                // create new course object (Models/Course.cs)
                Course NewCourse = new Course();

                // set properties of new course object to values retrieved from database
                NewCourse.ClassId = ClassId;
                NewCourse.ClassCode = ClassCode;
                NewCourse.TeacherId = TeacherId;
                NewCourse.StartDate = StartDate.ToString("D");
                NewCourse.FinishDate = FinishDate.ToString("D");
                NewCourse.ClassName = ClassName;

                // add this new course to list of courses
                Courses.Add(NewCourse);
            }

            // return list of Course objects
            return Courses;
        }
    }
}
