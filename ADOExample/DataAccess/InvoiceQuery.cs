using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using ADOExample.DataAccess.Models;

namespace ADOExample.DataAccess
{
    class InvoiceQuery
    {
        public List<Invoice> GetInvoiceByTrackFirstLetter(string firstCharacter) {

            using (var connection = new SqlConnection("Server=(local);Database=Chinook;Trusted_Connection=True;"))
            {
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = $@"select i.*
                                    from invoice i
                                        join InvoiceLine x on x.InvoiceId = i.InvoiceId
                                    where exists (select TrackId from Track 
                                        where Name like @FirstLetter + '%' and TrackId = x.TrackId)";

                var firstLeterParam = new SqlParameter("@FirstLetter", SqlDbType.NVarChar);
                firstLeterParam.Value = firstCharacter;
                cmd.Parameters.Add(firstLeterParam);
                // ^^ properly Paramater-izing a sql Command
                // remove the chance of sql injection by the user trying to ruin your db

                var reader = cmd.ExecuteReader();

                var invoices = new List<Invoice>();

                while (reader.Read())
                {
                    var invoice = new Invoice
                    {
                        InvoiceId = int.Parse(reader["InvoiceId"].ToString()),
                        CustomerId = int.Parse(reader["CustomerId"].ToString()),
                        InvoiceDate = Convert.ToDateTime(reader["InvoiceDate"].ToString()),
                        BillingAddress = reader["BillingAddress"].ToString(),
                        BillingCity = reader["BillingCity"].ToString(),
                        BillingState = reader["BillingState"].ToString(),
                        BillingCountry = reader["BillingCountry"].ToString(),
                        BillingPostalCode = reader["BillingPostalCode"].ToString(),
                        Total = double.Parse(reader["Total"].ToString())
                    };

                invoices.Add(invoice);

                }
                return invoices;
            }
        }
    }

}
