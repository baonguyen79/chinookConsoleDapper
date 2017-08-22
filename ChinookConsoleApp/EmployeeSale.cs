using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace ChinookConsoleApp
{
    public class saleResult
    {
        public string empName { get; set; }
        public decimal SaleTotal { get; set; }
    }

    public class EmployeeSale
    {
        // List years that have sale
        public void ListYears()
        {
            Console.Clear();

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            {
                var sql = $@"select distinct Year(InvoiceDate)
                                from Invoice
                                order by Year(InvoiceDate)";
                try
                {
                    var yearHasSale = connection.Query<int>(sql).ToArray();
                    foreach (var year in yearHasSale)
                    {
                        Console.WriteLine(year);
                    }

                    Console.WriteLine();
                    Console.WriteLine("Please select from availabel years ");
                    Console.WriteLine();

                    ListSale(int.Parse(Console.ReadLine()));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        // List total sale per employees for selected year
        public void ListSale(int yearSelect)
        {
            Console.Clear();
            Console.WriteLine($"Sale for {yearSelect} per employees");
            Console.WriteLine();

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            {
                //connection.Open();
                var sql = $@"select EmpName
	                              , SaleTotal
                               from (
	                                select Employee.FirstName + ' ' + Employee.LastName as EmpName
		                                , sum(Invoice.Total)                             as SaleTotal
	                                from Employee
	                                join Customer on Customer.SupportRepId = Employee.EmployeeId
	                                join Invoice on Invoice.CustomerId = Customer.CustomerId
	                                where Year(InvoiceDate) = @year
	                                group by Employee.FirstName + ' ' + Employee.LastName
                                    ) as T";

                try
                {
                    var saleResult = connection.Query<saleResult>(sql, new { year = yearSelect });

                    foreach (var employee in saleResult)
                    {
                        Console.WriteLine($"{employee.empName}   {employee.SaleTotal}");
                    }

                    Console.WriteLine("Press enter to return to the menu");
                    Console.ReadLine();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}