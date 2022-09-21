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
        string connectionString = "server=localhost;userid=Dev;password=lang6996A@;database=phonebookdb";
        int PhoneBookID = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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
                        MySqlCommand mySqlCommand = new MySqlCommand("ContactAddOrEdit", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("@PhoneBookID", PhoneBookID);
                        mySqlCommand.Parameters.AddWithValue("@FirstName", txtBoxName.Text.Trim());
                        mySqlCommand.Parameters.AddWithValue("@LastName", txtBoxLast.Text.Trim());
                        mySqlCommand.Parameters.AddWithValue("@Contact", txtBoxContact.Text.Trim());
                        mySqlCommand.Parameters.AddWithValue("@Email", txtBoxEmail.Text.Trim());
                        mySqlCommand.Parameters.AddWithValue("@Adress", txtBoxAdress.Text.Trim());
                        mySqlCommand.ExecuteNonQuery();
                        MessageBox.Show("Dados enviados com sucesso");
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
        }
    }
