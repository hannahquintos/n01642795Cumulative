using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using n01642795Cumulative1.Models;
using n01642795Cumulative1.Models.ViewModels;
using ZstdSharp.Unsafe;

namespace n01642795Cumulative1.Controllers
{
    // A controller which allows you to route to dynamic pages
    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Teacher/List
        // GET: /Teacher/List/SearchKey
        public ActionResult List(string SearchKey = null)
        {
            // connect controller to web api controller
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Teacher> Teachers = controller.ListTeachers(SearchKey);
            // pass list of teacher objects to /Views/Teacher/List.cshtml
            return View(Teachers);
        }

        // GET: /Teacher/Show/{id}
        public ActionResult Show(int id)
        {
            /* using ViewModel */
            ShowTeacher ViewModel = new ShowTeacher();
            TeacherDataController TeacherDataController = new TeacherDataController();
            CourseDataController CourseDataController = new CourseDataController(); 
            Teacher SelectedTeacher = TeacherDataController.FindTeacher(id);
            IEnumerable<Course> CoursesTaught = CourseDataController.GetCoursesTaughtByTeacher(id);
            ViewModel.Teacher = SelectedTeacher;
            ViewModel.CoursesTaught = CoursesTaught;

            return View(ViewModel);

            /* without ViewModel */
            /* TeacherDataController controller = new TeacherDataController();
             Teacher NewTeacher = controller.FindTeacher(id);
             // pass teacher object to /Views/Teacher/Show.cshtml
             return View(NewTeacher);*/
        }
    }
}