using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolDataSet.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Sex { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }


    }
}