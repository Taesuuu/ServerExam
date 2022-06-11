// using System.Net;

// HttpListener listener = new HttpListener();
// listener.Prefixes.Add("http://127.0.0.1:3000/GodDamn/");
// listener.Start();
// Console.WriteLine("Listening...");
// HttpListenerContext context = listener.GetContext();
// HttpListenerRequest request = context.Request;
// HttpListenerResponse response = context.Response;
// Stream body = request.InputStream;
// System.Text.Encoding encoding = request.ContentEncoding;
// System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);

// string s = reader.ReadToEnd();
// string responseString = s;    
// System.Console.WriteLine(responseString);
// byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
// response.OutputStream.Write(buffer, 0, buffer.Length);
// response.OutputStream.Close();
// listener.Stop();
// // 맥이여서 그런가?

using System;
using System.IO;
using System.Text;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace HttpListenerExample
{
    class HttpServer
    {
        public static HttpListener listener;
        public static string url = "http://127.0.0.1:3000/GodDamn/";
        public static int pageViews = 0;
        public static int requestCount = 1;
        public static List<HttpListenerContext> userList;

        public static void txtLog(string logstring) {
            string savePath = @"D:\Git\ServerExam\LogFile.txt";
            string logTime = DateTime.Now.ToString("hh:mm:ss tt ");
            string log = logTime + logstring;
            System.IO.File.AppendAllText(savePath, log, Encoding.UTF8);
        }
        public static async Task loginFunction(HttpListenerContext context) {
            bool runServer = true;
            while(runServer) {
                HttpListenerRequest req = context.Request;
                HttpListenerResponse resp = context.Response;
                if ((req.HttpMethod == "POST") && (req.Url.AbsolutePath == "/Login"))
                {
                    string responseString = "로그인 성공";
                    Console.WriteLine(responseString);
                    //System.Console.WriteLine(responseString);
                    byte[] buffers = System.Text.Encoding.UTF8.GetBytes(responseString);
                    resp.OutputStream.Write(buffers, 0, buffers.Length);
                    
                    resp.Close();
                }
            }
            
        }
        public static async Task HandleIncomingConnections()
        {
            bool runServer = true;

            while (runServer)
            {
               
                HttpListenerContext ctx = await listener.GetContextAsync();
                
                //userList.Add(ctx);
                // string test = ClientInformation(ctx);
                // Console.WriteLine(test);
                // HttpListenerBasicIdentity identity = (HttpListenerBasicIdentity)ctx.User.Identity;
                // Console.WriteLine(identity.Name);
                // Console.WriteLine(identity.Password);
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;
                //resp.AddHeader("Access-Control-Allow-Orgin","*");
                string newMessage = null;
                if(req.Url.AbsolutePath != "/favicon.ico"){
                    Console.WriteLine("Request #: " + requestCount++);
                Console.WriteLine(req.Url.ToString());
                Console.WriteLine(req.HttpMethod);
                Console.WriteLine(req.UserHostName);
                Console.WriteLine(req.UserAgent);
                Console.WriteLine();

                
                if ((req.HttpMethod == "POST") && (req.Url.AbsolutePath == "/shutdown"))
                {
                    Console.WriteLine("Shutdown requested");
                    runServer = false;
                }

                if ((req.HttpMethod == "POST") && (req.Url.AbsolutePath == "/GodDamn/"))
                {
                    Console.WriteLine(ctx.User + "한테 메시지 왔다");
                    Stream body = req.InputStream;
                    System.Text.Encoding encoding = req.ContentEncoding;
                    System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);
                    //StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
                    newMessage = await reader.ReadToEndAsync();
                    Console.WriteLine(newMessage);
                    Console.WriteLine();
                    txtLog(newMessage + "\n");
                    string test1 = "메시지 왔다";
                    //string responseString = JsonSerializer.Serialize(newMessage);
                    Console.WriteLine(newMessage);
                    //System.Console.WriteLine(responseString);
                    byte[] buffers = System.Text.Encoding.UTF8.GetBytes(newMessage);
                    await resp.OutputStream.WriteAsync(buffers, 0, buffers.Length);
                    
                    resp.OutputStream.Close();
                    
                }

                if ((req.HttpMethod == "POST") && (req.Url.AbsolutePath == "/Login/"))
                {
                    string responseString = "로그인 성공";
                    Console.WriteLine(responseString);
                    Stream body = req.InputStream;
                    System.Text.Encoding encoding = req.ContentEncoding;
                    System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);
                    //StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
                    responseString = await reader.ReadToEndAsync();
                    
                    // string responseJson = JsonSerializer.Serialize(responseString);
                    Console.WriteLine(responseString);
                    
                    txtLog(responseString + "\n");
                    string testString = "보내는 용도";
                    Console.WriteLine(responseString);
                    //System.Console.WriteLine(responseString);
                    byte[] buffers = System.Text.Encoding.UTF8.GetBytes(responseString);
                    
                    await resp.OutputStream.WriteAsync(buffers, 0, buffers.Length);
                    resp.OutputStream.Close();
                    
                }
               
                // if ((req.HttpMethod == "POST")&& (req.Url.AbsolutePath != "/favicon.ico")){
                //     pageViews += 1;
                // }
                if((req.Url.AbsolutePath == "/")){
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

                string disableSubmit = !runServer ? "disabled" : "";
                
                //byte[] data = Encoding.UTF8.GetBytes(String.Format(maxresponse_complete, pageViews, disableSubmit));
                resp.ContentType = "text/html";
                resp.ContentEncoding = Encoding.UTF8;
                resp.ContentLength64 = maxresponse_complete.LongLength;
                await resp.OutputStream.WriteAsync(maxresponse_complete, 0, maxresponse_complete.Length);
                resp.OutputStream.Close();
                }
                
                }
                
            }
        }
        public static string ClientInformation(HttpListenerContext context)
        {
            System.Security.Principal.IPrincipal user = context.User;
            System.Security.Principal.IIdentity id = user.Identity;
            if (id == null)
            {
                return "Client authentication is not enabled for this Web server.";
            }

            string display;
            if (id.IsAuthenticated)
            {
                display = String.Format("{0} was authenticated using {1}", id.Name,
                id.AuthenticationType);
            }
            else
            {       
                display = String.Format("{0} was not authenticated", id.Name);
            }
            return display;
            }

        public static void Main(string[] args)
        {
            listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Prefixes.Add("http://127.0.0.1:3000/");
            listener.Prefixes.Add("http://127.0.0.1:3000/Login/");
            // listener.AuthenticationSchemes = AuthenticationSchemes.Basic;
            listener.Start();
            Console.WriteLine("Listening for connections on {0}", url);

            Task listenTask = HandleIncomingConnections();
            listenTask.GetAwaiter().GetResult();
            listener.Close();
        }
    }
}


//****
// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Net;
// using System.Net.Sockets;
// using System.Text;

// //클래스를 하나 만들어서 클라이언트 이름과 스트림을 저장하도록 해야하나? 하나 하나 객체에다가
// // 그렇게 해서 이 모든 객체를 관리하는걸 main에 만들어서 요청이 오는걸 확인? 


// namespace Servereaxm
// {
//     internal class Program
//     {
//         static List<HttpListenerContext> userList = new List<HttpListenerContext>();
//         static HttpListener listener = new HttpListener();
//         private static void Main(string[] args)
//         {
//             if (!HttpListener.IsSupported)
//             {
//                 Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
//                 return;
//             }
//             // URI prefixes are required,
//             var prefixes = new List<string>() { "http://*:3000/" };

//             // Create a listener.
            
//             // Add the prefixes.
//             foreach (string s in prefixes)
//             {
//                 listener.Prefixes.Add(s);
//             }
//             listener.Start();

//             Console.WriteLine("Listening...");
//             while (true)
//             {
//                 // Note: The GetContext method blocks while waiting for a request.
//                 HttpListenerContext context = listener.GetContext();
                
//                 Console.WriteLine("접속한 유저");
//                 //유저 연결 저장
//                 userList.Add(context);

//                 for(int i = 0; i < userList.Count; i++){
//                     Console.WriteLine(userList[i]);
//                 }

//                 HttpListenerRequest request = context.Request;

//                 string documentContents;
//                 using (Stream receiveStream = request.InputStream)
//                 {
//                     using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
//                     {
//                         documentContents = readStream.ReadToEnd();
//                     }
//                 }
//                 Console.WriteLine($"Recived request for {request.Url}\n");
//                 Console.WriteLine(documentContents);

//                 // Obtain a response object.
//                 HttpListenerResponse response = context.Response;
                
//                 listener.BeginGetContext(onAcceptReader, request);
//                 //Console.WriteLine(response.OutputStream.ReadByte);
//                 // Construct a response.
//                 // string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
//                 string file = "index.html";
//                 FileStream readIn = new FileStream(file, FileMode.Open, FileAccess.Read);
//                 byte[] buffer = new byte[1024 * 1000];
//                 int nRead = readIn.Read(buffer, 0, 10240);
//                 int total = 0;
//                 while (nRead > 0)
//                 {
//                     total += nRead;
//                     nRead = readIn.Read(buffer, total, 10240);
//                 }
//                 readIn.Close();
//                 byte[] maxresponse_complete = new byte[total];
//                 System.Buffer.BlockCopy(buffer, 0, maxresponse_complete, 0, total);
//                 //byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
//                 // Get a response stream and write the response to it.
//                 //response.ContentType = "/index.html";
//                 response.ContentLength64 = buffer.Length;
//                 System.IO.Stream output = response.OutputStream;
//                 output.Write(maxresponse_complete, 0, maxresponse_complete.Length);
//                 // You must close the output stream.
//                 output.Close();
//             }
//             listener.Stop();
//         }

//         public static void onAcceptReader(IAsyncResult ar) {
//             int result = listener.GetContext().Request.InputStream.EndRead(ar);
//             byte[] maxresponse_complete = new byte[result];
//             string newMessage = Encoding.UTF8.GetString(maxresponse_complete, 0 , result);
//             Console.WriteLine(newMessage);
//             Console.Write("왔네");
//             listener.BeginGetContext(onAcceptReader, null);
//         }
//     }
// }
//   **\\\\
// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Net;
// using System.Net.Sockets;
// using System.Text;
// public class TwoWayHttpClient : IDisposable
// {
//     private readonly HttpClientHandler _handler;
//     private readonly HttpClient _client;
//     protected HttpListener _listener;
//     protected readonly FilterQueue<IIncomingMessage> _queue = new FilterQueue<IIncomingMessage>();
//     public string ListeningAddress { get; private set; }
//     private TwoWayHttpClient()
//     {
//         _handler = new HttpClientHandler()
//         {
//             CookieContainer = new CookieContainer()
//         };
//         _client = new HttpClient(_handler);
//     }
//     protected TwoWayHttpClient(string server)
//         : this()
//     {
//         _client.BaseAddress = new Uri("http://" + server);
//         _client.Timeout = TimeSpan.FromMinutes(10);
//     }
//     public void StartListener(int listenerPort, string baseAddress = "/")
//     {
//         if (_listener != null)
//         {
//             return;
//         }
//         //the listening address contaning local ip adress, so there aren't any issues about who the server needs to send the request to
//         ListeningAddress = "http://" + GetLocalIPAddress() + ":" + listenerPort + baseAddress;
//         _listener = new HttpListener();
//         //Add the prefix for the listener on the specified port, the + sign is important so it can recieve requests
//         _listener.Prefixes.Add("http://+:" + listenerPort + baseAddress);
//         _listener.Start();
//         //starts listening to requests
//         Task.Run(async () =>
//         {
//             try
//             {
//                 while (_listener.IsListening)
//                 {
//                     var context = await _listener.GetContextAsync();
//                     //for every request recieved put it on a queue.
//                     _queue.Add(GetIncommingMessageFromContext(context));
//                     //answer to the server with 200. This is important, so the server can keep sending other requests
//                     using (context.Response) { }
//                 }
//             }
//             catch (ObjectDisposedException)
//             {
//                 //listener was disposed, ignore it
//             }
//         });
//     }
//     CancellationTokenSource _cancelSource = new CancellationTokenSource();
//     public void InitiatePolling(string url)
//     {
//         _cancelSource = new CancellationTokenSource();
//         var token = _cancelSource.Token;
//         Task.Run(async () =>
//         {
//             while (true)
//             {
//                 _queue.WaitForWaiters();
//                 token.ThrowIfCancellationRequested();
//                 var response = await Request(url);
//                 if (!IsValidResponse(response))
//                 {
//                     throw new InvalidOperationException(response.ToString());
//                 }
//                 if (response.StatusCode != HttpStatusCode.NoContent)
//                 {
//                     _queue.Add(GetIncommingMessageFromResponse(response));
//                 }
//             }
//         }, token);
//     }
//     protected virtual IIncomingMessage GetIncommingMessageFromResponse(HttpResponseMessage response)
//     {
//         return new BaseIncomingMessage(response);
//     }
//     protected virtual IIncomingMessage GetIncommingMessageFromContext(HttpListenerContext context)
//     {
//         return new BaseIncomingMessage(context);
//     }
//     private static string GetLocalIPAddress()
//     {
//         var host = Dns.GetHostEntry(Dns.GetHostName());
//         foreach (var ip in host.AddressList)
//         {
//             if (ip.AddressFamily == AddressFamily.InterNetwork)
//             {
//                 return ip.ToString();
//             }
//         }
//         throw new Exception("Local IP Address Not Found!");
//     }
//     private Task<string> GetQueryStringFromDictionary(IDictionary<string, object> parameters)
//     {
//         var values = parameters.Where(p => p.Value != null)
//             .Select(p => new KeyValuePair<string, string>(p.Key, p.Value.ToString()));
//         using (var query = new FormUrlEncodedContent(values))
//         {
//             return query.ReadAsStringAsync();
//         }
//     }
//     private Cookie GetSetCookie(HttpResponseMessage response)
//     {
//         IEnumerable<string> authValues;
//         if (!response.Headers.TryGetValues("Set-Cookie", out authValues))
//         {
//             return null;
//         }
//         var cookieValue = authValues.FirstOrDefault();
//         var idxEquals = cookieValue.IndexOf('=');
//         var name = cookieValue.Substring(0, idxEquals);
//         var value = cookieValue.Substring(idxEquals + 1, cookieValue.IndexOf(';') - idxEquals - 1);
//         return new Cookie(name, value)
//         {
//             Domain = _client.BaseAddress.Host
//         };
//     }
//     public async Task<HttpResponseMessage> Request(string url, IDictionary<string, object> parameters = null, string name = "")
//     {
//         if (parameters != null && parameters.Count != 0)
//         {
//             var query = await GetQueryStringFromDictionary(parameters);
//             url = url + "?" + query;
//         }
//         var response = await _client.GetAsync(url);
//         var cookie = GetSetCookie(response);
//         if (cookie != null)
//         {
//             _handler.CookieContainer.Add(cookie);
//         }
//         return response;
//     }
//     protected async Task<TwoWayHttpResponse> RequestAndWaitForReply(string url, object parameters, IIncomingMessage expectedMessage)
//     {
//         if (parameters == null)
//         {
//             parameters = new object();
//         }
//         var response = await Request(url, parameters.ToDictionary());
//         if (IsValidResponse(response))
//         {
//             if (expectedMessage == null)
//             {
//                 return new TwoWayHttpResponse
//                 {
//                     Response = response
//                 };
//             }
//             var result = new TwoWayHttpResponse
//             {
//                 Response = response,
//                 ServerRequest = await WaitForMessage(expectedMessage)
//             };
//             return result;
//         }
//         return new TwoWayHttpResponse
//         {
//             Response = response
//         };
//     }
//     public virtual Task<IIncomingMessage> WaitForMessage(IIncomingMessage expectedMessage)
//     {
//         var message = _queue.Take(expectedMessage.IsExpectedMessage, Timeout.InfiniteTimeSpan);
//         return Task.FromResult(message);
//     }
//     public virtual void ClearQueue()
//     {
//         _queue.Clear();
//     }
//     protected virtual bool IsValidResponse(HttpResponseMessage response)
//     {
//         return (int)response.StatusCode >= 200 && (int)response.StatusCode < 400;
//     }
//     public void Dispose()
//     {
//         if (_listener != null)
//         {
//             _listener.Stop();
//             (_listener as IDisposable).Dispose();
//         }
//         if (_cancelSource != null)
//         {
//             _cancelSource.Cancel();
//         }
//         _client.Dispose();
//     }
// }

// internal class TwoWayHttpResponse
// {
//     public object Response { get; set; }
// }

// public class BaseIncomingMessage : IIncomingMessage
// {
//     #if DEBUG
//     private readonly HttpListenerContext _context;
//     private readonly HttpResponseMessage _response;
//     #endif
//     private readonly Task<string> _content;
//     public BaseIncomingMessage(HttpListenerContext context)
//     {
//         _context = context;
//         using (var reader = new StreamReader(_context.Request.InputStream))
//         {
//             _content = reader.ReadToEndAsync();
//         }
//     }
//     public BaseIncomingMessage(HttpResponseMessage response)
//     {
//         _response = response;
//         _content = response.Content.ReadAsStringAsync();
//     }
//     public BaseIncomingMessage()
//     {
//     }
//     public bool IsExpectedMessage(IIncomingMessage message)
//     {
//         return IsExpectedMessageImpl(message, _content.Result);
//     }
//     public virtual bool IsExpectedMessageImpl(IIncomingMessage originalMessage, string content)
//     {
//         return true;
//     }
// }
// public interface IIncomingMessage
// {
//     bool IsExpectedMessage(IIncomingMessage compareTo);
// }
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