using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DashBoardWebAPI.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public String EmployeeName { get; set; }
        public int isAdmin { get; set; }
        public string password { get; set; } = "";
    }
}
