using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CreateItemCode.Models;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;


namespace CreateItemCode
{
    public partial class frmCreate : Form
    {
        clsListItemMain Itemcode = new clsListItemMain();
        List<clsInfo> lst_info = new List<clsInfo>();
        List<clsInfo> lst_info_temp = new List<clsInfo>();
        List<clsDetails> lst_version = new List<clsDetails>();
        List<clsProgram> lst_pro_version = new List<clsProgram>();
        List<clsFile> lst_file_version = new List<clsFile>();
        List<clsUser> lst_user = new List<clsUser>();
        string oldcode = "";
        int m_id = 0;

        string uploadPropath = "", uploadHavpath = "";

        public frmCreate(clsListItemMain itm)
        {
            InitializeComponent();
            Itemcode = itm;
            dtVersion.DateTime = DateTime.Now;
            uploadPropath = ConfigurationManager.AppSettings["uploadpropath"];
            uploadHavpath = ConfigurationManager.AppSettings["uploadhavpath"];


            oldcode = itm != null ? itm.Itemcode : "";
            m_id = itm != null ? itm.ID : 0;


            if (itm != null && itm.Zagvartype == "Масс")
            {
                LoadCurrentItem(itm.Itemcode_eh);
            }
        }

        private void frmCreate_Load(object sender, EventArgs e)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));

            this.WindowState = FormWindowState.Maximized;
            LoadInfo();

            #region lookup datasource set


            for (int i = 1; i <= 9; i++)
            {
                Control[] tbxs = this.Controls.Find(("luplbl" + i), true);

                if (tbxs != null && tbxs.Length > 0)
                {
                    DevExpress.XtraEditors.LookUpEdit lup = tbxs[0] as DevExpress.XtraEditors.LookUpEdit;
                    lup.PreviewKeyDown -= new PreviewKeyDownEventHandler(this.LookUpClear_PreviewKeyDown);
                    lup.EditValueChanged -= new EventHandler(this.LookUp_EditValueChanged);
                }
            }

            this.luplbl3.EditValueChanged -= new EventHandler(this.luplbl3_EditValueChanged);
            this.luplbl6.EditValueChanged -= new EventHandler(this.luplbl6_EditValueChanged);
            this.txtlbl4.EditValueChanged -= new EventHandler(this.txtlbl4_EditValueChanged);
            txtMainColor2.EditValueChanged -= new EventHandler(this.txtMainColor2_EditValueChanged);

            for(int i= 1; i<=11; i++)
            {
                Control[] tbxs = this.Controls.Find(("lupc" + i), true);

                if (tbxs != null && tbxs.Length > 0)
                {
                    DevExpress.XtraEditors.LookUpEdit lup = tbxs[0] as DevExpress.XtraEditors.LookUpEdit;
                    lup.Properties.DataSource = lst_info.Where(s => s.L_name == "Level5").OrderBy(o => o.Sortid).ToList();
                    lup.Properties.DisplayMember = "CodeName";
                    lup.Properties.ValueMember = "Code";
                }
            }

            lupbrend.Properties.DataSource = lst_info.Where(s => s.L_name == "brend").OrderBy(o => o.Sortid).ToList();
            lupbrend.Properties.DisplayMember = "Name";
            lupbrend.Properties.ValueMember = "Name";
            lupbrend.PreviewKeyDown += new PreviewKeyDownEventHandler(this.LookUpClear_PreviewKeyDown);

            lupCollection.Properties.DataSource = lst_info.Where(s => s.L_name == "kollects").OrderBy(o => o.Sortid).ToList();
            lupCollection.Properties.DisplayMember = "Name";
            lupCollection.Properties.ValueMember = "Name";
            lupCollection.PreviewKeyDown += new PreviewKeyDownEventHandler(this.LookUpClear_PreviewKeyDown);

            lupUtasNo.Properties.DataSource = lst_info.Where(s => s.L_name == "utasno").OrderBy(o => o.Sortid).ToList();
            lupUtasNo.Properties.DisplayMember = "Name";
            lupUtasNo.Properties.ValueMember = "Name";
            lupUtasNo.PreviewKeyDown += new PreviewKeyDownEventHandler(this.LookUpClear_PreviewKeyDown);

            lupHolio.Properties.DataSource = lst_info.Where(s => s.L_name == "materialholio").OrderBy(o => o.Sortid).ToList();
            lupHolio.Properties.DisplayMember = "Name";
            lupHolio.Properties.ValueMember = "Name";
            lupHolio.PreviewKeyDown += new PreviewKeyDownEventHandler(this.LookUpClear_PreviewKeyDown);

            lupGage.Properties.DataSource = lst_info.Where(s => s.L_name == "geich").OrderBy(o => o.Sortid).ToList();
            lupGage.Properties.DisplayMember = "Name";
            lupGage.Properties.ValueMember = "Name";
            lupGage.PreviewKeyDown += new PreviewKeyDownEventHandler(this.LookUpClear_PreviewKeyDown);

            lupTuslahmat.Properties.DataSource = lst_info.Where(s => s.L_name == "erunhiituslahmaterial").OrderBy(o => o.Sortid).ToList();
            lupTuslahmat.Properties.DisplayMember = "Name";
            lupTuslahmat.Properties.ValueMember = "Name";
            lupTuslahmat.PreviewKeyDown += new PreviewKeyDownEventHandler(this.LookUpClear_PreviewKeyDown);

            lupSuljees.Properties.DataSource = lst_info.Where(s => s.L_name == "suljeeniiturul").OrderBy(o => o.Sortid).ToList();
            lupSuljees.Properties.DisplayMember = "Name";
            lupSuljees.Properties.ValueMember = "Name";
            lupSuljees.PreviewKeyDown += new PreviewKeyDownEventHandler(this.LookUpClear_PreviewKeyDown);

            lupZahSuljTurul.Properties.DataSource = lst_info.Where(s => s.L_name == "suljeeniiturul").OrderBy(o => o.Sortid).ToList();
            lupZahSuljTurul.Properties.DisplayMember = "Name";
            lupZahSuljTurul.Properties.ValueMember = "Name";
            lupZahSuljTurul.PreviewKeyDown += new PreviewKeyDownEventHandler(this.LookUpClear_PreviewKeyDown);

            lupIhbieManSulj.Properties.DataSource = lst_info.Where(s => s.L_name == "suljeeniiturul").OrderBy(o => o.Sortid).ToList();
            lupIhbieManSulj.Properties.DisplayMember = "Name";
            lupIhbieManSulj.Properties.ValueMember = "Name";
            lupIhbieManSulj.PreviewKeyDown += new PreviewKeyDownEventHandler(this.LookUpClear_PreviewKeyDown);

            lupHantsuManSulj.Properties.DataSource = lst_info.Where(s => s.L_name == "suljeeniiturul").OrderBy(o => o.Sortid).ToList();
            lupHantsuManSulj.Properties.DisplayMember = "Name";
            lupHantsuManSulj.Properties.ValueMember = "Name";
            lupHantsuManSulj.PreviewKeyDown += new PreviewKeyDownEventHandler(this.LookUpClear_PreviewKeyDown);

            lupUrt.Properties.DataSource = lst_info.Where(s => s.L_name == "erunhiiurt").OrderBy(o => o.Sortid).ToList();
            lupUrt.Properties.DisplayMember = "Name";
            lupUrt.Properties.ValueMember = "Name";
            lupUrt.PreviewKeyDown += new PreviewKeyDownEventHandler(this.LookUpClear_PreviewKeyDown);

            lupMurniiTurul.Properties.DataSource = lst_info.Where(s => s.L_name == "murniiturul").OrderBy(o => o.Sortid).ToList();
            lupMurniiTurul.Properties.DisplayMember = "Name";
            lupMurniiTurul.Properties.ValueMember = "Name";
            lupMurniiTurul.PreviewKeyDown += new PreviewKeyDownEventHandler(this.LookUpClear_PreviewKeyDown);

            lupEngerTurul.Properties.DataSource = lst_info.Where(s => s.L_name == "engeriinturul").OrderBy(o => o.Sortid).ToList();
            lupEngerTurul.Properties.DisplayMember = "Name";
            lupEngerTurul.Properties.ValueMember = "Name";
            lupEngerTurul.PreviewKeyDown += new PreviewKeyDownEventHandler(this.LookUpClear_PreviewKeyDown);

            lupZahturul.Properties.DataSource = lst_info.Where(s => s.L_name == "zahniiturul").OrderBy(o => o.Sortid).ToList();
            lupZahturul.Properties.DisplayMember = "Name";
            lupZahturul.Properties.ValueMember = "Name";
            lupZahturul.PreviewKeyDown += new PreviewKeyDownEventHandler(this.LookUpClear_PreviewKeyDown);

            lupHantsuuTurul.Properties.DataSource = lst_info.Where(s => s.L_name == "hanitsuuniiturul").OrderBy(o => o.Sortid).ToList();
            lupHantsuuTurul.Properties.DisplayMember = "Name";
            lupHantsuuTurul.Properties.ValueMember = "Name";
            lupHantsuuTurul.PreviewKeyDown += new PreviewKeyDownEventHandler(this.LookUpClear_PreviewKeyDown);

            lupHalaasTurul.Properties.DataSource = lst_info.Where(s => s.L_name == "halaasniiturul").OrderBy(o => o.Sortid).ToList();
            lupHalaasTurul.Properties.DisplayMember = "Name";
            lupHalaasTurul.Properties.ValueMember = "Name";
            lupHalaasTurul.PreviewKeyDown += new PreviewKeyDownEventHandler(this.LookUpClear_PreviewKeyDown);

            lupDesigner.Properties.DataSource = lst_user.Where(s => s.Workertype == "designer").ToList();
            lupDesigner.Properties.ValueMember = "ID";
            lupDesigner.Properties.DisplayMember = "Uname";
            lupDesigner.PreviewKeyDown += new PreviewKeyDownEventHandler(this.LookUpClear_PreviewKeyDown);

            lupZZB.Properties.DataSource = lst_user.Where(s => s.Workertype == "zzb").ToList();
            lupZZB.Properties.ValueMember = "ID";
            lupZZB.Properties.DisplayMember = "Uname";
            lupZZB.PreviewKeyDown += new PreviewKeyDownEventHandler(this.LookUpClear_PreviewKeyDown);

            lupProgramist.Properties.DataSource = lst_user.Where(s => s.Workertype == "program").ToList();
            lupProgramist.Properties.ValueMember = "ID";
            lupProgramist.Properties.DisplayMember = "Uname";
            lupProgramist.PreviewKeyDown += new PreviewKeyDownEventHandler(this.LookUpClear_PreviewKeyDown);

            lupCarTurul.Properties.DataSource = lst_info.Where(s => s.L_name == "cartype").ToList();
            lupCarTurul.Properties.ValueMember = "ID";
            lupCarTurul.Properties.DisplayMember = "Name";

            lupCarTurul.PreviewKeyDown += new PreviewKeyDownEventHandler(this.LookUpClear_PreviewKeyDown);
            lupCarMark.PreviewKeyDown += new PreviewKeyDownEventHandler(this.LookUpClear_PreviewKeyDown);

            #endregion

            txtCreatedUser.Text = lst_user.Where(s => s.ID == frmMain.UserID).First().Uname;
            txtSection.Text = lst_user.Where(s => s.ID == frmMain.UserID).First().Heltes;

            // set value
            if (Itemcode != null)
            {
                txtItemCode.Text = Itemcode.Itemcode;
                txtname.Text = Itemcode.Itemname;
                txtnameEn.Text = Itemcode.Itemnameen;
                luplbl1.EditValue = Itemcode.Lev1;
                luplbl2.EditValue = Itemcode.Lev2;
                
                txtNameCode.EditValue = Itemcode.Lev3;
                luplbl3.EditValue = Itemcode.L3n;
                
                txtlbl4.Text = Itemcode.Itemcode.Substring(4, 4);
                luplbl5.EditValue = Itemcode.Color1;
                luplbl6.EditValue = Itemcode.Lev6;
                luplbl7.EditValue = Itemcode.Color2;
                luplbl8.EditValue = Itemcode.Lev8;
                luplbl9.EditValue = Itemcode.Lev9;

                txtCreatedUser.Text = Itemcode.Cuser;
                txtSection.Text = lst_user.Where(s => s.ID == Itemcode.Cuserid).First().Heltes;
                txtCreatedDate.Text = Itemcode.Cdate.ToString();

                LoadComment(Itemcode.Itemcode);
                LoadVersion(Itemcode.Itemcode);
                LoadProVersion(Itemcode.Itemcode);
                LoadFileVersion(Itemcode.Itemcode);

                if (Core.ToInt(txtMVersion.Text) > 0) gcFile.DataSource = lst_file_version.Where(s => s.Mversion == Core.ToInt(txtMVersion.Text)).ToList();


                EnableChangeInControl(false);

                if (oldcode.Contains("#"))
                {
                    layoutControlItem7.Enabled = true;
                    layoutControlItem8.Enabled = true;
                    layoutControlItem9.Enabled = true;
                    layoutControlItem10.Enabled = true;
                    layoutControlItem11.Enabled = true;
                    layoutControlItem12.Enabled = true;
                    layoutControlItem13.Enabled = true;
                    layoutControlItem14.Enabled = true;
                    layoutControlItem17.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                }

                gvVersion.FocusedRowHandle = gvVersion.DataRowCount - 1;
            }

            for (int i = 1; i <= 9; i++)
            {
                Control[] tbxs = this.Controls.Find(("luplbl" + i), true);

                if (tbxs != null && tbxs.Length > 0)
                {
                    DevExpress.XtraEditors.LookUpEdit lup = tbxs[0] as DevExpress.XtraEditors.LookUpEdit;
                    lup.PreviewKeyDown += new PreviewKeyDownEventHandler(this.LookUpClear_PreviewKeyDown);
                    lup.EditValueChanged += new EventHandler(this.LookUp_EditValueChanged);
                }
            }
            this.txtlbl4.EditValueChanged += new EventHandler(this.txtlbl4_EditValueChanged);
            this.luplbl3.EditValueChanged += new EventHandler(this.luplbl3_EditValueChanged);
            this.luplbl6.EditValueChanged += new EventHandler(this.luplbl6_EditValueChanged);
            txtMainColor2.EditValueChanged += new EventHandler(this.txtMainColor2_EditValueChanged);
           
        }

        private void EnableChangeInControl(bool _type)
        {
            layoutControlItem1.Enabled = _type;
            layoutControlItem2.Enabled = _type;
            layoutControlItem3.Enabled = _type;
            layoutControlItem4.Enabled = _type;

            layoutControlItem5.Enabled = _type;
            layoutControlItem6.Enabled = _type;
            layoutControlItem7.Enabled = _type;
            layoutControlItem8.Enabled = _type;
            layoutControlItem9.Enabled = _type;

            layoutControlItem10.Enabled = _type;
            layoutControlItem11.Enabled = _type;
            layoutControlItem12.Enabled = _type;
            layoutControlItem13.Enabled = _type;
            layoutControlItem14.Enabled = _type;


            layoutControlItem17.Visibility = _type == false ? DevExpress.XtraLayout.Utils.LayoutVisibility.Never : DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

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
                    SqlCommand cmd = new SqlCommand("SELECT [id],[code],[name],[nameen],[l_name],[angilal],[sortid],[other1],[other2] FROM [t_info] " + CFunctions.StrWhere(), conn);
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

                    luplbl1.Properties.DataSource = lst_info.Where(s => s.L_name == "Level1").OrderBy(o => o.Sortid).ToList();
                    luplbl1.Properties.DisplayMember = "CodeName";
                    luplbl1.Properties.ValueMember = "Code";

                    lst_user.Clear();

                    conn = new SqlConnection(db.Connection.ConnectionString);
                    cmd = new SqlCommand("SELECT [id],[loginName],[uname],[pass],[heltes],[position],[createdate], role, workertype FROM [t_user] where isactive = 1", conn);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    dr = cmd.ExecuteReader();

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

        private void LoadComment(string _code)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                using (var db = new Gobibase())
                {
                    List<clsInfo> lst_t = new List<clsInfo>();

                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand("SELECT c.[id],[comment],[cdate],u.uname FROM [t_comment] as c inner join t_user as u on u.id =c.cuser  where itemcode = '" + _code + "' order by [cdate] desc", conn);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            clsInfo g = new clsInfo();
                            g.ID = dr.IsDBNull(0) ? 0 : Core.ToInt(dr.GetInt32(0));
                            g.Comment = dr.IsDBNull(1) ? "" : Core.ToStr(dr.GetString(1));
                            g.Cdate = dr.IsDBNull(2) ? DateTime.Now : Core.ToDateTime(dr.GetDateTime(2));
                            g.Name = dr.IsDBNull(3) ? "" : Core.ToStr(dr.GetString(3));
                            lst_t.Add(g);
                        }
                    }

                    conn.Close();
                    dr.Close();
                    Cursor.Current = Cursors.Default;

                    gcComment.DataSource = lst_t.ToList();

                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Aлдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCurrentItem(string _code)
        {
            try
            {
                using (var db = new Gobibase())
                {
                    clsListItemMain g = new clsListItemMain();

                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand(@"SELECT m.[id],[itemcode],[itemname],[itemnameen],[lev1],[lev2],[lev3],[lev4],[lev5],[lev6],[lev7],[lev8],[lev9],[createddate],u.uname, color1, 
			color2,[l1n],[l2n],[l3n],[l6n],[l8n],[l9n],[zagvartype],[itemcode_eh],u.id from t_main as m inner join t_user as u on u.id = m.[createduser] where [itemcode] = @p1 and [zagvartype] = N'Эх загвар'", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@p1", SqlDbType.NVarChar, 50).Value = _code;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Itemcode = new clsListItemMain();
                            Itemcode.ID = dr.IsDBNull(0) ? 0 : Core.ToInt(dr.GetInt32(0));
                            Itemcode.Itemcode = dr.IsDBNull(1) ? "" : Core.ToStr(dr.GetString(1));
                            Itemcode.Itemname = dr.IsDBNull(2) ? "" : Core.ToStr(dr.GetString(2));
                            Itemcode.Itemnameen = dr.IsDBNull(3) ? "" : Core.ToStr(dr.GetString(3));
                            Itemcode.Lev1 = dr.IsDBNull(4) ? "" : Core.ToStr(dr.GetString(4));
                            Itemcode.Lev2 = dr.IsDBNull(5) ? "" : Core.ToStr(dr.GetString(5));
                            Itemcode.Lev3 = dr.IsDBNull(6) ? "" : Core.ToStr(dr.GetString(6));
                            Itemcode.Lev4 = dr.IsDBNull(7) ? "" : Core.ToStr(dr.GetString(7));
                            Itemcode.Lev5 = dr.IsDBNull(8) ? "" : Core.ToStr(dr.GetString(8));
                            Itemcode.Lev6 = dr.IsDBNull(9) ? "" : Core.ToStr(dr.GetString(9));
                            Itemcode.Lev7 = dr.IsDBNull(10) ? "" : Core.ToStr(dr.GetString(10));
                            Itemcode.Lev8 = dr.IsDBNull(11) ? "" : Core.ToStr(dr.GetString(11));
                            Itemcode.Lev9 = dr.IsDBNull(12) ? "" : Core.ToStr(dr.GetString(12));
                            Itemcode.Cdate = dr.IsDBNull(13) ? DateTime.Now : Core.ToDateTime(dr.GetDateTime(13));
                            Itemcode.Cuser = dr.IsDBNull(14) ? "" : Core.ToStr(dr.GetString(14));
                            Itemcode.Color1 = dr.IsDBNull(15) ? "" : Core.ToStr(dr.GetString(15));
                            Itemcode.Color2 = dr.IsDBNull(16) ? "" : Core.ToStr(dr.GetString(16));
                            Itemcode.L1n = dr.IsDBNull(17) ? "" : Core.ToStr(dr.GetString(17));
                            Itemcode.L2n = dr.IsDBNull(18) ? "" : Core.ToStr(dr.GetString(18));
                            Itemcode.L3n = dr.IsDBNull(19) ? "" : Core.ToStr(dr.GetString(19));
                            Itemcode.L6n = dr.IsDBNull(20) ? "" : Core.ToStr(dr.GetString(20));
                            Itemcode.L8n = dr.IsDBNull(21) ? "" : Core.ToStr(dr.GetString(21));
                            Itemcode.L9n = dr.IsDBNull(22) ? "" : Core.ToStr(dr.GetString(22));
                            Itemcode.Zagvartype = dr.IsDBNull(23) ? "" : Core.ToStr(dr.GetString(23));
                            Itemcode.Itemcode_eh = dr.IsDBNull(24) ? "" : Core.ToStr(dr.GetString(24));
                            Itemcode.Cuserid = dr.IsDBNull(25) ? 0 : Core.ToInt(dr.GetInt32(25));

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
        }

        private void LoadVersion(string _code)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                using (var db = new Gobibase())
                {

                    lst_version.Clear();

                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand(@"SELECT  d.[id],[version],[versiondate],[brend],[collection],[utasno]
,[holio],[gage],[tuslahmat],[suljees],[zahsuljees],[ihbiemanjet],[hantsuumanjet]
,[zahjin],[zzb],[urt],[murniiturul],[engerturul],[zahturul],[hantsuuturul],[halaasniiturul]
,[zahialgach],[cusnum],[printnum],[designer],[omnohdugaar],[gobidugaar],[goyodugaar]
,[onlinenum],[code26],[code22],[barcode],d.[color1],d.[color2],[color3],[color4],[color5]
,[color6],[color7],[color8],[color9],[color10],[colorOther],[colorhuvi1],[colorhuvi2],[colorhuvi3]
,[colorhuvi4],[colorhuvi5],[colorhuvi6],[colorhuvi7],[colorhuvi8],[colorhuvi9],[colorhuvi10]
,[colorhuviOther],[createdU],[editedU],[editedDate], u.uname, 
e.uname,d.[itemcode],[ehzjin],[massjin],[tuuhiijin],[bohirjin],[hodzartsuulalttime],[oymolMatNum],[zagvartailbar],[tsuvralnum] FROM [t_details] as d 
inner join t_user as u on u.id = d.createdU
inner join dbo.t_main AS m ON d.itemcode = m.itemcode AND m.zagvartype = N'Эх загвар'
left outer join t_user as e on e.id = d.editedU where d.itemcode = @p1 order by [version]", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@p1", SqlDbType.NVarChar, 50).Value = _code;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            clsDetails g = new clsDetails();
                            g.ID = dr.IsDBNull(0) ? 0 : Core.ToInt(dr.GetInt32(0)); 
                            g.Version = dr.IsDBNull(1) ? 0 : Core.ToInt(dr.GetInt32(1));
                            g.Versiondate = dr.IsDBNull(2) ? DateTime.Now : Core.ToDateTime(dr.GetDateTime(2));
                            g.Brend = dr.IsDBNull(3) ? "" : Core.ToStr(dr.GetString(3));
                            g.Collection = dr.IsDBNull(4) ? "" : Core.ToStr(dr.GetString(4));
                            g.Utasno = dr.IsDBNull(5) ? "" : Core.ToStr(dr.GetString(5));
                            g.Holio = dr.IsDBNull(6) ? "" : Core.ToStr(dr.GetString(6));
                            g.Gage = dr.IsDBNull(7) ? "" : Core.ToStr(dr.GetString(7));
                            g.Tuslahmat = dr.IsDBNull(8) ? "" : Core.ToStr(dr.GetString(8));
                            g.Suljees = dr.IsDBNull(9) ? "" : Core.ToStr(dr.GetString(9));
                            g.Zahsuljees = dr.IsDBNull(10) ? "" : Core.ToStr(dr.GetString(10));
                            g.Ihbiemanjet = dr.IsDBNull(11) ? "" : Core.ToStr(dr.GetString(11));
                            g.Hantsuumanjet = dr.IsDBNull(12) ? "" : Core.ToStr(dr.GetString(12));
                            g.Zahjin = dr.IsDBNull(13) ? "" : Core.ToStr(dr.GetString(13));
                            g.Zzb = dr.IsDBNull(14) ? 0 : Core.ToInt(dr.GetInt32(14));
                            g.Urt = dr.IsDBNull(15) ? "" : Core.ToStr(dr.GetString(15));
                            g.Murniiturul = dr.IsDBNull(16) ? "" : Core.ToStr(dr.GetString(16));
                            g.Engerturul = dr.IsDBNull(17) ? "" : Core.ToStr(dr.GetString(17));
                            g.Zahturul = dr.IsDBNull(18) ? "" : Core.ToStr(dr.GetString(18));
                            g.Hantsuuturul = dr.IsDBNull(19) ? "" : Core.ToStr(dr.GetString(19));
                            g.Halaasniiturul = dr.IsDBNull(20) ? "" : Core.ToStr(dr.GetString(20));
                            g.Zahialgach = dr.IsDBNull(21) ? "" : Core.ToStr(dr.GetString(21));
                            g.Cusnum = dr.IsDBNull(22) ? "" : Core.ToStr(dr.GetString(22));
                            g.Printnum = dr.IsDBNull(23) ? "" : Core.ToStr(dr.GetString(23));
                            g.Designer = dr.IsDBNull(24) ? 0 : Core.ToInt(dr.GetInt32(24));
                            g.Omnohdugaar = dr.IsDBNull(25) ? "" : Core.ToStr(dr.GetString(25));
                            g.Gobidugaar = dr.IsDBNull(26) ? "" : Core.ToStr(dr.GetString(26));
                            g.Goyodugaar = dr.IsDBNull(27) ? "" : Core.ToStr(dr.GetString(27));
                            g.Onlinenum = dr.IsDBNull(28) ? "" : Core.ToStr(dr.GetString(28));
                            g.Code26 = dr.IsDBNull(29) ? "" : Core.ToStr(dr.GetString(29));
                            g.Code22 = dr.IsDBNull(30) ? "" : Core.ToStr(dr.GetString(30));
                            g.Barcode = dr.IsDBNull(31) ? "" : Core.ToStr(dr.GetString(31));
                            g.Color1 = dr.IsDBNull(32) ? "" : Core.ToStr(dr.GetString(32));
                            g.Color2 = dr.IsDBNull(33) ? "" : Core.ToStr(dr.GetString(33));
                            g.Color3 = dr.IsDBNull(34) ? "" : Core.ToStr(dr.GetString(34));
                            g.Color4 = dr.IsDBNull(35) ? "" : Core.ToStr(dr.GetString(35));
                            g.Color5 = dr.IsDBNull(36) ? "" : Core.ToStr(dr.GetString(36));
                            g.Color6 = dr.IsDBNull(37) ? "" : Core.ToStr(dr.GetString(37));
                            g.Color7 = dr.IsDBNull(38) ? "" : Core.ToStr(dr.GetString(38));
                            g.Color8 = dr.IsDBNull(39) ? "" : Core.ToStr(dr.GetString(39));
                            g.Color9 = dr.IsDBNull(40) ? "" : Core.ToStr(dr.GetString(40));
                            g.Color10 = dr.IsDBNull(41) ? "" : Core.ToStr(dr.GetString(41));
                            g.ColorOther = dr.IsDBNull(42) ? "" : Core.ToStr(dr.GetString(42));
                            g.Colorhuvi1 =  dr.IsDBNull(43) ? "" : Core.ToStr(dr.GetString(43));
                            g.Colorhuvi2 = dr.IsDBNull(44) ? "" : Core.ToStr(dr.GetString(44));
                            g.Colorhuvi3 = dr.IsDBNull(45) ? "" : Core.ToStr(dr.GetString(45));
                            g.Colorhuvi4 = dr.IsDBNull(46) ? "" : Core.ToStr(dr.GetString(46));
                            g.Colorhuvi5 = dr.IsDBNull(47) ? "" : Core.ToStr(dr.GetString(47));
                            g.Colorhuvi6 = dr.IsDBNull(48) ? "" : Core.ToStr(dr.GetString(48));
                            g.Colorhuvi7 = dr.IsDBNull(49) ? "" : Core.ToStr(dr.GetString(49));
                            g.Colorhuvi8 = dr.IsDBNull(50) ? "" : Core.ToStr(dr.GetString(50));
                            g.Colorhuvi9 = dr.IsDBNull(51) ? "" : Core.ToStr(dr.GetString(51));
                            g.Colorhuvi10 = dr.IsDBNull(52) ? "" : Core.ToStr(dr.GetString(52));
                            g.ColorhuviOther = dr.IsDBNull(53) ? "" : Core.ToStr(dr.GetString(53).ToString());
                            g.CreatedU = dr.IsDBNull(54) ? 0 : Core.ToInt(dr.GetInt32(54));
                            g.EditedU = dr.IsDBNull(55) ? 0 : Core.ToInt(dr.GetInt32(55));
                            g.EditedDate = dr.IsDBNull(56) ? "" : Core.ToStr(dr.GetDateTime(56).ToString("yyyy/MM/dd HH:mm"));
                            g.CreatedUser = dr.IsDBNull(57) ? "" : Core.ToStr(dr.GetString(57));
                            g.EditedUser =  dr.IsDBNull(58) ? "" : Core.ToStr(dr.GetString(58));
                            g.Itemcode = dr.IsDBNull(59) ? "" : Core.ToStr(dr.GetString(59));
                            g.Ehzjin = dr.IsDBNull(60) ? "" : Core.ToStr(dr.GetString(60));
                            g.Massjin = dr.IsDBNull(61) ? "" : Core.ToStr(dr.GetString(61));
                            g.Tuuhiijin = dr.IsDBNull(62) ? "" : Core.ToStr(dr.GetString(62));
                            g.Bohirjin = dr.IsDBNull(63) ? "" : Core.ToStr(dr.GetString(63));
                            g.Hodzartsuulalttime = dr.IsDBNull(64) ? "" : Core.ToStr(dr.GetString(64));
                            g.OymolMatNum = dr.IsDBNull(65) ? "" : Core.ToStr(dr.GetString(65));
                            g.Zagvartailbar = dr.IsDBNull(66) ? "" : Core.ToStr(dr.GetString(66));
                            g.Tsuvralnum = dr.IsDBNull(67) ? "" : Core.ToStr(dr.GetString(67));

                            lst_version.Add(g);
                        }
                    }

                    conn.Close();
                    dr.Close();
                    Cursor.Current = Cursors.Default;

                    gcVersion.DataSource = lst_version.ToList();

                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Aлдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProVersion(string _code)
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
left outer join t_info as cm on cm.id = p.car_markid and cm.l_name='carmark' where p.itemcode = @p1", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@p1", SqlDbType.NVarChar, 50).Value = _code;
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

                    gcProVersion.DataSource = lst_pro_version.ToList();

                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Aлдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadFileVersion(string _code)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                using (var db = new Gobibase())
                {

                    lst_file_version.Clear();

                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand(@"SELECT p.[id],[filename],[version],[tuluv],p.[createddate]
,[enddate],[comment],[cuser],u.uname,[mVersion] FROM t_file as p 
inner join dbo.t_main AS m ON p.itemcode = m.itemcode AND m.zagvartype = N'Эх загвар'
left join t_user as u on u.id = p.[cuser] where p.itemcode = @p1", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@p1", SqlDbType.NVarChar, 50).Value = _code;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            clsFile g = new clsFile();
                            g.ID = dr.IsDBNull(0) ? 0 : Core.ToInt(dr.GetInt32(0));
                            g.Filename = dr.IsDBNull(1) ? "" : Core.ToStr(dr.GetString(1));
                            g.Version = dr.IsDBNull(2) ? 0 : Core.ToInt(dr.GetInt32(2));
                            g.Tuluv = dr.IsDBNull(3) ? "" : Core.ToStr(dr.GetString(3));
                            g.Createddate = dr.IsDBNull(4) ? "" : Core.ToStr(dr.GetString(4));
                            g.Enddate = dr.IsDBNull(5) ? "" : Core.ToStr(dr.GetString(5));
                            g.Comment = dr.IsDBNull(6) ? "" : Core.ToStr(dr.GetString(6));
                            g.Cuser = dr.IsDBNull(7) ? 0 : Core.ToInt(dr.GetInt32(7));
                            g.Uname = dr.IsDBNull(8) ? "" : Core.ToStr(dr.GetString(8));
                            g.Mversion = dr.IsDBNull(9) ? 0 : Core.ToInt(dr.GetInt32(9));
                            lst_file_version.Add(g);
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

        private string GetColorCode(string _cCode)
        {
            string retVal = "";

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                using (var db = new Gobibase())
                {
                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand("select colormain from t_colormatrix where colorsub like '%-" + _cCode + "-%'", conn);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            retVal = dr.IsDBNull(0) ? "" : Core.ToStr(dr.GetString(0));
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

            return retVal;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtItemCode.Text == null || txtItemCode.Text == "")
            {
                MessageBox.Show("21 оронтой код үүсгэхийн тулд Level-ээс сонгох шаардлагатай.!", " Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtItemCode.Text.Contains("#"))
            {
                MessageBox.Show("Мэдээлэл дутуу сонгогдсон байна. Мэдээлэлээ шалгана уу.!", " Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (CheckItemCode(txtItemCode.Text))
            {
                MessageBox.Show(txtItemCode.Text + " гэсэн ITEMCODE өмнө нь үүсгэсэн байна. Мэдээллээ шалгана уу.!", " Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtItemCode.Text.Length == 23 && MessageBox.Show(txtItemCode.Text + " гэсэн ITEMCODE үүсгэх үү?", " Анхааруулга", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    using (var db = new Gobibase())
                    {
                        string strQry = ""; 

                        SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                        SqlCommand cmd;

                        if(!oldcode.Contains("#"))
                        {
                            strQry = @"INSERT INTO [t_main] ([itemcode],[itemname],[itemnameen],[lev1],[lev2],[lev3],[lev4],[lev5],[lev6]
,[lev7],[lev8],[lev9],[createddate],[createduser],[color1],[color2],[l1n],[l2n],[l3n],[l6n],[l8n],[l9n],[zagvartype],[itemcode_eh]) VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13,@p14,@p15,@p16,@p17,@p18,@p19,@p21,@p22,@p23,N'Эх загвар',null)";
                            cmd = new SqlCommand(strQry, conn);
                        }
                        else
                        {
                            strQry = @"UPDATE [t_main] SET  [itemcode]=@p1,[itemname]=@p2,[itemnameen]=@p3,[lev1]=@p4,[lev2]=@p5,[lev3]=@p6
                                            ,[lev4]=@p7,[lev5]=@p8,[lev6]=@p9,[lev7]=@p10,[lev8]=@p11,[lev9]=@p12,[createddate]=@p13,[createduser]=@p14
                                            ,[color1]=@p15,[color2]=@p16,[l1n]=@p17,[l2n]=@p18,[l3n]=@p19,[l6n]=@p21,[l8n]=@p22,[l9n]=@p23 WHERE ID = @p24; 
                                        Update     t_main set itemcode_eh = @p1 where itemcode_eh = @p25;
                                        Update     t_details set itemcode = @p1 where itemcode = @p25;
                                        Update     t_file set itemcode = @p1 where itemcode = @p25;
                                        Update     t_programist set itemcode = @p1 where itemcode = @p25;
                                        Update     t_comment set itemcode = @p1 where itemcode = @p25;";
                            cmd = new SqlCommand(strQry, conn);
                            cmd.Parameters.Add("@p24", SqlDbType.Int).Value = m_id;
                            cmd.Parameters.Add("@p25", SqlDbType.NVarChar, 50).Value = oldcode;
                        }
                        
                         
                        cmd.Parameters.Add("@p1", SqlDbType.NVarChar, 50).Value = txtItemCode.Text;
                        cmd.Parameters.Add("@p2", SqlDbType.NVarChar, 100).Value = Core.ToStr(txtname.Text);
                        cmd.Parameters.Add("@p3", SqlDbType.NVarChar, 100).Value = Core.ToStr(txtnameEn.Text);
                        cmd.Parameters.Add("@p4", SqlDbType.NVarChar, 50).Value = Core.ToStr(luplbl1.EditValue);
                        cmd.Parameters.Add("@p5", SqlDbType.NVarChar, 50).Value = Core.ToStr(luplbl2.EditValue);
                        cmd.Parameters.Add("@p6", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtNameCode.EditValue);
                        cmd.Parameters.Add("@p7", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtlbl4.Text);
                        cmd.Parameters.Add("@p8", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtMainColor1.EditValue);
                        cmd.Parameters.Add("@p9", SqlDbType.NVarChar, 50).Value = Core.ToStr(luplbl6.EditValue);
                        cmd.Parameters.Add("@p10", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtMainColor2.EditValue);
                        cmd.Parameters.Add("@p11", SqlDbType.NVarChar, 50).Value = Core.ToStr(luplbl8.EditValue);
                        cmd.Parameters.Add("@p12", SqlDbType.NVarChar, 50).Value = Core.ToStr(luplbl9.EditValue);
                        cmd.Parameters.Add("@p13", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@p14", SqlDbType.Int).Value = frmMain.UserID;
                        cmd.Parameters.Add("@p15", SqlDbType.NVarChar, 50).Value = Core.ToStr(luplbl5.EditValue);
                        cmd.Parameters.Add("@p16", SqlDbType.NVarChar, 50).Value = Core.ToStr(luplbl7.EditValue);
                        cmd.Parameters.Add("@p17", SqlDbType.NVarChar, 80).Value = Core.ToStr(luplbl1.Text);
                        cmd.Parameters.Add("@p18", SqlDbType.NVarChar, 80).Value = Core.ToStr(luplbl2.Text);
                        cmd.Parameters.Add("@p19", SqlDbType.NVarChar, 80).Value = Core.ToStr(luplbl3.Text);
                        cmd.Parameters.Add("@p21", SqlDbType.NVarChar, 80).Value = Core.ToStr(luplbl6.Text);
                        cmd.Parameters.Add("@p22", SqlDbType.NVarChar, 80).Value = Core.ToStr(luplbl8.Text);
                        cmd.Parameters.Add("@p23", SqlDbType.NVarChar, 80).Value = Core.ToStr(luplbl9.Text);

                        cmd.CommandType = CommandType.Text;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        this.Tag = "ok";

                        Itemcode = new clsListItemMain();
                        Itemcode.Itemcode = txtItemCode.Text;

                        LoadComment(Itemcode.Itemcode);
                        LoadVersion(Itemcode.Itemcode);

                        EnableChangeInControl(false);

                        string _ver = GetLastVersionNum(Itemcode.Itemcode);
                        if (_ver == "1")
                            CreateZagvarVersion(_ver);


                    }
                }
                catch (Exception ex)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Aлдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private bool CheckItemCode(string _code)
        {
            bool retVale = false;

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                using (var db = new Gobibase())
                {
                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand("Select count(*) cnt from t_main where zagvartype = N'Эх загвар' and itemcode  = '" + _code + "'", conn);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            int cnt = dr.IsDBNull(0) ? 0 : Core.ToInt(dr.GetInt32(0));
                            retVale = cnt > 0 ? true : false;
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

            return retVale;
        }

        private string CreateNewCode()
        {
            string _code = "";

            if (Core.ToStr(luplbl1.EditValue) != "")
                _code = _code + luplbl1.EditValue.ToString();
            else 
                 _code = _code + "#";
            if (Core.ToStr(luplbl2.EditValue) != "")
                _code = _code + luplbl2.EditValue.ToString();
            else 
                 _code = _code + "#";
            if (Core.ToStr(txtNameCode.EditValue) != "")
                _code = _code + txtNameCode.EditValue.ToString();
            else 
                 _code = _code + "##";
            if (Core.ToStr(txtlbl4.Text) != "" && txtlbl4.Text != "")
                _code = _code + txtlbl4.Text;
            else
                _code = _code + "####";

            _code = _code + "-";

            if (txtMainColor1.Text != null && txtMainColor1.Text != "")
                _code = _code + txtMainColor1.Text;
            else
                _code = _code + "####";
            if (luplbl6.EditValue != null)
                _code = _code + luplbl6.EditValue.ToString();
            else 
                 _code = _code + "#";
            if (txtMainColor2.Text != null && txtMainColor2.Text != "")
                _code = _code + txtMainColor2.Text;
            else
                _code = _code + "####";

            _code = _code + "-";

            if (Core.ToStr(luplbl8.EditValue) != "")
                _code = _code + luplbl8.EditValue.ToString();
            else 
                 _code = _code + "###";
            if (Core.ToStr(luplbl9.EditValue) != "")
                _code = _code + luplbl9.EditValue.ToString();
            else 
                 _code = _code + "#";

            return _code;
            
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

        private void LookUp_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.LookUpEdit lup = sender as DevExpress.XtraEditors.LookUpEdit;
            if (lup.Name == "luplbl5")
            {
                txtMainColor1.Text = this.GetColorCode(Core.ToStr(lup.EditValue));
                lupc1.EditValue = lup.EditValue;
                txtItemCode.Text = CreateNewCode();
            }
            else if (lup.Name == "luplbl7")
            {
                txtMainColor2.Text = this.GetColorCode(Core.ToStr(lup.EditValue));
                lupc2.EditValue = lup.EditValue;
                txtItemCode.Text = CreateNewCode();
            }
            else
            {
                txtItemCode.Text = CreateNewCode();
            }
        }

        private void txtlbl4_EditValueChanged(object sender, EventArgs e)
        {
            txtItemCode.Text = CreateNewCode();
        }

        private string GetLastVersionNum(string _code)
        {
            int ver = 0;

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                using (var db = new Gobibase())
                {
                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand("select isnull(MAX([version]),0) as ver from t_details as p inner join dbo.t_main AS m ON SUBSTRING(p.itemcode,1,8) = SUBSTRING(m.itemcode,1,8) AND m.zagvartype = N'Эх загвар' where p.itemcode  = N'" + _code + "'", conn);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ver = dr.IsDBNull(0) ? 0 : Core.ToInt(dr.GetInt32(0));
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

            ver = ver + 1;
            return ver.ToString();
        }

        private string GetLastProVersionNum(string _code)
        {
            int ver = 0;

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                using (var db = new Gobibase())
                {
                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand("select isnull(MAX([version]),0) as ver from t_programist as p inner join dbo.t_main AS m ON p.itemcode = m.itemcode AND m.zagvartype = N'Эх загвар' where p.itemcode  = '" + _code + "'", conn);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ver = dr.IsDBNull(0) ? 0 : Core.ToInt(dr.GetInt32(0));
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

            ver = ver + 1;
            return ver.ToString();
        }

        private string GetLastFileVersionNum(string _code, int _mver)
        {
            int ver = 0;

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                using (var db = new Gobibase())
                {
                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand("select isnull(MAX([version]),0) as ver from t_file as p inner join dbo.t_main AS m ON p.itemcode = m.itemcode AND m.zagvartype = N'Эх загвар' where p.itemcode  = '" + _code + "' and mVersion = " + _mver.ToString(), conn);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ver = dr.IsDBNull(0) ? 0 : Core.ToInt(dr.GetInt32(0));
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

            ver = ver + 1;
            return ver.ToString();
        }

        private void CreateZagvarVersion(string _ver)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                using (var db = new Gobibase())
                {


                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand(@"INSERT INTO [t_details]([itemcode],[version],[versiondate],[brend],[collection],[utasno]
,[holio],[gage],[tuslahmat],[suljees],[zahsuljees],[ihbiemanjet],[hantsuumanjet],[zahjin]
,[zzb],[urt],[murniiturul],[engerturul],[zahturul],[hantsuuturul],[halaasniiturul],[zahialgach]
,[cusnum],[printnum],[designer],[omnohdugaar],[gobidugaar],[goyodugaar],[onlinenum],[code26],[code22]
,[barcode],[color1],[color2],[color3],[color4],[color5],[color6],[color7],[color8],[color9]
,[color10],[colorOther],[colorhuvi1],[colorhuvi2],[colorhuvi3],[colorhuvi4],[colorhuvi5],[colorhuvi6]
,[colorhuvi7],[colorhuvi8],[colorhuvi9],[colorhuvi10],[colorhuviOther],[createdU],[ehzjin],[massjin],[tuuhiijin],[bohirjin],[hodzartsuulalttime],[oymolMatNum],[zagvartailbar],[tsuvralnum]) VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13,@p14,@p15
,@p16,@p17,@p18,@p19,@p20,@p21,@p22,@p23,@p24,@p25,@p26,@p27,@p28,@p29
,@p30,@p31,@p32,@p33,@p34,@p35,@p36,@p37,@p38,@p39,@p40,@p41,@p42,@p43
,@p44,@p45,@p46,@p47,@p48,@p49,@p50,@p51,@p52,@p53,@p54,@p55,@p56,@p57,@p58,@p59,@p60,@p61,@p62,@p63)", conn);
                    cmd.Parameters.Add("@p1", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtItemCode.Text);
                    cmd.Parameters.Add("@p2", SqlDbType.Int).Value = Core.ToInt(_ver);
                    cmd.Parameters.Add("@p3", SqlDbType.DateTime).Value = Core.ToDateTime(dtVersion.EditValue);
                    cmd.Parameters.Add("@p4", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupbrend.Text);
                    cmd.Parameters.Add("@p5", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupCollection.Text);
                    cmd.Parameters.Add("@p6", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupUtasNo.Text);
                    cmd.Parameters.Add("@p7", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupHolio.Text);
                    cmd.Parameters.Add("@p8", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupGage.Text);
                    cmd.Parameters.Add("@p9", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupTuslahmat.Text);
                    cmd.Parameters.Add("@p10", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupSuljees.Text);
                    cmd.Parameters.Add("@p11", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupZahSuljTurul.Text);
                    cmd.Parameters.Add("@p12", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupIhbieManSulj.Text);
                    cmd.Parameters.Add("@p13", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupHantsuManSulj.Text);
                    cmd.Parameters.Add("@p14", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtJin.Text);
                    cmd.Parameters.Add("@p15", SqlDbType.Int).Value = Core.ToInt(lupZZB.EditValue);
                    cmd.Parameters.Add("@p16", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupUrt.Text);
                    cmd.Parameters.Add("@p17", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupMurniiTurul.Text);
                    cmd.Parameters.Add("@p18", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupEngerTurul.Text);
                    cmd.Parameters.Add("@p19", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupZahturul.Text);
                    cmd.Parameters.Add("@p20", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupHantsuuTurul.Text);
                    cmd.Parameters.Add("@p21", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupHalaasTurul.Text);
                    cmd.Parameters.Add("@p22", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtOrderName.Text);
                    cmd.Parameters.Add("@p23", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtCustomerNo.Text);
                    cmd.Parameters.Add("@p24", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtPrintNum.Text);
                    cmd.Parameters.Add("@p25", SqlDbType.Int).Value = Core.ToInt(lupDesigner.EditValue);
                    cmd.Parameters.Add("@p26", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtOmnohdugaar.Text);
                    cmd.Parameters.Add("@p27", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtGobiNum.Text);
                    cmd.Parameters.Add("@p28", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtGoyoNum.Text);
                    cmd.Parameters.Add("@p29", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtOnlinenum.Text);
                    cmd.Parameters.Add("@p30", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtCode26.Text);
                    cmd.Parameters.Add("@p31", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtCode22.Text);
                    cmd.Parameters.Add("@p32", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtBarcode.Text);
                    cmd.Parameters.Add("@p33", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupc1.EditValue);
                    cmd.Parameters.Add("@p34", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupc2.EditValue);
                    cmd.Parameters.Add("@p35", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupc3.EditValue);
                    cmd.Parameters.Add("@p36", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupc4.EditValue);
                    cmd.Parameters.Add("@p37", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupc5.EditValue);
                    cmd.Parameters.Add("@p38", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupc6.EditValue);
                    cmd.Parameters.Add("@p39", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupc7.EditValue);
                    cmd.Parameters.Add("@p40", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupc8.EditValue);
                    cmd.Parameters.Add("@p41", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupc9.EditValue);
                    cmd.Parameters.Add("@p42", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupc10.EditValue);
                    cmd.Parameters.Add("@p43", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupc11.EditValue);
                    cmd.Parameters.Add("@p44", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtCHuvi1.Text);
                    cmd.Parameters.Add("@p45", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtCHuvi2.Text);
                    cmd.Parameters.Add("@p46", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtCHuvi3.Text);
                    cmd.Parameters.Add("@p47", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtCHuvi4.Text);
                    cmd.Parameters.Add("@p48", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtCHuvi5.Text);
                    cmd.Parameters.Add("@p49", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtCHuvi6.Text);
                    cmd.Parameters.Add("@p50", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtCHuvi7.Text);
                    cmd.Parameters.Add("@p51", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtCHuvi8.Text);
                    cmd.Parameters.Add("@p52", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtCHuvi9.Text);
                    cmd.Parameters.Add("@p53", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtCHuvi10.Text);
                    cmd.Parameters.Add("@p54", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtCHuviOther.Text);
                    cmd.Parameters.Add("@p55", SqlDbType.Int).Value = frmMain.UserID;

                    cmd.Parameters.Add("@p56", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtEZjin.Text);
                    cmd.Parameters.Add("@p57", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtMassJin.Text);
                    cmd.Parameters.Add("@p58", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtTuuhiiJin.Text);
                    cmd.Parameters.Add("@p59", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtBohirjin.Text);
                    cmd.Parameters.Add("@p60", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtHudZarTime.Text);
                    cmd.Parameters.Add("@p61", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtOymolDugaar.Text);
                    cmd.Parameters.Add("@p62", SqlDbType.NVarChar, 500).Value = Core.ToStr(memoDesc.Text);
                    cmd.Parameters.Add("@p63", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtTsuvral.Text);

                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    LoadVersion(Itemcode.Itemcode);

                    gvVersion.FocusedRowHandle = gvVersion.DataRowCount - 1;

                    Cursor.Current = Cursors.Default;

                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Aлдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSaveZagvar_Click(object sender, EventArgs e)
        {
            if (Itemcode == null) return;
            string _ver = GetLastVersionNum(Itemcode.Itemcode);

            if (MessageBox.Show("Загварын нэмэлтийн <" + _ver + "> хувилбарыг үүсгэж хадгалах уу?", "Анхааруулга", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            CreateZagvarVersion(_ver);

        }

        private void btnAddComment_Click(object sender, EventArgs e)
        {
            if (Itemcode != null && txtComment.Text != null && txtComment.Text != "")
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    using (var db = new Gobibase())
                    {
                        SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                        SqlCommand cmd = new SqlCommand(@"INSERT INTO [t_comment]([itemcode],[comment],[cdate],[cuser]) VALUES (@p1,@p2,@p3,@p4)", conn);
                        cmd.Parameters.Add("@p1", SqlDbType.NVarChar, 50).Value = Itemcode.Itemcode;
                        cmd.Parameters.Add("@p2", SqlDbType.NVarChar, 500).Value = Core.ToStr(txtComment.Text);
                        cmd.Parameters.Add("@p3", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@p4", SqlDbType.Int).Value = frmMain.UserID;
                       
                        cmd.CommandType = CommandType.Text;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        
                        LoadComment(Itemcode.Itemcode);
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

        private void gvVersion_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            clsDetails rw = (clsDetails)gvVersion.GetRow(e.FocusedRowHandle);
            if (rw == null)
                return;
            int num = rw.Version;
            LoadDetails(num);
            gcFile.DataSource = lst_file_version.Where(s => s.Mversion == num).ToList();
        }

        private void LoadDetails(int _ver)
        {
            try
            {
                foreach (clsDetails i in lst_version.Where(s => s.Version == _ver).ToList())
                {
                    lupDesigner.EditValue = i.Designer;
                    lupbrend.EditValue = i.Brend;
                    lupCollection.EditValue = i.Collection;
                    lupUtasNo.EditValue = i.Utasno;
                    lupHolio.EditValue = i.Holio;
                    lupGage.EditValue = i.Gage;
                    lupTuslahmat.EditValue = i.Tuslahmat;
                    lupSuljees.EditValue = i.Suljees;
                    lupZahSuljTurul.EditValue = i.Zahsuljees;
                    lupIhbieManSulj.EditValue = i.Ihbiemanjet;
                    lupHantsuManSulj.EditValue = i.Hantsuumanjet;
                    txtJin.Text = i.Zahjin;
                    lupZZB.EditValue = i.Zzb;
                    lupUrt.EditValue = i.Urt;
                    lupMurniiTurul.EditValue = i.Murniiturul;
                    lupEngerTurul.EditValue = i.Engerturul;
                    lupZahturul.EditValue = i.Zahturul;
                    lupHantsuuTurul.EditValue = i.Hantsuuturul;
                    lupHalaasTurul.EditValue = i.Halaasniiturul;
                    txtOrderName.Text = i.Zahialgach;
                    txtCustomerNo.Text = i.Cusnum;
                    txtVersion.Text = i.Version.ToString();
                    dtVersion.EditValue = i.Versiondate;
                    txtPrintNum.Text = i.Printnum;
                    txtOmnohdugaar.Text = i.Omnohdugaar;
                    txtGobiNum.Text = i.Gobidugaar;
                    txtGoyoNum.Text = i.Goyodugaar;
                    txtOnlinenum.Text = i.Onlinenum;
                    txtBarcode.Text = i.Barcode;
                    txtCode22.Text = i.Code22;
                    txtCode26.Text = i.Code26;
                    lupc3.EditValue = i.Color3;
                    lupc4.EditValue = i.Color4;
                    lupc5.EditValue = i.Color5;
                    lupc6.EditValue = i.Color6;
                    lupc7.EditValue = i.Color7;
                    lupc8.EditValue = i.Color8;
                    lupc9.EditValue = i.Color9;
                    lupc10.EditValue = i.Color10;
                    lupc11.EditValue = i.ColorOther;
                    txtCHuvi3.Text = i.Colorhuvi3;
                    txtCHuvi4.Text = i.Colorhuvi4;
                    txtCHuvi5.Text = i.Colorhuvi5;
                    txtCHuvi6.Text = i.Colorhuvi6;
                    txtCHuvi7.Text = i.Colorhuvi7;
                    txtCHuvi8.Text = i.Colorhuvi8;
                    txtCHuvi9.Text = i.Colorhuvi9;
                    txtCHuvi10.Text = i.Colorhuvi10;
                    txtCHuviOther.Text = i.ColorhuviOther;
                    txtMVersion.Text = i.Version.ToString();

                    txtEZjin.Text = i.Ehzjin;
                    txtMassJin.Text = i.Massjin;
                    txtTuuhiiJin.Text = i.Tuuhiijin;
                    txtBohirjin.Text = i.Bohirjin;
                    txtHudZarTime.Text = i.Hodzartsuulalttime;
                    txtOymolDugaar.Text = i.OymolMatNum;
                    memoDesc.Text = i.Zagvartailbar;
                    txtTsuvral.Text = i.Tsuvralnum;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Aлдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProDetails(int _ver)
        {
            try
            {
                foreach (clsProgram i in lst_pro_version.Where(s => s.Version == _ver).ToList())
                {
                    lupProgramist.EditValue = i.Proman;
                    dtProStart.EditValue = i.Startdate != "" ? i.Startdate : null;
                    dtProGive.EditValue = i.Givedate != "" ? i.Givedate : null;
                    txtProVer.Text = i.Version.ToString();
                    txtProTuluv.Text = i.Tuluv;
                    txtProFileName.Text = i.Profilename;
                    txtDetailsToo.Text = i.Detailnum;
                    lupCarTurul.EditValue = i.Car_typeid;
                    lupCarMark.EditValue = i.Car_markid;
                    txtProUnelge.Text = i.Progunelgee;
                    txtProTime.Text = i.Suljihtime;
                    txtProhurd.Text = i.Hurd;
                    txtSizeToo.Text = i.Sizetoo;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Aлдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadFileDetails(int _ver, int _mver)
        {
            try
            {
                foreach (clsFile i in lst_file_version.Where(s => s.Version == _ver && s.Mversion == _mver).ToList())
                {
                    txtFileName.Text = i.Filename;
                    txtFileVer.Text = i.Version.ToString();
                    dtFileEnddate.EditValue = i.Enddate != "" ? i.Enddate : null;
                    txtFileType.Text = i.Tuluv;
                    txtFileComment.Text = i.Comment;
                    txtFileCreUser.Text = i.Uname;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Aлдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditZagvar_Click(object sender, EventArgs e)
        {
            if (Itemcode == null || lst_version.Count == 0) return;
            if (MessageBox.Show("Загварын нэмэлтэд хийсэн өөрчлөлтийг хадгалах уу?", "Анхааруулга", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;
            try
            {
                int _index = gvVersion.FocusedRowHandle; 

                Cursor.Current = Cursors.WaitCursor;
                using (var db = new Gobibase())
                {
                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand(@"UPDATE [t_details] SET [brend]=@p1,[collection]=@p2,[utasno]=@p3,[holio]=@p4,[gage]=@p5,[tuslahmat]=@p6,[suljees]=@p7
,[zahsuljees]=@p8,[ihbiemanjet]=@p9,[hantsuumanjet]=@p10,[zahjin]=@p11,[zzb]=@p12,[urt]=@p13,[murniiturul]=@p14
,[engerturul]=@p15,[zahturul]=@p16,[hantsuuturul]=@p17,[halaasniiturul]=@p18,[zahialgach]=@p19,[cusnum]=@p20
,[printnum]=@p21,[designer]=@p22,[omnohdugaar]=@p23,[gobidugaar]=@p24,[goyodugaar]=@p25,[onlinenum]=@p26,[code26]=@p27
,[code22]=@p28,[barcode]=@p29,[color3]=@p30,[color4]=@p31,[color5]=@p32,[color6]=@p33,[color7]=@p34,[color8]=@p35
,[color9]=@p36,[color10]=@p37,[colorOther]=@p38,[colorhuvi3]=@p39,[colorhuvi4]=@p40,[colorhuvi5]=@p41,[colorhuvi6]=@p42
,[colorhuvi7]=@p43,[colorhuvi8]=@p44,[colorhuvi9]=@p45,[colorhuvi10]=@p46,[colorhuviOther]=@p47,[editedU]=@p48
,[editedDate]=@p49,[ehzjin] = @p52,[massjin] = @p53,[tuuhiijin] = @p54,[bohirjin] = @p55
,[hodzartsuulalttime] = @p56,[oymolMatNum] = @p57,[zagvartailbar]=@p58,[color1] = @p59, [color2] = @p60,[colorhuvi1]=@p61,[colorhuvi2]=@p62  WHERE itemcode = @p50 and [version] = @p51", conn);
                    cmd.Parameters.Add("@p1", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupbrend.Text);
                    cmd.Parameters.Add("@p2", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupCollection.Text);
                    cmd.Parameters.Add("@p3", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupUtasNo.Text);
                    cmd.Parameters.Add("@p4", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupHolio.Text);
                    cmd.Parameters.Add("@p5", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupGage.Text);
                    cmd.Parameters.Add("@p6", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupTuslahmat.Text);
                    cmd.Parameters.Add("@p7", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupSuljees.Text);
                    cmd.Parameters.Add("@p8", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupZahSuljTurul.Text);
                    cmd.Parameters.Add("@p9", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupIhbieManSulj.Text);
                    cmd.Parameters.Add("@p10", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupHantsuManSulj.Text);
                    cmd.Parameters.Add("@p11", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtJin.Text);
                    cmd.Parameters.Add("@p12", SqlDbType.Int).Value = Core.ToInt(lupZZB.EditValue);
                    cmd.Parameters.Add("@p13", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupUrt.Text);
                    cmd.Parameters.Add("@p14", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupMurniiTurul.Text);
                    cmd.Parameters.Add("@p15", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupEngerTurul.Text);
                    cmd.Parameters.Add("@p16", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupZahturul.Text);
                    cmd.Parameters.Add("@p17", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupHantsuuTurul.Text);
                    cmd.Parameters.Add("@p18", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupHalaasTurul.Text);
                    cmd.Parameters.Add("@p19", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtOrderName.Text);
                    cmd.Parameters.Add("@p20", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtCustomerNo.Text);
                    cmd.Parameters.Add("@p21", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtPrintNum.Text);
                    cmd.Parameters.Add("@p22", SqlDbType.Int).Value = Core.ToInt(lupDesigner.EditValue);
                    cmd.Parameters.Add("@p23", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtOmnohdugaar.Text);
                    cmd.Parameters.Add("@p24", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtGobiNum.Text);
                    cmd.Parameters.Add("@p25", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtGoyoNum.Text);
                    cmd.Parameters.Add("@p26", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtOnlinenum.Text);
                    cmd.Parameters.Add("@p27", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtCode26.Text);
                    cmd.Parameters.Add("@p28", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtCode22.Text);
                    cmd.Parameters.Add("@p29", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtBarcode.Text);
                    cmd.Parameters.Add("@p30", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupc3.EditValue);
                    cmd.Parameters.Add("@p31", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupc4.EditValue);
                    cmd.Parameters.Add("@p32", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupc5.EditValue);
                    cmd.Parameters.Add("@p33", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupc6.EditValue);
                    cmd.Parameters.Add("@p34", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupc7.EditValue);
                    cmd.Parameters.Add("@p35", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupc8.EditValue);
                    cmd.Parameters.Add("@p36", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupc9.EditValue);
                    cmd.Parameters.Add("@p37", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupc10.EditValue);
                    cmd.Parameters.Add("@p38", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupc11.EditValue);
                    cmd.Parameters.Add("@p39", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtCHuvi3.Text);
                    cmd.Parameters.Add("@p40", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtCHuvi4.Text);
                    cmd.Parameters.Add("@p41", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtCHuvi5.Text);
                    cmd.Parameters.Add("@p42", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtCHuvi6.Text);
                    cmd.Parameters.Add("@p43", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtCHuvi7.Text);
                    cmd.Parameters.Add("@p44", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtCHuvi8.Text);
                    cmd.Parameters.Add("@p45", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtCHuvi9.Text);
                    cmd.Parameters.Add("@p46", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtCHuvi10.Text);
                    cmd.Parameters.Add("@p47", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtCHuviOther.Text);
                    cmd.Parameters.Add("@p48", SqlDbType.Int).Value = frmMain.UserID;
                    cmd.Parameters.Add("@p49", SqlDbType.NVarChar, 50).Value = DateTime.Now;
                    cmd.Parameters.Add("@p50", SqlDbType.NVarChar, 50).Value = Itemcode.Itemcode;
                    cmd.Parameters.Add("@p51", SqlDbType.Int).Value = Core.ToInt(txtVersion.Text);

                    cmd.Parameters.Add("@p52", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtEZjin.Text);
                    cmd.Parameters.Add("@p53", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtMassJin.Text);
                    cmd.Parameters.Add("@p54", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtTuuhiiJin.Text);
                    cmd.Parameters.Add("@p55", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtBohirjin.Text);
                    cmd.Parameters.Add("@p56", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtHudZarTime.Text);
                    cmd.Parameters.Add("@p57", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtOymolDugaar.Text);
                    cmd.Parameters.Add("@p58", SqlDbType.NVarChar, 500).Value = Core.ToStr(memoDesc.Text);

                    cmd.Parameters.Add("@p59", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupc1.EditValue);
                    cmd.Parameters.Add("@p60", SqlDbType.NVarChar, 50).Value = Core.ToStr(lupc2.EditValue);
                    cmd.Parameters.Add("@p61", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtCHuvi1.Text);
                    cmd.Parameters.Add("@p62", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtCHuvi2.Text);

                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    LoadVersion(Itemcode.Itemcode);

                    gvVersion.FocusedRowHandle = _index;

                    Cursor.Current = Cursors.Default;

                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Aлдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gvProVersion_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            clsProgram rw = (clsProgram)gvProVersion.GetRow(e.FocusedRowHandle);
            if (rw == null)
                return;
            int num = rw.Version;
            LoadProDetails(num);
        }

        private void btnAddPgrm_Click(object sender, EventArgs e)
        {
            if (Itemcode == null) return;
            string _ver = GetLastProVersionNum(Itemcode.Itemcode);
            if (MessageBox.Show("Программын нэмэлтийн <" + _ver + "> хувилбарыг үүсгэж хадгалах уу?", "Анхааруулга", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                using (var db = new Gobibase())
                {
               
                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand(@"INSERT INTO [t_programist]([proman],[itemcode],[startdate],[givedate],[version]
,[tuluv],[car_typeid],[car_markid],[detailnum],[progunelgee],[suljihtime],[profilename],[hurd],[sizetoo]) VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13,@p14)", conn);
                    
                    cmd.Parameters.Add("@p1", SqlDbType.Int).Value = Core.ToInt(lupProgramist.EditValue);
                    cmd.Parameters.Add("@p2", SqlDbType.NVarChar, 50).Value = Itemcode.Itemcode;
                    cmd.Parameters.Add("@p3", SqlDbType.NVarChar, 50).Value = Core.ToStr(dtProStart.EditValue != null ? dtProStart.DateTime.ToString("yyyy/MM/dd HH:mm") : dtProStart.EditValue);
                    cmd.Parameters.Add("@p4", SqlDbType.NVarChar, 50).Value = Core.ToStr(dtProGive.EditValue != null ? dtProGive.DateTime.ToString("yyyy/MM/dd HH:mm") : dtProGive.EditValue);
                    cmd.Parameters.Add("@p5", SqlDbType.Int).Value = Core.ToInt(_ver);
                    cmd.Parameters.Add("@p6", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtProTuluv.Text);
                    cmd.Parameters.Add("@p7", SqlDbType.Int).Value = Core.ToInt(lupCarTurul.EditValue);
                    cmd.Parameters.Add("@p8", SqlDbType.Int).Value = Core.ToInt(lupCarMark.EditValue);
                    cmd.Parameters.Add("@p9", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtDetailsToo.Text);
                    cmd.Parameters.Add("@p10", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtProUnelge.Text);
                    cmd.Parameters.Add("@p11", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtProTime.Text);
                    cmd.Parameters.Add("@p12", SqlDbType.NVarChar, 100).Value = Core.ToStr(txtProFileName.Text);
                    cmd.Parameters.Add("@p13", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtProhurd.Text);
                    cmd.Parameters.Add("@p14", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtSizeToo.Text);

                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    LoadProVersion(Itemcode.Itemcode);

                    gvProVersion.FocusedRowHandle = gvProVersion.DataRowCount - 1;

                    Cursor.Current = Cursors.Default;

                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Aлдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSavePgrm_Click(object sender, EventArgs e)
        {
            if (Itemcode == null || lst_pro_version.Count == 0) return;
            if (MessageBox.Show("Мэдээлэлд өөрчлөлт оруулсан бол хадгалах уу?", "Анхааруулга", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;
            try
            {
                int _index = gvProVersion.FocusedRowHandle;

                Cursor.Current = Cursors.WaitCursor;
                using (var db = new Gobibase())
                {
                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand(@"UPDATE [t_programist] SET [proman] = @p1,[startdate] = @p2
,[givedate] = @p3,[tuluv] = @p4,[car_typeid] =@p5,[car_markid] =@p6
,[detailnum] = @p7,[progunelgee] =@p8,[suljihtime] =@p9, [profilename] = @p12, [hurd] = @p13, [sizetoo] = @p14 WHERE itemcode = @p10 and [version] = @p11", conn);

                    cmd.Parameters.Add("@p1", SqlDbType.Int).Value = Core.ToInt(lupProgramist.EditValue);
                    cmd.Parameters.Add("@p2", SqlDbType.NVarChar, 50).Value = Core.ToStr(dtProStart.EditValue != null ? dtProStart.DateTime.ToString("yyyy/MM/dd HH:mm") : dtProStart.EditValue);
                    cmd.Parameters.Add("@p3", SqlDbType.NVarChar, 50).Value = Core.ToStr(dtProGive.EditValue != null ? dtProGive.DateTime.ToString("yyyy/MM/dd HH:mm") : dtProGive.EditValue);
                    cmd.Parameters.Add("@p4", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtProTuluv.Text);
                    cmd.Parameters.Add("@p5", SqlDbType.Int).Value = Core.ToInt(lupCarTurul.EditValue);
                    cmd.Parameters.Add("@p6", SqlDbType.Int).Value = Core.ToInt(lupCarMark.EditValue);
                    cmd.Parameters.Add("@p7", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtDetailsToo.Text);
                    cmd.Parameters.Add("@p8", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtProUnelge.Text);
                    cmd.Parameters.Add("@p9", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtProTime.Text);
                    cmd.Parameters.Add("@p10", SqlDbType.NVarChar, 50).Value = Itemcode.Itemcode;
                    cmd.Parameters.Add("@p11", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtProVer.Text);
                    cmd.Parameters.Add("@p12", SqlDbType.NVarChar, 100).Value = Core.ToStr(txtProFileName.Text);
                    cmd.Parameters.Add("@p13", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtProhurd.Text);
                    cmd.Parameters.Add("@p14", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtSizeToo.Text);

                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    LoadProVersion(Itemcode.Itemcode);

                    gvProVersion.FocusedRowHandle = _index;

                    Cursor.Current = Cursors.Default;

                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Aлдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gvFile_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            clsFile rw = (clsFile)gvFile.GetRow(e.FocusedRowHandle);
            if (rw == null)
                return;
            int num = rw.Version;
            LoadFileDetails(num, rw.Mversion);
        }

        private void btnFileUpload_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) 
            {
                txtFileName.Text = openFileDialog1.SafeFileName;
                txtFileName.Text = DateTime.Now.ToString("yyyyMMddHH") + "_" + txtFileType.Text + "_" + txtFileName.Text;
                UploadFile(2);
            }
        }

        private void btnFileSave_Click(object sender, EventArgs e)
        {
            if (Itemcode == null) return;
            if (lst_version.Count == 0) return;

            int mVer = Core.ToInt(txtMVersion.Text);

            if (mVer == 0) return;

            string _ver = GetLastFileVersionNum(Itemcode.Itemcode, mVer);

            if (MessageBox.Show("Файлын <" + _ver + "> хувилбарыг үүсгэж хадгалах уу?", "Анхааруулга", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                using (var db = new Gobibase())
                {
                    

                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand(@"INSERT INTO [t_file]([itemcode],[filename],[version],[tuluv],[createddate]
,[enddate],[comment],[cuser],[mVersion]) VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9)", conn);

                    cmd.Parameters.Add("@p1", SqlDbType.NVarChar, 50).Value = Itemcode.Itemcode;
                    cmd.Parameters.Add("@p2", SqlDbType.NVarChar, 100).Value = txtFileName.Text;
                    cmd.Parameters.Add("@p3", SqlDbType.Int).Value = Core.ToInt(_ver);
                    cmd.Parameters.Add("@p4", SqlDbType.NVarChar, 50).Value = txtFileType.Text;
                    cmd.Parameters.Add("@p5", SqlDbType.NVarChar, 50).Value = DateTime.Now.ToString("yyyy/MM/dd HH:mm").ToString();
                    cmd.Parameters.Add("@p6", SqlDbType.NVarChar, 50).Value = Core.ToStr(dtFileEnddate.EditValue != null ? dtFileEnddate.DateTime.ToString("yyyy/MM/dd HH:mm") : dtFileEnddate.EditValue);
                    cmd.Parameters.Add("@p7", SqlDbType.NVarChar, 500).Value = txtFileComment.Text;
                    cmd.Parameters.Add("@p8", SqlDbType.Int).Value = frmMain.UserID;
                    cmd.Parameters.Add("@p9", SqlDbType.Int).Value = mVer;

                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    LoadFileVersion(Itemcode.Itemcode);

                    gcFile.DataSource = lst_file_version.Where(s => s.Mversion == mVer).ToList();

                    gvFile.FocusedRowHandle = gvFile.DataRowCount - 1;

                    Cursor.Current = Cursors.Default;

                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Aлдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UploadFile(int _tt)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                string sourceFile = "";
                string destinationFile = "";

                if (_tt == 1)
                {
                    sourceFile = openFileDialog2.FileName;
                    destinationFile = uploadPropath +txtProFileName.Text;
                }
                else
                {
                    sourceFile = openFileDialog1.FileName;
                    destinationFile = uploadHavpath + txtFileName.Text;
                }

                try
                {
                    if(File.Exists(destinationFile))
                    {
                        MessageBox.Show("Энэ файлтай ижил нэртэй файл өмнө нь үүссэн тул файлын нэрээ шалгана уу!", "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    File.Copy(sourceFile, destinationFile, false);
                    MessageBox.Show("Амжилттай хуулагдлаа.", "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (IOException iox)
                {
                    MessageBox.Show("Алдаа гарлаа." + iox.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }  

                Cursor.Current = Cursors.Default;
                
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default; 
                MessageBox.Show("ФАЙЛ хуулах үед алдаа гарлаа." + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFileEdit_Click(object sender, EventArgs e)
        {
            if (Itemcode == null || lst_file_version.Count == 0) return;
            if (MessageBox.Show("Мэдээлэлд өөрчлөлт оруулсан бол хадгалах уу?", "Анхааруулга", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;
            try
            {
                int _index = gvFile.FocusedRowHandle;

                Cursor.Current = Cursors.WaitCursor;
                using (var db = new Gobibase())
                {
                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand(@"UPDATE [t_file]
SET [filename] = @p1,[tuluv] = @p2
,[enddate] = @p3,[comment] = @p4 WHERE itemcode = @p5 and [version] = @p6 and [mVersion] = @p7", conn);

                    
                    cmd.Parameters.Add("@p1", SqlDbType.NVarChar, 100).Value = Core.ToStr(txtFileName.Text);
                    cmd.Parameters.Add("@p2", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtFileType.Text);
                    cmd.Parameters.Add("@p3", SqlDbType.NVarChar, 50).Value = Core.ToStr(dtFileEnddate.EditValue != null ? dtFileEnddate.DateTime.ToString("yyyy/MM/dd HH:mm") : dtFileEnddate.EditValue);
                    cmd.Parameters.Add("@p4", SqlDbType.NVarChar, 500).Value = Core.ToStr(txtFileComment.Text);
                    cmd.Parameters.Add("@p5", SqlDbType.NVarChar, 50).Value = Itemcode.Itemcode;
                    cmd.Parameters.Add("@p6", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtFileVer.Text);
                    cmd.Parameters.Add("@p7", SqlDbType.NVarChar, 50).Value = Core.ToStr(txtMVersion.Text);
                    
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    LoadFileVersion(Itemcode.Itemcode);

                    gcFile.DataSource = lst_file_version.Where(s => s.Mversion == Core.ToInt(txtMVersion.Text)).ToList();

                    gvFile.FocusedRowHandle = _index;

                    Cursor.Current = Cursors.Default;

                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Aлдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUploadFile_Click(object sender, EventArgs e)
        {
            UploadFile(2);
        }

        private void btnSelectPro_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog2.ShowDialog();
            if (result == DialogResult.OK) 
            {
                txtProFileName.Text = openFileDialog2.SafeFileName;
            }
        }

        private void btnUploadPro_Click(object sender, EventArgs e)
        {
            UploadFile(1);
        }

        private void btnShowFile_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                string destinationFile = "";

                destinationFile = uploadHavpath + txtFileName.Text;


                try
                {
                    if (File.Exists(destinationFile))
                    {
                        string name = txtFileName.Text;
                        string[] files1 = System.IO.Directory.GetFiles(uploadHavpath, name + "*", System.IO.SearchOption.AllDirectories);
                        
                        foreach (string f in files1)
                            System.Diagnostics.Process.Start(f);
                    }
                    else
                    {
                        MessageBox.Show(txtFileName.Text + " ийм нэртэй файл серверээс олдсонгүй!", "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (IOException iox)
                {
                    MessageBox.Show("Алдаа гарлаа." + iox.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("ФАЙЛ нээх үед алдаа гарлаа." + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dtVersion_QueryPopUp(object sender, CancelEventArgs e)
        {
            var edit = sender as DevExpress.XtraEditors.DateEdit;
            if (edit == null) return;
            if (edit.EditValue == null || edit.EditValue is DBNull)
                edit.EditValue = DateTime.Now;
        }

        private void dtProStart_QueryPopUp(object sender, CancelEventArgs e)
        {
            var edit = sender as DevExpress.XtraEditors.DateEdit;
            if (edit == null) return;
            if (edit.EditValue == null || edit.EditValue is DBNull)
                edit.EditValue = DateTime.Now;
        }

        private void btnProFileShow_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                string destinationFile = "";

                destinationFile = uploadPropath + txtProFileName.Text;


                try
                {
                    if (File.Exists(destinationFile))
                    {
                        string name = txtProFileName.Text;
                        string[] files1 = System.IO.Directory.GetFiles(uploadPropath, name + "*", System.IO.SearchOption.AllDirectories);

                        foreach (string f in files1)
                            System.Diagnostics.Process.Start(f);
                    }
                    else
                    {
                        MessageBox.Show(txtProFileName.Text + " ийм нэртэй файл серверээс олдсонгүй!", "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (IOException iox)
                {
                    MessageBox.Show("Алдаа гарлаа." + iox.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("ФАЙЛ нээх үед алдаа гарлаа." + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool CheckVersionUsed(string itemcode_eh, int ver_eh )
        {
            bool retVal = false;

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                using (var db = new Gobibase())
                {
                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand("select count(*) as cnt from t_main where zagvartype = N'Масс' and itemcode_eh = @p1 and version_eh = @p2", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@p1", SqlDbType.NVarChar, 50).Value = itemcode_eh;
                    cmd.Parameters.Add("@p2", SqlDbType.Int).Value = ver_eh;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    int cnt = 0;

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            cnt = dr.IsDBNull(0) ? 0 : Core.ToInt(dr.GetInt32(0));
                        }
                    }

                    conn.Close();
                    dr.Close();
                    Cursor.Current = Cursors.Default;

                    retVal = (cnt == 0 ? false : true);
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Aлдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return retVal;
        }

        private void repoRemoveVersion_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (gvVersion.FocusedRowHandle < 0) return;
            clsDetails curr = (clsDetails)gvVersion.GetFocusedRow();

            if (CheckVersionUsed(curr.Itemcode, curr.Version))
            {
                MessageBox.Show("Энэ цувралын дугаартай МАСС үүссэн байна. Устгах боломжгүй.", "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (curr.ID != 0 && MessageBox.Show("Сонгосон мөрийг хасах уу?", "Анхааруулга", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    using (var db = new Gobibase())
                    {
                        SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                        SqlCommand cmd = new SqlCommand(@"delete from t_details where id = @p1", conn);
                        cmd.Parameters.Add("@p1", SqlDbType.Int).Value = curr.ID;

                        cmd.CommandType = CommandType.Text;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();

                        LoadVersion(Itemcode.Itemcode);

                        gvVersion.FocusedRowHandle = gvVersion.DataRowCount - 1;

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

        private void repoRemoveFile_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (gvFile.FocusedRowHandle < 0) return;
            clsFile curr = (clsFile)gvFile.GetFocusedRow();

            if (curr.ID != 0 && MessageBox.Show("Сонгосон мөрийг хасах уу?", "Анхааруулга", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    using (var db = new Gobibase())
                    {
                        SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                        SqlCommand cmd = new SqlCommand(@"delete from t_file where id = @p1", conn);
                        cmd.Parameters.Add("@p1", SqlDbType.Int).Value = curr.ID;

                        cmd.CommandType = CommandType.Text;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();

                        string destinationFile = "";

                        destinationFile = uploadHavpath + txtFileName.Text;

                        if (File.Exists(destinationFile))
                        {
                            string name = txtFileName.Text;
                            string[] files1 = System.IO.Directory.GetFiles(uploadHavpath, name + "*", System.IO.SearchOption.AllDirectories);

                            foreach (string f in files1)
                                File.Delete(f);

                            txtFileName.Text = "";
                        }
                        else
                        {
                            MessageBox.Show(txtFileName.Text + " ийм нэртэй файл серверээс олдсонгүй!", "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }


                        LoadFileVersion(Itemcode.Itemcode);

                        gcFile.DataSource = lst_file_version.Where(s => s.Mversion == Core.ToInt(txtMVersion.Text)).ToList();

                        gvFile.FocusedRowHandle = gvFile.DataRowCount - 1;

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

        private void lupCarTurul_EditValueChanged(object sender, EventArgs e)
        {
            if (lupCarTurul.EditValue != null)
            {
                lupCarMark.Properties.DataSource = lst_info.Where(s => s.L_name == "carmark" && s.Other1 == lupCarTurul.Text.ToString()).ToList();
                lupCarMark.Properties.ValueMember = "ID";
                lupCarMark.Properties.DisplayMember = "Name";
            }
            else
            {
                lupCarMark.Properties.DataSource = lst_info.Where(s => s.L_name == "carmark").ToList();
                lupCarMark.Properties.ValueMember = "ID";
                lupCarMark.Properties.DisplayMember = "Name";
            }
        }

        private void repoRemovePro_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (gvProVersion.FocusedRowHandle < 0) return;
            clsProgram curr = (clsProgram)gvProVersion.GetFocusedRow();

            if (curr.ID != 0 && MessageBox.Show("Сонгосон мөрийг хасах уу?", "Анхааруулга", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    using (var db = new Gobibase())
                    {
                        SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                        SqlCommand cmd = new SqlCommand(@"delete from t_programist where id = @p1", conn);
                        cmd.Parameters.Add("@p1", SqlDbType.Int).Value = curr.ID;

                        cmd.CommandType = CommandType.Text;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();

                        string destinationFile = "";

                        destinationFile = uploadPropath + txtProFileName.Text;

                        if (File.Exists(destinationFile))
                        {
                            string name = txtProFileName.Text;
                            string[] files1 = System.IO.Directory.GetFiles(uploadPropath, name + "*", System.IO.SearchOption.AllDirectories);

                            foreach (string f in files1)
                                File.Delete(f);

                            txtProFileName.Text = "";
                        }
                        else
                        {
                            MessageBox.Show(txtProFileName.Text + " ийм нэртэй файл серверээс олдсонгүй!", "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }


                        LoadProVersion(Itemcode.Itemcode);

                        gvProVersion.FocusedRowHandle = gvProVersion.DataRowCount - 1;

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

        private void luplbl1_EditValueChanged(object sender, EventArgs e)
        {
            luplbl2.EditValue = null;
            luplbl3.EditValue = null;
            luplbl5.EditValue = null;
            luplbl6.EditValue = null;
            luplbl7.EditValue = null;
            luplbl8.EditValue = null;
            luplbl9.EditValue = null;
            txtlbl4.EditValue = null;
            txtNameCode.EditValue = null;


            lst_info_temp = new List<clsInfo>();
            lst_info_temp = lst_info.Where(s => s.Angilal.Contains(Core.ToStr(luplbl1.EditValue))).ToList();

            luplbl2.Properties.DataSource = lst_info_temp.Where(s => s.L_name == "Level2").OrderBy(o => o.Sortid).ToList();
            luplbl2.Properties.DisplayMember = "CodeName";
            luplbl2.Properties.ValueMember = "Code";

            luplbl3.Properties.DataSource = lst_info_temp.Where(s => s.L_name == "Level3").OrderBy(o => o.Sortid).ToList();
            luplbl3.Properties.DisplayMember = "Nameen";
            luplbl3.Properties.ValueMember = "Nameen";


            luplbl5.Properties.DataSource = lst_info_temp.Where(s => s.L_name == "Level5").OrderBy(o => o.Sortid).ToList();
            luplbl5.Properties.DisplayMember = "CodeName";
            luplbl5.Properties.ValueMember = "Code";

            luplbl6.Properties.DataSource = lst_info_temp.Where(s => s.L_name == "Level6").OrderBy(o => o.Sortid).ToList();
            luplbl6.Properties.DisplayMember = "CodeName";
            luplbl6.Properties.ValueMember = "Code";

            luplbl7.Properties.DataSource = lst_info_temp.Where(s => s.L_name == "Level5").OrderBy(o => o.Sortid).ToList();
            luplbl7.Properties.DisplayMember = "CodeName";
            luplbl7.Properties.ValueMember = "Code";

            luplbl8.Properties.DataSource = lst_info_temp.Where(s => s.L_name == "Level8").OrderBy(o => o.Sortid).ToList();
            luplbl8.Properties.DisplayMember = "CodeName";
            luplbl8.Properties.ValueMember = "Code";

            luplbl9.Properties.DataSource = lst_info_temp.Where(s => s.L_name == "Level9").OrderBy(o => o.Sortid).ToList();
            luplbl9.Properties.DisplayMember = "CodeName";
            luplbl9.Properties.ValueMember = "Code";
        }

        private void luplbl3_EditValueChanged(object sender, EventArgs e)
        {
            txtname.Text = null;
            txtnameEn.Text = null;
            txtNameCode.Text = null;
            clsInfo rw = (clsInfo)luplbl3.GetSelectedDataRow();

            if (rw != null)
            {
                txtname.Text = rw.Name;
                txtnameEn.Text = rw.Nameen;
                txtNameCode.Text = rw.Code;
            }
        }

        private void luplbl6_EditValueChanged(object sender, EventArgs e)
        {
            luplbl7.EditValue = null;
            txtMainColor2.EditValue = null;

            clsInfo rw = (clsInfo)luplbl6.GetSelectedDataRow();

            if (rw != null)
            {
                if (rw.CodeName.ToLower().Contains("принт"))
                {
                    layoutControlItem9.Enabled = false;
                    txtMainColor2.ReadOnly = false;
                }
                else
                {
                    layoutControlItem9.Enabled = true;
                    txtMainColor2.ReadOnly = true;
                }
            }
            
        }

        private void txtMainColor2_EditValueChanged(object sender, EventArgs e)
        {
            txtItemCode.Text = CreateNewCode();
        }
    }
}

