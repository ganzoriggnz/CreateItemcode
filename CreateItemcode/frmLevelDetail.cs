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
using CreateItemCode.Models;

namespace CreateItemCode
{
    public partial class frmLevelDetail : Form
    {
        clsInfo current;
        clsInfo lastRowData = new clsInfo();


        public frmLevelDetail(clsInfo _i)
        {
            InitializeComponent();
            current = _i;
        }
        private void LoadRefSection()
        {

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                using (var db = new Gobibase())
                {

                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand("select [l_name] from [t_info] group by [l_name] order by l_name", conn);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {

                            string cb = dr.IsDBNull(0) ? "" : Core.ToStr(dr.GetString(0));
                            txtComboL_name.Properties.Items.Add(cb);
                        }
                    }

                    conn.Close();
                    dr.Close();
    
                    Cursor.Current = Cursors.Default;
                }
            }

            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Aлдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LastRowData()
        {
            try
            {
                using (var db = new Gobibase())
                {
                    Cursor.Current = Cursors.WaitCursor;

                    lastRowData = new clsInfo();
                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand("select top 1 angilal,sortid from t_info where l_name = @p1 order by sortid desc", conn);
                    cmd.Parameters.Add("@p1", SqlDbType.NVarChar, 50).Value = frmCreateZagvar._lname;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lastRowData.Angilal = dr.IsDBNull(0) ? "" : Core.ToStr(dr.GetString(0));
                            lastRowData.Sortid = dr.IsDBNull(1) ? 0 : Core.ToInt(dr.GetInt32(1));
                        }
                    }

                    conn.Close();
                    dr.Close();

                    Cursor.Current = Cursors.Default;
                }

            }

            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Шинэчлэх үед алдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmLevelDetail_Load(object sender, EventArgs e)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));

            LoadRefSection();

            if (current != null)
            {

                txtCode.Text = current.Code;
                txtName.Text = current.Name;
                txtNameEn.Text = current.Nameen;
                txtComboL_name.Text = current.L_name;
                txtAngilal.Text = current.Angilal;
                textEdit7.Text = current.Sortid.ToString();
                textEdit8.Text = current.Other1;
                textBox1.Text = current.Other2;

                //txtComboL_name.Enabled = false;
            }
            else
            {
                txtComboL_name.Text = frmCreateZagvar._lname;

                LastRowData();

                if (lastRowData != null)
                {
                    txtAngilal.Text = lastRowData.Angilal;
                    textEdit7.Text = (Core.ToInt(lastRowData.Sortid) + 1).ToString();
                }
            }

            
        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (txtCode.Text == null || txtCode.Text == "")
            {
                MessageBox.Show("Code оруул!", "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCode.SelectAll();
                txtCode.Focus();
                return;
            }


            if (txtComboL_name.Text == null || txtComboL_name.Text == "")
            {
                MessageBox.Show("Түвшин оруул!", "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtComboL_name.SelectAll();
                txtComboL_name.Focus();
                return;
            }

            if (current == null && CheckID(txtCode.Text, txtComboL_name.Text, txtAngilal.Text))
            {
                MessageBox.Show("Code тавхцаж байна. Өөрчлөж оруулна уу!", "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCode.SelectAll();
                txtCode.Focus();
                return;
            }

            if (txtName.Text == null || txtName.Text == "")
            {
                MessageBox.Show("Нэр оруул!", "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.SelectAll();
                txtName.Focus();
                return;
            }
           
            if (txtAngilal.Text == null || txtAngilal.Text == "")
            {
                MessageBox.Show("Ангилал оруул!", "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAngilal.SelectAll();
                txtAngilal.Focus();
                return;
            }
            if (textEdit7.Text == null || textEdit7.Text == "")
            {
                MessageBox.Show("SortID оруул!", "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textEdit7.SelectAll();
                textEdit7.Focus();
                return;
            }
           

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                using (var db = new Gobibase())
                {

                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd;

                    if (current != null)
                    {
                        cmd = new SqlCommand(@"UPDATE [dbo].[t_info] SET [code] = @p2,[name] = @p3,[nameen] = @p4,[l_name] = @p5,[angilal] = @p6 ,
                                                [sortid] = @p7, [other1]= @p8,[other2]=@p9 WHERE id = @p1", conn);
                        cmd.Parameters.Add("@p1", SqlDbType.Int).Value = current.ID;
                    }
                    else
                    {
                        cmd = new SqlCommand(@"INSERT INTO [t_info]([code],[name],[nameen],[l_name],[angilal],
                                                [sortid],[other1],[other2])VALUES (@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9)", conn);
                    }

                    cmd.Parameters.Add("@p2", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtCode.Text);
                    cmd.Parameters.Add("@p3", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtName.Text);
                    cmd.Parameters.Add("@p4", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtNameEn.Text);
                    cmd.Parameters.Add("@p5", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtComboL_name.Text);
                    cmd.Parameters.Add("@p6", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtAngilal.Text);
                    cmd.Parameters.Add("@p7", SqlDbType.Int).Value = Core.ToStr(textEdit7.Text);
                    cmd.Parameters.Add("@p8", SqlDbType.NVarChar, 50).Value = Core.ToStr(textEdit8.Text);
                    cmd.Parameters.Add("@p9", SqlDbType.NVarChar, 50).Value = Core.ToStr(textBox1.Text);
                   

                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    Cursor.Current = Cursors.Default;

                    this.Tag = "ok";
                    this.Close();

                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Aлдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool CheckID(string _newCode, string _lname, string _angilal)
        {

            bool retVal = false;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                using (var db = new Gobibase())
                {
                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand("select count(*) as cnt from [t_info] where code=@p1 and l_name = @p2 and angilal = @p3", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@p1", SqlDbType.NVarChar, 50).Value = _newCode;
                    cmd.Parameters.Add("@p2", SqlDbType.NVarChar, 50).Value = _lname;
                    cmd.Parameters.Add("@p3", SqlDbType.NVarChar, 50).Value = _angilal;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            int _cnt = dr.IsDBNull(0) ? 0 : Core.ToInt(dr.GetInt32(0));
                            retVal = _cnt > 0 ? true : false;
                        }
                    }

                    conn.Close();
                    dr.Close();

                    Cursor.Current = Cursors.Default;
                }

                return retVal;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Aлдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Tag = null;
            this.Close();
        }
    }
}
