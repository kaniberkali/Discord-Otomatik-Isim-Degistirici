using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Threading;
using System.IO;

namespace Discord_Otomatik_İsim_Değiştirici
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }
        OpenQA.Selenium.Chrome.ChromeDriver drv; Thread th;
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length < 32)
            {
                listBox1.Items.Add(textBox1.Text);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            }
            else
                MessageBox.Show("Çok uzun isim girdiniz", "@kodzamani.tk");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Remove(listBox1.SelectedItem);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = listBox1.Text;
        }
        private void yükleniyor()
        {
            try
            {
                string dosya_yolu = @"İsimler.txt";
                FileStream fs = new FileStream(dosya_yolu, FileMode.Open, FileAccess.Read);
                StreamReader sw = new StreamReader(fs);
                string yazi = sw.ReadLine();
                while (yazi != null)
                {
                    if (yazi.Length < 32)
                    {
                        listBox1.Items.Add(yazi);
                        label6.Text = yazi + " Eklendi.";
                    }
                    else
                    {
                        label6.Text = "İsim çok uzun.";
                    }
                    yazi = sw.ReadLine();
                }
                sw.Close();
                fs.Close();
            }
            catch
            {
                label6.Text = "İsimler çekilemedi.";
            }
            Thread.Sleep(200);

            label6.Text = "Chrome Ayarları Yapılıyor.";
            ChromeOptions option = new ChromeOptions();
            option.AddExcludedArgument("enable-automation");
            option.AddAdditionalCapability("useAutomationExtension", false);
            label6.Text = "Chrome Ayarları Yapıldı.";
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            drv = new ChromeDriver(service, option);
            label6.Text = "Chrome Oluşturuldu.";
            drv.Navigate().GoToUrl("https://discord.com/login");
            label6.Text = "Lütfen giriş yapın.";
            button3.Enabled = true;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            th = new Thread(yükleniyor);th.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count >= 2)
            {
                if (button3.Text == "Başlat")
                {
                    check = false;
                    breaks = false;
                    kontrol = true;
                    button3.Text = "Durdur";
                    timer1.Enabled = true;
                }
                else
                {
                    check = true;
                    breaks = true;
                    timer1.Enabled = false;
                    kontrol = false;
                    button3.Text = "Başlat";
                }
            }
            else
                MessageBox.Show("Lütfen en az 2 adet isim girin.", "@kodzamani.tk");
        }
        bool kontrol = true;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (numericUpDown1.Value == 0)
                timer1.Interval = 100;
            else
            timer1.Interval = Convert.ToInt32(numericUpDown1.Value) * 1000;
            try
            {
                if (kontrol == true)
                    th = new Thread(islemler); th.Start();
            }
            catch { }
        }
        bool breaks = false;
        bool check = false;
        private void islemler()
        {
            if (drv.Url.Contains("login") == true)
                label6.Text = "Giriş Bekleniyor.";
            else if (drv.Url.Contains("channels/@me") == true)
                label6.Text = "Sunucu Bekleniyor.";
            else if (drv.Url.Contains("channels") == true)
            {
                label6.Text = "Sunucu Seçildi.";
                check = true;
            }
            if (check == true)
            {
                try
                {
                    kontrol = false;
                    for (int i = 0; i <= listBox1.Items.Count; i++)
                    {
                        if (breaks == true)
                        {
                            kontrol = false;
                            break;
                        }
                        if (i == listBox1.Items.Count)
                            kontrol = true;
                        else
                        {
                            listBox1.SelectedIndex = i;
                            label6.Text = "Kullaniciadi Siliniyor.";
                            drv.FindElement(By.XPath("//html/body/div/div[2]/div/div[2]/div/div/div/div[2]/div[1]/nav/div[1]")).Click();
                            Thread.Sleep(200);
                            drv.FindElement(By.XPath("//html/body/div/div[6]/div/div/div/div/div[7]/div[1]")).Click();
                            Thread.Sleep(200);
                            try
                            {
                                drv.FindElement(By.XPath("//html/body/div/div[6]/div[2]/div/div/form/div[2]/button")).Click();
                            }
                            catch { }
                            label6.Text = "Kullaniciadi Silindi.";
                            Thread.Sleep(200);
                            drv.FindElement(By.XPath("//html/body/div/div[6]/div[2]/div/div/form/div[2]/div[1]/input")).SendKeys(listBox1.Items[i].ToString());
                            label6.Text = "Yeni kullaniciadi Yazılıyor.";
                            Thread.Sleep(200);
                            drv.FindElement(By.XPath("//html/body/div/div[6]/div[2]/div/div/form/div[3]/button[1]")).Click();
                            label6.Text = "Kullaniciadi değiştirildi.";
                        }
                    }
                }
                catch
                {
                    kontrol = true;
                }
            }
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Move == 1)
            {
                this.SetDesktopLocation(MousePosition.X - Mouse_X, MousePosition.Y - Mouse_Y);
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            Move = 1;
            Mouse_X = e.X;
            Mouse_Y = e.Y;
        }
        int Move;
        int Mouse_X;
        int Mouse_Y;
        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            Move = 0;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                string dosya_yolu = @"İsimler.txt";
                FileStream fs = new FileStream(dosya_yolu, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                for (int i = 0; i < listBox1.Items.Count; i++)
                    sw.WriteLine(listBox1.Items[i].ToString());
                sw.Flush();
                sw.Close();
                fs.Close();
            }
            catch { }
            try
            {
                if (drv != null)
                    drv.Quit();
            }
            catch { }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }
    }
}
