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
    public partial class FaturaKaydi : Form
    {
        SqlCommand komut;
        SqlConnection baglanti;
        SqlDataAdapter da;
        string sqlServerAdi, sifre, kullaniciAdi, database;


        List<StokListe> stokListesi_ = new List<StokListe>();




        public void registryDegerOkuma()
        {
            sqlServerAdi = Registry.CurrentUser.OpenSubKey("BUDO").GetValue("sqlServerAdi").ToString();
            kullaniciAdi = Registry.CurrentUser.OpenSubKey("BUDO").GetValue("ID").ToString();
            sifre = Registry.CurrentUser.OpenSubKey("BUDO").GetValue("Şifre").ToString();
            database = Registry.CurrentUser.OpenSubKey("BUDO").GetValue("Database").ToString();
        }

        public void fatura()
        {
            baglanti = new SqlConnection($"server={sqlServerAdi};Initial Catalog={database};Persist Security Info=False; User ID={kullaniciAdi}; Password={sifre}");
            baglanti.Open();
            da = new SqlDataAdapter("Select * from fatura_kalem", baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
           // dataGridView2.DataSource = tablo;
            baglanti.Close();
        }

        private void dataGridView2_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

        }

        private void dataGridView2_Enter(object sender, EventArgs e)
        {
        
        }

        private void dataGridView2_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void btn_stok_Click(object sender, EventArgs e)
        {
            using (ItemListesi frehberItem = new ItemListesi())
            {
                if (frehberItem.ShowDialog() == DialogResult.OK)
                {
                    txt_stokKodu.Text = frehberItem.dataGridView1.CurrentRow.Cells["stok_kodu"].Value.ToString();
                    txt_stokAdı.Text = frehberItem.dataGridView1.CurrentRow.Cells["stok_adi"].Value.ToString();
                    txt_olcuBirimi.Text = frehberItem.dataGridView1.CurrentRow.Cells["olcu_birimi"].Value.ToString();
                    txt_kdvOrani.Text = frehberItem.dataGridView1.CurrentRow.Cells["kdv_orani"].Value.ToString();

                }

            }
        }

        private void btn_FisKaydet_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView2.RowCount-1; i++)
            {
                string stok_kodu;
                float fiyat, miktar;
                //DATAGRİD ÜZERİNDEKİ SÜTUNLARIN HÜCRELERİNDEN DEĞERLERİ SATIR SATIR OKUYORUM.
                stok_kodu = dataGridView2.Rows[i].Cells["stok_kodu"].Value.ToString();
                miktar = (float)Convert.ToDouble(dataGridView2.Rows[i].Cells["miktar"].Value);
                fiyat = (float)Convert.ToDouble(dataGridView2.Rows[i].Cells["fiyat"].Value);
                //BİTTİ

         
                //SQL TABLOSUNA YAZMA İŞLEMİ YAPIYORUM.
                baglanti = new SqlConnection($"server={sqlServerAdi};Initial Catalog={database};Persist Security Info=False; User ID={kullaniciAdi}; Password={sifre}");
                baglanti.Open();
                string sorgu = "INSERT INTO FaturaKalem(fatura_no,stok_kodu,miktar,fiyat , satir_numarasi) VALUES(@fatura_no, @stok_kodu, @miktar, @fiyat, @satir_numarasi)";
                komut = new SqlCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@stok_kodu", stok_kodu);
                komut.Parameters.AddWithValue("@miktar", miktar);
                komut.Parameters.AddWithValue("@fiyat", fiyat);
                komut.Parameters.AddWithValue("@fatura_no", txt_FisNo.Text);
                komut.Parameters.AddWithValue("@satir_numarasi", i+1);



                komut.ExecuteNonQuery(); // SQL SORGUSUNU ÇALIŞTIRAN SATIR


            }

            baglanti = new SqlConnection($"server={sqlServerAdi};Initial Catalog={database};Persist Security Info=False; User ID={kullaniciAdi}; Password={sifre}");
            baglanti.Open();
            string sorgu2 = "INSERT INTO FaturaUst(fatura_no, tarih, musteri_kodu) VALUES(@fatura_no, @tarih, @musteri_kodu)";
            komut = new SqlCommand(sorgu2, baglanti);
            komut.Parameters.AddWithValue("@fatura_no", txt_FisNo.Text);
            komut.Parameters.AddWithValue("@tarih", DateTime.Parse(dateTimePicker1.Text));
            komut.Parameters.AddWithValue("@musteri_kodu", txt_MusteriKodu.Text);
            komut.ExecuteNonQuery();

            MessageBox.Show("Fiş Kaydedildi");
            Temizle();
        }

        public void Temizle()
        {
            txt_MusteriKodu.Text = "";
            txt_MusteriAdi.Text = "";
            txt_VKN.Text = "";
            txt_vergiDaire.Text = "";
            //txt_FisNo.Text = "";
            dataGridView2.Rows.Clear();

            stokListesi_.Clear();

            baglanti = new SqlConnection($"server={sqlServerAdi};Initial Catalog={database};Persist Security Info=False; User ID={kullaniciAdi}; Password={sifre}");
            baglanti.Open();

            SqlCommand SqlSorgum = new SqlCommand("SELECT TOP (1)  [fatura_no]+1 as fatura_son_no FROM [test].[dbo].[FaturaUst] order by convert (int, fatura_no) desc", baglanti);
            SqlDataReader dr = SqlSorgum.ExecuteReader();
            if (dr.Read())
            {
                txt_FisNo.Text = dr["fatura_son_no"].ToString();
            }


        }

        public class StokListe
        {
            public string stok_kodu { get; set; }
            public string fiyat { get; set; }
            public string miktar { get; set; }


        }


        private void btn_StokEkle_Click(object sender, EventArgs e)
        {



            string stok_kodu = txt_stokKodu.Text;
            string fiyat = txt_fiyat.Text;
            string miktar = txt_miktar.Text;



            dataGridView2.Rows.Add(stok_kodu, fiyat, miktar);


            txt_stokKodu.Text = "";
            txt_stokAdı.Text = "";
            txt_olcuBirimi.Text = "";
            txt_kdvOrani.Text = "";
            txt_miktar.Text = "";
            txt_fiyat.Text = "";

        }

        private void btn_Fis_Click(object sender, EventArgs e)
        {
            using (FisListesi frehberFis = new FisListesi())
            {
                if (frehberFis.ShowDialog() == DialogResult.OK)
                {
                    txt_FisNo.Text = frehberFis.dataGridView1.CurrentRow.Cells["fatura_no"].Value.ToString();
                    dateTimePicker1.Text = frehberFis.dataGridView1.CurrentRow.Cells["tarih"].Value.ToString();
                    txt_MusteriKodu.Text = frehberFis.dataGridView1.CurrentRow.Cells["musteri_kodu"].Value.ToString();
                    txt_MusteriAdi.Text = frehberFis.dataGridView1.CurrentRow.Cells["musteri_unvan"].Value.ToString();
                    txt_VKN.Text = frehberFis.dataGridView1.CurrentRow.Cells["vkn"].Value.ToString();
                    txt_vergiDaire.Text = frehberFis.dataGridView1.CurrentRow.Cells["vergi_dairesi"].Value.ToString();

                }

            }
            //baglanti = new SqlConnection($"server={sqlServerAdi};Initial Catalog={database};Persist Security Info=False; User ID={kullaniciAdi}; Password={sifre}");
            //baglanti.Open();
            //da = new SqlDataAdapter($"select miktar, fiyat, stok_kodu from FaturaKalem where fatura_no = '{txt_FisNo.Text}' ", baglanti);
            //DataTable tablo = new DataTable();
            //da.Fill(tablo);
            //dataGridView2.DataSource = tablo;        
            //baglanti.Close();






            SqlConnection con = new SqlConnection($"server={sqlServerAdi};Initial Catalog={database};Persist Security Info=False; User ID={kullaniciAdi}; Password={sifre}");
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = $"select miktar, fiyat, stok_kodu from FaturaKalem where fatura_no = '{txt_FisNo.Text}'";
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();
             
            while (dr.Read())
            {
            dataGridView2.Rows.Add(dr["stok_kodu"], dr["fiyat"], dr["miktar"]);

            }

            dr.Close();
            con.Close();

        }

        private void txt_FisNo_Leave(object sender, EventArgs e)
        {

            if (txt_FisNo.Text != "")
            {
                baglanti = new SqlConnection($"server={sqlServerAdi};Initial Catalog={database};Persist Security Info=False; User ID={kullaniciAdi}; Password={sifre}");
                baglanti.Open();


                SqlCommand SqlSorgum = new SqlCommand($"select top 1 * from FaturaUst left join musteri on FaturaUst.musteri_kodu = musteri.musteri_kodu where fatura_no = '{txt_FisNo.Text}' ", baglanti);
                SqlDataReader dr = SqlSorgum.ExecuteReader();

                if (dr.Read())
                {

                    txt_FisNo.Text = dr["fatura_no"].ToString();
                    dateTimePicker1.Text = dr["tarih"].ToString();
                    txt_MusteriKodu.Text = dr["musteri_kodu"].ToString();
                    txt_MusteriAdi.Text = dr["musteri_unvan"].ToString();
                    txt_VKN.Text = dr["vkn"].ToString();
                    txt_vergiDaire.Text = dr["vergi_dairesi"].ToString();

                }
                dr.Close();


                SqlConnection con = new SqlConnection($"server={sqlServerAdi};Initial Catalog={database};Persist Security Info=False; User ID={kullaniciAdi}; Password={sifre}");
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"select miktar, fiyat, stok_kodu from FaturaKalem where fatura_no = '{txt_FisNo.Text}'";
                con.Open();

                SqlDataReader drSatir = cmd.ExecuteReader();
                using (con)
                {
                    if (drSatir.HasRows)
                    {
                        while (dr.Read())
                        {
                            dataGridView2.Rows.Add(dr["stok_kodu"], dr["fiyat"], dr["miktar"]);

                        }
                    }


                    dr.Close();
                    con.Close();
                }
              




                baglanti.Close();


                if (dataGridView2.Rows.Count == 0)
                {
                    //dataGridView2.DataSource = "";
                }
                
            }
            else
            {
                MessageBox.Show("textbox boş geçilemez.");
            }
          
        }

        private void btn_Temizle_Click(object sender, EventArgs e)
        {
            Temizle();
        }

        private void btn_mainmenu_Click(object sender, EventArgs e)
        {
            main_menu formMenu = new main_menu();
            formMenu.Show();
            this.Hide();
        }

        public FaturaKaydi()
        {
            InitializeComponent();
        }

        private void btn_Musteri_Click(object sender, EventArgs e)
        {


            using (MusteriListesi frehberMusteri = new MusteriListesi())
            {
                if (frehberMusteri.ShowDialog() == DialogResult.OK)
                {
                    txt_MusteriKodu.Text = frehberMusteri.dataGridView1.CurrentRow.Cells["musteri_kodu"].Value.ToString();
                    txt_MusteriAdi.Text = frehberMusteri.dataGridView1.CurrentRow.Cells["musteri_unvan"].Value.ToString();
                    txt_VKN.Text = frehberMusteri.dataGridView1.CurrentRow.Cells["vkn"].Value.ToString();
                    txt_vergiDaire.Text = frehberMusteri.dataGridView1.CurrentRow.Cells["vergi_dairesi"].Value.ToString();

                }

            }
        }
        private void FaturaKaydi_Load(object sender, EventArgs e)
        {
           
            registryDegerOkuma();
            //fatura();

            baglanti = new SqlConnection($"server={sqlServerAdi};Initial Catalog={database};Persist Security Info=False; User ID={kullaniciAdi}; Password={sifre}");
            baglanti.Open();

            SqlCommand SqlSorgum = new SqlCommand("SELECT TOP (1)  [fatura_no]+1 as fatura_son_no FROM [test].[dbo].[FaturaUst] order by convert (int, fatura_no) desc", baglanti);
            SqlDataReader dr = SqlSorgum.ExecuteReader();
            if (dr.Read())
            {
                txt_FisNo.Text = dr["fatura_son_no"].ToString();
            }

        }
    }
}
