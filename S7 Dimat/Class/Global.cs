using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace S7_Dimat.Class
{
    class Global
    {


        public static string ExeDirectory
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
        }

        public static string LocalDBName
        {
            get
            {
                return "data";
            }
        }


    }
}
