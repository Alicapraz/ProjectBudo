using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Win32;

namespace Registryislem
{
    public partial class MusteriListesi : Form
    {
        SqlCommand komut;
        SqlConnection baglanti;
        SqlDataAdapter da;
        string sqlServerAdi, sifre, kullaniciAdi, database;

        public void registryDegerOkuma()
        {
            sqlServerAdi = Registry.CurrentUser.OpenSubKey("BUDO").GetValue("sqlServerAdi").ToString();
            kullaniciAdi = Registry.CurrentUser.OpenSubKey("BUDO").GetValue("ID").ToString();
            sifre = Registry.CurrentUser.OpenSubKey("BUDO").GetValue("Şifre").ToString();
            database = Registry.CurrentUser.OpenSubKey("BUDO").GetValue("Database").ToString();
        }

        private void btn_Kapat_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {

        }

        private void btn_Sec_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;

        }

        public void musteriGetir()
        {
            baglanti = new SqlConnection($"server={sqlServerAdi};Initial Catalog={database};Persist Security Info=False; User ID={kullaniciAdi}; Password={sifre}");
            baglanti.Open();
            da = new SqlDataAdapter("Select * from musteri", baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }

        private void MusteriListesi_Load(object sender, EventArgs e)
        {
            registryDegerOkuma();
            musteriGetir();

        }

        public MusteriListesi()
        {
            InitializeComponent();
        }


    }
}
