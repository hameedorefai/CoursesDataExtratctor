using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DataAccessLayer
{
    public class Settings
    {
        public static string ConnectionString
        {
            get
            {
              return "Server=.;Database=CoursessDataExtratctor;User Id=sa;Password=sa123456;";

               // return ConnectionStrings["CoursessDataExtratctor"].ConnectionString;
            }
        }

    }
}
