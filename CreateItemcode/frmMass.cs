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
    public partial class frmMass : Form
    {
        clsListItemMain Itemcode = null;
        List<clsListItemMain> list_main = new List<clsListItemMain>();
        List<clsListItemMain> lst_savedData = new List<clsListItemMain>();
        List<clsAllData> lst_excel = new List<clsAllData>();
        List<clsInfo> lst_info = new List<clsInfo>();

        string CodeWord = "";

        int Transactionid = 0;

        string EhCode = "";

        public frmMass()
        {
            InitializeComponent();
        }

        private void frmMass_Load(object sender, EventArgs e)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));

            LoadInfo();

            for (int i = 1; i <= 9; i++)
            {
                Control[] tbxs = this.Controls.Find(("luplbl" + i), true);

                if (tbxs != null && tbxs.Length > 0)
                {
                    DevExpress.XtraEditors.LookUpEdit lup = tbxs[0] as DevExpress.XtraEditors.LookUpEdit;
                    lup.PreviewKeyDown += new PreviewKeyDownEventHandler(this.LookUpClear_PreviewKeyDown);
                }
            }

            this.luplbl6.EditValueChanged -= new EventHandler(this.luplbl6_EditValueChanged);

            SetControlValue();


            for (int i = 1; i <= 9; i++)
            {
                Control[] tbxs = this.Controls.Find(("luplbl" + i), true);

                if (tbxs != null && tbxs.Length > 0)
                {
                    DevExpress.XtraEditors.LookUpEdit lup = tbxs[0] as DevExpress.XtraEditors.LookUpEdit;
                    lup.EditValueChanged += new EventHandler(this.LookUp_EditValueChanged);
                }
            }

            this.luplbl6.EditValueChanged += new EventHandler(this.luplbl6_EditValueChanged);
        }

        private void SetControlValue()
        {
            gcSize.DataSource = null;
            if (Itemcode != null)
            {
                txtItemcode.Text = Itemcode.Itemcode;
                txtAngilal.Text = Itemcode.L1n;
                txtHuis.Text = Itemcode.L2n;
                txtNerturul.Text = Itemcode.L3n;
                txtDugaar.Text = Itemcode.Lev4;

                luplbl5.EditValue = Itemcode.Color1;
                luplbl6.EditValue = Itemcode.Lev6;
                luplbl7.EditValue = Itemcode.Color2;
                luplbl9.EditValue = Itemcode.Lev9;

                txtMainColor1.Text = this.GetColorCode(Core.ToStr(luplbl5.EditValue));
                txtMainColor2.Text = this.GetColorCode(Core.ToStr(luplbl7.EditValue));

                gcSize.DataSource = lst_info.Where(s => s.L_name == "Level8" && s.Angilal.Contains(Itemcode.Lev1)).OrderBy(o => o.Code).ToList();

                LoadListData(Itemcode.Itemcode.Substring(0, 8));

                gcList.DataSource = lst_savedData.Where(s => s.IsSend == false).ToList();
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
                    SqlCommand cmd = new SqlCommand("SELECT [id],[code],[name],[nameen],[l_name],[angilal],[sortid],[other1],[other2] FROM [t_info]" + StrWhere(), conn);
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

        private string StrWhere()
        {
            string retVal = "";

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                using (var db = new Gobibase())
                {
                    List<clsInfo> lst_t = new List<clsInfo>();

                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand("select buleg from t_userrole where userid = @p1", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@p1", SqlDbType.Int).Value = frmMain.UserID;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            string buleg = dr.IsDBNull(0) ? "" : Core.ToStr(dr.GetString(0));
                            if (retVal.Length == 0)
                            {
                                retVal = " WHERE angilal like N'%" + buleg + "%'";
                            }
                            else
                                retVal = retVal + " or angilal like N'%" + buleg + "%'";
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

            return retVal.Length == 0 ? " WHERE angilal = 'none permission'" : retVal;
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
            }
            else if (lup.Name == "luplbl7")
            {
                txtMainColor2.Text = this.GetColorCode(Core.ToStr(lup.EditValue));
            }

            gvList.ActiveFilterString = strActiveFilterString();
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

        private void repoRemove_Click(object sender, EventArgs e)
        {
            string num = gvMain.GetFocusedRowCellValue("Itemcode").ToString();
            if (num.Length > 0)
            {
                list_main.Remove(list_main.Where(s => s.Itemcode == num).First());
                gcMain.DataSource = list_main.ToList();
                gvMain.FocusedRowHandle = gvMain.DataRowCount - 1;
            }
        }

        private void repoRemoveMass_Click(object sender, EventArgs e)
        {
            string num = gvExcel.GetFocusedRowCellValue("Itemcode").ToString();
            if (num.Length > 0)
            {
                if (MessageBox.Show(num + " гэсэн Масс дугаарыг хасах уу?", "Анхааруулга", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        using (var db = new Gobibase())
                        {
                            SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                            SqlCommand cmd = new SqlCommand("delete from t_orderlist where transactionid = @p1 and itemcode = @p2", conn);
                            cmd.Parameters.Add("@p1", SqlDbType.Int).Value = Transactionid;
                            cmd.Parameters.Add("@p2", SqlDbType.NVarChar, 50).Value = num;
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();

                            GetItemcodeRowdata();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Aлдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void repoTrans_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            if (txtZahnum.Text == null || txtZahnum.Text == "")
            {
                lblMsg.Text = "Захиалгын дугаар хоосон байна.";
                txtZahnum.Focus();
                return;
            }
            if (txtOrder.Text == null || txtOrder.Text == "")
            {
                lblMsg.Text = "Захиалгын нэр хоосон байна.";
                txtOrder.Focus();
                return;
            }

            string num = gvList.GetFocusedRowCellValue("Itemcode").ToString();
            if (num.Length > 0)
            {
                foreach (clsListItemMain i in lst_savedData.Where(s => s.Itemcode == num).ToList())
                {
                    i.IsSend = true;
                    TransferItem(i);
                }
                gcList.DataSource = lst_savedData.Where(s => s.IsSend == false).ToList();
                GetItemcodeRowdata();
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
                    SqlCommand cmd = new SqlCommand("Select count(*) cnt from t_main where zagvartype = N'Масс' and itemcode  = '" + _code + "'", conn);
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

        private bool CheckItemCodeInLocal(string _code)
        {
            bool retVale = false;
            retVale = list_main.Where(s => s.Itemcode == _code).Count() == 0 ? false : true;

            int index = gvMain.LocateByValue("Itemcode", _code);
            if (index > -1)
            {
                gvMain.FocusedRowHandle = index;
            }

            return retVale;
        }

        private string CreateNewCode(string _size)
        {
            string _code = EhCode + "-";

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

            _code = _code + _size.ToString();
            

            if (luplbl9.EditValue != null)
                _code = _code + luplbl9.EditValue.ToString();
            else
                _code = _code + "#";

            return _code;

        }

        private void btnAddRow_Click(object sender, EventArgs e)
        {
            foreach (int i in gvSize.GetSelectedRows())
            {
                if (txtColorPart.Text == null || Core.ToStr(txtColorPart.Text) == "")
                {
                    MessageBox.Show("Үйлдвэрийн өнгө талбар хоосон байна.!", "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtColorPart.Focus();
                    txtColorPart.SelectAll();
                    return;
                }

                clsInfo rw = (clsInfo)gvSize.GetRow(i);
                try
                {
                    Cursor.Current = Cursors.WaitCursor;

                    clsListItemMain g = new clsListItemMain();
                    g.ID = 0;

                    g.Itemcode = CreateNewCode(rw.Code);

                    if (g.Itemcode.Contains("#"))
                    {
                        MessageBox.Show(g.Itemcode + " гэсэн ITEMCODE Level дутуу оруулсан байна.!", " Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (CheckItemCode(g.Itemcode))
                    {
                        MessageBox.Show(g.Itemcode + " гэсэн ITEMCODE өмнө нь үүсгэсэн байна. Мэдээллээ шалгана уу.!", " Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        int index = gvList.LocateByValue("Itemcode", g.Itemcode);
                        if (index > -1)
                        {
                            gvList.FocusedRowHandle = index;
                        }
                        continue;
                    }

                    if (CheckItemCodeInLocal(g.Itemcode))
                    {
                        MessageBox.Show(g.Itemcode + " гэсэн ITEMCODE доор нэмсэн байна.!", " Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        int index = gvMain.LocateByValue("Itemcode", g.Itemcode);
                        if (index > -1)
                        {
                            gvMain.FocusedRowHandle = index;
                        }
                        continue;
                    }

                    g.Itemcode_eh = txtItemcode.Text;
                    g.Version_eh = Core.ToInt(txtVer.Text);
                    g.Itemname = Itemcode.Itemname;
                    g.Itemnameen = Itemcode.Itemnameen;
                    g.Lev1 = Itemcode.Lev1;
                    g.Lev2 = Itemcode.Lev2;
                    g.Lev3 = Itemcode.Lev3;
                    g.Lev4 = Itemcode.Lev4;
                    g.Lev5 = txtMainColor1.Text;
                    g.Lev6 = Core.ToStr(luplbl6.EditValue);
                    g.Lev7 = Core.ToStr(txtMainColor2.EditValue);
                    g.Lev8 = rw.Code;
                    g.Lev9 = Core.ToStr(luplbl9.EditValue);
                    g.Color1 = Core.ToStr(luplbl5.EditValue);
                    g.Color2 = Core.ToStr(luplbl7.EditValue);


                    g.L1n = Itemcode.L1n;
                    g.L2n = Itemcode.L2n;
                    g.L3n = Itemcode.L3n;
                    g.L6n = Core.ToStr(luplbl6.Text);
                    g.L8n = Core.ToStr(rw.Code) + " - " + Core.ToStr(rw.Name);
                    g.L9n = Core.ToStr(luplbl9.Text);
                    g.Itemcodemain = txtItemcode.Text.Substring(0, 8);
                    g.Colorpart = Core.ToStr(txtColorPart.Text);

                    list_main.Add(g);

                    gcMain.DataSource = list_main.ToList();
                    gvMain.FocusedRowHandle = gvMain.DataRowCount - 1;

                    Cursor.Current = Cursors.Default;
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
            this.Tag = this.Tag != null ? "ok" : null;
            this.Close();
        }

        private void btnCreateCode_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            if (txtZahnum.Text == null || txtZahnum.Text == "")
            {
                lblMsg.Text = "Захиалгын дугаар хоосон байна.";
                txtZahnum.Focus();
                return;
            }
            if (txtOrder.Text == null || txtOrder.Text == "")
            {
                lblMsg.Text = "Захиалгын нэр хоосон байна.";
                txtOrder.Focus();
                return;
            }

            if (list_main.Count() > 0 && MessageBox.Show("Шинэ Itemcode-г хадгалах уу?", "Анхааруулга", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (Transactionid == 0)
                {
                    Transactionid = CFunctions.GetLastMassCounter();
                    CFunctions.SetLastMassCounter(1);
                }

                Cursor.Current = Cursors.WaitCursor;
                using (var db = new Gobibase())
                {
                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    SqlTransaction transaction;
                    transaction = conn.BeginTransaction("AddTrans");
                    cmd.Parameters.Add("@p1", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@p2", SqlDbType.NVarChar, 100);
                    cmd.Parameters.Add("@p3", SqlDbType.NVarChar, 100);
                    cmd.Parameters.Add("@p4", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@p5", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@p6", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@p7", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@p8", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@p9", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@p10", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@p11", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@p12", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@p13", SqlDbType.DateTime);
                    cmd.Parameters.Add("@p14", SqlDbType.Int);
                    cmd.Parameters.Add("@p15", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@p16", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@p17", SqlDbType.NVarChar, 80);
                    cmd.Parameters.Add("@p18", SqlDbType.NVarChar, 80);
                    cmd.Parameters.Add("@p19", SqlDbType.NVarChar, 80);
                    cmd.Parameters.Add("@p20", SqlDbType.NVarChar, 80);
                    cmd.Parameters.Add("@p21", SqlDbType.NVarChar, 80);
                    cmd.Parameters.Add("@p22", SqlDbType.NVarChar, 80);
                    cmd.Parameters.Add("@p23", SqlDbType.NVarChar, 80);
                    cmd.Parameters.Add("@p24", SqlDbType.Int);
                    cmd.Parameters.Add("@p25", SqlDbType.NVarChar, 500);

                    cmd.Connection = conn;
                    cmd.Transaction = transaction;

                    try
                    {

                        foreach (clsListItemMain i in list_main.ToList())
                        {
                            cmd.CommandText = @"INSERT INTO [t_main] ([itemcode],[itemname],[itemnameen],[lev1],[lev2],[lev3],[lev4],[lev5],[lev6]
,[lev7],[lev8],[lev9],[createddate],[createduser],[color1],[color2],[l1n],[l2n],[l3n],[l6n],[l8n],[l9n],[zagvartype],[itemcode_eh],[version_eh],[colorpart]) 
VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13,@p14,@p15,@p16,@p17,@p18,@p19,@p20,@p21,@p22,N'Масс',@p23,@p24,@p25)";
                            cmd.Parameters["@p1"].Value = i.Itemcode;
                            cmd.Parameters["@p2"].Value = i.Itemname;
                            cmd.Parameters["@p3"].Value = i.Itemnameen;
                            cmd.Parameters["@p4"].Value = i.Lev1;
                            cmd.Parameters["@p5"].Value = i.Lev2;
                            cmd.Parameters["@p6"].Value = i.Lev3;
                            cmd.Parameters["@p7"].Value = i.Lev4;
                            cmd.Parameters["@p8"].Value = i.Lev5;
                            cmd.Parameters["@p9"].Value = i.Lev6;
                            cmd.Parameters["@p10"].Value = i.Lev7;
                            cmd.Parameters["@p11"].Value = i.Lev8;
                            cmd.Parameters["@p12"].Value = i.Lev9;
                            cmd.Parameters["@p13"].Value = DateTime.Now;
                            cmd.Parameters["@p14"].Value = frmMain.UserID;
                            cmd.Parameters["@p15"].Value = i.Color1;
                            cmd.Parameters["@p16"].Value = i.Color2;
                            cmd.Parameters["@p17"].Value = i.L1n;
                            cmd.Parameters["@p18"].Value = i.L2n;
                            cmd.Parameters["@p19"].Value = i.L3n;
                            cmd.Parameters["@p20"].Value = i.L6n;
                            cmd.Parameters["@p21"].Value = i.L8n;
                            cmd.Parameters["@p22"].Value = i.L9n;
                            cmd.Parameters["@p23"].Value = i.Itemcode_eh;
                            cmd.Parameters["@p24"].Value = i.Version_eh;
                            cmd.Parameters["@p25"].Value = i.Colorpart;
                            cmd.ExecuteNonQuery();

                        }


                        foreach (clsListItemMain i in list_main.ToList())
                        {
                            cmd.CommandText = @"INSERT INTO [t_orderlist] ([transactionid],[zahialgach],[zahnum],[createdU],[createdDate],[itemcode],[colorpart]) 
VALUES (@p14,@p2,@p3,@p4,@p5,@p6,@p25)";
                            cmd.Parameters["@p14"].Value = Transactionid;
                            cmd.Parameters["@p2"].Value = txtOrder.Text;
                            cmd.Parameters["@p3"].Value = txtZahnum.Text;
                            cmd.Parameters["@p4"].Value = frmMain.cLoginUser.Uname;
                            cmd.Parameters["@p5"].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            cmd.Parameters["@p6"].Value = i.Itemcode;
                            cmd.Parameters["@p25"].Value = i.Colorpart;
                            cmd.ExecuteNonQuery();

                        }

                        cmd.CommandText = "UPDATE t_orderlist set zahnum = @p3, zahialgach = @p2 where transactionid = @p14";
                        cmd.Parameters["@p14"].Value = Transactionid;
                        cmd.Parameters["@p2"].Value = txtOrder.Text;
                        cmd.Parameters["@p3"].Value = txtZahnum.Text;
                        cmd.ExecuteNonQuery();
                        

                        transaction.Commit();

                        this.Tag = "ok";
                        
                        list_main.Clear();

                        gcMain.DataSource = list_main.ToList();
                        gvMain.FocusedRowHandle = gvMain.DataRowCount - 1;

                        conn.Close();

                        GetItemcodeRowdata();
                        
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

            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (gvExcel.DataRowCount == 0) return;
            string fileName = "";
            saveFileDialog1.Filter = "EXCEL 2003-20**|*.xlsx|Excel 97-2003|*.xls";
            DialogResult result = saveFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                fileName = saveFileDialog1.FileName;
                gcExcel.ExportToXlsx(fileName);
            }
        }

        private void LoadListData(string _code)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                using (var db = new Gobibase())
                {
                    lst_savedData.Clear();

                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand(@"SELECT itemcode, lev5, lev6, lev7,  lev8, lev9, color1, l6n, color2,  l8n, l9n, colorpart FROM t_main AS m WHERE (zagvartype = N'масс') AND (SUBSTRING(itemcode, 1, 8) = @p1) order by itemcode;", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@p1", SqlDbType.NVarChar, 50).Value = _code;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            clsListItemMain g = new clsListItemMain();
                            g.Itemcode = dr.IsDBNull(0) ? "" : Core.ToStr(dr.GetString(0));
                            g.Lev5 = dr.IsDBNull(1) ? "" : Core.ToStr(dr.GetString(1));
                            g.Lev6 = dr.IsDBNull(2) ? "" : Core.ToStr(dr.GetString(2));
                            g.Lev7 = dr.IsDBNull(3) ? "" : Core.ToStr(dr.GetString(3));
                            g.Lev8 = dr.IsDBNull(4) ? "" : Core.ToStr(dr.GetString(4));
                            g.Lev9 = dr.IsDBNull(5) ? "" : Core.ToStr(dr.GetString(5));

                            g.Color1 = dr.IsDBNull(6) ? "" : Core.ToStr(dr.GetString(6));
                            g.L6n = dr.IsDBNull(7) ? "" : Core.ToStr(dr.GetString(7));
                            g.Color2 = dr.IsDBNull(8) ? "" : Core.ToStr(dr.GetString(8));
                            g.L8n = dr.IsDBNull(9) ? "" : Core.ToStr(dr.GetString(9));
                            g.L9n = dr.IsDBNull(10) ? "" : Core.ToStr(dr.GetString(10));
                            g.Colorpart = dr.IsDBNull(11) ? "" : Core.ToStr(dr.GetString(11)); 

                            g.Itemcodemain = g.Itemcode.Substring(0, 8);
                            g.IsSend = false;
                            
                            lst_savedData.Add(g);
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

        private void btnFind_Click(object sender, EventArgs e)
        {
            frmFindEZ f = new frmFindEZ();
            f.ShowDialog();
            if (f.Tag != null)
            {
                Itemcode = new clsListItemMain();
                Itemcode = (clsListItemMain)f.Tag;
                txtVer.Text = Itemcode.Version.ToString();
                EhCode = Itemcode.Itemcode.Substring(0, 8);
                CodeWord = Itemcode.Itemcode.Substring(0, 1);

                for (int i = 1; i <= 9; i++)
                {
                    Control[] tbxs = this.Controls.Find(("luplbl" + i), true);

                    if (tbxs != null && tbxs.Length > 0)
                    {
                        DevExpress.XtraEditors.LookUpEdit lup = tbxs[0] as DevExpress.XtraEditors.LookUpEdit;
                        lup.EditValueChanged -= new EventHandler(this.LookUp_EditValueChanged);
                    }
                }

                this.luplbl6.EditValueChanged -= new EventHandler(this.luplbl6_EditValueChanged);

                SetControlValue();

                for (int i = 1; i <= 9; i++)
                {
                    Control[] tbxs = this.Controls.Find(("luplbl" + i), true);

                    if (tbxs != null && tbxs.Length > 0)
                    {
                        DevExpress.XtraEditors.LookUpEdit lup = tbxs[0] as DevExpress.XtraEditors.LookUpEdit;
                        lup.Properties.DataSource = lst_info.Where(s => (s.Angilal.Contains("W") || s.Angilal.Contains("S") || s.Angilal.Contains("K")) && s.L_name == "Level" + (i == 7 ? 5 : i)).OrderBy(o => o.Sortid).ToList();
                        lup.Properties.DisplayMember = "CodeName";
                        lup.Properties.ValueMember = "Code";
                        lup.EditValueChanged += new EventHandler(this.LookUp_EditValueChanged);
                    }
                }

                this.luplbl6.EditValueChanged += new EventHandler(this.luplbl6_EditValueChanged);
            }
        }

        private string strActiveFilterString()
        {
            string retVal = "";

            if (!String.IsNullOrEmpty(Core.ToStr(luplbl5.EditValue)))
            {
                retVal = retVal == "" ? String.Format("Lev5 = '{0}'", Core.ToStr(txtMainColor1.Text)) : retVal + String.Format(" and Lev5 = '{0}'", Core.ToStr(txtMainColor1.Text));
            }
            if (!String.IsNullOrEmpty(Core.ToStr(luplbl6.EditValue)))
            {
                retVal = retVal == "" ? String.Format("Lev6 = '{0}'", Core.ToStr(luplbl6.EditValue)) : retVal + String.Format(" and Lev6 = '{0}'", Core.ToStr(luplbl6.EditValue));
            }
            if (!String.IsNullOrEmpty(Core.ToStr(luplbl7.EditValue)))
            {
                retVal = retVal == "" ? String.Format("Lev7 = '{0}'", Core.ToStr(txtMainColor2.Text)) : retVal + String.Format(" and Lev7 = '{0}'", Core.ToStr(txtMainColor2.Text));
            }
            if (!String.IsNullOrEmpty(Core.ToStr(luplbl9.EditValue)))
            {
                retVal = retVal == "" ? String.Format("Lev9 = '{0}'", Core.ToStr(luplbl9.EditValue)) : retVal + String.Format(" and Lev9 = '{0}'", Core.ToStr(luplbl9.EditValue));
            }
            return retVal;
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
where transactionid = @p1 order by m.itemcode;", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@p1", SqlDbType.Int).Value = Transactionid;
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

        private void TransferItem(clsListItemMain itm)
        {
            try
            {
                using (var db = new Gobibase())
                {
                    if (Transactionid == 0)
                    {
                        Transactionid = CFunctions.GetLastMassCounter();
                        CFunctions.SetLastMassCounter(1);
                    }

                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand(@"INSERT INTO [t_orderlist] ([transactionid],[zahialgach],[zahnum],[createdU],[createdDate],[itemcode]) VALUES (@p1,@p2,@p3,@p4,@p5,@p6)", conn);
                    cmd.Parameters.Add("@p1", SqlDbType.Int).Value = Transactionid;
                    cmd.Parameters.Add("@p2", SqlDbType.NVarChar, 50).Value = txtOrder.Text;
                    cmd.Parameters.Add("@p3", SqlDbType.NVarChar,50).Value = txtZahnum.Text;
                    cmd.Parameters.Add("@p4", SqlDbType.NVarChar,50).Value = frmMain.cLoginUser.Uname;
                    cmd.Parameters.Add("@p5", SqlDbType.NVarChar,50).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    cmd.Parameters.Add("@p6", SqlDbType.NVarChar,50).Value = itm.Itemcode;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Aлдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    layoutControlItem10.Enabled = false;
                    txtMainColor2.ReadOnly = false;
                }
                else
                {
                    layoutControlItem10.Enabled = true;
                    txtMainColor2.ReadOnly = true;
                }
            }
        }

        private void luplbl5_EditValueChanged(object sender, EventArgs e)
        {
            txtColorPart.EditValue = null;
        }

        private void txtColorPart_EditValueChanged(object sender, EventArgs e)
        {
            string color1 = "";
            color1 = txtColorPart.Text; 

        }

        private void txtColorPart_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
           
        }
    }
}
    

