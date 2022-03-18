/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTTPServer
{
    public enum RequestMethod
    {
        GET,
        POST,
        HEAD
    }

    public enum HTTPVersion
    {
        HTTP10,
        HTTP11,
        HTTP09
    }

    class Request
    {
        string[] requestLines;
        RequestMethod method;
        public string relativeURI;
        Dictionary<string, string> headerLines;

        public Dictionary<string, string> HeaderLines
        {
            get { return headerLines; }
        }

        HTTPVersion httpVersion;
        string requestString;
        string[] contentLines;

        public Request(string requestString)
        {
            this.requestString = requestString;
        }
        /// <summary>
        /// Parses the request string and loads the request line, header lines and content, returns false if there is a parsing error
        /// </summary>
        /// <returns>True if parsing succeeds, false otherwise.</returns>
        public bool ParseRequest()
        {
            //throw new NotImplementedException();

            //TODO: parse the receivedRequest using the \r\n delimeter 
            string[] s = new string[] { "\r\n" };
            requestLines = requestString.Split(s, StringSplitOptions.None);

            // check that there is atleast 3 lines: Request line, Host Header, Blank line (usually 4 lines with the last empty line for empty content)

            // Parse Request line
            string[] line = requestLines[0].Split(' ');
            method = RequestMethod.GET;
            relativeURI = line[1];
            if (line[2] == "HTTP/1.1")
                httpVersion = HTTPVersion.HTTP11;
            else if (line[2] == "HTTP/1.0")
                httpVersion = HTTPVersion.HTTP10;
            else if (line[2] == "HTTP/9.0")
                httpVersion = HTTPVersion.HTTP09;

            int i = 1;
            int j = 0;
            string[] str = new string[] { ": " };
            while (!string.IsNullOrEmpty(requestLines[i]))
            {
                string header_content = requestLines[i];
                string[] data = header_content.Split(str, StringSplitOptions.None);
                headerLines = new Dictionary<string, string>();
                headerLines.Add(data[0], data[1]);
                i++;
                j = i;
            }
            // Validate blank line exists
            if (string.IsNullOrEmpty(requestLines[j]))
                return true;
            else
                return false;
            // Load header lines into HeaderLines dictionary
        }

        private bool ParseRequestLine()
        {
            throw new NotImplementedException();
        }

        private bool ValidateIsURI(string uri)
        {
            return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
        }

        private bool LoadHeaderLines()
        {
            throw new NotImplementedException();
        }

        private bool ValidateBlankLine()
        {
            throw new NotImplementedException();
        }

    }
}*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTTPServer
{
    public enum RequestMethod
    {
        GET,
        POST,
        HEAD
    }

    public enum HTTPVersion
    {
        HTTP10,
        HTTP11,
        HTTP09
    }

    class Request
    {
        string[] requestLines;
        RequestMethod method;
        public string relativeURI;
        Dictionary<string, string> headerLines;

        public Dictionary<string, string> HeaderLines
        {
            get { return headerLines; }
        }

        HTTPVersion httpVersion;
        string requestString;
        string[] contentLines;

        public Request(string requestString)
        {
            this.requestString = requestString;
        }
        /// <summary>
        /// Parses the request string and loads the request line, header lines and content, returns false if there is a parsing error
        /// </summary>
        /// <returns>True if parsing succeeds, false otherwise.</returns>
        public bool ParseRequest()
        {

            if (this.ValidateBlankLine() && this.ParseRequestLine() && this.LoadHeaderLines())
            {
                return true;
            }
            return false;
            //TODO: parse the receivedRequest using the \r\n delimeter   

            // check that there is atleast 3 lines: Request line, Host Header, Blank line (usually 4 lines with the last empty line for empty content)

            // Parse Request line

            // Validate blank line exists

            // Load header lines into HeaderLines dictionary
        }

        private bool ParseRequestLine()
        {
            string[] s = new string[] { "\r\n" };
            string[] tokens = this.requestString.Split(s, StringSplitOptions.None);
            string[] requestLine = tokens[0].Split(' ');
            if (requestLine[0].ToUpper().Trim() == "GET" && ValidateIsURI(requestLine[1]) &&
                (requestLine[2].ToUpper().Trim() == "HTTP/0.9" || requestLine[2].ToUpper().Trim() == "HTTP/1.0" || requestLine[2].ToUpper().Trim() == "HTTP/1.1"))
            {
                this.relativeURI = requestLine[1];
                this.method = RequestMethod.GET;
                //Check below condition if error
                this.httpVersion = (requestLine[2].ToUpper().Trim() == "HTTP/0.9") ? HTTPVersion.HTTP09 : ((requestLine[2].ToUpper().Trim() == "HTTP/1.0") ? HTTPVersion.HTTP10 : HTTPVersion.HTTP11);
                return true;
            }
            return false;
        }

        private bool ValidateIsURI(string uri)
        {
            return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
        }

        private bool LoadHeaderLines()
        {
            this.headerLines = new Dictionary<string, string>();
            string[] s = new string[] { "\r\n" };
            string[] tokens = this.requestString.Split(s, StringSplitOptions.None);
            for (int j = 1; j < (tokens.Length); j++)
            {
                string[] tokens2 = tokens[j].Split(':');
                for (int i = 0; i < (tokens2.Length - 1); i += 2)
                {
                    this.headerLines.Add(tokens2[i].ToUpper().Trim(), tokens2[i + 1].ToUpper().Trim());
                }
            }


            this.contentLines = this.requestString.Substring(this.requestString.IndexOf("\r\n\r\n")).Split('\n');


            for (int i = 0; i < (this.headerLines.Keys.Count); i++)
            {
                if (this.headerLines.ContainsKey("HOST:") && !string.IsNullOrEmpty(this.headerLines["HOST:"]))
                {
                    return true;
                }
            }
            return false;
        }

        private bool ValidateBlankLine()
        {
            return this.requestString.IndexOf("\r\n\r\n") >= 0;
        }

    }
}