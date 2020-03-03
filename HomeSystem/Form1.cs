using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Text;
using Microsoft.Win32;
using System.IO;
using Microsoft.Speech.Recognition;
using System.Threading;

namespace HomeSystem
{
    public partial class Form1 : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        CultureInfo _culture;
        SpeechRecognitionEngine _sre;
        private IniFile ini = new IniFile(Application.StartupPath + @"\commands.ini");
        public Form1()
        {
            InitializeComponent();
            _culture = new CultureInfo("ru-RU");
            _sre = new SpeechRecognitionEngine(_culture);
            //   _sre.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(sr_SpeechDetected);
            //   _sre.RecognizeCompleted += new EventHandler<RecognizeCompletedEventArgs>(sr_RecognizeCompleted);
            //    _sre.SpeechHypothesized += new EventHandler<SpeechHypothesizedEventArgs>(sr_SpeechHypothesized);
            //      _sre.SpeechRecognitionRejected += new EventHandler<SpeechRecognitionRejectedEventArgs>(sr_SpeechRecognitionRejected);
            _sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sr_SpeechRecognized);
            _sre.SetInputToDefaultAudioDevice();
            _sre.LoadGrammar(CreateSampleGrammar1());
            _sre.LoadGrammar(new Grammar(new GrammarBuilder("неизвестно", SubsetMatchingMode.SubsequenceContentRequired)));
            _sre.LoadGrammar(new Grammar(new GrammarBuilder(" ", SubsetMatchingMode.SubsequenceContentRequired)));
            _sre.LoadGrammar(new Grammar(new GrammarBuilder("фыавфыаф", SubsetMatchingMode.SubsequenceContentRequired)));
            _sre.LoadGrammar(new Grammar(new GrammarBuilder("вфыаафыфывфв", SubsetMatchingMode.SubsequenceContentRequired)));
            _sre.RecognizeAsync(RecognizeMode.Multiple);
        }

        void backgroud_load()
        {
            string brain = ini.getParam("Main", "brain");
            if (brain == "true")
            {
                axWindowsMediaPlayer1.Visible = true;
                axWindowsMediaPlayer1.URL = Application.StartupPath + @"\intellect\core\bg.mp4";
                axWindowsMediaPlayer1.uiMode = "none";
                axWindowsMediaPlayer1.settings.autoStart = true;
                axWindowsMediaPlayer1.settings.setMode("loop", true);
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
            else
            {
                axWindowsMediaPlayer1.Visible = false;
            }
        }

        void autoload()
        {
            string ExePath = System.Windows.Forms.Application.ExecutablePath;
            RegistryKey reg;
            reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
            reg.SetValue("HomeSystem", ExePath);
            reg.Close();
        }
        void load_params()
        {
            string indicators = ini.getParam("Indicators", "enabled");
            if(indicators == "true")
            {
                NUM.Visible = true;
                SCROLL.Visible = true;
                CAPS.Visible = true;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            load_params();
            backgroud_load();
            autoload();
            axWindowsMediaPlayer1.Ctlenabled = false;
        }

        private void sr_SpeechDetected(object sender, SpeechDetectedEventArgs e)
        {
            label1.Text = (e.ToString());
        }
        private Choices CreateSampleChoices()
        {
            try
            {
                var newch = new Choices();
                var section = ini.SectionNames();
                if(section!=null)
                {
                    foreach (var item in section)
                    {
                        newch.Add(item.ToString());
                    }
                }
                else
                {
                    newch.Add("привет");
                }
                return newch;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private Grammar CreateSampleGrammar1()
        {
            string dic = ini.getParam("Speech", "Core");
            if (dic == string.Empty)
            {
                dic = "система";
            }
            try
            {
                var programs = CreateSampleChoices();
                var grammarBuilder = new GrammarBuilder(dic, SubsetMatchingMode.SubsequenceContentRequired);
                grammarBuilder.Culture = _culture;
                grammarBuilder.Append(new SemanticResultKey("open", programs));
                return new Grammar(grammarBuilder);
            }
            catch (Exception)
            {
                throw;
            }
        }

        
        WMPLib.WindowsMediaPlayer wplayer1 = new WMPLib.WindowsMediaPlayer();
        private void sr_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            try
            {
                if (e.Result.Confidence > 0.5)
                {
                    var section = ini.SectionNames();
                    foreach (var item in section)
                    {
                        string sound = ini.Read(item.ToString(), "Sound");
                        string mainkey = ini.getParam("Speech", "Core");
                        string program = ini.Read(item.ToString(), "Program");
                        string special = ini.Read(item.ToString(), "Special");
                        if (e.Result.Text == mainkey + " " + item.ToString())
                        {
                            if (Path.GetExtension(sound) == ".mp3" && sound != string.Empty)
                            {
                                say(sound);
                            }
                            if (program != string.Empty)
                            {
                                Process.Start(program);
                            }
                            if (special != string.Empty)
                            {
                                special_commands(special);
                            }
                        }
                    }
                    label1.Text = (e.Result.Text);
                }
            }
            catch (Exception)
            {
                throw;
            }
           
        }
        void special_commands(string special)
        {
            switch (special)
            {
                case "немедленно остановить звуки":
                    wplayer1.controls.stop();
                    break;
                case "включить нумлок":
                    microcontroller.SetNumLockKey(true);
                    NUM.BackColor = System.Drawing.Color.Green;
                    break;
                case "выключить нумлок":
                    microcontroller.SetNumLockKey(false);
                    NUM.BackColor = System.Drawing.Color.White;
                    break;
                case "включить капслок":
                    microcontroller.SetCapsLockKey(true);
                    CAPS.BackColor = System.Drawing.Color.Green;
                    break;
                case "выключить капслок":
                    microcontroller.SetCapsLockKey(false);
                    CAPS.BackColor = System.Drawing.Color.White;
                    break;
                case "включить скроллок":
                    microcontroller.SetScrollLockKey(true);
                    SCROLL.BackColor = System.Drawing.Color.Green;
                    break;
                case "выключить скроллок":
                    microcontroller.SetScrollLockKey(false);
                    SCROLL.BackColor = System.Drawing.Color.White;
                    break;
                default:
                    break;
            }
        }
        void say(string path)
        {
            wplayer1.URL = path;
            wplayer1.controls.play();
        }

        private void axWindowsMediaPlayer1_MouseDownEvent(object sender, AxWMPLib._WMPOCXEvents_MouseDownEvent e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();
            settings.Show();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }
    }
}
