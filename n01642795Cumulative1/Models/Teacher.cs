using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace n01642795Cumulative1.Models
{
    // A model which allows you to represent information about a teacher
    public class Teacher
    {
        // properties of a teacher
        public int TeacherId;
        public string TeacherFname;
        public string TeacherLname;
        public string EmployeeNumber;
        public string HireDate;
        public decimal Salary;

        // parameter-less constructor function
        public Teacher() { }
    }
}