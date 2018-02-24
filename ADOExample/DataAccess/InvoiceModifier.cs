using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADOExample.DataAccess
{
    class InvoiceModifier
    {
        readonly string _connectionString = ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString;

        public bool Delete(int invoiceId)
        {
            if (!DeleteLineItems(invoiceId))
            {
                return false;
            };

            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"Delete
                                    From Invoice
                                    Where InvoiceId = @invoiceId";

                var invoiceIdParam = new SqlParameter("@invoiceId", SqlDbType.Int);
                invoiceIdParam.Value = invoiceId;
                cmd.Parameters.Add(invoiceIdParam);

                connection.Open();

                var result = cmd.ExecuteNonQuery();

                return result == 1;
            }
        }

        bool DeleteLineItems(int invoiceId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"Delete
                                    From InvoiceLine
                                    Where InvoiceId = @invoiceId";

                var invoiceIdParam = new SqlParameter("@invoiceId", SqlDbType.Int);
                invoiceIdParam.Value = invoiceId;
                cmd.Parameters.Add(invoiceIdParam);

                connection.Open();

                var result = cmd.ExecuteNonQuery();

                return result >= 1;
            }
        }
    }
}
