using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
namespace BusinessLayer
{
    public class DB
    {
        public SqlConnection Connection()
        {
            SqlConnection Connection = new SqlConnection();
            Connection.ConnectionString = ConfigurationManager.AppSettings["connectionstring"].ToString();
            return Connection;
        }
        public SqlDataReader GetDR(string Command)
        {
            SqlDataReader _Reader = null;
            try
            {
                var _Con = Connection();
                SqlCommand _CMD = new SqlCommand(Command);
                if (_Con.State == ConnectionState.Closed)
                    _Con.Open();
                _CMD.Connection = _Con;
                _CMD.CommandType = CommandType.Text;
                _Reader = _CMD.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch
            { }
            return _Reader;
        }
        public void ExecuteCommand(string Command)
        {

            try
            {
                using (var _Con = Connection())
                {
                    using (SqlCommand _CMD = new SqlCommand(Command))
                    {
                        if (_Con.State == ConnectionState.Closed)
                            _Con.Open();
                        _CMD.Connection = _Con;
                        _CMD.CommandType = CommandType.Text;
                        _CMD.ExecuteNonQuery();
                    }

                }
            }
            catch
            { }

        }
        public DataSet GetDataset(string CommandName, CommandType _CmdType, string Parameters)
        {
            DataSet _DsRecords = new DataSet();
            using (var _Con = Connection())
            {
                using (SqlCommand _CMD = new SqlCommand(CommandName))
                {
                    if (_Con.State == ConnectionState.Closed)
                        _Con.Open();
                    _CMD.Connection = _Con;
                    _CMD.CommandType = _CmdType;
                    if (Parameters.Trim().Length > 0)
                    {
                        string[] Param = Parameters.Split(':');
                        foreach (string _Prm in Param)
                        {
                            _CMD.Parameters.AddWithValue(_Prm.Split(',')[0], _Prm.Split(',')[1]);
                        }
                    }
                    SqlDataAdapter _Adp = new SqlDataAdapter(_CMD);
                    _Adp.Fill(_DsRecords);
                    _Adp.Dispose();
                }

            }
            return _DsRecords;
        }
        public bool ProductInsert(string StoreName, DataTable exceldt)
        {
            using (var con = Connection())
            {
                using (SqlCommand Cmd = new SqlCommand())
                {
                    try
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        Cmd.Connection = con;
                        Cmd.CommandType = CommandType.StoredProcedure;
                        Cmd.CommandTimeout = 0;
                        Cmd.CommandText = "MarkHub_ProductsInsert";
                        Cmd.Parameters.AddWithValue("@StoreName", StoreName);
                        Cmd.Parameters.AddWithValue("@Products", exceldt);
                        Cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public DataSet GetDatasetByPassDatatable(string ProcName, DataTable exceldt, string DatTableVariable, CommandType _Type, string Parameters)
        {
            DataSet _DS = new DataSet();
            using (var con = Connection())
            {
                using (SqlCommand Cmd = new SqlCommand())
                {
                    try
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        Cmd.Connection = con;
                        Cmd.CommandType = _Type;
                        Cmd.CommandTimeout = 0;
                        Cmd.CommandText = ProcName;
                        if (Parameters.Trim().Length > 0)
                        {
                            string[] Param = Parameters.Split(':');
                            foreach (string _Prm in Param)
                            {
                                Cmd.Parameters.AddWithValue(_Prm.Split(',')[0], _Prm.Split(',')[1]);
                            }
                        }
                        Cmd.Parameters.AddWithValue(DatTableVariable, exceldt);
                        SqlDataAdapter _Adp = new SqlDataAdapter(Cmd);
                        _Adp.Fill(_DS);
                    }
                    catch
                    {
                        return _DS;
                    }
                }
            }
            return _DS;
        }
    }


}
