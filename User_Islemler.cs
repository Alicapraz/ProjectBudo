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
    public partial class User_Islemler : Form
    {
        SqlCommand komut;
        SqlConnection baglanti;
        SqlDataAdapter da;
        string sqlServerAdi, sifre, kullaniciAdi, database;

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

        public void registryDegerOkuma()
        {
            sqlServerAdi = Registry.CurrentUser.OpenSubKey("BUDO").GetValue("sqlServerAdi").ToString();
            kullaniciAdi = Registry.CurrentUser.OpenSubKey("BUDO").GetValue("ID").ToString();
            sifre = Registry.CurrentUser.OpenSubKey("BUDO").GetValue("Şifre").ToString();
            database = Registry.CurrentUser.OpenSubKey("BUDO").GetValue("Database").ToString();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            main_menu formMenu = new main_menu();
            formMenu.Show();
            this.Hide();
        }

        private void User_Islemler_Load(object sender, EventArgs e)
        {
            registryDegerOkuma();
            musteriGetir();
        }

        public User_Islemler()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string sorgu = "delete from musteri where ID=@ID";
            komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@ID", Convert.ToInt32(txtID.Text));
            baglanti.Open();
            komut.ExecuteNonQuery();
            baglanti.Close();
            musteriGetir();
            MessageBox.Show("Kayıt Silindi");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string sorgu = "update musteri set ID=@ID,musteri_kodu=@musteri_kodu, musteri_unvan=@musteri_unvan, vkn=@vkn, vergi_dairesi=@vergi_dairesi where ID=@ID";
            komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@ID", Convert.ToInt32(txtID.Text));
            komut.Parameters.AddWithValue("@musteri_kodu", textBox1.Text);
            komut.Parameters.AddWithValue("@musteri_unvan", textBox2.Text);
            komut.Parameters.AddWithValue("@vkn", textBox3.Text);
            komut.Parameters.AddWithValue("@vergi_dairesi", textBox4.Text);

            baglanti.Open();
            komut.ExecuteNonQuery();
            baglanti.Close();
            musteriGetir();
            MessageBox.Show("Kayıt Güncellendi");

        }

        private void dataGridView1_CellEnter_1(object sender, DataGridViewCellEventArgs e)
        {
            txtID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();

        }

        private void label1_Click(object sender, EventArgs e)
        {
            txtID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {
                  
            string sorgu = "insert into musteri(ID, musteri_kodu, musteri_unvan, vkn, vergi_dairesi) values(@ID,@musteri_kodu,@musteri_unvan,@vkn,@vergi_dairesi)";
            komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@ID", txtID.Text);
            komut.Parameters.AddWithValue("@musteri_kodu", textBox1.Text);
            komut.Parameters.AddWithValue("@musteri_unvan", textBox2.Text);
            komut.Parameters.AddWithValue("@vkn", textBox3.Text);
            komut.Parameters.AddWithValue("@vergi_dairesi", textBox4.Text);
            baglanti.Open();
            komut.ExecuteNonQuery();
            baglanti.Close();
            musteriGetir();
            MessageBox.Show("Kayıt Olundu");
        }


    }
}
