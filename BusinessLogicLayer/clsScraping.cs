using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using DataAccessLayer;
using CoursesInfoScrapping;

namespace BusinessLogicLayer
{
    public class clsScraping
    {

        public static bool StartScraping(string directoryPath = "")
        {
            if (directoryPath == "")
                return false;



                string[] filePaths = Directory.GetFiles(directoryPath, "*.html");

                if (filePaths.Length == 0)
                {
                    Console.WriteLine("No HTML files found in the directory.");
                    return;
                }

                foreach (string filePath in filePaths)
                {
                    var doc = new HtmlDocument();
                    doc.Load(filePath, Encoding.UTF8);

                    var courseSections = doc.DocumentNode.SelectNodes("//div[@class='panel panel-default']");

                    if (courseSections == null)
                    {
                        Console.WriteLine($"No course sections found in {Path.GetFileName(filePath)}.");
                        continue;
                    }

                List < clsCourse> courses = ProcessCourseSections(courseSections, Path.GetFileNameWithoutExtension(filePath));

                foreach(var course in courses)
                {
                    clsCourse.InsertCourseIntoDatabase(course);

                }

                Console.WriteLine($"Total courses processed from {Path.GetFileName(filePath)}: {courses.Count}");
                }
            return true;
        }

        static List<clsCourse> ProcessCourseSections(HtmlNodeCollection courseSections, string major)
        {
            List<clsCourse> courses = new List<clsCourse>();

            foreach (var courseSection in courseSections)
            {
                var courseNameNode = courseSection.SelectSingleNode(".//a[@class='collapsed']");
                if (courseNameNode != null)
                {
                    try
                    {
                        clsCourse course = new clsCourse();


                        string courseNameCode = courseNameNode.InnerText.Trim();
                        int slashIndex = courseNameCode.IndexOf('/');
                        if (slashIndex >= 0)
                        {
                            course.CourseName = courseNameCode.Substring(slashIndex + 1).Trim();
                            course.CourseCode = courseNameCode.Substring(0, slashIndex).Trim();
                        }

                        var courseDescriptionNode = courseSection.SelectSingleNode(".//div[@class='panel-body']");
                        if (courseDescriptionNode != null)
                        {
                            string courseDescription = courseDescriptionNode.InnerText.Trim();

                            ExtractHours(courseDescription, out int creditHours, out int theoryHours, out int practicalHours);
                            course.CreditHours = creditHours;
                            course.TheoryHours = theoryHours;
                            course.PracticalHours = practicalHours;

                            course.PrerequisiteCourse = ExtractPrerequisites(courseDescription);

                            course.CourseDescription = RemoveHoursAndPrerequisites(courseDescription);
                        }

                        course.MajorThatInsertedFrom = major;

                        // for debugging purposes
                        Console.WriteLine("clsCourse Processed:");
                        Console.WriteLine($"  CourseName: {course.CourseName}");
                        Console.WriteLine($"  CourseCode: {course.CourseCode}");
                        Console.WriteLine($"  CourseDescription: {course.CourseDescription}");
                        Console.WriteLine($"  CreditHours: {course.CreditHours}");
                        Console.WriteLine($"  TheoryHours: {course.TheoryHours}");
                        Console.WriteLine($"  PracticalHours: {course.PracticalHours}");
                        Console.WriteLine($"  PrerequisiteCourse: {course.PrerequisiteCourse}");
                        Console.WriteLine($"  MajorThatInsertedFrom: {course.MajorThatInsertedFrom}");
                        Console.WriteLine();

                        courses.Add(course);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing course section: {ex.Message}");
                    }
                }
            }

            return courses;
        }



        static void ExtractHours(string description, out int creditHours, out int theoryHours, out int practicalHours)
        {
            creditHours = -1;
            theoryHours = -1;
            practicalHours = -1;

            // Regex pattern to match the hours format
            string pattern = @"الساعات المعتمدة\s*:\s*(\d+)\s*\(\s*نظري\s*:\s*(\d+)\s*,\s*عملي\s*:\s*(\d+)\s*\)";

            Match match = Regex.Match(description, pattern);
            if (match.Success)
            {
                creditHours = int.Parse(match.Groups[1].Value);
                theoryHours = int.Parse(match.Groups[2].Value);
                practicalHours = int.Parse(match.Groups[3].Value);
            }
        }

        static string RemoveHoursAndPrerequisites(string description)
        {

            int startIndex = description.IndexOf("الساعات");

            if (startIndex != -1)
            {

                int endIndex = description.IndexOf(')', startIndex);

                if (endIndex != -1)
                {
                    string cleanedDescription = description.Substring(0, startIndex) +
                                                 description.Substring(endIndex + 1);
                    cleanedDescription = cleanedDescription.Trim();

                    return cleanedDescription;
                }
            }
            return description;
        }

        public static string ExtractPrerequisites(string input)
        {
            string searchText = "متطلب سابق : ";
            string extractedText = "";

            // Regex pattern to match everything after "searchText" until the first space or end of string
            string pattern = $@"{Regex.Escape(searchText)}\s*:\s*([0-9/|]+)(?=\s|$)";

            Match match = Regex.Match(input, pattern);
            if (match.Success)
            {
                extractedText = match.Groups[1].Value.Trim();
            }

            return extractedText;
        }
    }
}
