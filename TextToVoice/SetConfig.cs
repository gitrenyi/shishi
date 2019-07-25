using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Text.RegularExpressions;

namespace TextToVoice
{
    public partial class SetConfig : Form
    {
        public SetConfig()
        {
            InitializeComponent();
        }
        Class1 CL = new Class1();
        private void SetConfig_Load(object sender, EventArgs e)
        {
            txtAPI.Text = ConfigurationManager.AppSettings["API"];
            txtSecret.Text = ConfigurationManager.AppSettings["Secret"];            
            txtTok.Text = ConfigurationManager.AppSettings["Tok"];
            txtCuid.Text = ConfigurationManager.AppSettings["Cuid"];
            txtCtp.Text = ConfigurationManager.AppSettings["Ctp"];
            txtLan.Text = ConfigurationManager.AppSettings["Lan"];
            txtSpd.Text = ConfigurationManager.AppSettings["Spd"];
            txtPit.Text = ConfigurationManager.AppSettings["Pit"];
            txtVol.Text = ConfigurationManager.AppSettings["Vol"];
            txtPer.Text = ConfigurationManager.AppSettings["Per"];
            txtAue.Text = ConfigurationManager.AppSettings["Aue"];

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetConfigValue("API", txtAPI.Text);
            SetConfigValue("Secret", txtSecret.Text);
            SetConfigValue("Tok", txtTok.Text);
            SetConfigValue("Cuid", txtCuid.Text);
            SetConfigValue("Ctp", txtCtp.Text);
            SetConfigValue("Lan", txtLan.Text);
            SetConfigValue("Spd", txtSpd.Text);
            SetConfigValue("Pit", txtPit.Text);
            SetConfigValue("Vol", txtVol.Text);
            SetConfigValue("Per", txtPer.Text);
            SetConfigValue("Aue", txtAue.Text);
            CL.loadConfig();
            MessageBox.Show("保存成功！");
        }


        /// <summary>
        /// 修改AppSettings中配置
        /// </summary>
        /// <param name="key">key值</param>
        /// <param name="value">相应值</param>
        public static bool SetConfigValue(string key, string value)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (config.AppSettings.Settings[key] != null)
                    config.AppSettings.Settings[key].Value = value;
                else
                    config.AppSettings.Settings.Add(key, value);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (txtAPI.Text != "" || txtSecret.Text != "")
            {
                string url = $"https://openapi.baidu.com/oauth/2.0/token?grant_type=client_credentials&client_id={txtAPI.Text}&client_secret={txtSecret.Text}";
                HttpHelper http = new HttpHelper();
                HttpItem item = new HttpItem()
                {
                    URL = url,//URL     必需项
                    Method = "get",//URL     可选项 默认为Get
                                                                                                                                                                       //ProxyPwd = "123456",//代理服务器密码     可选项
                                                                                                                                                                                //ProxyUserName = "administrator",//代理服务器账户名     可选项
                    ResultType = ResultType.String,//返回数据类型，是Byte还是String
                };
                HttpResult result = http.GetHtml(item);
                string html = result.Html;
                string cookie = result.Cookie;
                string xx = GetValue(html, "\"access_token\":\"", "\",\"");
                txtTok.Text = xx;
            }
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

    }
}
