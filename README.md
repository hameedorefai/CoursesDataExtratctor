
# Courses Data Extractor

## Overview


The Courses Data Extractor is a **.NET application** designed to scrape course information from HTML files and store it in an **SQL Server** database.
The application utilizes HtmlAgilityPack for parsing HTML content and **ADO.NET** for data access and database operations.

## Features

- **HTML Scraping**: Extracts course details from HTML files located in a specified directory.
- **Data Parsing**: Processes course information including course name, code, description, credit hours, and prerequisites.
- **Database Insertion**: Inserts the parsed data into a SQL Server database.

## Components

- **Scraping Logic**: 
  - Scrapes HTML files to extract course details.
  - Uses HtmlAgilityPack for HTML parsing.

- **Data Access**:
  - Uses ADO.NET to interact with SQL Server.
  - Inserts extracted course data into a SQL Server database.

- **Database**:
  - **Database Name**: `Courses`
  - **Table Structure**: Contains a table for storing course information with columns for:
  -  CourseID, CourseName, CourseNo, CourseDescription, CourseCode, CreditHours, TheoryHours, PracticalHours, PrerequisiteCourse, and MajorThatInsertedFrom.

- **Database Backup**:
  - **Backup File**: The database backup file containing stored and extracted course information is available.

- **Screenshot for a table**:
![image](https://github.com/user-attachments/assets/6663acb9-e743-499f-af68-b9d1cfe505e8)
