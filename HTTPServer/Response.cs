using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{

    public enum StatusCode
    {
        OK = 200,
        InternalServerError = 500,
        NotFound = 404,
        BadRequest = 400,
        Redirect = 301
    }

    class Response
    {
        string responseString;
        public string ResponseString
        {
            get
            {
                return responseString;
            }
        }
        StatusCode code;
        List<string> headerLines = new List<string>();
        public Response(StatusCode code, string contentType, string content, string redirectoinPath)
        {
            // TODO: Add headlines (Content-Type, Content-Length,Date, [location if there is redirection])
            headerLines.Add(contentType);
            headerLines.Add(content.Length.ToString());
            headerLines.Add(DateTime.Now.ToString("ddd, dd MMM yyy HH':mm’mm’:’ss'EST’"));

            if (StatusCode.Redirect == code)
            {
                this.responseString = GetStatusLine(code) + "Content_Type: " + contentType + "\r\n" + "Content_Length: " + content.Length
                    + "\r\n" + "Date: " + DateTime.Now.ToString("MM\\/dd\\/yyyy h\\:mm tt") + "\r\n" + "Location: " + redirectoinPath + "\r\n" +
                    "Content: " + content + "\r\n";
            }
            else
            {
                this.responseString = GetStatusLine(code) + "Content_Type: " + contentType + "\r\n" + "Content_Length: " + content.Length
                    + "\r\n" + "Date: " + DateTime.Now.ToString("MM\\/dd\\/yyyy h\\:mm tt") + "\r\n" +
                    "Content: " + content + "\r\n";
            }

            // TODO: Create the request string

        }

        private string GetStatusLine(StatusCode code)
        {
            // TODO: Create the response status line and return it
            string statusLine = string.Empty;
            if (code == StatusCode.OK)
            {
                statusLine = "HTTP/1.1" + " " + code + " " + "OK";

            }
            else if (code == StatusCode.Redirect)
            {
                statusLine = "HTTP/1.1" + " " + code + " " + "Redirect";

            }
            else if (code == StatusCode.BadRequest)
            {
                statusLine = "HTTP/1.1" + " " + code + " " + "BadRequest";

            }
            else if (code == StatusCode.NotFound)
            {
                statusLine = "HTTP/1.1" + " " + code + " " + "NotFound";

            }
            else if (code == StatusCode.InternalServerError)
            {
                statusLine = "HTTP/1.1" + " " + code + " " + "InternalServerError";
            }

            return statusLine;
        }
    }
}
