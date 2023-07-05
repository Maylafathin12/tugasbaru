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
    public partial class FormMahasiswa : Form
    {
        private string StringConnection = "data source=MAYLA;" + "database=tugasbaru;User ID=sa;Password=123";
        private SqlConnection koneksi;
        private string kstr;
        private string nim, nama, alamat, jk, prodi;
        private DateTime tgl;
        BindingSource customersBindingSource = new BindingSource();

        private void FormMahasiswa_Load()
        {
            koneksi.Open();
            SqlDataAdapter dataAdapter1 = new SqlDataAdapter(new SqlCommand("select m.nim, m.nama_mahasiswa,"
                + "m.alamat, m.jenis_kelamin, m.tgl_lahir, p.nama_prodi From dbo.Mahasisswa m "
                + "join dbo.Prodi p on m.id_prodi = p.id_prodi", koneksi));
            DataSet ds = new DataSet();
            dataAdapter1.Fill(ds);

            this.customersBindingSource.DataSource = ds.Tables[0];
            this.textBox1.DataBindings.Add(
                new Binding("Text", this.customersBindingSource, "nim", true));
            this.textBox2.DataBindings.Add(
                new Binding("Text", this.customersBindingSource, "nama_mahasiswa", true));
            this.textBox3.DataBindings.Add(
                new Binding("Text", this.customersBindingSource, "alamat", true));
            this.comboBox1.DataBindings.Add(
                new Binding("Text", this.customersBindingSource, "jenis_kelamin", true));
            this.dateTimePicker1.DataBindings.Add(
                new Binding("Text", this.customersBindingSource, "tgl_lahir", true));
            this.comboBox2.DataBindings.Add(
                new Binding("Text", this.customersBindingSource, "nama_prodi", true));
            koneksi.Close();
        }

        private void Prodicbx()
        {
            koneksi.Open();
            string str = "select nama_prodi from dbo.Prodi";
            SqlCommand cmd = new SqlCommand(str, koneksi);
            SqlDataAdapter da = new SqlDataAdapter(str, koneksi);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cmd.ExecuteReader();
            koneksi.Close();
            comboBox2.DisplayMember = "nama_prodi";
            comboBox2.ValueMember = "id_prodi";
            comboBox2.DataSource = ds.Tables[0];
        }

        private void Add_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            dateTimePicker1.Value = DateTime.Today;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            comboBox1.Enabled = true;
            textBox3.Enabled = true;
            dateTimePicker1.Enabled = true;
            comboBox2.Enabled = true;
            Prodicbx();
            Save.Enabled = true;
            Clear.Enabled = true;
            Add.Enabled = true;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            nim = textBox1.Text;
            nama = textBox2.Text;
            jk = comboBox1.Text;
            alamat = textBox3.Text;
            tgl = dateTimePicker1.Value;
            prodi = comboBox2.Text;
            int hs = 0;
            koneksi.Open();
            string strs = "select id_prodi from dbp.Prodi where nama_prodi = @dd";
            SqlCommand cm = new SqlCommand(strs, koneksi);
            cm.CommandType = CommandType.Text;
            cm.Parameters.Add(new SqlParameter("@add", prodi));
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                hs = int.Parse(dr["id_prodi"].ToString());
            }
            dr.Close();
            string str = "nsert into dbo.Maahasisswa (nim, nama_mahasiswa, jenis_kelamin, alamat, tgl_lahir, id_prodi)" + "values(@NIM, @Nm," +
                "@jk, @Al, @Tgll, @Idp)";
            SqlCommand cmd = new SqlCommand(str, koneksi);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("NIM", nim));
            cmd.Parameters.Add(new SqlParameter("Nm", nama));
            cmd.Parameters.Add(new SqlParameter("Jk", jk));
            cmd.Parameters.Add(new SqlParameter("Al", alamat));
            cmd.Parameters.Add(new SqlParameter("Tgll", tgl));
            cmd.Parameters.Add(new SqlParameter("Idp", hs));
            cmd.ExecuteNonQuery();

            koneksi.Close();

            MessageBox.Show("Data Berhasil DIsimpan", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

            refreshform();

        }

        private void Clear_Click(object sender, EventArgs e)
        {
            refreshform();
        }


        public FormMahasiswa()
        {
            InitializeComponent();
            koneksi = new SqlConnection(kstr);
            this.bindingNavigator1.BindingSource = this.customersBindingSource;
            refreshform();
        }

        private void clearBinding()
        {
            this.textBox1.DataBindings.Clear();
            this.textBox2.DataBindings.Clear();
            this.textBox3.DataBindings.Clear();
            this.comboBox1.DataBindings.Clear();
            this.dateTimePicker1.DataBindings.Clear();
            this.comboBox2.DataBindings.Clear();
        }

        private void refreshform()
        {
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            comboBox1.Enabled = false;
            textBox3.Enabled = false;
            dateTimePicker1.Enabled = false;
            comboBox2.Enabled = false;
            Add.Enabled = false;
            Save.Enabled = false;
            Clear.Enabled = false;
            clearBinding();
            FormMahasiswa_Load();
        }
    }
}
