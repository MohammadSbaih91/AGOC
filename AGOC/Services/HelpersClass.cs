using AGOC.Repository.Interfaces;

namespace AGOC.Services
{
    public class HelpersClass
    {
        private readonly IUnitOfWork _unitOfWork;

        public HelpersClass(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

    }
}