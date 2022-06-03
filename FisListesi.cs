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
    public partial class FisListesi : Form
    {
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

        public void FisGetir()
        {
            baglanti = new SqlConnection($"server={sqlServerAdi};Initial Catalog={database};Persist Security Info=False; User ID={kullaniciAdi}; Password={sifre}");
            baglanti.Open();
            da = new SqlDataAdapter("select * from FaturaUst left join musteri on FaturaUst.musteri_kodu = musteri.musteri_kodu", baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }

        private void FisListesi_Load(object sender, EventArgs e)
        {
            registryDegerOkuma();
            FisGetir();
        }

        private void btn_Sec_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        public FisListesi()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btn_Kapat_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
