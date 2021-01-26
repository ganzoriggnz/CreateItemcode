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
    public partial class frmUserDetail : Form
    {
        clsUser c;
        List<string> heltes = new List<string>();
        List<string> position = new List<string>();

        public frmUserDetail(clsUser _c)
        {
            InitializeComponent();
            c = _c;
        }

        private void LoadData()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                using (var db = new Gobibase())
                {
                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand("SELECT [heltes] FROM [t_user] group by [heltes] order by heltes", conn);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            string _h = dr.IsDBNull(0) ? "" : Core.ToStr(dr.GetString(0));
                            txtHeltes.Properties.Items.Add(_h);
                        }
                    }

                    conn.Close();
                    dr.Close();

                    conn = new SqlConnection(db.Connection.ConnectionString);
                    cmd = new SqlCommand("SELECT [position] FROM [t_user] group by [position] order by position", conn);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            string _p = dr.IsDBNull(0) ? "" : Core.ToStr(dr.GetString(0));
                            txtPosition.Properties.Items.Add(_p);
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

        private void frmUserDetail_Load(object sender, EventArgs e)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));

            LoadData();

            radioGroup1.EditValue = "worker";
            radioGroup2.EditValue = "none";

            if (c != null)
            {
                txtLogin.Text = c.LoginName;
                txtUname.Text = c.Uname;
                txtPass.Text = c.Pass;
                txtHeltes.Text = c.Heltes;
                txtPosition.Text = c.Position;
                txtCreatedate.Text = c.Createdate.ToString("yyyy-MM-dd HH:mm");
                radioGroup1.EditValue = c.Role;
                radioGroup2.EditValue = c.Workertype;

                layoutControlItem1.Enabled = false;

                if (frmMain.cLoginUser.Role == "worker")
                {
                    layoutControlItem9.Enabled = false;
                    layoutControlItem10.Enabled = false;
                }


            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Tag = null;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtLogin.Text == null || txtLogin.Text == "")
            {
                MessageBox.Show("Нэвтрэх нэр оруул!", "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLogin.SelectAll();
                txtLogin.Focus();
                return;
            }
            if (c == null && CheckUserName())
            {
                MessageBox.Show("Нэвтрэх тавхцаж байна. Өөрчлөж оруулна уу!", "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLogin.SelectAll();
                txtLogin.Focus();
                return;
            }
            if (txtUname.Text == null || txtUname.Text == "")
            {
                MessageBox.Show("Хэрэглэгчийн нэр оруул!", "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUname.SelectAll();
                txtUname.Focus();
                return;
            }
            if (txtPass.Text == null || txtPass.Text == "")
            {
                MessageBox.Show("Нууд үг нэр оруул!", "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPass.SelectAll();
                txtPass.Focus();
                return;
            }
            if (txtHeltes.Text == null || txtHeltes.Text == "")
            {
                MessageBox.Show("Хэлтэсийн нэр оруул!", "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHeltes.SelectAll();
                txtHeltes.Focus();
                return;
            }
            if (txtPosition.Text == null || txtPosition.Text == "")
            {
                MessageBox.Show("Албан тушаал оруул!", "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPosition.SelectAll();
                txtPosition.Focus();
                return;
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                using (var db = new Gobibase())
                {

                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd;
                    if (c != null)
                    {
                        cmd = new SqlCommand(@"UPDATE [dbo].[t_user] SET [uname] = @p2,[pass] = @p3,[heltes] = @p4,[position] = @p5 ,[createdate] = @p6, [role]= @p8,[workertype]=@p9 WHERE id = @p7", conn);
                        cmd.Parameters.Add("@p7", SqlDbType.Int).Value = c.ID;
                    }
                    else
                    {
                        cmd = new SqlCommand(@"INSERT INTO [t_user]([loginName],[uname],[pass],[heltes],[position],[isActive],[createdate],[role],[workertype])VALUES (@p1,@p2,@p3,@p4,@p5,1,@p6,@p8, @p9)", conn);
                    }

                    cmd.Parameters.Add("@p1", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtLogin.Text);
                    cmd.Parameters.Add("@p2", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtUname.Text);
                    cmd.Parameters.Add("@p3", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtPass.Text);
                    cmd.Parameters.Add("@p4", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtHeltes.Text);
                    cmd.Parameters.Add("@p5", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtPosition.Text);
                    cmd.Parameters.Add("@p6", SqlDbType.DateTime).Value = DateTime.Now;
                    cmd.Parameters.Add("@p8", SqlDbType.NVarChar, 50).Value = Core.ToStr(radioGroup1.EditValue);
                    cmd.Parameters.Add("@p9", SqlDbType.NVarChar, 50).Value = Core.ToStr(radioGroup2.EditValue);
                   

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
        private bool CheckUserName()
        {
            bool retVal = false;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                using (var db = new Gobibase())
                {
                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand("SELECT count(*) as cnt FROM [t_user] where loginName = N'" + txtLogin.Text +"' and isactive = 1", conn);
                    cmd.CommandType = CommandType.Text;
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
    }
}
