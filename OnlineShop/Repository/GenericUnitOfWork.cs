using OnlineShop.DAL;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Web;

namespace OnlineShop.Repository
{
	public class GenericUnitOfWork:IDisposable
	{
		private masterEntities1 DBEntity = new masterEntities1();
		public IRepository<Tbl_EntityType> GetRepositoryInstance<Tbl_EntityType>()where Tbl_EntityType : class
		{
			return new GenericRepository<Tbl_EntityType>(DBEntity);
		}
		public void SaveChanges()
		{
			DBEntity.SaveChanges();
		}


		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					DBEntity.Dispose();
				}
			}
			this.disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private bool disposed = false;

	}
}