using HTTP5112_Assignment3_CarrieNg.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace HTTP5112_Assignment3_CarrieNg.Controllers
{
    public class TeacherDataController : ApiController
    {
        private SchoolDbContext school = new SchoolDbContext();

        /// <summary>
        /// contacts the database and returns articles in the teachers table. Search Bar can be used to seach for specific teachers by their first and lastnames
        /// </summary>
        /// <example> 
        /// GET: api/TeacherData/ListTeachers/{SearchKey?} -> "Linda Chan"
        /// </example>
        /// <returns>
        /// a list of all the teachers first and last names, when you select each teacher you can see all information of teacher from database
        /// </returns>

        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]
        public IEnumerable<Teacher> ListTeachers(string SearchKey=null)
        {
            //Create a connection
            MySqlConnection Conn = school.AccessDatabase();

            //Open the connection between the webserver and database -> School database
            Conn.Open();

            //make a command/query for the database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or lower(concat (teacherfname, ' ', teacherlname)) like lower(@key)";

            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultsSet = cmd.ExecuteReader();

            //Create an empty list of teacher Names
            List<Teacher> Teachers = new List<Teacher>();

            //Loop through each row the Result Set
            while (ResultsSet.Read())
            {
                int TeacherId = (int)ResultsSet["teacherid"];
                string TeacherFname = (string)ResultsSet["teacherfname"];
                string TeacherLname = (string)ResultsSet["teacherlname"];
                string EmpolyeeNumber = (string)ResultsSet["employeenumber"];
                double Salary = Double.Parse((ResultsSet["salary"].ToString()));
                
                DateTime HireDate = (DateTime)ResultsSet["hiredate"];

                Teacher NewTeacher = new Teacher();
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.EmployeeNumber = EmpolyeeNumber;
                NewTeacher.Salary = Salary;
                NewTeacher.HireDate = HireDate;
                //string TeacherName = ResultsSet["teacherfname"] + " " + ResultsSet["teacherlname"];

                Teachers.Add(NewTeacher);

            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of teachers
            return Teachers;

        }
        [HttpGet]
        public Teacher FindTeacher(int id)
        {
            Teacher NewTeacher = new Teacher();

            //Create a connection
            MySqlConnection Conn = school.AccessDatabase();

            //Open the connection between the webserver and database
            Conn.Open();

            //make a command/query for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from teachers where teacherid ="+id;

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultsSet = cmd.ExecuteReader();

            //Loop through each row the Result Set
            while (ResultsSet.Read())
            {
                int TeacherId = (int)ResultsSet["teacherid"];
                string TeacherFname = (string)ResultsSet["teacherfname"];
                string TeacherLname = (string)ResultsSet["teacherlname"];
                string EmpolyeeNumber = (string)ResultsSet["employeenumber"];
                double Salary = Double.Parse((ResultsSet["salary"].ToString()));
                DateTime HireDate = (DateTime)ResultsSet["hiredate"];

                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.EmployeeNumber = EmpolyeeNumber;
                NewTeacher.Salary = Salary;
                NewTeacher.HireDate = HireDate;
            }

            //Return the final list of teachers
            return NewTeacher;

        }
    }
}
