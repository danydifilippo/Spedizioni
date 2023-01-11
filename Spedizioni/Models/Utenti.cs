using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Spedizioni.Models
{
    public class Utenti
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        public bool Autenticato(string username, string password)
        {
            SqlConnection con = Shared.GetConnection();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM [USER] WHERE Username = @username and [Password]=@Password", con);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
            finally
            {
                con.Close();
            }
            return false;
        }
    }
}