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
    public partial class FormStatusMahasiswa : Form
    {
        private string StringConnection = "data source=MAYLA;" + "database=tugasbaru;User ID=sa;Password=123";
        private SqlConnection koneksi;
        private string kstr;
        private string s;
        private string queryString;
        public FormStatusMahasiswa()
        {
            InitializeComponent();
            koneksi = new SqlConnection(kstr);
            refreshform();
        }

        private void FormStatusMahasiswa_FormClosed(object sender, EventArgs e)
        {
            Form1 fm = new Form1();
            fm.Show();
            this.Hide();
        }
        
        private void refreshform()
        {
            NamaMhs.Enabled = false;
            StatusMhs.Enabled = false;
            TahunMhs.Enabled = false;
            NamaMhs.SelectedIndex = -1;
            StatusMhs.SelectedIndex = -1;
            TahunMhs.SelectedIndex = -1;
            NIM.Visible = false;
            Save.Enabled = false;
            Clear.Enabled = false;
            Add.Enabled = false;
        }

        private void dataGridView()
        {
            koneksi.Open();
            string str = "select * form dbo.status_mahasiswa";
            SqlDataAdapter da = new SqlDataAdapter(str, koneksi);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            koneksi.Close();
        }

        private void cbNama()
        {
            koneksi.Open();
            string str = "select nama_mahasiswa from dbo.Mahasisswa where " +
                "not EXISTS(select id_status from dbo.status_mahasiswa where " +
                "status_mahasiswa.nim = mahasisswa.nim";
            SqlCommand cmd = new SqlCommand(str, koneksi);
            SqlDataAdapter da = new SqlDataAdapter(str, koneksi);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cmd.ExecuteReader();
            koneksi.Close();

            NamaMhs.DisplayMember = "nama_mahasiswa";
            NamaMhs.ValueMember = "NIM";
            NamaMhs.DataSource = ds.Tables[0];

        }

        private void cbTahunMasuk()
        {
            int y = DateTime.Now.Year - 2010;
            string[] type = new string[y];
            int i = 0;
            for (i = 0; i < type.Length; i++)
            {
                if (i == 0)
                {
                    TahunMhs.Items.Add("2010");
                }
                else
                {
                    int l = 2010 + i;
                    TahunMhs.Items.Add(l.ToString());
                }
            }
        }

        private void NamaMhs_SelectedIndexChanged(object sender, EventArgs e)
        {
            koneksi.Open();
            string nim = "";
            string str = "select NIM form dbo.mahasisswa where nama_mahasiswa = @nm";
            SqlCommand cm = new SqlCommand(str, koneksi);
            cm.CommandType = CommandType.Text;
            cm.Parameters.Add(new SqlParameter("@nm", NamaMhs.Text));
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                nim = dr["NIM"].ToString();
            }
            dr.Close();
            koneksi.Close();

            NIM.Text = nim;
        }

        private void Add_Click(object sender, EventArgs e)
        {
            TahunMhs.Enabled = true;
            NamaMhs.Enabled = true;
            StatusMhs.Enabled = true;
            NIM.Visible = true;
            cbTahunMasuk();
            cbNama();
            Clear.Enabled = true;
            Save.Enabled = true;
            Add.Enabled = true;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            string nim = NIM.Text;
            string statusMahasiswa = StatusMhs.Text;
            string tahunMasuk = TahunMhs.Text;
            int count = 0;
            string tempKodeStatus = "";
            string KodeStatus = "";
            koneksi.Open();

            string str = "select count (*) from dbo.status_mahasiswa";
            SqlCommand cm = new SqlCommand(str, koneksi);
            count = (int)cm.ExecuteScalar();

            if(count == 0)
            {
                KodeStatus = "1";            
            }
            else
            {
                SqlCommand cmStatusMahasiswaSum = new SqlCommand(s, koneksi);
                int totalStatusMMahasiswa = (int)cmStatusMahasiswaSum.ExecuteScalar();
                int finalKodeStatusInt = totalStatusMMahasiswa + 1;
                KodeStatus = Convert.ToString(finalKodeStatusInt);

            }
            string queryString = "insert into dbo.status_mahasiswa (id_status, nim, " + 
                " status_mahasiswa, tahun_masuk)" + "values(@ids, @NIM, @sm, @tm)";
            SqlCommand cmd = new SqlCommand(queryString, koneksi);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("ids", KodeStatus));
            cmd.Parameters.Add(new SqlParameter("NIM", nim));
            cmd.Parameters.Add(new SqlParameter("sm", statusMahasiswa));
            cmd.Parameters.Add(new SqlParameter("tm", tahunMasuk));
            cmd.ExecuteNonQuery();
            koneksi.Close();

            MessageBox.Show("Data Berhasil Disimpan", "Sukses", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            refreshform();
            dataGridView();
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            refreshform();
        }

        private void Open_Click(object sender, EventArgs e)
        {
            dataGridView();
            Open.Enabled = false;
        }
    }
}
