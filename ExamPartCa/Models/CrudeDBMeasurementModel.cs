using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamPartCa.Models
{
    public class CrudeDBMeasurementModel
    {
        public DbConnectionStatusModel ConnectionStatus { get; set; }

        public long SlowestInMemoryQuery { set; get; }

        public long SlowestRemoteQuery { set; get; }
    }
}
