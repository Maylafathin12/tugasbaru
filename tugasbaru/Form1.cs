using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace tugasbaru
{
    public partial class Form1 : Form
    {
        private string StringConnection = "data source=MAYLA;" + "database=tugasbaru;User ID=sa;Password=123";
        private SqlConnection koneksi;

        public Form1()
        {
            InitializeComponent();
        }

        private void dataProdiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormProdi fa = new FormProdi();
            fa.Show();
            this.Hide();
        }

        private void dataMahasisswaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormMahasiswa fo = new FormMahasiswa();
            fo.Show();
            this.Hide();
        }

        private void dataStatusMahasiswaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormStatusMahasiswa fr = new FormStatusMahasiswa();
            fr.Show();
            this.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
