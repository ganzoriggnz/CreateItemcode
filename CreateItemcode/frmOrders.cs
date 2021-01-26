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
    public partial class frmOrders : Form
    {
        List<clsListItemMain> list_main = new List<clsListItemMain>();
        List<clsAllData> lst_excel = new List<clsAllData>();

        public frmOrders()
        {
            InitializeComponent();
        }

        private void frmOrders_Load(object sender, EventArgs e)
        {
            GetItemcodeRowdata();
        }

        private void GetItemcodeRowdata()
        {
            try
            {
                using (var db = new Gobibase())
                {
                    lst_excel.Clear();

                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand(@"Select m.id, m.itemcode, m.itemname, m.itemnameen, m.lev1, m.lev2, m.lev3, m.lev4, m.lev5, m.color1 as mcolor1, m.lev6, 
m.lev7, m.color2 as mcolor2, m.lev8, m.lev9, m.createddate, u.uname, m.l1n, m.l2n, m.l3n, m.l6n, m.l8n, 
m.l9n, m.zagvartype, m.itemcode_eh, m.createduser,
d.version, d.versiondate, d.brend, d.collection, d.utasno, d.holio, d.gage, d.tuslahmat, d.suljees, d.zahsuljees, d.ihbiemanjet, d.hantsuumanjet, d.zahjin, 
z.uname as zzb, d.urt, d.murniiturul, d.engerturul, d.zahturul, d.hantsuuturul, d.halaasniiturul, d.zahialgach, d.cusnum, d.printnum, z1.uname, d.omnohdugaar, d.gobidugaar, d.goyodugaar, d.onlinenum, d.code26, d.code22, d.barcode, 
d.color1, d.color2, d.color3, d.color4, d.color5, d.color6, d.color7, d.color8, d.color9, d.color10, d.colorOther, d.colorhuvi1, d.colorhuvi2, d.colorhuvi3, d.colorhuvi4, d.colorhuvi5, d.colorhuvi6, d.colorhuvi7, 
d.colorhuvi8, d.colorhuvi9, d.colorhuvi10, d.colorhuviOther,
d.createdU, d.editedU, d.editedDate,u1.uname, u2.uname, d.ehzjin, d.massjin, d.tuuhiijin, d.bohirjin, d.hodzartsuulalttime, d.oymolMatNum,col.colorname,o.colorpart,
o.zahnum, o.zahialgach
from t_main as m inner join t_details as d on m.itemcode_eh = d.itemcode
inner join t_user as u on u.id= m.createduser
inner join t_orderlist as o on o.itemcode = m.itemcode and m.version_eh = d.version
left outer join t_user as u1 on u1.id= d.createdU
left outer join t_user as u2 on u2.id= d.editedU
left outer join t_user as z on z.id= d.zzb
left outer join t_user as z1 on z1.id= d.designer
left outer join t_colormatrix as col on col.colormain = m.lev5
where o.createdU = @p1 order by m.itemcode;", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@p1", SqlDbType.NVarChar, 50).Value = frmMain.cLoginUser.Uname;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            clsAllData g = new clsAllData();
                            g.ID = dr.IsDBNull(0) ? 0 : Core.ToInt(dr.GetInt32(0));
                            g.Itemcode = dr.IsDBNull(1) ? "" : Core.ToStr(dr.GetString(1));
                            g.Itemname = dr.IsDBNull(2) ? "" : Core.ToStr(dr.GetString(2));
                            g.Itemnameen = dr.IsDBNull(3) ? "" : Core.ToStr(dr.GetString(3));
                            g.Lev1 = dr.IsDBNull(4) ? "" : Core.ToStr(dr.GetString(4));
                            g.Lev2 = dr.IsDBNull(5) ? "" : Core.ToStr(dr.GetString(5));
                            g.Lev3 = dr.IsDBNull(6) ? "" : Core.ToStr(dr.GetString(6));
                            g.Lev4 = dr.IsDBNull(7) ? "" : Core.ToStr(dr.GetString(7));
                            g.Lev5 = dr.IsDBNull(8) ? "" : Core.ToStr(dr.GetString(8));
                            g.Mcolor1 = dr.IsDBNull(9) ? "" : Core.ToStr(dr.GetString(9));
                            g.Lev6 = dr.IsDBNull(10) ? "" : Core.ToStr(dr.GetString(10));
                            g.Lev7 = dr.IsDBNull(11) ? "" : Core.ToStr(dr.GetString(11));
                            g.Mcolor2 = dr.IsDBNull(12) ? "" : Core.ToStr(dr.GetString(12));
                            g.Lev8 = dr.IsDBNull(13) ? "" : Core.ToStr(dr.GetString(13));
                            g.Lev9 = dr.IsDBNull(14) ? "" : Core.ToStr(dr.GetString(14));
                            g.Cdate = dr.IsDBNull(15) ? DateTime.Now : Core.ToDateTime(dr.GetDateTime(15));
                            g.Cuser = dr.IsDBNull(16) ? "" : Core.ToStr(dr.GetString(16));
                            g.L1n = dr.IsDBNull(17) ? "" : Core.ToStr(dr.GetString(17));
                            g.L2n = dr.IsDBNull(18) ? "" : Core.ToStr(dr.GetString(18));
                            g.L3n = dr.IsDBNull(19) ? "" : Core.ToStr(dr.GetString(19));
                            g.L6n = dr.IsDBNull(20) ? "" : Core.ToStr(dr.GetString(20));
                            g.L8n = dr.IsDBNull(21) ? "" : Core.ToStr(dr.GetString(21));
                            g.L9n = dr.IsDBNull(22) ? "" : Core.ToStr(dr.GetString(22));
                            g.Zagvartype = dr.IsDBNull(23) ? "" : Core.ToStr(dr.GetString(23));
                            g.Itemcode_eh = dr.IsDBNull(24) ? "" : Core.ToStr(dr.GetString(24));
                            g.Itemcode_eh = g.Itemcode_eh.Substring(0, 8);
                            g.Cuserid = dr.IsDBNull(25) ? 0 : Core.ToInt(dr.GetInt32(25));
                            g.Version = dr.IsDBNull(26) ? 0 : Core.ToInt(dr.GetInt32(26));
                            g.Versiondate = dr.IsDBNull(27) ? DateTime.Now : Core.ToDateTime(dr.GetDateTime(27));
                            g.Brend = dr.IsDBNull(28) ? "" : Core.ToStr(dr.GetString(28));
                            g.Collection = dr.IsDBNull(29) ? "" : Core.ToStr(dr.GetString(29));
                            g.Utasno = dr.IsDBNull(30) ? "" : Core.ToStr(dr.GetString(30));
                            g.Holio = dr.IsDBNull(31) ? "" : Core.ToStr(dr.GetString(31));
                            g.Gage = dr.IsDBNull(32) ? "" : Core.ToStr(dr.GetString(32));
                            g.Tuslahmat = dr.IsDBNull(33) ? "" : Core.ToStr(dr.GetString(33));
                            g.Suljees = dr.IsDBNull(34) ? "" : Core.ToStr(dr.GetString(34));
                            g.Zahsuljees = dr.IsDBNull(35) ? "" : Core.ToStr(dr.GetString(35));
                            g.Ihbiemanjet = dr.IsDBNull(36) ? "" : Core.ToStr(dr.GetString(36));
                            g.Hantsuumanjet = dr.IsDBNull(37) ? "" : Core.ToStr(dr.GetString(37));
                            g.Zahjin = dr.IsDBNull(38) ? "" : Core.ToStr(dr.GetString(38));
                            g.Zzbname = dr.IsDBNull(39) ? "" : Core.ToStr(dr.GetString(39));
                            g.Urt = dr.IsDBNull(40) ? "" : Core.ToStr(dr.GetString(40));
                            g.Murniiturul = dr.IsDBNull(41) ? "" : Core.ToStr(dr.GetString(41));
                            g.Engerturul = dr.IsDBNull(42) ? "" : Core.ToStr(dr.GetString(42));
                            g.Zahturul = dr.IsDBNull(43) ? "" : Core.ToStr(dr.GetString(43));
                            g.Hantsuuturul = dr.IsDBNull(44) ? "" : Core.ToStr(dr.GetString(44));
                            g.Halaasniiturul = dr.IsDBNull(45) ? "" : Core.ToStr(dr.GetString(45));
                            g.Zahialgach = dr.IsDBNull(46) ? "" : Core.ToStr(dr.GetString(46));
                            g.Cusnum = dr.IsDBNull(47) ? "" : Core.ToStr(dr.GetString(47));
                            g.Printnum = dr.IsDBNull(48) ? "" : Core.ToStr(dr.GetString(48));
                            g.Designername = dr.IsDBNull(49) ? "" : Core.ToStr(dr.GetString(49));
                            g.Omnohdugaar = dr.IsDBNull(50) ? "" : Core.ToStr(dr.GetString(50));
                            g.Gobidugaar = dr.IsDBNull(51) ? "" : Core.ToStr(dr.GetString(51));
                            g.Goyodugaar = dr.IsDBNull(52) ? "" : Core.ToStr(dr.GetString(52));
                            g.Onlinenum = dr.IsDBNull(53) ? "" : Core.ToStr(dr.GetString(53));
                            g.Code26 = dr.IsDBNull(54) ? "" : Core.ToStr(dr.GetString(54));
                            g.Code22 = dr.IsDBNull(55) ? "" : Core.ToStr(dr.GetString(55));
                            g.Barcode = dr.IsDBNull(56) ? "" : Core.ToStr(dr.GetString(56));
                            g.Color1 = dr.IsDBNull(57) ? "" : Core.ToStr(dr.GetString(57));
                            g.Color2 = dr.IsDBNull(58) ? "" : Core.ToStr(dr.GetString(58));
                            g.Color3 = dr.IsDBNull(59) ? "" : Core.ToStr(dr.GetString(59));
                            g.Color4 = dr.IsDBNull(60) ? "" : Core.ToStr(dr.GetString(60));
                            g.Color5 = dr.IsDBNull(61) ? "" : Core.ToStr(dr.GetString(61));
                            g.Color6 = dr.IsDBNull(62) ? "" : Core.ToStr(dr.GetString(62));
                            g.Color7 = dr.IsDBNull(63) ? "" : Core.ToStr(dr.GetString(63));
                            g.Color8 = dr.IsDBNull(64) ? "" : Core.ToStr(dr.GetString(64));
                            g.Color9 = dr.IsDBNull(65) ? "" : Core.ToStr(dr.GetString(65));
                            g.Color10 = dr.IsDBNull(66) ? "" : Core.ToStr(dr.GetString(66));
                            g.ColorOther = dr.IsDBNull(67) ? "" : Core.ToStr(dr.GetString(67));
                            g.Colorhuvi1 = dr.IsDBNull(68) ? "" : Core.ToStr(dr.GetString(68));
                            g.Colorhuvi2 = dr.IsDBNull(69) ? "" : Core.ToStr(dr.GetString(69));
                            g.Colorhuvi3 = dr.IsDBNull(70) ? "" : Core.ToStr(dr.GetString(70));
                            g.Colorhuvi4 = dr.IsDBNull(71) ? "" : Core.ToStr(dr.GetString(71));
                            g.Colorhuvi5 = dr.IsDBNull(72) ? "" : Core.ToStr(dr.GetString(72));
                            g.Colorhuvi6 = dr.IsDBNull(73) ? "" : Core.ToStr(dr.GetString(73));
                            g.Colorhuvi7 = dr.IsDBNull(74) ? "" : Core.ToStr(dr.GetString(74));
                            g.Colorhuvi8 = dr.IsDBNull(75) ? "" : Core.ToStr(dr.GetString(75));
                            g.Colorhuvi9 = dr.IsDBNull(76) ? "" : Core.ToStr(dr.GetString(76));
                            g.Colorhuvi10 = dr.IsDBNull(77) ? "" : Core.ToStr(dr.GetString(77));
                            g.ColorhuviOther = dr.IsDBNull(78) ? "" : Core.ToStr(dr.GetString(78).ToString());
                            g.CreatedU = dr.IsDBNull(79) ? 0 : Core.ToInt(dr.GetInt32(79));
                            g.EditedU = dr.IsDBNull(80) ? 0 : Core.ToInt(dr.GetInt32(80));
                            g.EditedDate = dr.IsDBNull(81) ? "" : Core.ToStr(dr.GetDateTime(81).ToString("yyyy/MM/dd HH:mm"));
                            g.CreatedUser = dr.IsDBNull(82) ? "" : Core.ToStr(dr.GetString(82));
                            g.EditedUser = dr.IsDBNull(83) ? "" : Core.ToStr(dr.GetString(83));
                            g.Ehzjin = dr.IsDBNull(84) ? "" : Core.ToStr(dr.GetString(84));
                            g.Massjin = dr.IsDBNull(85) ? "" : Core.ToStr(dr.GetString(85));
                            g.Tuuhiijin = dr.IsDBNull(86) ? "" : Core.ToStr(dr.GetString(86));
                            g.Bohirjin = dr.IsDBNull(87) ? "" : Core.ToStr(dr.GetString(87));
                            g.Hodzartsuulalttime = dr.IsDBNull(88) ? "" : Core.ToStr(dr.GetString(88));
                            g.OymolMatNum = dr.IsDBNull(89) ? "" : Core.ToStr(dr.GetString(89));
                            g.Colorname = dr.IsDBNull(90) ? "" : Core.ToStr(dr.GetString(90));
                            g.Colorpart = dr.IsDBNull(91) ? "" : Core.ToStr(dr.GetString(91));
                            g.Zahnum = dr.IsDBNull(92) ? "" : Core.ToStr(dr.GetString(92));
                            g.Zahialgach2 = dr.IsDBNull(93) ? "" : Core.ToStr(dr.GetString(93));

                            string[] tt = g.L8n.Split('-');
                            if (tt.Length > 1)
                            {
                                g.Sizef = tt[1].Trim();
                            }

                            lst_excel.Add(g);
                        }
                    }

                    conn.Close();
                    dr.Close();

                    gcExcel.DataSource = lst_excel.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Aлдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (gvExcel.DataRowCount == 0) return;
            string fileName = "";
            saveFileDialog1.Filter = "EXCEL 2003-20**|*.xlsx|Excel 97-2003|*.xls";
            DialogResult result = saveFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                fileName = saveFileDialog1.FileName;
                gvExcel.ExportToXlsx(fileName);
            }
        }
    }
}
