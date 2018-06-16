using Google.Cloud.Translation.V2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PI3_UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Pegar numero do algoritmo

            var num = 1;

            // fazer chamada pra API

            var client = new HttpClient();
            var response = client.GetAsync($"http://fortunecookieapi.herokuapp.com/v1/cookie?limit={num}").Result;
            var result = JsonConvert.DeserializeObject<List<Models.Cookie>>(response.Content.ReadAsStringAsync().Result);

            //TranslationClient clientT = TranslationClient.Create();
            //var responseT = clientT.TranslateText(result.Last().fortune.message, "br", "us");
            
            // Printar resultados
            LuckNum.Text = String.Join(" ", result.Last().lotto.nums);
            LuckMessage.Text = result.Last().fortune.message;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Selecione a imagem do numero";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                NumImagem.ImageLocation = openFileDialog1.FileName;
            }

        }
    }
}
