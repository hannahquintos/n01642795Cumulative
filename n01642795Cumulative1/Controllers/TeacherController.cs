using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using n01642795Cumulative1.Models;
using n01642795Cumulative1.Models.ViewModels;
using ZstdSharp.Unsafe;
using System.Diagnostics;

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

        // GET: /Teacher/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = controller.FindTeacher(id);
            return View(NewTeacher);
        }

        // POST: /Teacher/Delete/{id}
        public ActionResult Delete(int id)
        {
            TeacherDataController Controller = new TeacherDataController();
            Controller.DeleteTeacher(id);
            return RedirectToAction("List");
        }

        // GET: /Teacher/New
        public ActionResult New()
        {
            return View();
        }

        // POST: /Teacher/Create
        [HttpPost]
        public ActionResult Create(string TeacherFname, string TeacherLname, string EmployeeNumber, string HireDate, string Salary)
        {
            // use Debug.WriteLine to confirm route is correct and informaton is passed from files
                // Debug.WriteLine("Accessed create method");
                // Debug.WriteLine(TeacherFname);
                // Debug.WriteLine(TeacherLname);
                // Debug.WriteLine(EmployeeNumber);
                // Debug.WriteLine(HireDate);
                // Debug.WriteLine(Salary);

            // create new teacher object
            Teacher NewTeacher = new Teacher();

            // set properties of new teacher object to the parameter values of the form inputs
            NewTeacher.TeacherFname = TeacherFname;
            NewTeacher.TeacherLname = TeacherLname;
            NewTeacher.EmployeeNumber = EmployeeNumber;
            NewTeacher.HireDate = HireDate;
            NewTeacher.Salary = Convert.ToDecimal(Salary);

            // create new instance of TeacherDataController to access database
            TeacherDataController controller = new TeacherDataController();

            // add teacher to the database
            controller.AddTeacher(NewTeacher);

            return RedirectToAction("List");
        }
    }
}