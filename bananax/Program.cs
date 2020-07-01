// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace SampleApp
{
    public class Program
    {

        private static readonly StringBuilder sb = new StringBuilder(500);

        private static readonly StringBuilder sb2 = new StringBuilder(500);
        private static readonly string url = "http://android.fuliapps.com/search?page=1&wd={0}&apiversion=28";
        private static int count2;

        private static List<string> arrayStock = new List<string>
        {
                "������",
            "������",
            "�ʿư�",
            "����ľ",
            "������",
            "�д��",
            "С����",
            "��ʹ��",
            "������",
            "���ΰ�",

            "������",
            "ӣ����",
            "����",
            "���ϣ",
            "�ɵ���",
            "����δ",
            "��Ұǧ",
            "������",
            "��ԭ����",
            "������",
            "��ԭ־",
            "��ʯ��",
            "����",
            "�ٶ�� ",
            "�Ӻ��� ",
            "�ű���   ",
            "������   ",
            "��������   ",
            "�������   ",

            "���ջ� ",

            "���༆��   ",
            "��������   ",

            "������   ",
            "��׵��   ",

            "����ľ�� ",
            "������     ",


            "ӣľ��     ",
            "��m ",

            "�Ծ���     ",

            "ˮ��ӣ     ",
            "�Ё���   ",

            "�캣��     ",
            "����Ұ�� ",
            "������     ",

            "����ʥ  ",
            "������  ",
            "��ԭ��   ",
            "԰����   ",
        
            "ǰ����",
           
            "��ǧ�{     ",
            "��˾       ",
            "���ػ���   ",

            "������",
            "������",
            "AIKA",
            "ˮҰ",
            "����",
            "������",
            "����",
            "����",
            "��",
            "��ľ",
            "julia",
            "��������",
            "׵��",
            "ͩ��",
            "�񲨶�һ",
            "С��������",
            "������",
            "����",
            "�ɵ�",
            "����ϣ",
            "��������",
            "�ɺ�",
            "��ɽ",
            "����",
            "һ֮��",
            "��Ұ",
            "����",
            "԰��",
            "��ʯ",
            "�S����",
            "����",
            "��ׯ��",
            "ɽ��",
            "�",
            "����",
            "����",
            "����",
            "���ɻ�",
            "����ɴ����",
            "ܥ���ν���",
            "ʯԭ����",
            "��������",
            "ϣ��",
            "��Ұ��",
            "��ɽ��",
            "�д�����",
            "�Ŵ�",
			"��ľ��",
			"ϣ־",
		    "��������",
			"���ΰ�",
			"�Ұ",
			"����",
			"��������",
			"����ľ",
			"������",
			"������",
			"���ﰮ",
			













        };

          

        private static ILogger<Program> logger;
        public static void Main(string[] args)
        {
            var loggingConfiguration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("logging.json", optional: false, reloadOnChange: true)
                .Build();

            // A Web App based program would configure logging via the WebHostBuilder.
            // Create a logger factory with filters that can be applied across all logger providers.
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddConfiguration(loggingConfiguration.GetSection("Logging"))
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("SampleApp.Program", LogLevel.Debug)
                    .AddLog4Net()
                    .AddConsole()
                    .AddEventLog();
            });

            // Make sure to dispose ILoggerFactory
            using var factory = loggerFactory;
            logger = loggerFactory.AddLog4Net().CreateLogger<Program>();
            Console.WriteLine("������start���У�ֱ������over������continue������ֱ������over����end������");
            Console.ReadLine();

            foreach (var el in arrayStock)
            {
                SetpTwo(el.Trim().ToString());

            }
        }

        private static void SetpTwo(string code)
        {

            string responsedata = Reader(string.Format(url, code), false);
            
             var obj = JsonConvert.DeserializeObject<ResponseData>(responsedata);



            var i = 0;
            foreach (var el in obj.data.vodrows)
            {
                var tempurl = String.Format("http://android.fuliapps.com{0}", el.play_url);
                sb.Append(tempurl).Append("\r\n");
                i++;
                SetpThree(tempurl, code, i);
            }
            count2++;

            if (count2 > 0)
            {

                //logger.LogInformation(sb.ToString());

                sb.Clear();

            }



            Console.WriteLine("�����ϰ� \n" + code);
        }
        private static void SetpThree(string url, string code, int i)
        {

            //http://android.fuliapps.com/vod/reqplay/4695
            try
            {


                string responsedata = Reader(url, true);
                var obj = JsonConvert.DeserializeObject<ResponseData2>(responsedata);





                sb2.Append(code).Append(i);
                sb2.ToString().Replace(",", "");
                sb2.Append(",")
                    .Append(string.IsNullOrEmpty(obj.data.httpurl) ? obj.data.httpurl_preview.Replace("https:","http:") : obj.data.httpurl.Replace("https:", "http:"));


                if (sb2.Length > 0)
                {

                    


                    logger.LogInformation("sb2.ToString()",null);
                    sb2.Clear();

                }

            }
            catch (Exception)
            {

                return;
            }
            finally
            {
                Console.WriteLine("�����ϰ� \n" + url);
            }

        }
        private static string Reader(string url, bool isNeedCookie)
        {
            while (true)
            {
                Thread.Sleep(500);
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/json; charset=UTF-8";
                request.Method = "get";
                request.Headers.Set("Accept-Language", "zh-CN");
                if (isNeedCookie)
                {
                    var cc = new CookieContainer();
                    string cookie = "xxx_api_auth=3832323264383563303339333739333763346336653561326132663061613035";
                    cc.SetCookies(new Uri("http://android.fuliapps.com"), cookie);
                    request.CookieContainer = cc;
                }
                // request.Credentials = CredentialCache.DefaultCredentials;
                // request.Timeout = 1000 * 1;
                // request.ServicePoint.ConnectionLimit = 1000;

                try
                {
                    var httpResponse = (HttpWebResponse)request.GetResponse();


                    //using (var stream = new GZipStream(httpResponse.GetResponseStream(), CompressionMode.Decompress))
                    //{
                    //    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    //    {
                    //        return reader.ReadToEnd();
                    //    }
                    //}

                    //if (httpResponse.ContentEncoding.ToLower().Contains("gzip"))
                    //{
                    //    using (GZipStream stream = new GZipStream(httpResponse.GetResponseStream(), CompressionMode.Decompress))
                    //    {
                    //        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    //        {
                    //            return reader.ReadToEnd();
                    //        }
                    //    }
                    //}


                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream(),Encoding.GetEncoding("utf-8")))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    Thread.Sleep(1000 * 10);
                }
            }
        }



    }

   

    public class ResponseData2
    {
        public ResultSets2 data { get; set; }
        public int retcode { get; set; }
        public string errmsg { get; set; }
    }
    public class ResultSets2
    {
        public string httpurl { get; set; }

        public string httpurl_preview { get; set; }


    }

    public class ResponseData
    {
        public ResultSets data { get; set; }
        public int retcode { get; set; }
        public string errmsg { get; set; }
    }

    public class ResultSets
    {
        public List<ColDes> vodrows { get; set; }

    }

    public class ColDes
    {
        public string down_url { get; set; }
        public string play_url { get; set; }
    }
}
