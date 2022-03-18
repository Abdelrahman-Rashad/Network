using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
    class Logger
    {
        //static StreamWriter sr = new StreamWriter("log.txt");
        public static void LogException(Exception ex)
        {
            // TODO: Create log file named log.txt to log exception details in it
            //Datetime:
            //message:
            // for each exception write its details associated with datetime 
            FileStream fs = new FileStream(@"D:\Documents\SC 3rd\network\Project_materials\Networks Project Template\Template[2021-2022]\HTTPServer\bin\Debug\log.txt", FileMode.OpenOrCreate);
            //Datetime:
            //message:
            // for each exception write its details associated with datetime 
            StreamWriter fw = new StreamWriter(fs);
            fw.WriteLine("Date Time :" + DateTime.Now.ToString());
            fw.WriteLine("Message :" + ex.Message);

            fw.Close();
        }
    }
}
