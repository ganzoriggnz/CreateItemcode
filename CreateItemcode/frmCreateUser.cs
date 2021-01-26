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
    public partial class frmCreateUser : Form
    {
        List<clsUser> lst_info = new List<clsUser>();

        public frmCreateUser()
        {
            InitializeComponent();
        }

        private void frmCreateUser_Load(object sender, EventArgs e)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            LoadData();
            gridColumn9.Visible = frmMain.cLoginUser.Role == "admin" ? true : false;
            layoutControlItem2.Visibility = frmMain.cLoginUser.Role == "worker" ? DevExpress.XtraLayout.Utils.LayoutVisibility.Never : DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
        }

        private void LoadData()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                using (var db = new Gobibase())
                {
                    lst_info.Clear();
                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand("SELECT [id],[loginName],[uname],[pass],[heltes],[position],[createdate], role, workertype FROM [t_user] where [role] <> 'admin' and isactive = 1", conn);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            clsUser g = new clsUser();
                            g.ID = dr.IsDBNull(0) ? 0 : Core.ToInt(dr.GetInt32(0));
                            g.LoginName = dr.IsDBNull(1) ? "" : Core.ToStr(dr.GetString(1));
                            g.Uname = dr.IsDBNull(2) ? "" : Core.ToStr(dr.GetString(2));
                            g.Pass = dr.IsDBNull(3) ? "" : Core.ToStr(dr.GetString(3));
                            g.Heltes = dr.IsDBNull(4) ? "" : Core.ToStr(dr.GetString(4));
                            g.Position = dr.IsDBNull(5) ? "" : Core.ToStr(dr.GetString(5));
                            g.Createdate = dr.IsDBNull(6) ? DateTime.Now : Core.ToDateTime(dr.GetDateTime(6));
                            g.Role = dr.IsDBNull(7) ? "" : Core.ToStr(dr.GetString(7));
                            g.Workertype = dr.IsDBNull(8) ? "" : Core.ToStr(dr.GetString(8));
                            lst_info.Add(g);
                        }
                    }

                    conn.Close();
                    dr.Close();

                    gridControl1.DataSource = lst_info.OrderBy(o => o.Uname).ToList();

                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Aлдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void repoEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            clsUser curr = (clsUser)gridView1.GetFocusedRow();

            if (curr.ID != 0)
            {
                frmUserDetail fu = new frmUserDetail(curr);
                fu.ShowDialog();

                if (fu.Tag != null)
                {
                    LoadData();
                }
            }
        }

        private void repoRemove_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            clsUser curr = (clsUser)gridView1.GetFocusedRow();

            if (curr.ID != 0 && MessageBox.Show("Сонгосон хэрэглэгчийг идэвхгүй болгох уу?", "Анхааруулга", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    using (var db = new Gobibase())
                    {

                        SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                        SqlCommand cmd = new SqlCommand(@"UPDATE [dbo].[t_user] SET isActive = 0 WHERE id = @p1", conn);


                        cmd.Parameters.Add("@p1", SqlDbType.Int).Value = curr.ID;

                        cmd.CommandType = CommandType.Text;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();

                        LoadData();

                        Cursor.Current = Cursors.Default;

                      
                    }
                }
                catch (Exception ex)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Aлдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            frmUserDetail fu = new frmUserDetail(null);
            fu.ShowDialog();

            if (fu.Tag != null)
            {
                LoadData();
            }
        }

        private void btnRole_Click(object sender, EventArgs e)
        {
            new frmUserRole().ShowDialog();
        }
    }
}
