using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using Trandora.Models;

namespace Trandora.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserRepository userrepo;
        private readonly Productrepository prorep;
        private readonly string _config;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,UserRepository userrepo,Productrepository prod, IConfiguration con)
        {
            _logger = logger;
            this._config = con.GetConnectionString("mycon");
            this.userrepo = userrepo;
            this.prorep = prod;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login()
        {
            if (Request.Cookies["userid"] != null && Request.Cookies["username"] != null)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (userrepo.ValidateUser(email, password))
            {
                string name = userrepo.GetUserNameByEmail(email);
                int userId = userrepo.GetUserIdByEmail(email);

              
                Response.Cookies.Delete("username");
                Response.Cookies.Delete("userid");

           
                CookieOptions option = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(1),
                };

                Response.Cookies.Append("username", name, option);
                Response.Cookies.Append("userid", userId.ToString(), option);

                return RedirectToAction("Index");
            }

            ViewBag.Error = "Invalid email or password!";
            return View();
        }

        public IActionResult Register()
        {

            if (Request.Cookies["userid"] != null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Register(User us)
        {
               if (ModelState.IsValid)
            {
                userrepo.Register(us);
                return RedirectToAction("Login");
            }
            return View(us);
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("username");
            Response.Cookies.Delete("userid");
            return RedirectToAction("Login");
        }


       

        public IActionResult Products()
        {
            List<Product> products = prorep.GetProducts();
            return View(products);
        }

        public IActionResult ProductDetails(int productId)
        {
            var product = prorep.GetProductById(productId);
            return View(product);
        }

      
        public IActionResult Cart()
        {
            string? userIdString = Request.Cookies["userid"];
            if (string.IsNullOrEmpty(userIdString))
            {
                 return RedirectToAction("Login");
            }

            int userId = int.Parse(userIdString);
            var cartList = new List<CartItem>();

            using var con = new SqlConnection(_config);
            con.Open();
            string sql = @"
        SELECT ci.CartItemId, ci.Quantity, ci.ProductId, p.Name AS ProductName, p.Price, c.UserId
        FROM CartItems ci
        JOIN Carts c ON ci.CartId = c.CartId
        JOIN Products p ON ci.ProductId = p.ProductId
        WHERE c.UserId = @UserId";

            var cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@UserId", userId);

            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                cartList.Add(new CartItem
                {
                    CartItemId = (int)rdr["CartItemId"],
                    Quantity = (int)rdr["Quantity"],
                    ProductId = (int)rdr["ProductId"],
                    ProductName = rdr["ProductName"].ToString(),
                    Price = (decimal)rdr["Price"]
                });
            }
            return View(cartList);
        }
         [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            string? userIdString = Request.Cookies["userid"];
            if (string.IsNullOrEmpty(userIdString))
                return RedirectToAction("Login");

            int userId = Convert.ToInt32(userIdString);

            using var con = new SqlConnection(_config);
            con.Open();

            
            string check = "SELECT CartId FROM Carts WHERE UserId = @UserId";
            using var cmdCheck = new SqlCommand(check, con);
            cmdCheck.Parameters.AddWithValue("@UserId", userId);
            object? obj = cmdCheck.ExecuteScalar();

            int cartId;
            if (obj == null)
            {
                string ins = "INSERT INTO Carts (UserId) OUTPUT INSERTED.CartId VALUES (@UserId)";
                using var cmdIns = new SqlCommand(ins, con);
                cmdIns.Parameters.AddWithValue("@UserId", userId);
                cartId = (int)cmdIns.ExecuteScalar();
            }
            else
            {
                cartId = (int)obj;
            }

            
            string insertItem = @"
    INSERT INTO CartItems (CartId, ProductId, Quantity)
    VALUES (@CartId, @ProductId, @Quantity)";
            using var cmdItem = new SqlCommand(insertItem, con);
            cmdItem.Parameters.AddWithValue("@CartId", cartId);
            cmdItem.Parameters.AddWithValue("@ProductId", productId);
            cmdItem.Parameters.AddWithValue("@Quantity", quantity);
            cmdItem.ExecuteNonQuery();


            return RedirectToAction("Cart");

        }

        public static class TempCart
        {
            public static List<CartItem> CartItems { get; set; } = new List<CartItem>();
        }



        public IActionResult Remove(int id)
        {
            var userIdCookie = Request.Cookies["userid"];
            if (string.IsNullOrEmpty(userIdCookie))
            {
                return RedirectToAction("Login");
            }

            int userId = int.Parse(userIdCookie);

            using (var con = new SqlConnection(_config))
            {
                con.Open();

                 string cartIdQuery = "SELECT CartId FROM Carts WHERE UserId = @UserId";
                SqlCommand cartCmd = new SqlCommand(cartIdQuery, con);
                cartCmd.Parameters.AddWithValue("@UserId", userId);

                var cartIdObj = cartCmd.ExecuteScalar();
                if (cartIdObj == null)
                {
                        return RedirectToAction("Cart");
                }

                int cartId = (int)cartIdObj;

                string deleteQuery = "DELETE FROM CartItems WHERE CartItemId = @Id AND CartId = @CartId";
                SqlCommand deleteCmd = new SqlCommand(deleteQuery, con);
                deleteCmd.Parameters.AddWithValue("@Id", id);
                deleteCmd.Parameters.AddWithValue("@CartId", cartId);

                deleteCmd.ExecuteNonQuery();
            }

            return RedirectToAction("Cart");
        }







        public IActionResult Checkout()
        {
            string? userIdString = Request.Cookies["userid"];
            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login");
            }

            int userId = Convert.ToInt32(userIdString);
            var cartItems = new List<CartItem>();

            using (SqlConnection con = new SqlConnection(_config))
            {
                con.Open();

                string query = @"
            SELECT 
                ci.CartItemId, 
                p.Name AS ProductName, 
                ci.Quantity, 
                p.Price, 
                c.UserId
            FROM CartItems ci
            INNER JOIN Carts c ON ci.CartId = c.CartId
            INNER JOIN Products p ON ci.ProductId = p.ProductId
            WHERE c.UserId = @UserId";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cartItems.Add(new CartItem
                            {
                                CartItemId = (int)reader["CartItemId"],
                                ProductName = reader["ProductName"].ToString(),
                                Quantity = (int)reader["Quantity"],
                                Price = (decimal)reader["Price"],
                                UserId = (int)reader["UserId"]
                            });
                        }
                    }
                }
            }

            return View(cartItems); 
        }



        [HttpPost]
        public IActionResult PlaceOrder(UserOrderInfo userInfo)
        {
            string? userIdString = Request.Cookies["userid"];
            if (string.IsNullOrEmpty(userIdString))
                return RedirectToAction("Login");

            int userId = int.Parse(userIdString);

            if (string.IsNullOrWhiteSpace(userInfo.Name) ||
                string.IsNullOrWhiteSpace(userInfo.Address) ||
                string.IsNullOrWhiteSpace(userInfo.Phone))
            {
                ModelState.AddModelError("", "All fields are required.");
                return View("Checkout"); 
            }

            using (SqlConnection con = new SqlConnection(_config))
            {
                con.Open();

              
                string cartQuery = @"
                    SELECT ci.CartItemId, ci.ProductId, ci.Quantity, p.Price
                    FROM CartItems ci
                      INNER JOIN Carts c ON ci.CartId = c.CartId
                      INNER JOIN Products p ON ci.ProductId = p.ProductId
                    WHERE c.UserId = @UserId";
                var orderItems = new List<OrderItem>();
                decimal total = 0;

                using (SqlCommand cmd = new SqlCommand(cartQuery, con))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int productId = (int)reader["ProductId"];
                            int qty = (int)reader["Quantity"];
                            decimal price = (decimal)reader["Price"];
                            total += price * qty;

                            orderItems.Add(new OrderItem
                            {
                                ProductId = productId,
                                Quantity = qty,
                                Price = price
                            });
                        }
                    }
                }

                if (orderItems.Count == 0)
                {
                   
                    return RedirectToAction("Checkout");
                }

             
                string insertOrder = @"
                    INSERT INTO Orders (UserId, Name, Address, Phone, Total)
                    OUTPUT INSERTED.OrderId
                    VALUES (@UserId, @Name, @Address, @Phone, @Total)";

                int orderId;
                using (SqlCommand cmd = new SqlCommand(insertOrder, con))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Name", userInfo.Name);
                    cmd.Parameters.AddWithValue("@Address", userInfo.Address);
                    cmd.Parameters.AddWithValue("@Phone", userInfo.Phone);
                    cmd.Parameters.AddWithValue("@Total", total);

                    orderId = (int)cmd.ExecuteScalar();
                }

                foreach (var item in orderItems)
                {
                    string insertItem = @"
                        INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
                        VALUES (@OrderId, @ProductId, @Quantity, @Price)";
                    using (SqlCommand cmd = new SqlCommand(insertItem, con))
                    {
                        cmd.Parameters.AddWithValue("@OrderId", orderId);
                        cmd.Parameters.AddWithValue("@ProductId", item.ProductId);
                        cmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                        cmd.Parameters.AddWithValue("@Price", item.Price);
                        cmd.ExecuteNonQuery();
                    }
                }

            
                string clearCart = @"
                    DELETE ci
                    FROM CartItems ci
                    INNER JOIN Carts c ON ci.CartId = c.CartId
                    WHERE c.UserId = @UserId";

                using (SqlCommand cmd = new SqlCommand(clearCart, con))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.ExecuteNonQuery();
                }
            }

            return RedirectToAction("OrderSuccess");
        }



        public IActionResult AllOrders()
        {
            var orders = new List<Order>();

            using (SqlConnection con = new SqlConnection(_config))
            {
                con.Open();

                string query = @"
            SELECT OrderId, UserId, Name, Address, Phone, Total
            FROM Orders";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(new Order
                            {
                                OrderId = (int)reader["OrderId"],
                                UserId = (int)reader["UserId"],
                                Name = reader["Name"].ToString(),
                                Address = reader["Address"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                Total = (decimal)reader["Total"],
                            });
                        }
                    }
                }
            }

            return View(orders);
        }




        public IActionResult OrderSuccess()
        {
            return View();
        }



    

        public IActionResult Orders(int orderId)
        {
            Order order = null;

            using (SqlConnection con = new SqlConnection(_config))
            {
                con.Open();

             
                string orderQuery = @"
            SELECT OrderId, UserId, Name, Address, Phone, Total, CreatedAt
            FROM Orders
            WHERE OrderId = @OrderId";

                using (SqlCommand cmd = new SqlCommand(orderQuery, con))
                {
                    cmd.Parameters.AddWithValue("@OrderId", orderId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            order = new Order
                            {
                                OrderId = (int)reader["OrderId"],
                                UserId = (int)reader["UserId"],
                                Name = reader["Name"].ToString(),
                                Address = reader["Address"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                Total = (decimal)reader["Total"],
                                CreatedAt = (DateTime)reader["CreatedAt"]
                            };
                        }
                    }
                }

                if (order == null)
                    return NotFound();

                string itemQuery = @"
            SELECT OrderItemId, ProductId, Quantity, Price
            FROM OrderItems
            WHERE OrderId = @OrderId";

                using (SqlCommand cmd = new SqlCommand(itemQuery, con))
                {
                    cmd.Parameters.AddWithValue("@OrderId", orderId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            order.Items.Add(new OrderItem
                            {
                                OrderItemId = (int)reader["OrderItemId"],
                                ProductId = (int)reader["ProductId"],
                                Quantity = (int)reader["Quantity"],
                                Price = (decimal)reader["Price"]
                            });
                        }
                    }
                }
            }

            return View(order); 
        }


        public IActionResult OrderDetails(int orderId)
        {
            Order order = null;

            using (SqlConnection con = new SqlConnection(_config))
            {
                con.Open();

              
                string orderQuery = @"
            SELECT OrderId, UserId, Name, Address, Phone, Total
            FROM Orders
            WHERE OrderId = @OrderId";

                using (SqlCommand cmd = new SqlCommand(orderQuery, con))
                {
                    cmd.Parameters.AddWithValue("@OrderId", orderId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            order = new Order
                            {
                                OrderId = (int)reader["OrderId"],
                                UserId = (int)reader["UserId"],
                                Name = reader["Name"].ToString(),
                                Address = reader["Address"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                Total = (decimal)reader["Total"],
                              
                            };
                        }
                    }
                }

                if (order == null)
                    return NotFound();

             
                string itemQuery = @"
            SELECT OrderItemId, ProductId, Quantity, Price
            FROM OrderItems
            WHERE OrderId = @OrderId";

                using (SqlCommand cmd = new SqlCommand(itemQuery, con))
                {
                    cmd.Parameters.AddWithValue("@OrderId", orderId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            order.Items.Add(new OrderItem
                            {
                                OrderItemId = (int)reader["OrderItemId"],
                                ProductId = (int)reader["ProductId"],
                                Quantity = (int)reader["Quantity"],
                                Price = (decimal)reader["Price"]
                            });
                        }
                    }
                }
            }

            return View(order); 
        }


       
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
