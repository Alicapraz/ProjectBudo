using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Registryislem
{
    public partial class BaglantiAyarlari : Form
    {
        public BaglantiAyarlari()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Registry.CurrentUser.CreateSubKey("BUDO").SetValue("sqlServerAdi", textBox3.Text);
            Registry.CurrentUser.CreateSubKey("BUDO").SetValue("ID", textBox1.Text);
            Registry.CurrentUser.CreateSubKey("BUDO").SetValue("Şifre", txtSifre.Text);
            Registry.CurrentUser.CreateSubKey("BUDO").SetValue("Database", textBox4.Text);

            MessageBox.Show("Kayıt Başarılı");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MusteriGiris form2 = new MusteriGiris();
            form2.Show();
            this.Hide();
        }

      
    }
}
