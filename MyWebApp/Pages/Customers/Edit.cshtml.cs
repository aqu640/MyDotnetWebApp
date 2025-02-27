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
    public class Edit : PageModel
    {
        [BindProperty]
        public int Id { get; set; }

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

        public string ErrorMessage { get; set; } = "";

        public void OnGet(int id)
        {
            try
            {
                string connectionString = "Server=DESKTOP-7DVA8FP\\SQLEXPRESS01;Database=master;Trusted_Connection=True;TrustServerCertificate=True;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM [master].[dbo].[customers] WHERE id = @id;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Id = reader.GetInt32(0);
                                FirstName = reader.GetString(1);
                                LastName = reader.GetString(2);
                                Email = reader.GetString(3);
                                Phone = reader.IsDBNull(4) ? null : reader.GetString(4);
                                Address = reader.IsDBNull(5) ? null : reader.GetString(5);
                                Company = reader.GetString(6);
                                Notes = reader.IsDBNull(7) ? null : reader.GetString(7);
                            }
                            else
                            {
                                Response.Redirect("/Customers/index");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }

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
                        UPDATE [master].[dbo].[customers] 
                        SET 
                            firstname = @FirstName,
                            lastname = @LastName,
                            email = @Email,
                            phone = @Phone,
                            address = @Address,
                            company = @Company,
                            notes = @Notes
                        WHERE id = @Id;";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Id", Id);
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
                ModelState.AddModelError("", "Error updating customers.");
                return Page();
            }
        }
    }
}