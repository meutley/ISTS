using System;
using System.Threading.Tasks;

using AutoMapper;

using ISTS.Domain.Users;

namespace ISTS.Application.Users
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserValidator _userValidator;
        private readonly IUserRepository _userRepository;

        public UserService(
            IMapper mapper,
            IUserValidator userValidator,
            IUserRepository userRepository)
        {
            _mapper = mapper;
            _userValidator = userValidator;
            _userRepository = userRepository;
        }
        
        public async Task<UserDto> CreateAsync(UserPasswordDto model)
        {
            var user = User.Create(
                _userValidator,
                model.Email,
                model.DisplayName,
                model.PostalCode,
                model.Password);

            var entity = await _userRepository.CreateAsync(user);

            var result = _mapper.Map<UserDto>(entity);
            return result;
        }
    }
}