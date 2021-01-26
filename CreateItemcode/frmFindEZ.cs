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
    public partial class frmFindEZ : Form
    {
        List<clsListItemMain> list_main = new List<clsListItemMain>();

        public frmFindEZ()
        {
            InitializeComponent();
        }

        private void frmFindEZ_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                using (var db = new Gobibase())
                {
                    list_main.Clear();

                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand(@"SELECT m.[id],m.[itemcode],[itemname],[itemnameen],[lev1],[lev2],[lev3],[lev4],[lev5],
[lev6],[lev7],[lev8],[lev9],[createddate],u.uname, m.color1, m.color2,[l1n],[l2n],[l3n],
[l6n],[l8n],[l9n],[zagvartype],[itemcode_eh],u.id,
d.gobidugaar, d.onlinenum, d.code26, d.version
from t_main as m 
inner join t_user as u on u.id = m.[createduser]
left outer join t_details as d on d.itemcode = m.itemcode
where [zagvartype] = N'эх загвар'"  + CFunctions.StrMainWhere(true) , conn);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            clsListItemMain g = new clsListItemMain();
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
                            g.Gobidugaar = dr.IsDBNull(26) ? "" : Core.ToStr(dr.GetString(26));
                            g.Onlinenum = dr.IsDBNull(27) ? "" : Core.ToStr(dr.GetString(27));
                            g.Code26 = dr.IsDBNull(28) ? "" : Core.ToStr(dr.GetString(28));
                            g.Version = dr.IsDBNull(29) ? 0 : Core.ToInt(dr.GetInt32(29));

                            list_main.Add(g);
                        }
                    }

                    conn.Close();
                    dr.Close();

                    gcMain.DataSource = list_main.ToList();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Aлдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void repoSelect_Click(object sender, EventArgs e)
        {
            string num = gvMain.GetFocusedRowCellValue("Itemcode").ToString();
            string ver = gvMain.GetFocusedRowCellValue("Version").ToString();

            if (num.Length > 0)
            {
                this.Tag = list_main.Where(s => s.Itemcode == num && s.Version == Core.ToInt(ver)).First();
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Tag = null;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (gvMain.FocusedRowHandle >= 0)
            {
                string num = gvMain.GetFocusedRowCellValue("Itemcode").ToString();
                string ver = gvMain.GetFocusedRowCellValue("Version").ToString();

                if (num.Length > 0)
                {
                    this.Tag = list_main.Where(s => s.Itemcode == num && s.Version == Core.ToInt(ver)).First();
                    this.Close();
                }
            }
        }
    }
}
