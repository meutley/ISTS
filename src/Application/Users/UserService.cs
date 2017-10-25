using System;
using System.Linq;
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
        private readonly IUserPasswordService _userPasswordService;

        public UserService(
            IMapper mapper,
            IUserValidator userValidator,
            IUserRepository userRepository,
            IUserPasswordService userPasswordService)
        {
            _mapper = mapper;
            _userValidator = userValidator;
            _userRepository = userRepository;
            _userPasswordService = userPasswordService;
        }

        public async Task<UserDto> AuthenticateAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            var users = await _userRepository.GetAsync(u => u.Email == email);
            var user = users.SingleOrDefault();
            if (user == null)
            {
                return null;
            }

            if (!_userPasswordService.ValidateHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            var result = _mapper.Map<UserDto>(user);
            return result;
        }

        public async Task<UserDto> CreateAsync(UserPasswordDto model)
        {
            var user = User.Create(
                _userValidator,
                _userPasswordService,
                model.Email,
                model.DisplayName,
                model.Password,
                model.PostalCode);

            var entity = await _userRepository.CreateAsync(user);

            var result = _mapper.Map<UserDto>(entity);
            return result;
        }
    }
}