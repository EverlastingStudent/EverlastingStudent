namespace EverlastingStudent.Data.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    using EverlastingStudent.Common.Contracts;
    using EverlastingStudent.Data.Repositories.Contracts;

    public class DeletableEntityRepository<T> : GenericRepository<T>, IDeletableEntityRepository<T>
         where T : class, IDeletableEntity, new()
    {
        public DeletableEntityRepository(DbContext context)
            : base(context)
        {
        }

        public override IQueryable<T> All()
        {
            return base.All().OfType<T>().Where(x => !x.IsDeleted);
        }

        public IQueryable<T> AllWithDeleted()
        {
            return base.All().OfType<T>();
        }

        public override void Delete(T entity)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.Now;

            this.Update(entity);
        }

        public override void DeleteById(object id)
        {
            var entity = this.GetById(id);

            this.Delete(entity);
        }
    }
}
