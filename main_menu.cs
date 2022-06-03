using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Registryislem
{
    public partial class main_menu : Form
    {
        public main_menu()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            User_Islemler formMusteriKayıt = new User_Islemler();
            formMusteriKayıt.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            item_kayit formMalzeme = new item_kayit();
            formMalzeme.Show();
            this.Hide();
        }

        private void main_menu_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            FaturaKaydi formFatura = new FaturaKaydi();
            formFatura.Show();
            this.Hide();
        }
    }
}
