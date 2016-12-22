using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KodKopyalayici
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var kurumAdlari = this.KurumlariGetir();
            var kurumAdlari2 = this.KurumlariGetir();
            this.cmbKaynakKurum.DataSource = kurumAdlari;
            this.cmbHedefKurum.DataSource = kurumAdlari2;//yoksa birini seçince diğeri de değişiyor.

        }

        private string AyarlarDosyaAdi = "Ayarlar.txt";
        public string[] KurumlariGetir()
        {
            return File.ReadAllLines(AyarlarDosyaAdi);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string anaDosya, hedefDosya;
            anaDosya = Path.Combine(cmbKaynakKurum.Text, txtDosyaAdi.Text);
            hedefDosya = Path.Combine(cmbHedefKurum.Text, txtDosyaAdi.Text);

            await TortoiseDiffAsync(anaDosya, hedefDosya);
            SonrakiHedef();
        }
        private async void button2_Click(object sender, EventArgs e)
        {
            string anaDosya, hedefDosya;
            anaDosya = Path.Combine(cmbKaynakKurum.Text, txtDosyaAdi.Text);
            hedefDosya = Path.Combine(cmbHedefKurum.Text, txtDosyaAdi.Text);

            await BeyondeCompareDiffAsync(anaDosya, hedefDosya);
            SonrakiHedef();
        }
        public void SonrakiHedef()
        {
            var index = cmbHedefKurum.SelectedIndex;
            index++;
            var count = cmbHedefKurum.Items.Count;
            if (index < count)
            {
                cmbHedefKurum.SelectedIndex = index;
            }
        }
        private Task BeyondeCompareDiffAsync(string AnaDosya, string HedefDosya)
        {
            return Task.Run(() =>
            {

                //Open Tortoise for show diff
                var commandLineArgument = string.Format("\"{0}\" \"{1}\" /title1=\"{0}\" /title2=\"{1}\"",
                    AnaDosya,
                    HedefDosya
                    );

                var exeName = @"C:\Users\Hplaptop6570b\Desktop\Beyond Compare 4\BComp.exe";

                System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(exeName, commandLineArgument);
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo = info;
                p.Start();
                p.WaitForExit();
            });

        }

        private Task TortoiseDiffAsync(string AnaDosya, string HedefDosya)
        {
            return Task.Run(() =>
            {

                //Open Tortoise for show diff
                var commandLineArgument = string.Format("/base:\"{0}\" /mine:\"{1}\" /basename:\"Öncesi\" /minename:\"Sonrası\"",
                    AnaDosya,
                    HedefDosya
                    );

                var exeName = @"C:\Program Files\TortoiseSVN\bin\TortoiseMerge.exe";

                System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(exeName, commandLineArgument);
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo = info;
                p.Start();
                p.WaitForExit();

            });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string anaDosya, hedefDosya;
            anaDosya = Path.Combine(cmbKaynakKurum.Text, txtDosyaAdi.Text);
            hedefDosya = Path.Combine(cmbHedefKurum.Text, txtDosyaAdi.Text);
            if (anaDosya != hedefDosya
                && File.Exists(anaDosya)
                && File.Exists(hedefDosya) == false)
            {
                File.Copy(anaDosya, hedefDosya, false);
            }
            SonrakiHedef();
        }
    }
}
