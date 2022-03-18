using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Call CreateRedirectionRulesFile() function to create the rules of redirection 
            CreateRedirectionRulesFile();
            String path = @"D:\Documents\SC 3rd\network\Project_materials\Networks Project Template\Template[2021-2022]\HTTPServer\bin\Debug\redirectionRules.txt";
            //Start server
            // 1) Make server object on port 1000
            // 2) Start Server
            Server server = new Server(1000,path);
            server.StartServer();
        }

        static void CreateRedirectionRulesFile()
        {
            // TODO: Create file named redirectionRules.txt
            // each line in the file specify a redirection rule
            // example: "aboutus.html,aboutus2.html"
            // means that when making request to aboustus.html,, it redirects me to aboutus2
            FileStream f = File.Create(@"D:\Documents\SC 3rd\network\Project_materials\Networks Project Template\Template[2021-2022]\HTTPServer\bin\Debug\redirectionRules.txt");
            StreamWriter stream = new StreamWriter(f);
            stream.WriteLine("aboutus.html,aboutus2.html");
            stream.Close();

        }
         
    }
}
