using Microsoft.Data.SqlClient;

namespace Trandora.Models
{
    public class UserRepository
    {
        private string? connectionstring;
        public UserRepository(IConfiguration configuration)
        {
            connectionstring = configuration.GetConnectionString("mycon");
        }
        public void Register(User u)
        {
            using SqlConnection con = new SqlConnection(connectionstring);
            string query = "INSERT INTO Users(Username,  Email, Password) VALUES(@Name, @Email, @Password)";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Name", u.Name);
            cmd.Parameters.AddWithValue("@Email", u.Email);
            cmd.Parameters.AddWithValue("@Password", u.Password);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public bool ValidateUser(string email, string password)
        {

            using SqlConnection con = new SqlConnection(connectionstring);
            string query = "SELECT COUNT(*) FROM Users WHERE Email=@Email AND Password=@Password";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Password", password);
            con.Open();
            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }


        public string GetUserNameByEmail(string email)
        {
            using SqlConnection con = new SqlConnection(connectionstring);
            string query = "SELECT Username FROM Users WHERE Email = @Email";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Email", email);
            con.Open();
            var result = cmd.ExecuteScalar();
            return result?.ToString() ?? "";
        }

        public int GetUserIdByEmail(string email)
        {
            using SqlConnection con = new SqlConnection(connectionstring);
            string query = "SELECT UserId FROM Users WHERE Email = @Email";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Email", email);
            con.Open();
            var result = cmd.ExecuteScalar();
            return result != null ? Convert.ToInt32(result) : 0;
        }

    }
}
