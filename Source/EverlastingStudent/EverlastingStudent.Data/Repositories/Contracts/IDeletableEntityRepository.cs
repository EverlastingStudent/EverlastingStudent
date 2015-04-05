namespace EverlastingStudent.Data.Repositories.Contracts
{
    using System.Linq;

    public interface IDeletableEntityRepository<T> : IGenericRepository<T> where T : class
    {
        IQueryable<T> AllWithDeleted();
    }
}
