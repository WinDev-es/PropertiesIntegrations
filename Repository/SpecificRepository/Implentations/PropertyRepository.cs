using Context.Data;
using Context.Entities;
using DataTransferObjets.Entities;

//using DataTransferObjets.Entities;
using Repository.GenericRepository.Implentations;
using Repository.SpecificRepository.Interfaces;

namespace Repository.SpecificRepository.Implentations
{
    public class PropertyRepository : GenericRepository<Property>, IPropertyRepository
    {
        private readonly ApplicationDbContext context;
        public PropertyRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
