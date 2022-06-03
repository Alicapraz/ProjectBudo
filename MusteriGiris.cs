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
using System.Data.SqlClient;


namespace Registryislem
{
    public partial class MusteriGiris : Form
    {
        string sqlServerAdi, kullaniciAdi, sifre, loginKullaniciAdi, database;
        int loginSifre;

        SqlConnection baglanti;
        SqlCommand komut;

        public void registryDegerOkuma()
        {
            sqlServerAdi = Registry.CurrentUser.OpenSubKey("BUDO").GetValue("sqlServerAdi").ToString();
            kullaniciAdi = Registry.CurrentUser.OpenSubKey("BUDO").GetValue("ID").ToString();
            sifre = Registry.CurrentUser.OpenSubKey("BUDO").GetValue("Şifre").ToString();
            database= Registry.CurrentUser.OpenSubKey("BUDO").GetValue("Database").ToString();
        }

        private void btnGiris_Click(object sender, EventArgs e)
        {
            baglanti = new SqlConnection($"server={sqlServerAdi};Initial Catalog={database};Persist Security Info=False; User ID={kullaniciAdi}; Password={sifre}");
            
            try
            {
                loginKullaniciAdi = txtID.Text.Trim();
                loginSifre = Convert.ToInt32(txtSifre.Text);

                baglanti.Open();
                string sql = $"SELECT * FROM users WHERE loginName='{loginKullaniciAdi}' AND password='{loginSifre}'"; 
                komut = new SqlCommand(sql, baglanti); 
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(komut);
                da.Fill(dt);

                if(dt.Rows.Count > 0)
                {
                    main_menu formMenu = new main_menu();
                    formMenu.Show();
                    this.Hide();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Hatalı Giriş :" + ex.Message);
            }
        
           
        }


        private void button1_Click(object sender, EventArgs e)
        {
            BaglantiAyarlari form1 = new BaglantiAyarlari();
            form1.Show(); 
            this.Hide();
        }

        private void txtID_TextChanged(object sender, EventArgs e)
        {

        }

        public MusteriGiris()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            registryDegerOkuma();
        }
    }
}
