using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace MyWebApp.Pages.Customers
{
    public class Index : PageModel
    {
        public List<CustomersInfo> CustomersList { get; set; } = [];
        public void OnGet()
        {
            try
            {
                // 連接到資料庫
                string connectionString = "Server=DESKTOP-7DVA8FP\\SQLEXPRESS01;Database=master;Trusted_Connection=True;TrustServerCertificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT * FROM [master].[dbo].[customers] ORDER BY Id DESC";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CustomersInfo customersInfo = new CustomersInfo();
                                {
                                    customersInfo.Id = reader.GetInt32(0);
                                    customersInfo.FirstName = reader.GetString(1);
                                    customersInfo.LastName = reader.GetString(2);
                                    customersInfo.Email = reader.GetString(3);
                                    customersInfo.Phone = reader.GetString(4);
                                    customersInfo.Address = reader.GetString(5);
                                    customersInfo.Company = reader.GetString(6);
                                    customersInfo.Notes = reader.GetString(7);
                                    customersInfo.CreatedAt = reader.GetDateTime(8).ToString();
                                    CustomersList.Add(customersInfo);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");  // 改用 Debug.WriteLine
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");  // 新增堆疊追蹤
            }
        }

        public class CustomersInfo
        {
            public int Id { get; set; }
            public string FirstName { get; set; } = "";
            public string LastName { get; set; } = "";
            public string Email { get; set; } = "";
            public string Phone { get; set; } = "";
            public string Address { get; set; } = "";
            public string Company { get; set; } = "";
            public string Notes { get; set; } = "";

            public string CreatedAt { get; set; } = "";

        }


    }
}