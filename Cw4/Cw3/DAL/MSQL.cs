using Cw3.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.DAL
{
    public class MSQL : IDbService
    {
        public void createStudent(Student student)
        {
            throw new NotImplementedException();
        }



        public IEnumerable<Student> GetStudents()
        {
            initTable();
            List<Student> students = null;
            using (var con = new SqlConnection(
                "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\cw3-master\\Cw3\\Cw3\\localDB.mdf;Integrated Security=True;Connect Timeout=30"
                // "Data Source=db-mssql;Initial Catalog=s16701;Integrated Security=True"
                ))
            using (var com = new SqlCommand())
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "SELECT IndexNumber, FirstName, LastName, BirthDate FROM dbo.Student";
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Student();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.BirthDate = dr["BirthDate"].ToString();


                    if (students == null)
                    {
                        students = new List<Student> { st };
                    }
                    else
                    {

                        students.Add(st);
                    }

                }

            }
            
            return students ?? new List<Student> { };
        }

        public IEnumerable<Enrollment> GetStudentsSemestr(string index)
        {
            initTable();
            List<Enrollment> enrollments = new List<Enrollment> { };
            using (var con = new SqlConnection(
                "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\cw3-master\\Cw3\\Cw3\\localDB.mdf;Integrated Security=True;Connect Timeout=30"
                // "Data Source=db-mssql;Initial Catalog=s16701;Integrated Security=True"
                ))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();
                com.CommandText = "SELECT e.* FROM dbo.Student s " +
                                        "INNER JOIN dbo.Enrollment e " +
                                        "ON s.IdEnrollment = e.IdEnrollment and s.IndexNumber = @id";
                com.Parameters.AddWithValue("id", index);
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Enrollment();
                    st.IdStudy = Int32.Parse(dr["IdStudy"].ToString());
                    st.Semester = Int32.Parse(dr["Semeester"].ToString());
                    st.IdEnrollment = Int32.Parse(dr["IdEnrollment"].ToString());
                    st.StartDate = dr["StartDate"].ToString();

                    enrollments.Add(st);
                }

            }
            return enrollments;
        }

        public void removeId(int id)
        {
            throw new NotImplementedException();
        }

        public void updateStudent(int id, Student student)
        {
            throw new NotImplementedException();
        }

        private static void initTable()
        {
            using (SqlConnection connection = new SqlConnection(
                "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\cw3-master\\Cw3\\Cw3\\localDB.mdf;Integrated Security=True;Connect Timeout=30"
                // "Data Source=db-mssql;Initial Catalog=s16701;Integrated Security=True"
                ))
            {
                SqlCommand command = new SqlCommand(@"

CREATE TABLE IF NOT EXISTS[dbo].[Studies]
(

    [IdStudy] INT NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL
)
CREATE TABLE IF NOT EXISTS[dbo].[Enrollment]
(

    [IdEnrollment]   INT NOT NULL PRIMARY KEY, 
    [Semester] int NOT NULL, 
    [IdStudy] int NOT NULL, 
    [StartDate] date NOT NULL
    FOREIGN KEY(IdStudy) REFERENCES Studies(IdStudy)
)
CREATE TABLE IF NOT EXISTS[dbo].[Student]
        (

   [IndexNumber] NVARCHAR(100) NOT NULL PRIMARY KEY,

   [FirstName] NVARCHAR(100) NOT NULL,

   [LastName] NVARCHAR(100) NOT NULL,

   [BirthDate] DATE NOT NULL, 
    [IdEnrollment] INT NOT NULL
    FOREIGN KEY(IdEnrollment) REFERENCES Enrollment(IdEnrollment)
)"
                    
                    
                    , connection);
                command.Connection.Open();
                try
                {

                    command.ExecuteNonQuery();
                }
                catch (Exception e) { }

            }
        }
    }
}
