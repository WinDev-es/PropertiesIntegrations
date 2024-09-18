using Context.Data;
using DataTransferObjets.Entities;
using Repository.GenericRepository.Implentations;
using Repository.SpecificRepository.Interfaces;

namespace Repository.SpecificRepository.Implentations
{
    public class OwnerRepository : GenericRepository<Owner>, IOwnerRepository
    {
        private readonly ApplicationDbContext context;
        public OwnerRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
