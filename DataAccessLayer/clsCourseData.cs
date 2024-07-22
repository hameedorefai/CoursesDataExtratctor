using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class clsCourseData
    {
        static public bool InsertCourseIntoDatabase(
            string courseName,
            string courseCode,
            string courseDescription,
            int creditHours,
            int theoryHours,
            int practicalHours,
            string prerequisiteCourse,
            string majorThatInsertedFrom)
        {
            using (SqlConnection connection = new SqlConnection(Settings.ConnectionString))
            {
                connection.Open();

                try
                {
                    string query = @"
            INSERT INTO Courses (CourseName, CourseDescription, CourseCode, CreditHours, TheoryHours, PracticalHours, PrerequisiteCourse, MajorThatInsertedFrom)
            VALUES (@CourseName, @CourseDescription, @CourseCode, @CreditHours, @TheoryHours, @PracticalHours, @PrerequisiteCourse, @MajorThatInsertedFrom);";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CourseName", courseName ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@CourseDescription", courseDescription ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@CourseCode", courseCode ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@CreditHours", creditHours);
                        command.Parameters.AddWithValue("@TheoryHours", theoryHours);
                        command.Parameters.AddWithValue("@PracticalHours", practicalHours);
                        command.Parameters.AddWithValue("@PrerequisiteCourse", prerequisiteCourse ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@MajorThatInsertedFrom", majorThatInsertedFrom ?? (object)DBNull.Value);

                        int rowsAffected = command.ExecuteNonQuery();
                        return true;
                        // for debugging purposes
                        //Console.WriteLine("Course Inserted:");
                        //Console.WriteLine($"  CourseName: {courseName}");
                        //Console.WriteLine($"  CourseCode: {courseCode}");
                        //Console.WriteLine($"  CourseDescription: {courseDescription}");
                        //Console.WriteLine($"  CreditHours: {creditHours}");
                        //Console.WriteLine($"  TheoryHours: {theoryHours}");
                        //Console.WriteLine($"  PracticalHours: {practicalHours}");
                        //Console.WriteLine($"  PrerequisiteCourse: {prerequisiteCourse}");
                        //Console.WriteLine($"  MajorThatInsertedFrom: {majorThatInsertedFrom}");
                        //Console.WriteLine($"  Rows affected: {rowsAffected}");
                        //Console.WriteLine();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error inserting course '{courseName}': {ex.Message}");
                    return false;
                }
            }
        }
    }
}
