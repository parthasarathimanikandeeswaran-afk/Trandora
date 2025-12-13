using Microsoft.Data.SqlClient;
using Trandora.Models;

public class Productrepository
{
    private readonly string connectionstring;

    public Productrepository(IConfiguration configuration)
    {
        connectionstring = configuration.GetConnectionString("mycon");
    }

    public List<Product> GetProducts()
    {
        List<Product> productslist = new List<Product>();

        using (SqlConnection con = new SqlConnection(connectionstring))
        {
            SqlCommand cmd = new SqlCommand("SELECT ProductId, Name, Description, Price, Stock, CategoryId, ImageUrl FROM Products", con);
            con.Open();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Product p = new Product
                    {
                        ProductId = (int)reader["ProductId"],
                        Name = reader["Name"].ToString(),
                        Description = reader["Description"] as string,
                        Price = (decimal)reader["Price"],
                        Stock = (int)reader["Stock"],
                        CategoryId = reader["CategoryId"] as int? ?? 0,
                        ImageUrl = reader["ImageUrl"] as string
                    };

                    productslist.Add(p);
                }
            }
        }

        return productslist;
    }

    public Product GetProductById(int id)
    {
        Product product = null;

        using (SqlConnection con = new SqlConnection(connectionstring))
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Products WHERE ProductId=@Id", con);
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    product = new Product
                    {
                        ProductId = (int)reader["ProductId"],
                        Name = reader["Name"].ToString(),
                        Description = reader["Description"] as string,
                        Price = (decimal)reader["Price"],
                        Stock = (int)reader["Stock"],
                        CategoryId = reader["CategoryId"] as int? ?? 0,
                        ImageUrl = reader["ImageUrl"] as string
                    };
                }
            }
        }

        return product;
    }


    public List<CartItem> GetCartList(int userId)
    {
        List<CartItem> cartItems = new List<CartItem>();

        using (SqlConnection con = new SqlConnection(connectionstring))
        {
            string query = @"
                SELECT c.CartItemId, c.UserId, c.ProductId, c.Quantity,
                       p.Name AS ProductName, p.Price
                FROM CartItems c
                INNER JOIN Products p ON c.ProductId = p.ProductId
                WHERE c.UserId = @UserId";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@UserId", userId);

            con.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    cartItems.Add(new CartItem
                    {
                        CartItemId = (int)reader["CartItemId"],
                        UserId = (int)reader["UserId"],
                        ProductId = (int)reader["ProductId"],
                        ProductName = reader["ProductName"].ToString(),
                        Price = (decimal)reader["Price"],
                        Quantity = (int)reader["Quantity"]
                    });
                }
            }
        }
        return cartItems;
    }
}
