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
    public partial class frmAllData : Form
    {
        List<clsAllData> lst_main = new List<clsAllData>();
        List<clsInfo> lst_info = new List<clsInfo>();
        List<clsInfo> cn = new List<clsInfo>() ;

        string[] _array ={"Itemcode","Itemname","Itemnameen","L1n","L2n","L3n","L6n","Lev4","Lev5","Lev6","Lev7"
,"L8n","L9n","Cdate","Cuser","Version","Versiondate","Brend"
,"Collection","Utasno","Holio","Gage","Tuslahmat","Suljees","Zahsuljees"
,"Ihbiemanjet","Hantsuumanjet","Zahjin","Zzb","Urt","Murniiturul","Engerturul"
,"Zahturul","Hantsuuturul","Halaasniiturul","Zahialgach","Cusnum","Printnum"
,"Designer","Omnohdugaar","Gobidugaar","Goyodugaar","Onlinenum","Code26","Code22"
,"Barcode","Color1","Color2","Color3","Color4","Color5","Color6","Color7","Color8","Color9"
,"Color10","ColorOther","Colorhuvi1","Colorhuvi2","Colorhuvi3","Colorhuvi4","Colorhuvi5","Colorhuvi6"
,"Colorhuvi7","Colorhuvi8","Colorhuvi9","Colorhuvi10","ColorhuviOther","Ehzjin","Massjin","Tuuhiijin","Bohirjin","Hodzartsuulalttime","OymolMatNum"};

        public frmAllData()
        {
            InitializeComponent();
        }

        private void frmAllData_Load(object sender, EventArgs e)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));

            LoadInfo();

            luplbl1.Properties.DataSource = lst_info.Where(s => s.L_name == "Level1").OrderBy(o => o.Sortid).ToList();
            luplbl1.Properties.DisplayMember = "CodeName";
            luplbl1.Properties.ValueMember = "Code";
            luplbl1.PreviewKeyDown += new PreviewKeyDownEventHandler(this.LookUpClear_PreviewKeyDown);

            luplbl2.Properties.DataSource = lst_info.Where(s => s.L_name == "Level3").OrderBy(o => o.Sortid).ToList();
            luplbl2.Properties.DisplayMember = "CodeName";
            luplbl2.Properties.ValueMember = "Code";
            luplbl2.PreviewKeyDown += new PreviewKeyDownEventHandler(this.LookUpClear_PreviewKeyDown);
            CreadColumns();
        }

        private void CreadColumns()
        {
            cn.Add(new clsInfo() { Name = "Itemcode", Nameen = "КОД", ID = 2 });
            cn.Add(new clsInfo() { Name = "Version", Nameen = "Хувилбар", ID = 3 });
            cn.Add(new clsInfo() { Name = "Gobidugaar", Nameen = "Говь дугаар", ID = 4 });
            cn.Add(new clsInfo() { Name = "L1n", Nameen = "Ангилал", ID = 5 });
            cn.Add(new clsInfo() { Name = "L2n", Nameen = "Хүйс", ID = 6 });
            cn.Add(new clsInfo() { Name = "L3n", Nameen = "Нэр төрөл", ID = 7 });
            cn.Add(new clsInfo() { Name = "Lev4", Nameen = "Дугаар", ID = 8 });
            cn.Add(new clsInfo() { Name = "Lev5", Nameen = "Үндсэн өнгө", ID = 9 });
            cn.Add(new clsInfo() { Name = "L6n", Nameen = "Загварын нэмэлт", ID = 10 });
            cn.Add(new clsInfo() { Name = "Lev7", Nameen = "Туслах өнгө", ID = 11 });
            cn.Add(new clsInfo() { Name = "L8n", Nameen = "Размер", ID = 12 });
            cn.Add(new clsInfo() { Name = "L9n", Nameen = "Өмсгөл", ID = 13 });
            cn.Add(new clsInfo() { Name = "Cdate", Nameen = "Үүсгэсэн огноо", ID = 14 });
            cn.Add(new clsInfo() { Name = "Cuser", Nameen = "Үүсгэсэн хэрэглэгч", ID = 15 });
            cn.Add(new clsInfo() { Name = "Zzbname", Nameen = "ЗЗБ", ID = 16 });
            cn.Add(new clsInfo() { Name = "Designername", Nameen = "Дизайнер", ID = 17 });
            cn.Add(new clsInfo() { Name = "Brend", Nameen = "Бренд", ID = 18 });
            cn.Add(new clsInfo() { Name = "Collection", Nameen = "Коллекц", ID = 19 });
            cn.Add(new clsInfo() { Name = "Utasno", Nameen = "Утасны номер", ID = 20 });
            cn.Add(new clsInfo() { Name = "Holio", Nameen = "Холио", ID = 21 });
            cn.Add(new clsInfo() { Name = "Gage", Nameen = "Гейч", ID = 22 });
            cn.Add(new clsInfo() { Name = "Tuslahmat", Nameen = "Туслах материал", ID = 23 });
            cn.Add(new clsInfo() { Name = "Suljees", Nameen = "Сүлжээс", ID = 24 });
            cn.Add(new clsInfo() { Name = "Zahsuljees", Nameen = "Захны сүлжээс", ID = 25 });
            cn.Add(new clsInfo() { Name = "Ihbiemanjet", Nameen = "Их бие манжет", ID = 26 });
            cn.Add(new clsInfo() { Name = "Hantsuumanjet", Nameen = "Ханцуй манжет", ID = 27 });
            cn.Add(new clsInfo() { Name = "Zahjin", Nameen = "Захиалагч жин", ID = 28 });
            cn.Add(new clsInfo() { Name = "Urt", Nameen = "Урт", ID = 29 });
            cn.Add(new clsInfo() { Name = "Murniiturul", Nameen = "Мөрний төрөл", ID = 30 });
            cn.Add(new clsInfo() { Name = "Engerturul", Nameen = "Энгэрийн төрөл", ID = 31 });
            cn.Add(new clsInfo() { Name = "Zahturul", Nameen = "Захны төрөл", ID = 32 });
            cn.Add(new clsInfo() { Name = "Hantsuuturul", Nameen = "Ханцуйны төрөл", ID = 33 });
            cn.Add(new clsInfo() { Name = "Halaasniiturul", Nameen = "Халаасны төрөл", ID = 34 });
            cn.Add(new clsInfo() { Name = "Zahialgach", Nameen = "Захиалагч", ID = 35 });
            cn.Add(new clsInfo() { Name = "Cusnum", Nameen = "Харилцагчын дугаар", ID = 36 });
            cn.Add(new clsInfo() { Name = "Printnum", Nameen = "Принт дугаар", ID = 37 });
            cn.Add(new clsInfo() { Name = "Omnohdugaar", Nameen = "өмнөх дугаар", ID = 38 });
            cn.Add(new clsInfo() { Name = "Goyodugaar", Nameen = "Гоёо дугаар", ID = 39 });
            cn.Add(new clsInfo() { Name = "Onlinenum", Nameen = "Онлайн дугаар", ID = 40 });
            cn.Add(new clsInfo() { Name = "Code26", Nameen = "Код26", ID = 41 });
            cn.Add(new clsInfo() { Name = "Code22", Nameen = "Код 22", ID = 42 });
            cn.Add(new clsInfo() { Name = "Barcode", Nameen = "Бар код", ID = 43 });
            cn.Add(new clsInfo() { Name = "Ehzjin", Nameen = "Эх загварын жин", ID = 44 });
            cn.Add(new clsInfo() { Name = "Massjin", Nameen = "Масс жин", ID = 45 });
            cn.Add(new clsInfo() { Name = "Tuuhiijin", Nameen = "Түүхий жин", ID = 46 });
            cn.Add(new clsInfo() { Name = "Bohirjin", Nameen = "Бохир жин", ID = 47 });
            cn.Add(new clsInfo() { Name = "Hodzartsuulalttime", Nameen = "Хөдөлмөр зарцуулалтын цаг", ID = 48 });
            cn.Add(new clsInfo() { Name = "OymolMatNum", Nameen = "Оёмолын материалын дугаар", ID = 49 });  

            foreach (clsInfo n in cn)
            {
                DevExpress.XtraGrid.Columns.GridColumn myCol = new DevExpress.XtraGrid.Columns.GridColumn() { Caption = n.Nameen, Visible = true, FieldName = n.Name, VisibleIndex = n.ID };
                if (myCol.VisibleIndex == 2)
                {
                    myCol.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "Itemcode", "{0}") });
            
                }
                gridView1.Columns.Add(myCol);
                gridView1.BestFitColumns();
            }
        }

        private void LoadInfo()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                using (var db = new Gobibase())
                {
                    lst_info.Clear();
                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand("SELECT [id],[code],[name],[nameen],[l_name],[angilal],[sortid],[other1],[other2] FROM [t_info] " + CFunctions.StrWhere() , conn);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            clsInfo g = new clsInfo();
                            g.ID = dr.IsDBNull(0) ? 0 : Core.ToInt(dr.GetInt32(0));
                            g.Code = dr.IsDBNull(1) ? "" : Core.ToStr(dr.GetString(1));
                            g.Name = dr.IsDBNull(2) ? "" : Core.ToStr(dr.GetString(2));
                            g.Nameen = dr.IsDBNull(3) ? "" : Core.ToStr(dr.GetString(3));
                            g.L_name = dr.IsDBNull(4) ? "" : Core.ToStr(dr.GetString(4));
                            g.Angilal = dr.IsDBNull(5) ? "" : Core.ToStr(dr.GetString(5));
                            g.Sortid = dr.IsDBNull(6) ? 0 : Core.ToInt(dr.GetInt32(6));
                            g.Other1 = dr.IsDBNull(7) ? "" : Core.ToStr(dr.GetString(7));
                            g.Other2 = dr.IsDBNull(8) ? "" : Core.ToStr(dr.GetString(8));
                            lst_info.Add(g);
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

        

        private void LookUpClear_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            DevExpress.XtraEditors.LookUpEdit lup = sender as DevExpress.XtraEditors.LookUpEdit;

            if (e.KeyCode != Keys.Delete && e.KeyCode != Keys.Back)
            {
                base.OnPreviewKeyDown(e);
            }
            else
            {
                lup.EditValue = null;
            }
        }

        private void LoadData(object _p1, object _p2)
        {
            try
            {
                using (var db = new Gobibase())
                {
                    lst_main.Clear();

                    string p1 = _p1 != null ? _p1.ToString() : "";
                    string p2 = _p2 != null ? _p2.ToString() : "";
                    
                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand(@"Select m.id, m.itemcode, m.itemname, m.itemnameen, m.lev1, m.lev2, m.lev3, m.lev4, m.lev5, m.color1 as mcolor1, m.lev6, 
m.lev7, m.color2 as mcolor2, m.lev8, m.lev9, m.createddate, u.uname, m.l1n, m.l2n, m.l3n, m.l6n, m.l8n, 
m.l9n, m.zagvartype, m.itemcode_eh, m.createduser,
d.version, d.versiondate, d.brend, d.collection, d.utasno, d.holio, d.gage, d.tuslahmat, d.suljees, d.zahsuljees, d.ihbiemanjet, d.hantsuumanjet, d.zahjin, 
z.uname, d.urt, d.murniiturul, d.engerturul, d.zahturul, d.hantsuuturul, d.halaasniiturul, d.zahialgach, d.cusnum, d.printnum, z1.uname, d.omnohdugaar, d.gobidugaar, d.goyodugaar, d.onlinenum, d.code26, d.code22, d.barcode, 
d.color1, d.color2, d.color3, d.color4, d.color5, d.color6, d.color7, d.color8, d.color9, d.color10, d.colorOther, d.colorhuvi1, d.colorhuvi2, d.colorhuvi3, d.colorhuvi4, d.colorhuvi5, d.colorhuvi6, d.colorhuvi7, 
d.colorhuvi8, d.colorhuvi9, d.colorhuvi10, d.colorhuviOther,
 d.createdU, d.editedU, d.editedDate,u1.uname, u2.uname, d.ehzjin, d.massjin, d.tuuhiijin, d.bohirjin, d.hodzartsuulalttime, d.oymolMatNum
from t_main as m inner join t_details as d on m.itemcode = d.itemcode
and m.zagvartype = N'Эх загвар'
inner join t_user as u on u.id= m.createduser
left outer join t_user as u1 on u1.id= d.createdU
left outer join t_user as u2 on u2.id= d.editedU
left outer join t_user as z on z.id= d.zzb
left outer join t_user as z1 on z1.id= d.designer
where (@p1 = '' or m.lev1 = @p1) and (@p2 = '' or lev3 = @p2) " + CFunctions.StrMainWhere(true) +
"order by m.itemcode", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@p1", SqlDbType.NVarChar, 50).Value = p1;
                    cmd.Parameters.Add("@p2", SqlDbType.NVarChar, 50).Value = p2;
                    cmd.Parameters.Add("@p3", SqlDbType.NVarChar, 50).Value = p2;

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

                            lst_main.Add(g);
                        }
                    }

                    conn.Close();
                    dr.Close();

                    gridControl1.DataSource = lst_main.ToList();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Aлдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            LoadData(luplbl1.EditValue, luplbl2.EditValue);
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

        private void repoedit_Click(object sender, EventArgs e)
        {
            string num = gridView1.GetFocusedRowCellValue("Itemcode").ToString();
            if (num.Length > 0)
            {
                frmCreate f = new frmCreate(LoadDataEh(num));
                f.ShowDialog();
                LoadData(luplbl1.EditValue, luplbl2.EditValue);
            }
        }

        private void btnExl_Click(object sender, EventArgs e)
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

    }
}
