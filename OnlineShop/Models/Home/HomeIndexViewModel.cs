using Newtonsoft.Json;
using OnlineShop.DAL;
using OnlineShop.Models;
using OnlineShop.Repository;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using System.Drawing.Printing;

namespace OnlineShop.Models.Home { 

    public class HomeIndexViewModel
    {
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        masterEntities1 context = new masterEntities1();
        public IPagedList<Tbl_Product> ListOfProducts { get; set; }
        public HomeIndexViewModel CreateModel(string search, string categoryName, int pageSize, int? page)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@search",search??(object)DBNull.Value)
            };
            IPagedList<Tbl_Product> data = context.Database.SqlQuery<Tbl_Product>("GetBySearch @search", param).ToList().ToPagedList(page ?? 1, pageSize);
            return new HomeIndexViewModel()
            {
                ListOfProducts = data
            };

        }


        public HomeIndexViewModel CreateModel2(int? minPrice, int? maxPrice, string sortType, int pageSize, int? page)
        {
            // Build SQL query with parameters for price range and sorting
            string query = "SELECT * FROM Tbl_Product WHERE (@minPrice IS NULL OR Price >= @minPrice) AND (@maxPrice IS NULL OR Price <= @maxPrice)";
            string orderByClause = ""; // Initialize the order by clause

            if (!string.IsNullOrEmpty(sortType))
            {
                if (sortType == "lowToHigh")
                {
                    orderByClause = " ORDER BY Price ASC";
                }
                else if (sortType == "highToLow")
                {
                    orderByClause = " ORDER BY Price DESC";
                }
            }

            query += orderByClause; // Append the order by clause to the query

            // Execute SQL query and retrieve products
            SqlParameter[] parameters = new SqlParameter[]
            {
        new SqlParameter("@minPrice", minPrice ?? (object)DBNull.Value),
        new SqlParameter("@maxPrice", maxPrice ?? (object)DBNull.Value)
            };
            List<Tbl_Product> products = context.Database.SqlQuery<Tbl_Product>(query, parameters).ToList();

            // Paginate the sorted products
            int pageNumber = page ?? 1;
            IPagedList<Tbl_Product> pagedProducts = products.ToPagedList(pageNumber, pageSize);

            // Create the model with paginated products
            return new HomeIndexViewModel
            {
                ListOfProducts = pagedProducts
            };
        }

    }
}
    