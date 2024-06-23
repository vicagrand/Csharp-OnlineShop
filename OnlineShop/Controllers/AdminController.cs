using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OnlineShop.DAL;
using OnlineShop.Models;
using OnlineShop.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace OnlineShop.Controllers
{
	public class AdminController : Controller
	{
		// GET: Admin
		public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
		public ActionResult Dashbord()
		{
			return View();
		}
		public ActionResult Categories()
		{
			List<Tbl_Category> allcategories = _unitOfWork.GetRepositoryInstance<Tbl_Category>().GetAllRecordsIQueryable().Where(i => i.IsDelete == false).ToList();
			return View(allcategories);
		}
		
		public ActionResult UpdateCategory(CategoryDetail category)
		{
			if (ModelState.IsValid)
			{
				if (category.CategoryId > 0) // Existing category
				{
					// Update existing category
					Tbl_Category existingCategory = _unitOfWork.GetRepositoryInstance<Tbl_Category>().GetFirstorDefault(category.CategoryId);
					if (existingCategory != null)
					{
						existingCategory.CategoryName = category.CategoryName;
						_unitOfWork.GetRepositoryInstance<Tbl_Category>().Update(existingCategory);
					}
				
				}
				else // New category
				{
					// Set IsActive to true and IsDelete to false for new category
					category.IsActive = true;
					category.IsDelete = false;

					// Add new category
					Tbl_Category newCategory = new Tbl_Category
					{
						CategoryName = category.CategoryName,
						IsActive = category.IsActive,
						IsDelete = category.IsDelete
						// Assign other properties accordingly
					};
					_unitOfWork.GetRepositoryInstance<Tbl_Category>().Add(newCategory);
				}

				// Redirect to Categories action after adding/updating the category
				return RedirectToAction("Categories");
			}
			// If model state is not valid, return to the UpdateCategory view with the same model to display validation errors
			return View("UpdateCategory", category);
		}
		public ActionResult Product()
		{
			return View(_unitOfWork.GetRepositoryInstance<Tbl_Product>().GetProduct());
			

		}
		public ActionResult ProductEdit(int ProductId)
		{
			ViewBag.CategoryList = GetCategory();
			return View(_unitOfWork.GetRepositoryInstance<Tbl_Product>().GetFirstorDefault(ProductId));

		}
		[HttpPost]

		public ActionResult ProductEdit(Tbl_Product tbl, HttpPostedFileBase file)
		{
			string pic = null;
			if (file != null)
			{
				pic = System.IO.Path.GetFileName(file.FileName);
				string path = System.IO.Path.Combine(Server.MapPath("~/ProductImg"), pic);
				file.SaveAs(path);
			}
			tbl.ProductImg = file!=null?pic:tbl.ProductImg;
			tbl.ModifiedDate = DateTime.Now;
			_unitOfWork.GetRepositoryInstance<Tbl_Product>().Update(tbl);
			return RedirectToAction("Product");
		}

		public ActionResult ProductAdd()
		{
			ViewBag.CategoryList = GetCategory();
			return View();
		}
		[HttpPost]

		public ActionResult ProductAdd(Tbl_Product tbl,HttpPostedFileBase file)
		{
			string pic=null;
			if (file != null)
			{
				 pic = System.IO.Path.GetFileName(file.FileName);
				string path=System.IO.Path.Combine(Server.MapPath("~/ProductImg"),pic);
				file.SaveAs(path);
			}
			tbl.ProductImg = pic;
			tbl.CreateDate = DateTime.Now;
			_unitOfWork.GetRepositoryInstance<Tbl_Product>().Add(tbl);
			return RedirectToAction("Product");
		}
		public List<SelectListItem> GetCategory()
		{
			List<SelectListItem> list = new List<SelectListItem>();
			var cat = _unitOfWork.GetRepositoryInstance<Tbl_Category>().GetAllRecords();
			foreach (var item in cat)
			{
				list.Add(new SelectListItem { Value = item.CategoryId.ToString(), Text = item.CategoryName });

			}
			return list;
		}
		public ActionResult CategoryEdit(int catId)
		{
			return View(_unitOfWork.GetRepositoryInstance<Tbl_Category>().GetFirstorDefault(catId));

		}
		[HttpPost]
		public ActionResult CategoryEdit(Tbl_Category tbl)
		{
			_unitOfWork.GetRepositoryInstance<Tbl_Category>().Update(tbl);
			return RedirectToAction("Categories");
		}
	}
}