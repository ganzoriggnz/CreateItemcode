using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using CreateItemCode.Models;

namespace CreateItemCode
{
    public static class CFunctions
    {
        public static int GetLastMassCounter()
        {
            int cnt = 0;

            try
            {
                using (var db = new Gobibase())
                {
                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand("SELECT [LastMasscounter] FROM [t_counter]", conn);
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            cnt = dr.IsDBNull(0) ? 0 : Core.ToInt(dr.GetInt32(0));
                        }
                    }

                    conn.Close();
                    dr.Close();

                    cnt = cnt == 0 ? 1 : cnt;
                }
            }
            catch (Exception ex)
            {
                cnt = -1;
            }

            return cnt;
        }

        public static void SetLastMassCounter(int cnt)
        {
            try
            {
                using (var db = new Gobibase())
                {
                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand("update [t_counter] set LastMasscounter = LastMasscounter + @p1", conn);
                    cmd.Parameters.Add("@p1", System.Data.SqlDbType.Int).Value = cnt;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                ;
            }
        }

        public static string StrWhere()
        {
            string retVal = "";

            try
            {
                using (var db = new Gobibase())
                {
                    List<clsInfo> lst_t = new List<clsInfo>();

                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand("select buleg from t_userrole where userid = @p1", conn);
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add("@p1", System.Data.SqlDbType.Int).Value = frmMain.UserID;
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
                }
            }
            catch (Exception ex)
            {
                ;
            }

            return retVal.Length == 0 ? " WHERE angilal = 'none permission'" : retVal;
        }

        public static string StrMainWhere(bool _isRow)
        {
            string retVal = "";

            try
            {
                using (var db = new Gobibase())
                {
                    List<clsInfo> lst_t = new List<clsInfo>();

                    SqlConnection conn = new SqlConnection(db.Connection.ConnectionString);
                    SqlCommand cmd = new SqlCommand("select buleg from t_userrole where userid = @p1", conn);
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add("@p1", System.Data.SqlDbType.Int).Value = frmMain.UserID;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            string buleg = dr.IsDBNull(0) ? "" : Core.ToStr(dr.GetString(0));
                            if (retVal.Length == 0)
                            {
                                retVal = " lev1 like N'%" + buleg + "%'";
                            }
                            else
                                retVal = retVal + " or lev1 like N'%" + buleg + "%'";
                        }
                    }

                    conn.Close();
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                ;
            }

            if (!_isRow)
            {
                if (retVal.Length == 0)
                {
                    retVal = " WHERE lev1 = 'none permission'";
                }
                else
                    retVal = " where (" + retVal + ")";
            }
            else
            {
                if (retVal.Length == 0)
                    retVal = " and (lev1 = 'none permission') ";
                else
                    retVal = " and (" + retVal + ")";
            }

            return retVal;
        }

        private static string connectionStr;
        public static string ConnectionStr
        {
            get { return connectionStr; }
            set { connectionStr = value; }
        }
    }
}
