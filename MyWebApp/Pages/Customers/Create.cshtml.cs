using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace MyWebApp.Pages.Customers
{
    public class Create : PageModel
    {
        [BindProperty, Required(ErrorMessage = "The First Name is required")]
        public string FirstName { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "The Last Name is required")]
        public string LastName { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "The Email is required")]
        public string Email { get; set; } = "";

        [BindProperty]
        public string? Phone { get; set; } = "";

        [BindProperty]
        public string? Address { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "The Company is required")]
        public string Company { get; set; } = "";

        [BindProperty]
        public string? Notes { get; set; } = "";

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                string connectionString = "Server=DESKTOP-7DVA8FP\\SQLEXPRESS01;Database=master;Trusted_Connection=True;TrustServerCertificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"
                        INSERT INTO [master].[dbo].[customers] 
                        (firstname, lastname, email, phone, address, company, notes, created_at) 
                        VALUES 
                        (@FirstName, @LastName, @Email, @Phone, @Address, @Company, @Notes, GETDATE());";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", FirstName);
                        command.Parameters.AddWithValue("@LastName", LastName);
                        command.Parameters.AddWithValue("@Email", Email);
                        command.Parameters.AddWithValue("@Phone", Phone ?? "");
                        command.Parameters.AddWithValue("@Address", Address ?? "");
                        command.Parameters.AddWithValue("@Company", Company);
                        command.Parameters.AddWithValue("@Notes", Notes ?? "");

                        command.ExecuteNonQuery();
                    }
                }

                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                return Page();
            }
        }
    }
}