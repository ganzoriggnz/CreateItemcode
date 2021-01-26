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
using System.IO;
using System.Diagnostics;
using System.Configuration;

namespace CreateItemCode
{
    public partial class frmLogin : Form
    {
        string DB = ConfigurationManager.AppSettings["DB"];

        List<clsUser> lst_t = new List<clsUser>();

        string localVersion = "";
        string serverVersion = "";

        string serverpath = "";
        string serverpathFull = "";
        string localpath = "";

        string postProcessFile = Application.StartupPath + @"\updater.exe";
        string postProcessCommand = @" /Alert Seconds 75";

        public frmLogin()
        {
            CFunctions.ConnectionStr = "Data Source=192.168.80.14;Initial Catalog=" + DB + ";User id=sa;Password=Engine11;";

            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes)
            {
                if (process.ProcessName == "updater")
                {
                    process.Kill();
                    process.WaitForExit();
                }
            }

            InitializeComponent();
        }

        private void GetUser()
        {
            try
            {
                string _p = Application.StartupPath + "\\config.txt";
                if (File.Exists(_p))
                {
                    string[] lines = System.IO.File.ReadAllLines(_p);

                    foreach (string line in lines)
                    {
                        if (txtLname.Text == "") txtLname.Text = line;
                        else txtpass.Text = line;
                    }
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Aлдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetUser()
        {
            try
            {
                string _p = Application.StartupPath + "\\config.txt";
                if (File.Exists(_p))
                {
                    File.Delete(_p);

                    using (StreamWriter sw = File.CreateText(_p))
                    {
                        sw.WriteLine(txtLname.Text);
                        sw.WriteLine(txtpass.Text);
                    }
                }
                else
                {
                    using (StreamWriter sw = File.CreateText(_p))
                    {
                        sw.WriteLine(txtLname.Text);
                        sw.WriteLine(txtpass.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Aлдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            try
            {
                serverpathFull = ConfigurationManager.AppSettings["updatepath"];
                serverpath = serverpathFull.Substring(0, serverpathFull.Length - 8) + "updater.txt";
                localpath = Application.StartupPath;

                GetLocalUpdater();
                GetServerUpdater();

                if (serverVersion != localVersion && MessageBox.Show("Сервер дээр програм шинэчлэл орсон байна. Шинэчлэлтийг татах уу?", "Анхааруулга", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = postProcessFile;
                    startInfo.Arguments = postProcessCommand;
                    Process.Start(startInfo);
                    this.Close();
                }

                GetUser();
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Aлдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GetLocalUpdater()
        {
            try
            {
                string _p = localpath + "\\updater.txt";
                if (File.Exists(_p))
                {
                    string[] lines = System.IO.File.ReadAllLines(_p);

                    foreach (string line in lines)
                    {
                        localVersion = line;
                    }
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Aлдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GetServerUpdater()
        {
            try
            {
                if (File.Exists(serverpath))
                {
                    string[] lines = System.IO.File.ReadAllLines(serverpath);

                    foreach (string line in lines)
                    {
                        serverVersion = line;
                    }
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Aлдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetLocalUpdater()
        {
            try
            {
                string _p = Application.StartupPath + "\\updater.txt";
                if (File.Exists(_p))
                {
                    File.Delete(_p);

                    using (StreamWriter sw = File.CreateText(_p))
                    {
                        sw.WriteLine(serverVersion);
                    }
                }
                else
                {
                    using (StreamWriter sw = File.CreateText(_p))
                    {
                        sw.WriteLine(serverVersion);
                    }
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Aлдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUsers()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                using (var db = new Gobibase())
                {
                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand("select id, loginName,uname,pass, heltes,position,role, workertype,[createdate] from t_user where isActive = 1", conn);
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
                            g.Role = dr.IsDBNull(6) ? "" : Core.ToStr(dr.GetString(6));
                            g.Workertype = dr.IsDBNull(7) ? "" : Core.ToStr(dr.GetString(7));
                            g.Createdate = dr.IsDBNull(8) ? DateTime.Now : Core.ToDateTime(dr.GetDateTime(8));
                            lst_t.Add(g);
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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new Gobibase())
                {
                    clsUser c = lst_t.Where(w => w.LoginName == txtLname.Text && w.Pass == txtpass.Text).FirstOrDefault();

                    if (c != null)
                    {
                        this.Tag = c;
                        SetUser();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Нэвтрэх нэр нууц үгээ шалгана уу!", "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtpass.SelectAll();
                        txtpass.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Алдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Tag = null;
            this.Close();
        }

        private void txtLname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                try
                {
                    using (var db = new Gobibase())
                    {
                        clsUser c = lst_t.Where(w => w.LoginName == txtLname.Text && w.Pass == txtpass.Text).FirstOrDefault();

                        if (c != null)
                        {
                            this.Tag = c;
                            SetUser();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Нэвтрэх нэр нууц үгээ шалгана уу!", "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtpass.SelectAll();
                            txtpass.Focus();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Алдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtpass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                try
                {
                    using (var db = new Gobibase())
                    {
                        clsUser c = lst_t.Where(w => w.LoginName == txtLname.Text && w.Pass == txtpass.Text).FirstOrDefault();

                        if (c != null)
                        {
                            this.Tag = c;
                            SetUser();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Нэвтрэх нэр нууц үгээ шалгана уу!", "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtpass.SelectAll();
                            txtpass.Focus();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Алдаа гарлаа!" + Environment.NewLine + ex.Message, "Анхааруулга", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

       
    }
}
