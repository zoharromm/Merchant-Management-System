using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class APPConfig
    {
        public dynamic GetAppConfigValue(string StoreName, string ConfigName)
        {
            dynamic Result = null;
            DB db = new DB();
            try
            {

                SqlDataReader _Reader = null;
                var _Con = db.Connection();
                SqlCommand _CMD = new SqlCommand("select top 1 configvalue from appconfig join Store on store.StoreID=appconfig.storeid where appconfig.name='" + ConfigName + "' and store.StoreName='" + StoreName + "'");
                if (_Con.State == ConnectionState.Closed)
                    _Con.Open();
                _CMD.Connection = _Con;
                _CMD.CommandType = CommandType.Text;
                _Reader = _CMD.ExecuteReader(CommandBehavior.CloseConnection);
                if (_Reader.HasRows)
                {
                    while (_Reader.Read())
                    {
                        Result = _Reader[0];
                    }
                }
                _Reader.Close();
            }
            catch
            { }
            return Result;

        }
    }
}
