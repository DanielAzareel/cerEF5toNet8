using BusinessModel.Persistence.CertificadosElectronicosMS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.DataAccess
{
    public class StoredProcedure
    {
        public static string Merged(DataTable dtMerge, string sNombreDataTable, string sNombreProcedure, string sNombreVariable)
        {
            SqlDataReader reader;

            string sMensaje = "";
            try
            {
                using (var conn = new CertificadosMediaSuperiorEntities ())
                {
                    var SQLConn = (SqlConnection)conn.Database.Connection;
                    SqlCommand insertCommand = new SqlCommand(sNombreProcedure, SQLConn);
                    SQLConn.Open();

                    SqlParameter tvpParam = insertCommand.Parameters.AddWithValue("@" + sNombreVariable, dtMerge);
                    tvpParam.SqlDbType = SqlDbType.Structured;
                    tvpParam.TypeName = sNombreDataTable;
                    insertCommand.CommandType = CommandType.StoredProcedure;
                    reader = insertCommand.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            sMensaje = reader.GetString(0);
                            break;
                        }
                    }
                    else
                    {
                        sMensaje = "Error";
                    }
                }
            }
            catch (Exception ex)
            {
                sMensaje = ex.Message;
                Console.WriteLine(ex.Message);
            }
            return sMensaje;
        }

        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                ///if (prop.ComponentType.IsSealed) {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                /// }
            }
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
    }
}
