
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Servereaxm
{
    internal class Program
    {
        static HttpListener listener = new HttpListener();
        private static void Main(string[] args)
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }
            // URI prefixes are required,
            var prefixes = new List<string>() { "http://*:3000/" };

            // Create a listener.
            
            // Add the prefixes.
            foreach (string s in prefixes)
            {
                listener.Prefixes.Add(s);
            }
            listener.Start();
            Console.WriteLine("Listening...");
            while (true)
            {
                // Note: The GetContext method blocks while waiting for a request.
                HttpListenerContext context = listener.GetContext();

                HttpListenerRequest request = context.Request;

                string documentContents;
                using (Stream receiveStream = request.InputStream)
                {
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                    {
                        documentContents = readStream.ReadToEnd();
                    }
                }
                Console.WriteLine($"Recived request for {request.Url}\n");
                //Console.WriteLine(documentContents);

                // Obtain a response object.
                HttpListenerResponse response = context.Response;
                
                listener.BeginGetContext(onAcceptReader, response);
                //Console.WriteLine(response.OutputStream.ReadByte);
                // Construct a response.
                // string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
                string file = "index.html";
                FileStream readIn = new FileStream(file, FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[1024 * 1000];
                int nRead = readIn.Read(buffer, 0, 10240);
                int total = 0;
                while (nRead > 0)
                {
                    total += nRead;
                    nRead = readIn.Read(buffer, total, 10240);
                }
                readIn.Close();
                byte[] maxresponse_complete = new byte[total];
                System.Buffer.BlockCopy(buffer, 0, maxresponse_complete, 0, total);
                //byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                // Get a response stream and write the response to it.
                //response.ContentType = "/index.html";
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(maxresponse_complete, 0, maxresponse_complete.Length);
                // You must close the output stream.
                output.Close();
                
                response.Close();
            }
            listener.Stop();
        }

        public static void onAcceptReader(IAsyncResult ar) {
            int result = listener.GetContext().Response.OutputStream.EndRead(ar);
            byte[] maxresponse_complete = new byte[result];
            string newMessage = Encoding.UTF8.GetString(maxresponse_complete, 0 , result);
            Console.WriteLine(newMessage);
            Console.Write("왔네");
        }
    }
}

// using System;
// using System.Net;
// using System.Threading;
// using System.Linq;
// using System.Text;
// using System.IO;

// namespace SimpleWebServer
// {
//     public class WebServer
//     {
//         private readonly HttpListener _listener = new HttpListener();
//         private readonly Func<HttpListenerRequest, string> _responderMethod;

//         public WebServer(string[] prefixes, Func<HttpListenerRequest, string> method)
//         {
//             if (!HttpListener.IsSupported)
//                 throw new NotSupportedException(
//                     "Needs Windows XP SP2, Server 2003 or later.");

//             // URI prefixes are required, for example 
//             // "http://localhost:8080/index/".
//             if (prefixes == null || prefixes.Length == 0)
//                 throw new ArgumentException("prefixes");

//             // A responder method is required
//             if (method == null)
//                 throw new ArgumentException("method");

//             foreach (string s in prefixes)
//                 _listener.Prefixes.Add(s);

//             _responderMethod = method;
//             _listener.Start();
//             _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);
//         }
//         private static void ListenerCallback(IAsyncResult result)
//         {
//             HttpListener listener = (HttpListener)result.AsyncState;
//             listener.BeginGetContext(new AsyncCallback(ListenerCallback), listener);
//             Console.WriteLine("New request.");

//             HttpListenerContext context = listener.EndGetContext(result);
//             HttpListenerRequest request = context.Request;
//             HttpListenerResponse response = context.Response;


//             byte[] page = GetFile("/index.html");

//             response.ContentLength64 = page.Length;
//             Stream output = response.OutputStream;
//             output.Write(page, 0, page.Length);
//             output.Close();


//         }
//         public static byte[] GetFile(string file)
//         {
//             if (!File.Exists(file)) return null;
//             FileStream readIn = new FileStream(file, FileMode.Open, FileAccess.Read);
//             byte[] buffer = new byte[1024 * 1000];
//             int nRead = readIn.Read(buffer, 0, 10240);
//             int total = 0;
//             while (nRead > 0)
//             {
//                 total += nRead;
//                 nRead = readIn.Read(buffer, total, 10240);
//             }
//             readIn.Close();
//             byte[] maxresponse_complete = new byte[total];
//             System.Buffer.BlockCopy(buffer, 0, maxresponse_complete, 0, total);
//             return maxresponse_complete;
//         }

//         public WebServer(Func<HttpListenerRequest, string> method, params string[] prefixes)
//             : this(prefixes, method) { }

//         public void Run()
//         {
//             ThreadPool.QueueUserWorkItem((o) =>
//             {
//                 Console.WriteLine("Webserver running...");
//                 try
//                 {
//                     while (_listener.IsListening)
//                     {
//                         ThreadPool.QueueUserWorkItem((c) =>
//                         {
//                             var ctx = c as HttpListenerContext;
//                             try
//                             {
//                                 string rstr = _responderMethod(ctx.Request);
//                                 byte[] buf = Encoding.UTF8.GetBytes(rstr);
//                                 ctx.Response.ContentLength64 = buf.Length;
//                                 ctx.Response.OutputStream.Write(buf, 0, buf.Length);
//                             }
//                             catch { } // suppress any exceptions
//                             finally
//                             {
//                                 // always close the stream
//                                 ctx.Response.OutputStream.Close();
//                             }
//                         }, _listener.GetContext());
//                     }
//                 }
//                 catch { } // suppress any exceptions
//             });
//         }
//             public void Stop()
//             {
//             _listener.Stop();
//             _listener.Close();
//             }

            
//     }   
// }

// namespace Jts
// {
//     class HttpServer {

//         HttpServer() {

//         }
//         HttpClient client = new HttpClient();

//     }
// }


// using System.Net;
// using System.Text;
// using System.IO;
// using System.Net.Sockets;
// internal class Program
// {
//     private static void Main(string[] args)
//     {
//         HttpListener listener = new HttpListener();
//         listener.Prefixes.Add("http://127.0.0.1:3000/Login/");
//         listener.Start();

//         Console.WriteLine("Listening...");

//         HttpListenerContext context = listener.GetContext();
//         HttpListenerRequest request = context.Request;
//         HttpListenerResponse response = context.Response;
        
//        //FileStream fileStream = File.Open("/index.html", FileMode.Open);
//         try {
            
//             string file = File.ReadAllText("@/index.html");
//             // StreamReader sr = new StreamReader("/index.html");
//             // FileStream file = File.ReadAllBytes("@/index.html");
//             Console.WriteLine(file);
//             byte[] buffer = Encoding.UTF8.GetBytes(file);
//             response.OutputStream.Write(buffer, 0, buffer.Length);
//             Console.WriteLine("성공");
//         }
//         catch {
//             Console.WriteLine("실패");
//             response.OutputStream.Close();
//             listener.Stop();
//         }
       
        
//     }
// }