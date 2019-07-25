using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using System.Configuration;
using System.Text.RegularExpressions;
using CCWin;

namespace TextToVoice
{
    public partial class Form1 : CCWin.CCSkinMain
    {
        public Form1()
        {
            InitializeComponent();
        }
        Mp3Player mp3Play = new Mp3Player();
        Class1 CL = new Class1();
        private void button1_Click(object sender, EventArgs e)
        {
           


        }


        /// <summary>
        /// 获得字符串中开始和结束字符串中间得值
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="s">开始</param>
        /// <param name="e">结束</param>
        /// <returns></returns> 
        public static string GetValue(string str, string s, string e)
        {
            Regex rg = new Regex("(?<=(" + s + "))[.\\s\\S]*?(?=(" + e + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            return rg.Match(str).Value;
        }


        /// <summary>
        /// 使用utf-8编码，urlencode
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }

            return (sb.ToString());
        }



        /// <summary>
        /// 字符串编码base64
        /// </summary>
        /// <param name="text">待编码的文本字符串</param>
        /// <returns>编码的文本字符串.</returns>
        public static string Encode(string text)
        {
            var plainTextBytes = Encoding.ASCII.GetBytes(text);
            //var base64 = Convert.ToBase64String(plainTextBytes).Replace('+', '-').Replace('/', '_').TrimEnd('=');
            var base64 = Convert.ToBase64String(plainTextBytes);
            return base64;
        }

        /// <summary>
        /// 解码安全的URL文本字符串的Base64
        /// </summary>
        /// <param name="secureUrlBase64">Base64编码字符串安全的URL.</param>
        /// <returns>Cadena de texto decodificada.</returns>
        public static string Decode(string secureUrlBase64)
        {
            secureUrlBase64 = secureUrlBase64.Replace('-', '+').Replace('_', '/');
            switch (secureUrlBase64.Length % 4)
            {
                case 2:
                    secureUrlBase64 += "==";
                    break;
                case 3:
                    secureUrlBase64 += "=";
                    break;
            }
            var bytes = Convert.FromBase64String(secureUrlBase64);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>  
        /// 时间戳转为C#格式时间  
        /// </summary>  
        /// <param name="timeStamp">Unix时间戳格式</param>  
        /// <returns>C#格式时间</returns>  
        public static DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }


        /// <summary>  
        /// DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time"> DateTime时间格式</param>  
        /// <returns>Unix时间戳格式</returns>  
        public static int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

       
        /// <summary>
        /// MD5　32位加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UserMd5(string str)
        {
            string cl = str;
            string pwd = "";
            MD5 md5 = MD5.Create();//实例化一个md5对像
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString("x2");

            }
            return pwd;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CL.loadConfig();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetConfig fm2 = new SetConfig();
            fm2.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            mp3Play.StopT();
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            SetConfig fm2 = new SetConfig();
            fm2.Show();
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入需要合成的内容！");
                return;
            }


            string con = $"tex={UrlEncode(textBox1.Text)}&lan=zh&cuid={CL.Cuid}&ctp={CL.Ctp}&aue={CL.Aue}&tok={CL.Tok}&spd={CL.Spd}&per={CL.Per}&pit={CL.Pit}&vol={CL.Vol}";

            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = "https://tsn.baidu.com/text2audio",
                Method = "post",
                ContentType = "application/x-www-form-urlencoded; charset=utf-8",
                ResultType = ResultType.Byte,
                Postdata = con

            };

            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            string cookie = result.Cookie;
            string h = result.Header.ToString();
            int x = h.IndexOf("audio/mp3");//a会等于1

            string path = @Application.StartupPath + "\\audio\\" + string.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + ".mp3";
            if (x > 0)
            {
                byte[] byteArray = result.ResultByte;
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);

                }
                Mp3Player mp3Play = new Mp3Player()
                {
                    FileName = path,
                };
                mp3Play.play();
            }
            else
            {
                string xx = GetValue(html, "\"err_no\":", ",\"");
                switch (xx)
                {
                    case "500":
                        MessageBox.Show("不支持输入");
                        break;
                    case "501":
                        MessageBox.Show("输入参数不正确");
                        break;
                    case "502":
                        MessageBox.Show("token验证失败");
                        break;
                    case "503":
                        MessageBox.Show("合成后端错误");
                        break;

                    default:
                        MessageBox.Show("未知错误！");
                        break;
                }
            }


        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            mp3Play.StopT();
        }
    }
}
