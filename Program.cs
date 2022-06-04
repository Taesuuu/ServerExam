using System.Net;
using System.Text;
using System.IO;

internal class Program
{
    private static void Main(string[] args)
    {
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add("http://127.0.0.1:3000/Login/");
        listener.Start();

        Console.WriteLine("Listening...");

        HttpListenerContext context = listener.GetContext();
        HttpListenerRequest request = context.Request;
        HttpListenerResponse response = context.Response;
        
       //FileStream fileStream = File.Open("/index.html", FileMode.Open);
        try {
            string file = File.ReadAllText("@/index.html");
            // StreamReader sr = new StreamReader("/index.html");
            // FileStream file = File.ReadAllBytes("@/index.html");
            Console.WriteLine(file);
            byte[] buffer = Encoding.UTF8.GetBytes(file);
            response.OutputStream.Write(buffer, 0, buffer.Length);
            Console.WriteLine("성공");
        }
        catch {
            Console.WriteLine("실패");
            response.OutputStream.Close();
            listener.Stop();
        }
       
        
    }
}




// namespace Jts
// {
//     class HttpServer {

//         HttpServer() {

//         }
//         HttpClient client = new HttpClient();

//     }
// }