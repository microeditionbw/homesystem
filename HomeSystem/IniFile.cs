using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HomeSystem
{
    class IniFile
    {

        public string path;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
                 string key, string def, StringBuilder retVal,
            int size, string filePath);
        [DllImport("kernel32")]
        static extern uint GetPrivateProfileSectionNames(IntPtr pszReturnBuffer, uint nSize, string lpFileName);
        internal void Write(string v1, string v2, Func<string> toString)
        {
            throw new NotImplementedException();
        }
        public string[] SectionNames()
        {
            string path = this.path;
            uint MAX_BUFFER = 32767;
            IntPtr pReturnedString = Marshal.AllocCoTaskMem((int)MAX_BUFFER);
            uint bytesReturned = GetPrivateProfileSectionNames(pReturnedString, MAX_BUFFER, path);
            if (bytesReturned == 0)
                return null;
            string local = Marshal.PtrToStringAnsi(pReturnedString, (int)bytesReturned).ToString();
            Marshal.FreeCoTaskMem(pReturnedString);
            //use of Substring below removes terminating null for split
            return local.Substring(0, local.Length - 1).Split('\0');
        }

        public IniFile(string INIPath)
        {
            path = INIPath;
            if (!File.Exists(path))
            {
                File.WriteAllText(path, String.Empty);
            }
        }
        private string parampath = Application.StartupPath + "/Params.ini";
        public void setParam(string Section, string Key, string Value)
        {
            if (!File.Exists(parampath))
            {
                File.WriteAllText(path, String.Empty);
            }
            WritePrivateProfileString(Section, Key, Value, parampath);
        }
        public string getParam(string Section, string Key)
        {
            if (!File.Exists(parampath))
            {
                File.WriteAllText(path, String.Empty);
            }
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, " ", temp,
                                            255, Application.StartupPath + "/Params.ini");
            return temp.ToString();
        }
        public void Write(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.path);
        }
        public void Delete(string Section)
        {
            WritePrivateProfileString(Section, null, null, this.path);
        }

        public string Read(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, " ", temp,
                                            255, this.path);
            return temp.ToString();
        }

        public string Crypt(string text)
        {
            string rtnStr = string.Empty;
            foreach (char c in text)
            {
                rtnStr += (char)((int)c ^ 8);
            }

            return rtnStr;
        }
    }
}
