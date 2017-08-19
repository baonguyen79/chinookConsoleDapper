using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace ChinookConsoleApp
{
    public class DeleteEmployee
    {
        public void Delete()
        {
            var employeeList = new ListEmployees();
            var firedEmployee = employeeList.List("Pick an employee to transition:");

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            {
                //connection.Open();
                var sqlDelete = $@"Delete From Employee where EmployeeId = @EmployeeId";

                try
                {
                    var affectedRows = connection.Execute(sqlDelete, new { EmployeeId = firedEmployee });

                    if (affectedRows == 1)
                    {
                        Console.WriteLine("Success");
                    }
                    else if (affectedRows > 1)
                    {
                        Console.WriteLine("AAAAHHHHHH");
                    }
                    else
                    {
                        Console.WriteLine("Failed to find a matching Id");
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