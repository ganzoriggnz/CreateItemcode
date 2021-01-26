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
    public partial class frmCreateZagvar : Form
    {
        List<clsInfo> lst = new List<clsInfo>();
      

        public static string _lname = "";
        public frmCreateZagvar()
        {
            InitializeComponent();
        }

        private void frmCreateZagvar_Load(object sender, EventArgs e)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            LoadRefSection();
        }

        private void LoadRefSection()
        {

            try
            {
                using (var db = new Gobibase())
                {
                    Cursor.Current = Cursors.WaitCursor;

                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand("select l_name from t_info group by l_name order by l_name", conn);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    List<clsInfo> temp_l = new List<clsInfo>();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            clsInfo f = new clsInfo();
                            f.Name = dr.IsDBNull(0) ? "" : Core.ToStr(dr.GetString(0));
                            temp_l.Add(f);
                        }
                    }

                    conn.Close();
                    dr.Close();

                    comboBoxEdit1.Properties.ValueMember = "Name";
                    comboBoxEdit1.Properties.DisplayMember = "Name";
                    comboBoxEdit1.Properties.DataSource = temp_l.ToList();


                    Cursor.Current = Cursors.Default;
                }
            }

            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Шинэчлэх үед алдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadData(string _Code)
        {
            _lname = "";
            try
            {
                lst.Clear();
                using (var db = new Gobibase())
                {
                    Cursor.Current = Cursors.WaitCursor;

                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand("SELECT [id],[code],[name],[nameen],[l_name],[angilal],[sortid],[other1],[other2] FROM [t_info] where l_name='" + _Code + "'", conn);
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            clsInfo f = new clsInfo();
                            f.ID = dr.IsDBNull(0) ? 0 : Core.ToInt(dr.GetInt32(0));
                            f.Code = dr.IsDBNull(1) ? "" : Core.ToStr(dr.GetString(1));
                            f.Name = dr.IsDBNull(2) ? "" : Core.ToStr(dr.GetString(2));
                            f.Nameen = dr.IsDBNull(3) ? "" : Core.ToStr(dr.GetString(3));
                            f.L_name = dr.IsDBNull(4) ? "" : Core.ToStr(dr.GetString(4));
                            f.Angilal = dr.IsDBNull(5) ? "" : Core.ToStr(dr.GetString(5));
                            f.Sortid = dr.IsDBNull(6) ? 0 : Core.ToInt(dr.GetInt32(6));
                            f.Other1 = dr.IsDBNull(7) ? "" : Core.ToStr(dr.GetString(7));
                            f.Other2 = dr.IsDBNull(8) ? "" : Core.ToStr(dr.GetString(8));
                            lst.Add(f);
                        }
                    }

                    conn.Close();
                    dr.Close();

                    _lname = Core.ToStr(comboBoxEdit1.Text);

                    Cursor.Current = Cursors.Default;
                }

                gridControl1.DataSource = lst.ToList();
            }

            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Шинэчлэх үед алдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void repoEdit_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            clsInfo curr = (clsInfo)gridView1.GetFocusedRow();

            if (curr.ID != 0)
            {
                frmLevelDetail fu = new frmLevelDetail(curr);
                fu.ShowDialog();

                if (fu.Tag != null)
                {
                    LoadRefSection();
                    LoadData(Core.ToStr(comboBoxEdit1.EditValue));
                }
            }
        
        }

      
        private void comboBoxEdit1_EditValueChanged(object sender, EventArgs e)
        {
            LoadData(Core.ToStr(comboBoxEdit1.EditValue));
        }



        private void simpleButton1_Click(object sender, EventArgs e)
        {
            frmLevelDetail fu = new frmLevelDetail(null);
            fu.ShowDialog();

            if (fu.Tag != null)
            {
                LoadRefSection();
                LoadData(Core.ToStr(comboBoxEdit1.EditValue));
            }

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Tag = null;
            this.Close();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (comboBoxEdit1.Text == null || comboBoxEdit1.Text == "") return;
            frmLevelDetail fu = new frmLevelDetail(null);
            fu.ShowDialog();

            if (fu.Tag != null)
            {
                LoadRefSection();
                LoadData(Core.ToStr(comboBoxEdit1.EditValue));
            }
        }
    }
}
