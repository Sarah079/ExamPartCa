using ExamPartCa.Models;
using ExamPartCa.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ExamPartCa.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MSSQLService _dbService;

        public HomeController(ILogger<HomeController> logger, MSSQLService dbService)
        {
            _logger = logger;
            _dbService = dbService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SectionCQuestion1a() {

            bool connectionStatus = _dbService.connectToDB();

            //View Model for passing the connection status and a nice message to the question (a) view.
            DbConnectionStatusModel conModel = new DbConnectionStatusModel {
                Status = connectionStatus,
                Message = connectionStatus? DbConnectionStatusModel.Works : DbConnectionStatusModel.DWorks

            };

            return View(conModel);
        }


        public IActionResult SectionCQuestion1b()
        {

            //connecting to the database
            bool connectionStatus = _dbService.connectToDB();
            DbConnectionStatusModel conModel = new DbConnectionStatusModel
            {
                Status = connectionStatus,
                Message = connectionStatus ? DbConnectionStatusModel.Works : DbConnectionStatusModel.DWorks

            };


            //getting the patients and all next of kins
            IEnumerable<PatientModel> patients = _dbService.getPatients();


            //calculating the slowest retrieval
            long slowestInMemoryQuery = -1;
            foreach (PatientModel patient in patients) {
                Stopwatch watch = System.Diagnostics.Stopwatch.StartNew(); //starting stop watch to count the method execution
                
                
                IEnumerable<NextOfKinModel>  nextOfKins = _dbService.getPatientsNextOfKinsMem(patient.PatientNumber);


                watch.Stop(); //stopping the watch
                long elapsedMs = watch.ElapsedMilliseconds;
                if (elapsedMs > slowestInMemoryQuery)
                {
                    slowestInMemoryQuery = elapsedMs;
                }
            }


            //calculating the slowest retrieval
            long slowestRemoteQuery = -1;
            foreach (PatientModel patient in patients)
            {
                Stopwatch watch = System.Diagnostics.Stopwatch.StartNew(); //starting stop watch to count the method execution
                
                IEnumerable<NextOfKinModel> nextOfKins = _dbService.getPatientsNextOfKinsRem(patient.PatientNumber);

                watch.Stop(); //stopping the watch
                long elapsedMs = watch.ElapsedMilliseconds;
                if (elapsedMs > slowestRemoteQuery) {
                    slowestRemoteQuery = elapsedMs;
                }
            }

            //View Model for passing the connection status and measurements to the question (b) view.
            CrudeDBMeasurementModel measurementsModel = new CrudeDBMeasurementModel
            {
                ConnectionStatus = conModel,
                SlowestInMemoryQuery = slowestInMemoryQuery,
                SlowestRemoteQuery = slowestRemoteQuery
            };


            return View(measurementsModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
