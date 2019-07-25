using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace TextToVoice
{
    public  class Class1
    {
        public string API = "";
        public string Secret = "";
        public string Tok = "";
        public string Cuid = "";
        public string Ctp = "";
        public string Lan = "";
        public string Spd = "";
        public string Pit = "";
        public string Vol = "";
        public string Per = "";
        public string Aue = "";

        public  bool loadConfig()
        {
            API = ConfigurationManager.AppSettings["API"];
            Secret = ConfigurationManager.AppSettings["Secret"];
            Tok = ConfigurationManager.AppSettings["Tok"];
            Cuid = ConfigurationManager.AppSettings["Cuid"];
            Ctp = ConfigurationManager.AppSettings["Ctp"];
            Lan = ConfigurationManager.AppSettings["Lan"];
            Spd = ConfigurationManager.AppSettings["Spd"];
            Pit = ConfigurationManager.AppSettings["Pit"];
            Vol = ConfigurationManager.AppSettings["Vol"];
            Per = ConfigurationManager.AppSettings["Per"];
            Aue = ConfigurationManager.AppSettings["Aue"];
            return true;
        }
    }
}
