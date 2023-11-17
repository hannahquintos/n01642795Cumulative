using n01642795Cumulative1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace n01642795Cumulative1.Controllers
{
    // A controller which allows you to route to dynamic pages
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Student/List
        // GET: /Student/List/SearchKey
        public ActionResult List(string SearchKey = null)
        {
            // connect controller to web api controller
            StudentDataController controller = new StudentDataController();
            IEnumerable<Student> Students = controller.ListStudents(SearchKey);
            // pass list of student objects to /Views/Student/List.cshtml
            return View(Students);
        }

        // GET: /Student/Show/{id}
        public ActionResult Show(int id)
        {
            StudentDataController controller = new StudentDataController();
            Student NewStudent = controller.FindStudent(id);
            // pass student object to /Views/Student/Show.cshtml
            return View(NewStudent);
        }
    }
}