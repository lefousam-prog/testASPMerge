using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolDataSet.Classes;
using SchoolDataSet.Models;
using System.Data;

namespace SchoolDataSet.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            bool status = DataLibrary.TestConnection();
            List<Student> student = new List<Student>();
            DataSet dataset = new DataSet();
            DataTable datatable = new DataTable();
            dataset = Classes.DataLibrary.GetData("select * from Students ");
            datatable = Classes.Library.DataSetToDataTable(dataset);
            foreach (DataRow item in datatable.Rows)
            {
                Student stu = new Student();
                stu.Id = Convert.ToInt32(item["Id"].ToString());
                stu.Name = item["Names"].ToString();
                stu.LastName  = item["LastName"].ToString();
                //stu.BirthDate = Convert.ToDateTime(item["Birthdate"].ToString());
                stu.Sex = item["Sex"].ToString();
                stu.Address = item["Adress"].ToString();
                stu.Phone = item["Phone"].ToString();
                student.Add(stu);
            }

            ViewData["students"] = student;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}