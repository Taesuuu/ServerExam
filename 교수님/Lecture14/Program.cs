using System.Net;
using System.Text.Json;
using Lecture14;

HttpListener listener = new HttpListener();
listener.Prefixes.Add("http://127.0.0.1:3000/GodDamn/");
listener.Start();
Console.WriteLine("Listening...");
HttpListenerContext context = listener.GetContext();
HttpListenerRequest request = context.Request;
HttpListenerResponse response = context.Response;
response.AddHeader("Access-Control-Allow-Origin", "*");
Stream body = request.InputStream;
System.Text.Encoding encoding = request.ContentEncoding;
System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);

string rawurl = request.RawUrl;
string httpmethod = request.HttpMethod;

string result = "";

result += string.Format("httpmethod = {0}\r\n", httpmethod);
result += string.Format("rawurl = {0}\r\n", rawurl);
System.Console.WriteLine(result);

soo s = new soo();
s.body = "MaryJane";

string responseString = JsonSerializer.Serialize(s);
System.Console.WriteLine(responseString);
byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
response.OutputStream.Write(buffer, 0, buffer.Length);
response.OutputStream.Close();
listener.Stop();