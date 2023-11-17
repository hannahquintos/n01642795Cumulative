using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace n01642795Cumulative1.Models.ViewModels
{
    // use this ViewModel to display students enrolled in a course
    public class ShowCourse
    {
        public Course Course;
        public IEnumerable<Student> StudentsEnrolled;
    }
}