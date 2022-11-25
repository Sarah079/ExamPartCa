using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamPartCa.Models
{
    public class DbConnectionStatusModel
    {
        public static String Works = "Well done. Your code works.";
        public static String DWorks = "Does not work yet. Something went wrong with your code";

        public bool Status { get; set; }
        public String Message { get; set; }
    }
}
