using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ExamPartCa.Models;

namespace ExamPartCa.Services
{//instructions found below brackets
    public class MSSQLService
    {
        protected String connectionString;
        protected SqlConnection dbConnection;

        public MSSQLService() //-------------------------------------
        {
            SqlConnectionStringBuilder stringBuilder = new SqlConnectionStringBuilder();

            //TODO: Complete this connection string and correct any problems it currently has

            stringBuilder["Data Source"] = "."; //pc
            stringBuilder["Initial Catalogs"] = "Daspoort_Clinic"; //db name
            stringBuilder["Integrated Security"] = true; //stops errors
            connectionString = stringBuilder.ConnectionString; //referencing the sting so can use it over and over 
        }

        public IEnumerable<PatientModel> getPatients()
        {
            List<PatientModel> allPatients = new List<PatientModel>(); // only using patient model info. not whole db table
            connectToDB(); 

            String command = "select * from dbo.Patients"; //collect everything but only declare and add aspects in PatientModel
            using(SqlCommand cmd = new SqlCommand( command, dbConnection))
            {

                SqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    PatientModel p = new PatientModel(); //patient model is referenced as p from here

                    p.PatientNumber = (int)reader["PatientNumber"]; 
                    p.Name = (string)reader["PatientName"];
                    p.Surname = (string)reader["PatientSurname"];
                    p.Title = (int)reader["PatientTitle"]; //strong convert = ].Toint();

                    allPatients.Add(p);
                }
            }

            disConnectFromDB(); //remember opening and closing db.This method is found below
            
            return allPatients;
        }

        public IEnumerable<NextOfKinModel> getNextOfKins()
        {//basically same as getPatients just read different info

            List<NextOfKinModel> allNextofKins = new List<NextOfKinModel>();
            connectToDB();

            String command = "select * from dbo.NextOfKin";
            using (SqlCommand cmd = new SqlCommand(command, dbConnection))
            {
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    NextOfKinModel kin = new NextOfKinModel();
                    kin.NextOfKinId = (int)reader["NextOfKinId"];
                    kin.FirstName = (string)reader["FirstName "];
                    kin.Surname = (string)reader["Surname"];
                    kin.PatientNumber = (int)reader["PatientNumber"];
                    allNextofKins.Add(kin);
                }
            }
                disConnectFromDB();
            return allNextofKins;
        }

        //1marks
        public IEnumerable<NextOfKinModel> getPatientsNextOfKinsMem(int patientNumber)
        {//Linq lambda where statement where patient number equals the patientnumber from the patient list

            List<NextOfKinModel> nextofKinsOfPatient = new List<NextOfKinModel>(); 
            
            nextofKinsOfPatient = getNextOfKins().Where(x => x.PatientNumber == patientNumber).ToList();
            //can just referencing getNextOfKins() istead of rereading the data
            return nextofKinsOfPatient;
        }

        //2 marks
        public IEnumerable<NextOfKinModel> getPatientsNextOfKinsRem(int patientNumber)
        {
            List< NextOfKinModel > nextofKinsOfPatient = new List<NextOfKinModel>();
            connectToDB();//because they opened and closed they want us to NOT use a previous method ----!!!!

            //TODO: retrieve nextofkin from the database that is only associated with the relevent patient

            String command = "select * from dbo.NextOfKin where PatientNumber !=" + patientNumber;
            using (SqlCommand cmd = new SqlCommand(command, dbConnection))
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    NextOfKinModel p = new NextOfKinModel();
                    p.NextOfKinId = (int)reader["NextOfKinId"];
                    p.FirstName = (string)reader["FirstName "];
                    p.Surname = (string)reader["Surname"];
                    p.PatientNumber = (int)reader["PatientNumber"];
                    nextofKinsOfPatient.Add(p);
                }
            }

            disConnectFromDB();
            return nextofKinsOfPatient;
        }



        //Don't mind the methods below.
        public bool connectToDB()
        {
            try
            {
                dbConnection = new SqlConnection(connectionString);
                dbConnection.Open();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool disConnectFromDB()
        {
            try
            {
                dbConnection.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }

}
//------------------- getPatients() --------------
//read all patuents from db
// do not set all attributes found in table only the ones found in PatientModel class

//------------------- getPatientsNextOfKinsMem() --------------
//TODO: filter the nextOfKinds returned by getNextOfKins() and return only ones that are associated with the patients
//Hint: You do not need to return the nextofKinsOfPatient object. You can construct your own list.

