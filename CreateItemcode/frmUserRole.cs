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
    public partial class frmUserRole : Form
    {
        List<clsUserRole> lst_role = new List<clsUserRole>();
        List<clsUserRole> lst_UserRole = new List<clsUserRole>();

        public frmUserRole()
        {
            InitializeComponent();
        }

        private void frmUserRole_Load(object sender, EventArgs e)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));

            using (var db = new Gobibase())
            {
                SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                SqlCommand cmd = new SqlCommand("SELECT [id],[loginName],[uname],[pass],[heltes],[position],[createdate],[role], [workertype] FROM [t_user] where isactive = 1", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                List<clsUser> lst_user = new List<clsUser>();
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
                        lst_user.Add(g);
                    }
                }

                gridControl1.DataSource = lst_user;

                conn.Close();
                dr.Close();

                conn = new SqlConnection(db.Connection.ConnectionString);
                cmd = new SqlCommand("select code, name from t_info where l_name = 'Level1' order by sortid", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                dr = cmd.ExecuteReader();

                List<clsInfo> lst_info = new List<clsInfo>();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        clsInfo g = new clsInfo();
                        g.Code = dr.IsDBNull(0) ? "" : Core.ToStr(dr.GetString(0));
                        g.Name = dr.IsDBNull(1) ? "" : Core.ToStr(dr.GetString(1));
                        g.Nameen = g.Code + " - " + g.Name;
                        lst_info.Add(g);
                    }
                }

                gridControl2.DataSource = lst_info;

                conn.Close();
                dr.Close();

            }

            LoadUserRole();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            clsUser us = (clsUser)gridView1.GetRow(gridView1.FocusedRowHandle);
            if (us == null)
                return;

            if (gridView2.GetSelectedRows().Count() > 0 && MessageBox.Show("Сонгосон бүлгийг нэмэх үү?","Анхааруулга", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int userID = us.ID;
                Cursor.Current = Cursors.WaitCursor;
                using (var db = new Gobibase())
                {
                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    SqlTransaction transaction;
                    transaction = conn.BeginTransaction("AddTrans");
                    cmd.Parameters.Add("@p1", SqlDbType.Int);
                    cmd.Parameters.Add("@p2", SqlDbType.NVarChar, 50);

                    cmd.Connection = conn;
                    cmd.Transaction = transaction;

                    try
                    {
                        foreach (int i in gridView2.GetSelectedRows())
                        {
                            clsInfo rw = (clsInfo)gridView2.GetRow(i);
                            cmd.CommandText = @"INSERT INTO [t_userrole] ([userid],[buleg]) VALUES (@p1,@p2)";
                            cmd.Parameters["@p1"].Value = userID;
                            cmd.Parameters["@p2"].Value = rw.Code;

                            if (lst_UserRole.Where(s => s.Userid == userID && s.Buleg == rw.Code).Count() > 0) continue;
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        conn.Close();

                    }
                    catch (Exception ex)
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Алдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (Exception ex2)
                        {
                            MessageBox.Show("Rollback Exception Type: {0}", ex2.GetType().ToString() + Environment.NewLine + "  Message: " + ex2.Message);
                        }
                    }
                }

                foreach (int i in gridView2.GetSelectedRows())
                {
                    gridView2.UnselectRow(i);
                }

                LoadUserRole();
                gcMain.DataSource = lst_UserRole.Where(s => s.Userid == userID).ToList();
            }
        }

        private void LoadUserRole()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                lst_UserRole.Clear();

                using (var db = new Gobibase())
                {
                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand(@"select r.id,r.buleg,t.name,r.userid   from t_user as u inner join t_userrole as r on u.id = r.userid
inner join t_info as t on t.code = r.buleg and t.l_name = 'level1' 
order by t.sortid", conn);
                    cmd.CommandType = CommandType.Text;
                    
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            clsUserRole c = new clsUserRole();
                            c.ID = dr.IsDBNull(0) ? 0 : Core.ToInt(dr.GetInt32(0));
                            c.Buleg = dr.IsDBNull(1) ? "" : Core.ToStr(dr.GetString(1));
                            c.Bulegname = dr.IsDBNull(2) ? "" : Core.ToStr(dr.GetString(2));
                            c.Userid = dr.IsDBNull(3) ? 0 : Core.ToInt(dr.GetInt32(3));
                            lst_UserRole.Add(c);
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

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            clsUser rw = (clsUser)gridView1.GetRow(e.FocusedRowHandle);
            if (rw == null)
                return;
            int num = rw.ID;
            gcMain.DataSource = lst_UserRole.Where(s => s.Userid == num).ToList();
        }

        private void repoRemove_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (gvMain.FocusedRowHandle < 0) return;
            clsUserRole curr = (clsUserRole)gvMain.GetFocusedRow();

            clsUser rw = (clsUser)gridView1.GetRow(gridView1.FocusedRowHandle);
            if (rw == null)
                return;
            int num = rw.ID;

            if (curr.ID != 0 && MessageBox.Show("Сонгосон бүлгийг хасах уу?", "Анхааруулга", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    using (var db = new Gobibase())
                    {

                        SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                        SqlCommand cmd = new SqlCommand(@"delete from t_userrole where id = @p1", conn);
                        cmd.Parameters.Add("@p1", SqlDbType.Int).Value = curr.ID;

                        cmd.CommandType = CommandType.Text;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();

                        LoadUserRole();

                        gcMain.DataSource = lst_UserRole.Where(s => s.Userid == num).ToList();

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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
