using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamPartCa.Models
{
    public class NextOfKinModel
    {
        public int NextOfKinId { get; set; }

        public String FirstName { get; set; }

        public String Surname { get; set; }

        public int PatientNumber { get; set; } //the id of the patient who listed them as a nex of kin
    }
}
