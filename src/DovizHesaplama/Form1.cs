using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Security.Cryptography;

namespace DovizHesaplama
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private Dictionary<string, double> rates;
        private bool success;
        private async Task DovizKuruCek() // 17.11.2023 2 günümü aldı
        {
            double amount = Convert.ToDouble(AmountVeri.Text);

            if (ComboFromVeri.SelectedItem == null) 
                return;

            if (ComboToVeri.SelectedItem == null)
                return;

            string fromCurrency = ComboFromVeri.SelectedItem.ToString();
            string toCurrency = ComboToVeri.SelectedItem.ToString(); // convert kullanmadım

            double fromRate = rates[fromCurrency];
            double toRate = rates[toCurrency]; // burda aslında apiye tekrardan basecurrency göndermek yerine
                                               // onun yerine iki kur arasındaki farkı karşılaştırıyor.

            double result = (amount / fromRate) * toRate;

            MessageBox.Show(result.ToString("N2"));
        }


        private async void Form1_Load(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                string apiurl = ""; // api konusunda https://exchangeratesapi.io kullandım.
                                    // ayrıca eğer isterseniz https://www.exchangerate-api.com kullanabilirsiniz yanlız rates değişkenini orası conversion_rates olarak almakta.
                                    // birde exchangerate-api sitesindeki success tam tersi yani result değerinin string değerini aratıp onu success ile eşitleyin.
                                    // if you want the use program, visit this site https://exchangeratesapi.io and get a api.
                string geridonus = await client.GetStringAsync(apiurl);
                var data = JsonConvert.DeserializeObject<dynamic>(geridonus);
                if (bool.TryParse(data.success.ToString(), out success))
                {
                    rates = JsonConvert.DeserializeObject<Dictionary<string, double>>(data.rates.ToString());
                    ComboFromVeri.Items.AddRange(rates.Keys.ToArray());
                    ComboToVeri.Items.AddRange(rates.Keys.ToArray());
                }
                else
                {
                    MessageBox.Show("Veriye ulaşılamıyor.");
                }
            }
        }

        private void ComboFromVeri_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ComboToVeri_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void AmountVeri_TextChanged(object sender, EventArgs e)
        {

        }

        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            await DovizKuruCek(); // apinin yavaş olması halinde async ve await koydum
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2CircleButton2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
