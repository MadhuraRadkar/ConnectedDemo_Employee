using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;

namespace ConnectedDemo
{
    public partial class Form2 : Form
    {
        SqlConnection con;
        SqlDataAdapter da;
        SqlCommandBuilder builder;
        DataSet ds;
        public Form2()
        {
            InitializeComponent();
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                string qry = "select * from departments";
                da = new SqlDataAdapter(qry, con);
                ds = new DataSet();
                da.Fill(ds, "Dept");
                combDepartment.DataSource = ds.Tables["Dept"];
                combDepartment.DisplayMember = "dname";
                combDepartment.ValueMember = "did";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private DataSet GetEmployees()
        {
            string qry = "select * from Employeee";
            // assign the query
            da = new SqlDataAdapter(qry, con);
            // when app load the in DataSet, we need to manage the PK also
            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            // SCB will track the DataSet & update quries to the DataAdapter
            builder = new SqlCommandBuilder(da);
            ds = new DataSet();
            da.Fill(ds, "Employee");// this name given to the DataSet table
            return ds;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ds = GetEmployees();
                // create new row to add recrod
                DataRow row = ds.Tables["Employee"].NewRow();
                // assign value to the row
                row["ename"] = txtName.Text;
                row["email"] = txtEmail.Text;
                row["age"] = txtAge.Text;
                row["salary"] = txtSalary.Text;
                row["did"] = combDepartment.SelectedValue;
                // attach this row in DataSet table
                ds.Tables["Employee"].Rows.Add(row);
                // update the changes from DataSet to DB
                int result = da.Update(ds.Tables["Employee"]);
                if (result >= 1)
                {
                    MessageBox.Show("Record inserted");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                ds = GetEmployees();
                // find the row
                DataRow row = ds.Tables["Employee"].Rows.Find(txtId.Text);
                if (row != null)
                {
                    row["ename"] = txtName.Text;
                    row["email"] = txtEmail.Text;
                    row["age"] = txtAge.Text;
                    row["salary"] = txtSalary.Text;
                    row["did"] = combDepartment.SelectedValue;
                    // update the changes from DataSet to DB
                    int result = da.Update(ds.Tables["Employee"]);
                    if (result >= 1)
                    {
                        MessageBox.Show("Record updated");
                    }
                }
                else
                {
                    MessageBox.Show("Id not matched");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ds = GetEmployees();
                // find the row
                DataRow row = ds.Tables["Employee"].Rows.Find(txtId.Text);
                if (row != null)
                {
                    // delete the current row from DataSet table
                    row.Delete();
                    // update the changes from DataSet to DB
                    int result = da.Update(ds.Tables["Employee"]);
                    if (result >= 1)
                    {
                        MessageBox.Show("Record deleted");
                    }
                }
                else
                {
                    MessageBox.Show("Id not matched");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnShowall_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "select e.*,d.dname from employeee e inner join departments d on d.did = e.did";
                da = new SqlDataAdapter(qry, con);
                da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                ds = new DataSet();
                da.Fill(ds, "emp");
                dataGridView1.DataSource = ds.Tables["emp"];
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "select e.*, d.dname from Employeee e inner join departments d on d.did = e.did ";
                da = new SqlDataAdapter(qry, con);
                da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                ds = new DataSet();
                da.Fill(ds, "emp");
                //find method can only seach the data if PK is applied in the DataSet table
                DataRow row = ds.Tables["emp"].Rows.Find(txtId.Text);
                if (row != null)
                {
                    txtName.Text = row["ename"].ToString();
                    txtEmail.Text = row["email"].ToString();
                    txtAge.Text = row["age"].ToString();
                    txtSalary.Text = row["salary"].ToString();
                    combDepartment.Text = row["dname"].ToString();
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

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           txtId.Text = dataGridView1.CurrentRow.Cells["id"].Value.ToString();
            txtName.Text = dataGridView1.CurrentRow.Cells["ename"].Value.ToString();
            txtAge.Text = dataGridView1.CurrentRow.Cells["age"].Value.ToString();
            txtEmail.Text = dataGridView1.CurrentRow.Cells["email"].Value.ToString();
            txtSalary.Text = dataGridView1.CurrentRow.Cells["salary"].Value.ToString();
            combDepartment.Text = dataGridView1.CurrentRow.Cells["dname"].Value.ToString() ;
        }
    }
}


