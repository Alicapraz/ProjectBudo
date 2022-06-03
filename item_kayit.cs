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
    public partial class item_kayit : Form
    {
        SqlCommand komut;
        SqlConnection baglanti;
        SqlDataAdapter da;
        string sqlServerAdi, sifre, kullaniciAdi, database;

        void itemGetir()
        {
            baglanti = new SqlConnection($"server={sqlServerAdi};Initial Catalog={database};Persist Security Info=False; User ID={kullaniciAdi}; Password={sifre}");
            baglanti.Open();
            da = new SqlDataAdapter("Select * from items", baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            main_menu formMenu = new main_menu();
            formMenu.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti = new SqlConnection($"server={sqlServerAdi};Initial Catalog={database};Persist Security Info=False; User ID={kullaniciAdi}; Password={sifre}");
            baglanti.Open();
            string sorgu = "insert into items(stok_kodu, stok_adi, olcu_birimi, kdv_orani) values(@stok_kodu,@stok_adi,@olcu_birimi,@kdv_orani)";
            komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@stok_kodu", textBox1.Text);
            komut.Parameters.AddWithValue("@stok_adi", textBox2.Text);
            komut.Parameters.AddWithValue("@olcu_birimi", textBox3.Text);
            komut.Parameters.AddWithValue("@kdv_orani", textBox4.Text);

            komut.ExecuteNonQuery();
            baglanti.Close();
            itemGetir();
            MessageBox.Show("Kayıt Olundu");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string sorgu = "delete from items where stok_kodu=@stok_kodu";
            komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@stok_kodu", Convert.ToInt32(textBox1.Text));
            baglanti.Open();
            komut.ExecuteNonQuery();
            baglanti.Close();
            itemGetir();
            MessageBox.Show("Kayıt Silindi");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string sorgu = "update items set stok_kodu=@stok_kodu, stok_adi=@stok_adi, olcu_birimi=@olcu_birimi, kdv_orani=@kdv_orani where stok_kodu=@stok_kodu";
            komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@stok_kodu", Convert.ToInt32(textBox1.Text));
            komut.Parameters.AddWithValue("@stok_adi", textBox2.Text);
            komut.Parameters.AddWithValue("@olcu_birimi", textBox3.Text);
            komut.Parameters.AddWithValue("@kdv_orani", textBox4.Text);

            baglanti.Open();
            komut.ExecuteNonQuery();
            baglanti.Close();
            itemGetir();
            MessageBox.Show("Kayıt Güncellendi");
        }

        public void registryDegerOkuma()
        {
            sqlServerAdi = Registry.CurrentUser.OpenSubKey("BUDO").GetValue("sqlServerAdi").ToString();
            kullaniciAdi = Registry.CurrentUser.OpenSubKey("BUDO").GetValue("ID").ToString();
            sifre = Registry.CurrentUser.OpenSubKey("BUDO").GetValue("Şifre").ToString();
            database = Registry.CurrentUser.OpenSubKey("BUDO").GetValue("Database").ToString();
        }

        public item_kayit()
        {
            InitializeComponent();
        }

        private void item_kayit_Load(object sender, EventArgs e)
        {
            registryDegerOkuma();
            itemGetir();
        }
    }
}
