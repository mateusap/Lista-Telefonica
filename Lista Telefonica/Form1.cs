using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Lista_Telefonica
{
    public partial class Form1 : Form
    {
        string connectionString = "server=localhost;userid=root;password=;database=phonebookdb";
        int PhoneBookID = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GridFill();
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
        }

        private void lblFooter_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtBoxName.Text.Trim() != "" && txtBoxLast.Text.Trim() != "" && txtBoxContact.Text.Trim() != "")
            {
                Regex reg = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = reg.Match(txtBoxEmail.Text.Trim());
                if (match.Success)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                    {
                        mySqlConnection.Open();
                        MySqlCommand comm = mySqlConnection.CreateCommand();
                        comm.CommandText = "INSERT INTO phonebook(PhoneBookID,FirstName,LastName,Contact,Email,Adress) VALUES (@pbID,@first,@last,@contact,@email,@adress)";
                        comm.Parameters.AddWithValue("@pbID", PhoneBookID);
                        comm.Parameters.AddWithValue("@first", txtBoxName.Text);
                        comm.Parameters.AddWithValue("@last", txtBoxLast.Text);
                        comm.Parameters.AddWithValue("@contact", txtBoxContact.Text);
                        comm.Parameters.AddWithValue("@email", txtBoxEmail.Text);
                        comm.Parameters.AddWithValue("@adress", txtBoxAdress.Text);
                        comm.ExecuteNonQuery();
                        mySqlConnection.Close();
                        MessageBox.Show("Dados enviados com sucesso");
                        Clear();
                        GridFill();
                    }
                }
                else
                {
                    MessageBox.Show("Email inválido");
                }
            }
            else
            {
                MessageBox.Show("Preencha os campos obrigatórios");
            }
        }
        void Clear()
        {
            txtBoxAdress.Text = txtBoxContact.Text = txtBoxEmail.Text = txtBoxLast.Text = txtBoxName.Text = txtBoxSearch.Text = "";
            PhoneBookID = 0;
            btnSave.Text = "Salvar";
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
            btnSave.Enabled = true;
        }
        void GridFill()
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
            {
                mySqlConnection.Open();
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter("ContactViewAll", mySqlConnection);
                mySqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dataTable = new DataTable();
                mySqlDataAdapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Index != -1)
            {
                txtBoxName.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                txtBoxLast.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                txtBoxContact.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                txtBoxEmail.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                txtBoxAdress.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                PhoneBookID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                btnSave.Enabled = false;
                btnDelete.Enabled = true;
                btnUpdate.Enabled = true;
            }
            
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
            {
                mySqlConnection.Open();
                MySqlCommand comm = mySqlConnection.CreateCommand();
                comm.CommandText = "UPDATE phonebook SET FirstName =@first, LastName =@last, Contact =@contact, Email = @email, Adress = @adress WHERE PhoneBookID = @pbID";
                comm.Parameters.AddWithValue("@first", txtBoxName.Text);
                comm.Parameters.AddWithValue("@last", txtBoxLast.Text);
                comm.Parameters.AddWithValue("@contact", txtBoxContact.Text);
                comm.Parameters.AddWithValue("@email", txtBoxEmail.Text);
                comm.Parameters.AddWithValue("@adress", txtBoxAdress.Text);
                comm.Parameters.AddWithValue("@pbID", PhoneBookID);
                comm.ExecuteNonQuery();
                mySqlConnection.Close();
                MessageBox.Show("Dados atualizados com sucesso");
                Clear();
                GridFill();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
            {
                mySqlConnection.Open();
                MySqlCommand comm = mySqlConnection.CreateCommand();
                comm.CommandText = "DELETE from phonebook WHERE PhoneBookID = @pbID";
                comm.Parameters.AddWithValue("@pbID", PhoneBookID);
                comm.ExecuteNonQuery();
                mySqlConnection.Close();
                MessageBox.Show("Apagado com sucesso");
                Clear();
                GridFill();
            }
        }

        private void txtBoxSearch_TextChanged(object sender, EventArgs e)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
            {
                mySqlConnection.Open();
                MySqlCommand comm = mySqlConnection.CreateCommand();
                comm.CommandText = "SELECT * from phonebook WHERE FirstName like @search OR LastName like @search";
                comm.Parameters.AddWithValue("@search","%"+txtBoxSearch.Text+"%");
                MySqlDataAdapter mysqdata = new MySqlDataAdapter(comm);
                DataTable datatb = new DataTable();
                mysqdata.Fill(datatb);
                dataGridView1.DataSource = datatb;
            }
        }
    }
}
