using System;
using System.IO;
using System.Text;
using System.Text.Json;
// 201727026_정태수
namespace FinalTestSample
{
    public class FileLogger
    {
        public string _fileName { get; set; }

        public string logPath = null;
        public FileStream fsa = null;

        public FileLogger(string fileName)
        {
            //문제 6:
            //_fileName에 filename + 날짜.log 로 로그파일명 저장
            //예제 > fileName이 Log일때 Log220616.log
            //_fileName = Log220616.log
            // _fileName = fileName + DateTime.Now.ToString("yyMMdd") + ".log";
            string savePath = @fileName + DateTime.Now.ToString("yyMMdd") + ".log";
            logPath = savePath;
            if (!File.Exists(savePath))
            {
                using (fsa = File.Create(savePath))
                {
                    Console.WriteLine("파일 생성 성공");
                    fsa.Close();
                }
            }
            else
            {
                Console.WriteLine("이미 파일이 존재 합니다.");
            }
            
        }

        public void Write(string logType, string logMessage)
        {
            //문제 7:
            //로그는 [날짜 시간] <logType> logMessage로 작성되도록 아래 log라는 string에 저장
            //예시 > [2022-06-16 12:00:00] <Error> 로그인 에러
            //fsa = File.Open(logPath, FileMode.Open);
            string logTime = DateTime.Now.ToString("[yyyy-MM-dd hh:mm:ss]");

            string logText = logTime + " <" + logType +"> " + logMessage + '\n';
            System.IO.File.AppendAllText(logPath, logText, Encoding.UTF8);
            if(fsa != null)
            {
                fsa.Close();
            }
            
        }

        

    }
}
