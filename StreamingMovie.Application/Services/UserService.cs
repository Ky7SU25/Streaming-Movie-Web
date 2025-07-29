using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamingMovie.Application.Interfaces;
using StreamingMovie.Domain.Entities;
using StreamingMovie.Domain.UnitOfWorks;

namespace StreamingMovie.Application.Services
{
    public class UserService : GenericService<User>, IUserService
    {
        private readonly IUnitOfWork _unitOfWork;


        public UserService(IUnitOfWork unitOfWork)
            : base(unitOfWork.UserRepository)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<User>> GetPremiumUsersAsync()
        {
            return await _unitOfWork.UserRepository.GetPremiumUsersAsync();
        }
        public async Task<int> GettotalUsersCountAsync()
        {
            return await _unitOfWork.UserRepository.GetTotalUsersCountAsync();
        }
    }
}
