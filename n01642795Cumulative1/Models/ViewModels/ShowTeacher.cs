using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace n01642795Cumulative1.Models.ViewModels
{
    // use this ViewModel to display courses taught by a teacher
    public class ShowTeacher
    {
        public Teacher Teacher;
        public IEnumerable<Course> CoursesTaught;
    }
}