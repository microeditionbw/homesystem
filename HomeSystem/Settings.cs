using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HomeSystem
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            label16.Text = "Команда: " + textBox1.Text + " + " + textBox10.Text;
        }
        private IniFile ini = new IniFile(Application.StartupPath + @"\commands.ini");
        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = Application.StartupPath + @"\Intellect";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    textBox11.Text = openFileDialog1.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (textBox10.Text != "")
            {
                button8.Enabled = true;
                ini.Write(textBox10.Text, "Sound", textBox11.Text);
                if(textBox2.Text != "")
                {
                    ini.Write(textBox10.Text, "Program", textBox2.Text);
                }
                if (comboBox2.Text != "")
                {
                    ini.Write(textBox10.Text, "Special", comboBox2.Text);
                }
                if(keyboard_timer)
                {
                    ini.Write(textBox10.Text, "timer", textBox5.Text);
                }
                MessageBox.Show("ok");
            }
            else
            {
                MessageBox.Show("Укажите сообщение голосовое");
            }
        }
        public void del_key(string key)
        {
            ini.Delete(key);
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox3.Text = "";
            textBox4.Text = "";
            textBox12.Text = "";
            string sound = ini.Read(comboBox1.Text, "Sound");
            string program = ini.Read(comboBox1.Text, "Program");
            string special = ini.Read(comboBox1.Text, "Special");
            textBox12.Text = ini.Read(comboBox1.Text, "Sound");
            if(program != string.Empty)
            {
                textBox3.Text = program;
            }
            if (special != string.Empty)
            {
                textBox4.Text = special;
            }
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            textBox1.Text = ini.getParam("Speech", "Core");
            textBox11.Text = Application.StartupPath + @"\Intellect\Jarvis\Да сэр.mp3";
            comboBox1.Items.Clear();
            var section = ini.SectionNames();
            if (section != null)
            {
                foreach (var item in section)
                {
                    if (item.ToString() != "Speech")
                    {
                        comboBox1.Items.Add(item.ToString());
                    }
                }
            }

            string[] specials = new string[] {
                "немедленно остановить звуки",
                "включить нумлок", "выключить нумлок", "включить капслок",
                "выключить капслок", "включить скроллок", "выключить скроллок"
            };
            foreach (var special in specials)
            {
                comboBox2.Items.Add(special);
            }
            string indicators = ini.getParam("Indicators", "enabled");
            if(indicators=="true")
            {
                checkBox1.Checked = true;
            }
            else
            {
                checkBox1.Checked = false;
            }
            string brain = ini.getParam("Main", "brain");
            if (brain == "true")
            {
                checkBox2.Checked = true;
            }
            else
            {
                checkBox2.Checked = false;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if(comboBox1.Text != "")
            {
                del_key(comboBox1.Text);
                MessageBox.Show("Удалено: " + comboBox1.Text);
                comboBox1.Items.Clear();
                var section = ini.SectionNames();
                if(section != null)
                {
                    foreach (var item in section)
                    {
                        if (item.ToString() != "Speech")
                        {
                            comboBox1.Items.Add(item.ToString());
                        }
                    }
                }

                comboBox1.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox12.Text = "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Application.ExecutablePath);
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ini.setParam("Speech", "Core", textBox1.Text);
            MessageBox.Show("готово");
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog2 = new OpenFileDialog();

            openFileDialog2.InitialDirectory = Application.StartupPath + @"\Intellect";
            openFileDialog2.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog2.FilterIndex = 2;
            openFileDialog2.RestoreDirectory = true;

            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    textBox2.Text = openFileDialog2.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.CheckState == CheckState.Checked)
            {
                ini.setParam("Indicators", "enabled", "true");
            }
            else
            {
                ini.setParam("Indicators", "enabled", "false");
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.CheckState == CheckState.Checked)
            {
                ini.setParam("Main", "brain", "true");
            }
            else
            {
                ini.setParam("Main", "brain", "false");
            }
        }

        private void Settings_MouseMove(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

        bool keyboard_timer = false;
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text == "включить нумлок" || comboBox2.Text == "включить капслок" || comboBox2.Text == "включить скроллок")
            {
                label7.Visible = true;
                textBox5.Visible = true;
                keyboard_timer = true;
            }
            else
            {
                keyboard_timer = false;
                label7.Visible = false;
                textBox5.Visible = false;
            }

        }
    }
}
