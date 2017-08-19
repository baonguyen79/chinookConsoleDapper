using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;

namespace ChinookConsoleApp
{
    public class UpdateEmployee
    {
        public void Update()
        {
            var employeeList = new ListEmployees();
            int updateEmployeeId = employeeList.List("Pick an employee to update:");

            Console.WriteLine($"Employee {updateEmployeeId} , enter last name");
            var lastName = Console.ReadLine();

            Console.WriteLine($"Employee {updateEmployeeId} , enter first name");
            var firstName = Console.ReadLine();

            Console.WriteLine($"Update employee id {updateEmployeeId} {lastName} {firstName}");

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            {
                //connection.Open();
                               
                try
                {

                    var rowsAffected = connection.Execute($@"Update Employee
                                                                set FirstName = @firstName,
                                                                    LastName = @lastName
                                                             Where EmployeeId = @employeeId",
                                                          
                        new { firstName = firstName, lastName = lastName, employeeId = updateEmployeeId });

                    Console.WriteLine(rowsAffected != 1 ? "Update Failed" : "Success!");


                    if (rowsAffected == 1)
                    {
                        Console.WriteLine("Success");
                    }
                    else if (rowsAffected > 1)
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
