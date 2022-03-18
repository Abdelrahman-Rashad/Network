using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace HTTPServer
{
    class Server
    {
        Socket serverSocket;

        public Server(int portNumber, string redirectionMatrixPath)
        {
            //TODO: call this.LoadRedirectionRules passing redirectionMatrixPath to it
            //TODO: initialize this.serverSocket
            this.LoadRedirectionRules(redirectionMatrixPath);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint hostEndPoint = new IPEndPoint(IPAddress.Any, portNumber);
            serverSocket.Bind(hostEndPoint);
        }

        public void StartServer()
        {
            // TODO: Listen to connections, with large backlog.
            Console.WriteLine("start listening");
            serverSocket.Listen(1000);
            // TODO: Accept connections in while loop and start a thread for each connection on function "Handle Connection"
            while (true)
            {
                //TODO: accept connections and start thread for each accepted connection.
                Socket clientsocket = serverSocket.Accept();
                Console.WriteLine("new client accepted : {0}", clientsocket.RemoteEndPoint);
                Thread obj = new Thread(new ParameterizedThreadStart(HandleConnection));
                obj.Start(clientsocket);
            }
        }

        public void HandleConnection(object obj)
        {
            // TODO: Create client socket 
            // set client socket ReceiveTimeout = 0 to indicate an infinite time-out period
            Socket clientsock = (Socket)obj;
            clientsock.ReceiveTimeout = 0;
            int length;
            // TODO: receive requests in while true until remote client closes the socket.
            while (true)
            {
                try
                {
                    // TODO: Receive request

                    byte[] data = new byte[65536];
                    length = clientsock.Receive(data);
                    String s = Encoding.ASCII.GetString(data,0,length);
                    //StreamReader reader = new StreamReader(s);

                    // TODO: break the while loop if receivedLen==0

                    if (length == 0)
                        break;

                    // TODO: Create a Request object using received request string

                    Request request = new Request(s);

                    // TODO: Call HandleRequest Method that returns the response

                    Response response = HandleRequest(request);
                    Console.WriteLine(response.ResponseString);
                    data = Encoding.ASCII.GetBytes(response.ResponseString);

                    // TODO: Send Response back to client

                    clientsock.Send(data);
                }
                catch (Exception ex)
                {
                    // TODO: log exception using Logger class
                    Logger.LogException(ex);
                }
            }

            // TODO: close client socket
            clientsock.Close();
        }

        Response HandleRequest(Request request)
        {
            //throw new NotImplementedException();
            string content="";
            StatusCode code;
            try
            {

                //TODO: check for bad request 
                if (!request.ParseRequest())
                {
                    code = StatusCode.BadRequest;
                    content = "<!DOCTYPE html>< html >< body >< h1 > 400 Bad Request</ h1 >< p > 400 Bad Request</ p ></ body ></ html > ";
                    string location = "http://localhost:1000/" + Configuration.BadRequestDefaultPageName;
                    Response res = new Response(code, "text/html", content, location);
                    return res;
                }
                //TODO: map the relativeURI in request to get the physical path of the resource.
                string[] name = request.relativeURI.Split('/');
                string physical_path = Configuration.RootPath + '\\' + name[1];
                //TODO: check for redirect
               
                string check = GetRedirectionPagePathIFExist(request.relativeURI);
                if (!string.IsNullOrEmpty(check))
                {
                    code = StatusCode.Redirect;
                    content = File.ReadAllText(check);
                    string location = "http://localhost:1000/" + name[1];
                    Response res = new Response(code, "text/html", content, location);
                    return res;
                }

                //TODO: check file exists
                if (!File.Exists(physical_path))
                {
                    physical_path = Configuration.RootPath + '\\' + "NotFound.html";
                    code = StatusCode.NotFound;
                    content = File.ReadAllText(physical_path);
                }
                else
                {
                    content = File.ReadAllText(physical_path);
                    code = StatusCode.OK;
                }

                //TODO: read the physical file

                // Create OK response
                Response r = new Response(code, "text/html", content, physical_path);
                return r;
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);
                // TODO: in case of exception, return Internal Server Error. 
                string physical_path = Configuration.RootPath + '\\' + "InternalError.html";
                code = StatusCode.InternalServerError;
                content = File.ReadAllText(physical_path);
                Response r = new Response(code, "text/html", content, physical_path);
                return r;
            }
        }

        private string GetRedirectionPagePathIFExist(string relativePath)
        {
            // using Configuration.RedirectionRules return the redirected page path if exists else returns empty
            for (int i = 0; i < Configuration.RedirectionRules.Count; i++)
            {
                if ('/' + Configuration.RedirectionRules.Keys.ElementAt(i).ToString() == relativePath)
                {
                    string redirected = '/' + Configuration.RedirectionRules.Values.ElementAt(i).ToString();
                    string physical_path = Configuration.RootPath + '\\' + redirected;
                    return physical_path;
                }
            }
            return string.Empty;
        }

        private string LoadDefaultPage(string defaultPageName)
        {
            string filePath = Path.Combine(Configuration.RootPath, defaultPageName);
            // TODO: check if filepath not exist log exception using Logger class and return empty string
            string content = " ";
            try
            {
                if (File.Exists(filePath))
                {
                    content = File.ReadAllText(filePath);
                }
            }
            catch(Exception ex)
            {
                Logger.LogException(ex);
            }
            // else read file and return its content
            return content;
        }

        private void LoadRedirectionRules(string filePath)
        {
            try
            {
                // TODO: using the filepath paramter read the redirection rules from file 
                // then fill Configuration.RedirectionRules dictionary 
                FileStream file = File.OpenRead(filePath);
                StreamReader streamReader = new StreamReader(file);

                while (streamReader.Peek()!=-1)
                {
                    string s = streamReader.ReadLine();
                    string[] data = s.Split(',');
                    if (data[0] == "")
                        break;
                    Configuration.RedirectionRules = new Dictionary<string, string>();
                    Configuration.RedirectionRules.Add(data[0], data[1]);
                }
                file.Close();
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);
                Environment.Exit(1);
            }
        }
    }
}
