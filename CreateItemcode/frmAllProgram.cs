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
    public partial class frmAllProgram : Form
    {
        List<clsProgram> lst_pro_version = new List<clsProgram>();

        public frmAllProgram()
        {
            InitializeComponent();
        }

        private void frmAllProgram_Load(object sender, EventArgs e)
        {
            LoadProVersion();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        }

        private void LoadProVersion()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                using (var db = new Gobibase())
                {

                    lst_pro_version.Clear();

                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand(@"SELECT p.[id],[proman],p.[itemcode],[startdate],[givedate]
,[version],[tuluv],[car_typeid],[car_markid],[detailnum],[progunelgee],[suljihtime],u.uname,ct.name as cartype, cm.name as carmark,profilename,hurd,sizetoo
FROM [t_programist] as p 
inner join dbo.t_main AS m ON p.itemcode = m.itemcode AND m.zagvartype = N'Эх загвар'
left join t_user as u on u.id = p.proman 
left outer join t_info as ct on ct.id = p.car_typeid and ct.l_name='cartype'
left outer join t_info as cm on cm.id = p.car_markid and cm.l_name='carmark' order by p.itemcode;", conn);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            clsProgram g = new clsProgram();
                            g.ID = dr.IsDBNull(0) ? 0 : Core.ToInt(dr.GetInt32(0));
                            g.Proman = dr.IsDBNull(1) ? 0 : Core.ToInt(dr.GetInt32(1));
                            g.Itemcode = dr.IsDBNull(2) ? "" : Core.ToStr(dr.GetString(2));
                            g.Startdate = dr.IsDBNull(3) ? "" : Core.ToStr(dr.GetString(3));
                            g.Givedate = dr.IsDBNull(4) ? "" : Core.ToStr(dr.GetString(4));
                            g.Version = dr.IsDBNull(5) ? 0 : Core.ToInt(dr.GetInt32(5));
                            g.Tuluv = dr.IsDBNull(6) ? "" : Core.ToStr(dr.GetString(6));
                            g.Car_typeid = dr.IsDBNull(7) ? 0 : Core.ToInt(dr.GetInt32(7));
                            g.Car_markid = dr.IsDBNull(8) ? 0 : Core.ToInt(dr.GetInt32(8));
                            g.Detailnum = dr.IsDBNull(9) ? "" : Core.ToStr(dr.GetString(9));
                            g.Progunelgee = dr.IsDBNull(10) ? "" : Core.ToStr(dr.GetString(10));
                            g.Suljihtime = dr.IsDBNull(11) ? "" : Core.ToStr(dr.GetString(11));
                            g.Proname = dr.IsDBNull(12) ? "" : Core.ToStr(dr.GetString(12));
                            g.Cartype = dr.IsDBNull(13) ? "" : Core.ToStr(dr.GetString(13));
                            g.Carmark = dr.IsDBNull(14) ? "" : Core.ToStr(dr.GetString(14));
                            g.Profilename = dr.IsDBNull(15) ? "" : Core.ToStr(dr.GetString(15));
                            g.Hurd = dr.IsDBNull(16) ? "" : Core.ToStr(dr.GetString(16));
                            g.Sizetoo = dr.IsDBNull(17) ? "" : Core.ToStr(dr.GetString(17));
                            lst_pro_version.Add(g);
                        }
                    }

                    conn.Close();
                    dr.Close();
                    Cursor.Current = Cursors.Default;

                    gridControl1.DataSource = lst_pro_version.ToList();

                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Aлдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (gridView1.DataRowCount == 0) return;
            string fileName = "";
            saveFileDialog1.Filter = "EXCEL 2003-20**|*.xlsx|Excel 97-2003|*.xls";
            DialogResult result = saveFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                fileName = saveFileDialog1.FileName;
                gridControl1.ExportToXlsx(fileName);
            }
        }

        private void repoShow_Click(object sender, EventArgs e)
        {
            string num = gridView1.GetFocusedRowCellValue("Itemcode").ToString();
            if (num.Length > 0)
            {
                frmCreate f = new frmCreate(LoadDataEh(num));
                f.ShowDialog();
                LoadProVersion();
            }
        }

        private clsListItemMain LoadDataEh(string _p1)
        {
            clsListItemMain g = new clsListItemMain();
            try
            {
                using (var db = new Gobibase())
                {
                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand(@"SELECT m.[id],[itemcode],[itemname],[itemnameen],[lev1],[lev2],[lev3],[lev4],[lev5],[lev6],[lev7],[lev8],[lev9],
[createddate],u.uname, color1, color2,[l1n],[l2n],[l3n],[l6n],[l8n],[l9n],[zagvartype],[itemcode_eh],u.id 
from t_main as m inner join t_user as u on u.id = m.[createduser] where itemcode = @p1 and zagvartype = N'Эх загвар';", conn);
                    cmd.Parameters.Add("@p1", SqlDbType.NVarChar, 50).Value = _p1;
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            g.ID = dr.IsDBNull(0) ? 0 : Core.ToInt(dr.GetInt32(0));
                            g.Itemcode = dr.IsDBNull(1) ? "" : Core.ToStr(dr.GetString(1));
                            g.Itemname = dr.IsDBNull(2) ? "" : Core.ToStr(dr.GetString(2));
                            g.Itemnameen = dr.IsDBNull(3) ? "" : Core.ToStr(dr.GetString(3));
                            g.Lev1 = dr.IsDBNull(4) ? "" : Core.ToStr(dr.GetString(4));
                            g.Lev2 = dr.IsDBNull(5) ? "" : Core.ToStr(dr.GetString(5));
                            g.Lev3 = dr.IsDBNull(6) ? "" : Core.ToStr(dr.GetString(6));
                            g.Lev4 = dr.IsDBNull(7) ? "" : Core.ToStr(dr.GetString(7));
                            g.Lev5 = dr.IsDBNull(8) ? "" : Core.ToStr(dr.GetString(8));
                            g.Lev6 = dr.IsDBNull(9) ? "" : Core.ToStr(dr.GetString(9));
                            g.Lev7 = dr.IsDBNull(10) ? "" : Core.ToStr(dr.GetString(10));
                            g.Lev8 = dr.IsDBNull(11) ? "" : Core.ToStr(dr.GetString(11));
                            g.Lev9 = dr.IsDBNull(12) ? "" : Core.ToStr(dr.GetString(12));
                            g.Cdate = dr.IsDBNull(13) ? DateTime.Now : Core.ToDateTime(dr.GetDateTime(13));
                            g.Cuser = dr.IsDBNull(14) ? "" : Core.ToStr(dr.GetString(14));
                            g.Color1 = dr.IsDBNull(15) ? "" : Core.ToStr(dr.GetString(15));
                            g.Color2 = dr.IsDBNull(16) ? "" : Core.ToStr(dr.GetString(16));

                            g.L1n = dr.IsDBNull(17) ? "" : Core.ToStr(dr.GetString(17));
                            g.L2n = dr.IsDBNull(18) ? "" : Core.ToStr(dr.GetString(18));
                            g.L3n = dr.IsDBNull(19) ? "" : Core.ToStr(dr.GetString(19));
                            g.L6n = dr.IsDBNull(20) ? "" : Core.ToStr(dr.GetString(20));
                            g.L8n = dr.IsDBNull(21) ? "" : Core.ToStr(dr.GetString(21));
                            g.L9n = dr.IsDBNull(22) ? "" : Core.ToStr(dr.GetString(22));
                            g.Zagvartype = dr.IsDBNull(23) ? "" : Core.ToStr(dr.GetString(23));
                            g.Itemcode_eh = dr.IsDBNull(24) ? "" : Core.ToStr(dr.GetString(24));
                            g.Cuserid = dr.IsDBNull(25) ? 0 : Core.ToInt(dr.GetInt32(25));

                        }
                    }

                    conn.Close();
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Aлдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return g;
        }
    }
}
