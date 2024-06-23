using OnlineShop.Models.Home;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using OnlineShop.Models;
using OnlineShop.DAL;
using OnlineShop.Repository;
using System;
using System.Drawing.Printing;
using PagedList;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Text;


namespace OnlineShop.Controllers
{
	public class HomeController : Controller
	{
		masterEntities1 ctx = new masterEntities1();

		public ActionResult Login()
		{

			return View();
		}

		[HttpPost]
		public ActionResult Login(Tbl_Users user)
		{
			if (!ModelState.IsValid)
			{
				return View(user);
			}

			try
			{
				// Check if user exists in the database
				var existingUser = ctx.Tbl_Users.FirstOrDefault(u => u.UserId == user.UserId && u.Password == user.Password);

				if (existingUser != null)
				{
					// User exists, set session variable and redirect to home page
					Session["UsernameSS"] = existingUser.UserId;
					Session["IsLoggedIn"] = true;

					CreateDeliveryRecord(existingUser.UserId);
					return RedirectToAction("Index", "Home");



				}
				else
				{
					// User not found, display error message
					ViewBag.ErrorMessage = "Invalid username or password.";
					return View(user);
				}


			}
			catch (Exception ex)
			{
				// Log the exception
				ViewBag.ErrorMessage = "An error occurred while processing your request.";
				return View(user);
			}

		}

		private void CreateDeliveryRecord(string userId)
		{
			// Check if a delivery record already exists for the provided userId
			bool deliveryExists = ctx.Tbl_Delivery.Any(d => d.UserName == userId);

			if (!deliveryExists)
			{
				// Create a new delivery record
				Tbl_Delivery newDelivery = new Tbl_Delivery();
				newDelivery.UserName = userId;
				ctx.Tbl_Delivery.Add(newDelivery);
				ctx.SaveChanges();
			}


		}

		private void UpdateDeliveryRecord(string username, int orderNumber)
		{
			try
			{
				// Find the delivery record for the provided username
				var deliveryRecord = ctx.Tbl_Delivery.FirstOrDefault(d => d.UserName == username);

				if (deliveryRecord != null)
				{
					// If the OrderId column is null or empty, simply set it to the order number
					if (string.IsNullOrEmpty(deliveryRecord.OrderId))
					{
						deliveryRecord.OrderId = orderNumber.ToString();
					}
					else
					{
						// Append the order number with a comma to the existing OrderId column value
						deliveryRecord.OrderId += "," + orderNumber.ToString();
					}

					ctx.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				// Handle the exception (e.g., log the error)
				Console.WriteLine("Error updating delivery record: " + ex.Message);
				// Optionally, rethrow the exception
				throw;
			}
		}


		public ActionResult IndexCheck()
		{
			return View(ctx.Tbl_Users.ToList());
		}


		public ActionResult Index(string productName, string categoryName, string minPrice, string maxPrice, string sortType, int? page, int? orderNumber)
		{
			HomeIndexViewModel model;
			ViewBag.SortType = sortType;

			// If minPrice or maxPrice is provided, use CreateModel2 for filtering
			if ((!string.IsNullOrEmpty(minPrice) || !string.IsNullOrEmpty(maxPrice)) || !string.IsNullOrEmpty(sortType))

			{
				int pageSize = 8;
				model = new HomeIndexViewModel().CreateModel2(ConvertToInt(minPrice), ConvertToInt(maxPrice), sortType, pageSize, page);
			}
			else
			{
				// Otherwise, use CreateModel for search and sorting
				int pageSize = 8;
				model = new HomeIndexViewModel().CreateModel(productName, categoryName, pageSize, page);
			}

			if (orderNumber != null)
			{
				// Get the username from session
				string username = Session["UsernameSS"] as string;

				// Call UpdateDeliveryRecord
				UpdateDeliveryRecord(username, orderNumber.Value);
				ClearCart();
			}

			return View(model);
		}

		private int? ConvertToInt(string value)
		{
			return string.IsNullOrEmpty(value) ? (int?)null : int.Parse(value);
		}


		public ActionResult AddToCart(int productId)
		{
			if (Session["cart"] == null)
			{
				List<Item> cart = new List<Item>();
				var product = ctx.Tbl_Product.Find(productId);
				cart.Add(new Item()
				{
					product = product,
					Qnantity = 1
				});
				Session["cart"] = cart;

			}
			else
			{
				List<Item> cart = (List<Item>)Session["cart"];
				var product = ctx.Tbl_Product.Find(productId);
				cart.Add(new Item()
				{
					product = product,
					Qnantity = 1
				});
				Session["cart"] = cart;


			}


			return Redirect("Index");
		}

		public ActionResult ClearCart()
		{
			Session["cart"] = null;
			// Redirect to a different view, like the cart view or any other relevant page
			return RedirectToAction("Index", "Home"); // Redirect to home page for example
		}


		public ActionResult RemoveFromCart(int productId)
		{

			List<Item> cart = (List<Item>)Session["cart"];
			Item existingItem = cart.FirstOrDefault(item => item.product.ProcuctId == productId);
			if (existingItem.Qnantity == 1)
			{
              cart.Remove(existingItem);
			}
			else
			{
				existingItem.Qnantity--;
			}

			Session["cart"] = cart;
			return Redirect("Checkout");
		}

		public ActionResult QuantityChange(int productId)
		{
			List<Item> cart = (List<Item>)Session["cart"];
			Item existingItem = cart.FirstOrDefault(item => item.product.ProcuctId == productId);
			existingItem.Qnantity++;
			// Update the cart in session
			Session["cart"] = cart;
			return Redirect("Checkout");
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		public ActionResult Checkout()
		{

			return View();
		}
		public ActionResult CheckoutDetails()
		{

			return View();
		}

		public ActionResult About()
		{
			return View();
		}
		public ActionResult iphone()
		{
			var iphoneProducts = ctx.Tbl_Product.Where(p => p.ProductName.Contains("iPhone")).ToList();

			// Convert the list of products to a paged list
			int pageNumber = 1; // Set the page number
			int pageSize = 10; // Set the page size
			var pagedProducts = iphoneProducts.ToPagedList(pageNumber, pageSize);

			// Create an instance of HomeIndexViewModel and set the ListOfProducts property
			var model = new HomeIndexViewModel
			{
				ListOfProducts = pagedProducts
			};

			// Pass the HomeIndexViewModel instance to the view
			return View("iphone", model);
		}

		public ActionResult ipad()
		{
			var ipadProducts = ctx.Tbl_Product.Where(p => p.ProductName.Contains("ipad")).ToList();

			// Convert the list of products to a paged list
			int pageNumber = 1; // Set the page number
			int pageSize = 10; // Set the page size
			var pagedProducts = ipadProducts.ToPagedList(pageNumber, pageSize);

			// Create an instance of HomeIndexViewModel and set the ListOfProducts property
			var model = new HomeIndexViewModel
			{
				ListOfProducts = pagedProducts
			};

			// Pass the HomeIndexViewModel instance to the view
			return View("ipad", model);
		}

		public ActionResult Watch()
		{
			var ipadProducts = ctx.Tbl_Product.Where(p => p.ProductName.Contains("Watch")).ToList();

			// Convert the list of products to a paged list
			int pageNumber = 1; // Set the page number
			int pageSize = 10; // Set the page size
			var pagedProducts = ipadProducts.ToPagedList(pageNumber, pageSize);

			// Create an instance of HomeIndexViewModel and set the ListOfProducts property
			var model = new HomeIndexViewModel
			{
				ListOfProducts = pagedProducts
			};

			// Pass the HomeIndexViewModel instance to the view
			return View("Watch", model);
		}

		public ActionResult Airpods()
		{
			var ipadProducts = ctx.Tbl_Product.Where(p => p.ProductName.Contains("airpods")).ToList();

			// Convert the list of products to a paged list
			int pageNumber = 1; // Set the page number
			int pageSize = 10; // Set the page size
			var pagedProducts = ipadProducts.ToPagedList(pageNumber, pageSize);

			// Create an instance of HomeIndexViewModel and set the ListOfProducts property
			var model = new HomeIndexViewModel
			{
				ListOfProducts = pagedProducts
			};

			// Pass the HomeIndexViewModel instance to the view
			return View("Airpods", model);
		}

		public ActionResult Mac()
		{
			var ipadProducts = ctx.Tbl_Product.Where(p => p.ProductName.Contains("Mac")).ToList();

			// Convert the list of products to a paged list
			int pageNumber = 1; // Set the page number
			int pageSize = 10; // Set the page size
			var pagedProducts = ipadProducts.ToPagedList(pageNumber, pageSize);

			// Create an instance of HomeIndexViewModel and set the ListOfProducts property
			var model = new HomeIndexViewModel
			{
				ListOfProducts = pagedProducts
			};

			// Pass the HomeIndexViewModel instance to the view
			return View("Mac", model);
		}

		public ActionResult InSaleProd()
		{
			// Retrieve discounted products from your database (assuming there's a field indicating discounts)

			var discountedProducts = ctx.Tbl_Product.Where(p => p.Sale > 0).ToList();




			// Convert the list of products to a paged list
			int pageNumber = 1; // Set the page number
			int pageSize = 10; // Set the page size
			var pagedProducts = discountedProducts.ToPagedList(pageNumber, pageSize);

			// Create an instance of HomeIndexViewModel and set the ListOfProducts property
			var model = new HomeIndexViewModel
			{
				ListOfProducts = pagedProducts
			};

			// Pass the HomeIndexViewModel instance to the view
			return View("InSaleProduct", model);
		}
		public ActionResult MostPopular()
		{
			var featuredProducts = ctx.Tbl_Product.Where(p => p.IsFeatured == true).ToList();

			// Convert the list of products to a paged list
			int pageNumber = 1; // Set the page number
			int pageSize = 10; // Set the page size
			var pagedProducts = featuredProducts.ToPagedList(pageNumber, pageSize);

			// Create an instance of HomeIndexViewModel and set the ListOfProducts property
			var model = new HomeIndexViewModel
			{
				ListOfProducts = pagedProducts
			};

			// Pass the HomeIndexViewModel instance to the view
			return View("MostPopular", model);
		}


		public ActionResult DeliveryDetails()
		{
			// Get the username from session
			string username = Session["UsernameSS"] as string;

			// Retrieve delivery record associated with the username
			var deliveryRecord = ctx.Tbl_Delivery.FirstOrDefault(d => d.UserName == username);

			if (deliveryRecord != null)
			{
				// Split the OrderId string into individual order numbers
				string[] orderIds = deliveryRecord.OrderId.Split(',');

				// Create a list to store the delivery details
				var deliveryDetailsList = new List<Delivery>();

				// Randomly select an order status and tracking number for each split order ID
				string[] orderStatuses = { "preparing", "sent", "received" };
				const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

				Random rnd = new Random();

				foreach (var orderId in orderIds)
				{
					// Randomly select an order status from the list
					string orderStatus = orderStatuses[rnd.Next(orderStatuses.Length)];

					// Generate a random tracking number (a combination of numbers and letters)
					string trackingNumber = new string(Enumerable.Repeat(chars, 10)
						.Select(s => s[rnd.Next(s.Length)]).ToArray());

					// Create a Delivery model instance and populate it with data
					var deliveryDetails = new Delivery
					{
						UserName = username,
						OrderStatus = orderStatus,
						OrderTracking = trackingNumber,
						OrderId = orderId
					};

					// Add the delivery details to the list
					deliveryDetailsList.Add(deliveryDetails);
				}

				// Pass the list of delivery details to the view
				return View(deliveryDetailsList);
			}
			else
			{
				// Handle case when delivery record is not found
				// You can redirect the user to an error page or display a message
				return RedirectToAction("Error");
			}
		}


	}
}
