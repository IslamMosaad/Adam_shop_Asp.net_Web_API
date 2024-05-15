using EcommerceAPI.Models;
using EcommerceAPI.Repositories.Interfaces;
using EcommerceAPI.Repositories.Repos;

namespace EcommerceAPI.Unit_OF_Work
{
    public class UnitOfWork<T> : IUnitOfWork<T> where T : class
    {
        ECommerceDBContext db;
        IRepository<T> repo;


        public UnitOfWork(ECommerceDBContext db)
        {
            this.db = db;
        }

        public IRepository<T> Repository
        {
            get
            {
                if (repo == null){ repo = new Repository<T>(db);}
                return repo;
            }
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }
    }
}
