using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyWebApp.Pages.Customers
{
    public class Delete : PageModel
    {
        public string ErrorMessage { get; set; } = "";

        // OnGet 方法：顯示確認頁面
        public IActionResult OnGet(int id)
        {
            // 直接刪除而不顯示確認頁面，可以取消下面的註解
            /*
            try
            {
                DeleteCustomers(id);
                return RedirectToPage("Index");
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
            */

            // 顯示確認頁面
            return Page();
        }

        public IActionResult OnPost(int id)
        {
            try
            {
                DeleteCustomers(id);
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }

        private void DeleteCustomers(int id)
        {
            try
            {
                string connectionString = "Server=DESKTOP-7DVA8FP\\SQLEXPRESS01;Database=master;Trusted_Connection=True;TrustServerCertificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM [master].[dbo].[customers] WHERE id = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting customers: {ex.Message}");
                throw;
            }
        }
    }
}