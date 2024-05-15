using EcommerceAPI.Repositories.Interfaces;

namespace EcommerceAPI.Unit_OF_Work
{
    public interface IUnitOfWork<T> where T : class
    {
        public IRepository<T> Repository { get; }
        public void SaveChanges();
    }
}
