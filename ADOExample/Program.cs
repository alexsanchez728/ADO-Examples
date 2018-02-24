using ADOExample.DataAccess;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ADOExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var firstLetter = Console.ReadLine();
            var invoiceQuery = new InvoiceQuery();
            var invoices = invoiceQuery.GetInvoiceByTrackFirstLetter(firstLetter);

            foreach (var invoice in invoices)
            {
                Console.WriteLine("Invoices Id: {0} was shipped to {1}", invoice.InvoiceId, invoice.BillingAddress);
            }

            var invoiceModifier = new InvoiceModifier();

            invoiceModifier.Delete(9);

            Console.ReadLine();
        }
    }
}
