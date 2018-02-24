using System;
using System.Data.SqlClient;

namespace ADOExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var firstLetter = Console.ReadLine();

            using (var connection = new SqlConnection("Server=(local);Database=Chinook;Trusted_Connection=True;"))
            //                         ^^ The pipe to which the connection is made
            // The using statement here makes var connection disposable 
            //  that guarentees that the connection will be closed upon succuesses or failures
            {
                var cmd = connection.CreateCommand();
            // ^^ The thing we're sending throught the pipe

            cmd.CommandText = $@"select x.invoiceId,BillingCountry
                from invoice i
                    join InvoiceLine x on x.InvoiceId = i.InvoiceId
                where exists (select TrackId from Track where Name like '{firstLetter}%' and TrackId = x.TrackId)";

            // ^^ sending the string literal containing what we want completed to pass through

            connection.Open();
            // ^^ opening the connection, sending somethiing through the pipe 


                var reader = cmd.ExecuteReader();
                // ^^ saving the resulting reader from the query

                while (reader.Read())
                // ^^ "hey reader, move to the next row / pull the next row to the stack." returns true as long as there is another row
                {
                    /* ex. 1
                    var invoiceId = reader[0];
                         ^^ give me the 0-indexed column, for the row that was just read
                         but it doesn't know what it is so it just defaults to object
                    */
                    var invoiceId = reader.GetInt32(0);
                    /* use the .Get[T] when you know what you're getting back from the query
                     takes in which column it's going to be getting
                     which, for each read, will be the next index column down
                    */

                    var billingCountry = reader["BillingCountry"].ToString();

                    Console.WriteLine("Invoice {0}, is going to {1}", invoiceId, billingCountry);
                }
            }

            // ^^ it is VERY IMPORTANT to close your connections, or else there are more pipes that the connection can handle
            Console.ReadLine();
        }
    }
}
