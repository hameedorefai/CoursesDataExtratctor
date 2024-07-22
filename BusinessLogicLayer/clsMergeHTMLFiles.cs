using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;
using System.Text.RegularExpressions;

namespace CoursesInfoScrapping
{


    public class clsMergeHTMLFiles
    {


        public static void Merge()
        {
            string directoryPath = @"D:\download\MergedHTMLCourses";
            string outputFilePath = @"D:\download\MergedCourses.html";

            var allFiles = Directory.GetFiles(directoryPath, "*.html");

            List<clsCourse> allCourses = new List<clsCourse>();

            foreach (var filePath in allFiles)
            {
                var doc = new HtmlDocument();
                doc.Load(filePath, Encoding.UTF8);

                var courseSections = doc.DocumentNode.SelectNodes("//div[@class='panel panel-default']");

                if (courseSections != null)
                {
                    List<clsCourse> courses = ProcessCourseSections(courseSections);
                    allCourses.AddRange(courses);
                }
            }

            GenerateMergedHTMLFile(allCourses, outputFilePath);

            Console.WriteLine($"Total courses processed and merged: {allCourses.Count}");
            Console.WriteLine($"Merged HTML file created: {outputFilePath}");
            Console.ReadLine();
        }
        static List<clsCourse> ProcessCourseSections(HtmlNodeCollection courseSections)
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

                        // Extract course name and code
                        string courseNameCode = courseNameNode.InnerText.Trim();
                        int slashIndex = courseNameCode.IndexOf('/');
                        if (slashIndex >= 0)
                        {
                            course.CourseName = courseNameCode.Substring(slashIndex + 1).Trim();
                            course.CourseCode = courseNameCode.Substring(0, slashIndex).Trim();
                        }

                        // Extract course description
                        var courseDescriptionNode = courseSection.SelectSingleNode(".//div[@class='panel-body']/p[last()]");
                        if (courseDescriptionNode != null)
                        {
                            course.CourseDescription = courseDescriptionNode.InnerText.Trim();
                        }

                        // Extract credit hours, theory hours, practical hours, and prerequisites
                        var detailNodes = courseSection.SelectNodes(".//div[@class='panel-body']/p");
                        if (detailNodes != null)
                        {
                            foreach (var detailNode in detailNodes)
                            {
                                var detailText = detailNode.InnerText.Trim();

                                // Extract credit hours, theory hours, and practical hours
                                if (detailText.Contains("الساعات المعتمدة:"))
                                {
                                    var hoursMatch = Regex.Match(detailText, @"(\d+)\s*\(\s*نظري:\s*(\d+),\s*عملي:\s*(\d+)\s*\)");
                                    if (hoursMatch.Success)
                                    {
                                        course.CreditHours = int.Parse(hoursMatch.Groups[1].Value);
                                        course.TheoryHours = int.Parse(hoursMatch.Groups[2].Value);
                                        course.PracticalHours = int.Parse(hoursMatch.Groups[3].Value);
                                    }
                                }

                                // Extract prerequisite courses
                                if (detailText.Contains("متطلب سابق:"))
                                {
                                    course.PrerequisiteCourse = detailText.Substring(detailText.IndexOf(':') + 1).Trim();
                                }
                            }
                        }

                        // Set the major that inserted from
                        course.MajorThatInsertedFrom = "MergedCourses";

                        // Add to the list of courses
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
        static void GenerateMergedHTMLFile(List<clsCourse> courses, string outputFilePath)
        {
            using (StreamWriter writer = new StreamWriter(outputFilePath, false, Encoding.UTF8))
            {
                writer.WriteLine("<html>");
                writer.WriteLine("<head><title>Merged Courses</title></head>");
                writer.WriteLine("<body>");

                foreach (var course in courses)
                {
                    writer.WriteLine("<div class='panel panel-default'>");
                    writer.WriteLine("<div class='panel-heading'>");
                    writer.WriteLine("<h4 class='panel-title'>");
                    writer.WriteLine($"<a class='collapsed' data-toggle='collapse' data-parent='#mergedCourses' href='#collapse{Guid.NewGuid()}'>{course.CourseCode}/{course.CourseName}</a>");
                    writer.WriteLine("</h4>");
                    writer.WriteLine("</div>");
                    writer.WriteLine("<div class='panel-collapse collapse'>");
                    writer.WriteLine("<div class='panel-body'>");
                    writer.WriteLine($"<p class='text-justify'><span class='bold'>الساعات المعتمدة:</span> {course.CreditHours} (<span class='bold'>نظري:</span> {course.TheoryHours}, <span class='bold'>عملي:</span> {course.PracticalHours})</p>");
                    writer.WriteLine($"<p class='text-justify'><span class='bold'>متطلب سابق:</span> {course.PrerequisiteCourse}</p>");
                    writer.WriteLine($"<p class='text-justify'>{course.CourseDescription}</p>");
                    writer.WriteLine("</div>");
                    writer.WriteLine("</div>");
                    writer.WriteLine("</div>");
                }

                writer.WriteLine("</body>");
                writer.WriteLine("</html>");
            }
        }

    }
}