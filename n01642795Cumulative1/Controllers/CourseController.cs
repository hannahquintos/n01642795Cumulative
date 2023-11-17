using n01642795Cumulative1.Models;
using n01642795Cumulative1.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace n01642795Cumulative1.Controllers
{
    // A controller which allows you to route to dynamic pages
    public class CourseController : Controller
    {
        // GET: Course
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Course/List
        // GET: /Course/List/SearchKey

        public ActionResult List(string SearchKey = null)
        {
            // connect controller to web api controller
            CourseDataController controller = new CourseDataController();
            IEnumerable<Course> Courses = controller.ListCourses(SearchKey);
            // pass list of course objects to /Views/Course/List.cshtml
            return View(Courses);
        }

        // GET: /Course/Show/{id}
        public ActionResult Show(int id)
        {
            /* using ViewModel */
            ShowCourse ViewModel = new ShowCourse();
            CourseDataController CourseDataController = new CourseDataController();
            StudentDataController StudentDataController = new StudentDataController();
            Course SelectedCourse = CourseDataController.FindCourse(id);
            IEnumerable<Student> StudentsEnrolled = StudentDataController.GetStudentsInClass(id);
            ViewModel.Course = SelectedCourse;
            ViewModel.StudentsEnrolled = StudentsEnrolled;

            return View(ViewModel);

            /* without ViewModel */
            /* CourseDataController controller = new CourseDataController();
             Course NewCourse = controller.FindCourse(id);
             // pass course object to /Views/Course/Show.cshtml
             return View(NewCourse);*/
        }
    }
}