using Context.Data;
using Context.Entities;
using DataTransferObjets.Entities;
using Repository.GenericRepository.Implentations;
using Repository.SpecificRepository.Interfaces;

namespace Repository.SpecificRepository.Implentations
{
    public class PropertyImageRepository : GenericRepository<PropertyImage>, IPropertyImageRepository
    {
        private readonly ApplicationDbContext context;
        public PropertyImageRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}