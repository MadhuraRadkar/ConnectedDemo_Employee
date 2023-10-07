using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;

namespace ConnectedDemo
{
    public partial class Form1 : Form
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader reader;
        public Form1()
        {
            InitializeComponent();
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                List<Dept> list = new List<Dept>();
                string qry = "select * from departments";
                cmd = new SqlCommand(qry, con);
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Dept dept = new Dept();
                        dept.Did = Convert.ToInt32(reader["did"]);
                        dept.Dname = reader["dname"].ToString();
                        list.Add(dept);
                    }
                }
                //Display dname & on selection of dname 

                combDepartment.DataSource = list;
                combDepartment.DisplayMember = "Dname";
                combDepartment.ValueMember = "Did";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "insert into Employeee values(@name,@email,@age,@salary,@did)";
                cmd = new SqlCommand(qry, con);
                // assign value to each parameter
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@age", Convert.ToInt32(txtAge.Text));
                cmd.Parameters.AddWithValue("@salary", Convert.ToDouble(txtSalary.Text));
                cmd.Parameters.AddWithValue("@did", Convert.ToInt32(combDepartment.SelectedValue));
                con.Open();
                int result = cmd.ExecuteNonQuery();
                if (result >= 1)
                {
                    MessageBox.Show("Record inserted");
                    ClearField();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
            GetAllEmps();

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "select e.*, d.dname from Employeee e inner join departments d on d.did = e.did where e.id=@id";
                cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtId.Text));
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        txtName.Text = reader["ename"].ToString();
                        txtEmail.Text = reader["email"].ToString();
                        txtAge.Text = reader["age"].ToString();
                        txtSalary.Text = reader["salary"].ToString();
                        combDepartment.Text = reader["dname"].ToString();
                      
                    }
                }
                else
                {
                    MessageBox.Show("Record not found");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
         

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "update Employeee set ename=@name,email=@email,age=@age,salary=@salary,did=@did where id=@id";
                cmd = new SqlCommand(qry, con);
                // assign value to each parameter
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@age", Convert.ToInt32(txtAge.Text));
                cmd.Parameters.AddWithValue("@salary", Convert.ToDouble(txtSalary.Text));
                cmd.Parameters.AddWithValue("@did", Convert.ToInt32(combDepartment.SelectedValue));
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtId.Text));
                con.Open();
                int result = cmd.ExecuteNonQuery();
                if (result >= 1)
                {
                    MessageBox.Show("Record updated");
                    ClearField();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
            GetAllEmps();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "delete from Employeee where id=@id";
                cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtId.Text));
                con.Open();
                int result = cmd.ExecuteNonQuery();
                if (result >= 1)
                {
                    MessageBox.Show("Record deleted");
                    ClearField();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
            GetAllEmps();
        }
        private void GetAllEmps()
        {
            string qry = "select e.*, d.dname from Employeee e inner join departments d on d.did = e.did";
            cmd = new SqlCommand(qry, con);
            con.Open();
            reader = cmd.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            dataGridView1.DataSource = table;
            con.Close();
        }
        public void ClearField()
        {
           txtId.Clear();
           txtName.Clear();
            txtEmail.Clear();
            txtAge.Clear();
            txtSalary.Clear();
            combDepartment.Refresh();
        }

        private void btnShowall_Click(object sender, EventArgs e)
        {
            GetAllEmps();
        }
    }
}

