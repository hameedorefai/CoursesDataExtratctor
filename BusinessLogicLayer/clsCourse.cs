using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesInfoScrapping
{
    public class clsCourse
    {
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public string CourseDescription { get; set; }
        public int CreditHours { get; set; }
        public int TheoryHours { get; set; }
        public int PracticalHours { get; set; }
        public string PrerequisiteCourse { get; set; }
        public string MajorThatInsertedFrom { get; set; }

        public clsCourse()
        {
            CourseName = "";
            CourseCode = "";
            CourseDescription = "";
            CreditHours = -1;
            TheoryHours = -1;
            PracticalHours = -1;
            PrerequisiteCourse = "";
            MajorThatInsertedFrom = "";
        }

        static public bool InsertCourseIntoDatabase(clsCourse course)
        {
            return clsCourseData.InsertCourseIntoDatabase(course.CourseName, course.CourseCode,
                                                                course.CourseDescription,
                                               course.CreditHours, course.TheoryHours, course.PracticalHours,
                                               course.PrerequisiteCourse, course.MajorThatInsertedFrom);
        }
    }
}
