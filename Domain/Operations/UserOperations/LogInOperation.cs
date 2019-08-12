using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Classes;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;


namespace Domain.Operations.UserOperations
{
    public class LogInOperation : IExecutableOperation<User>
    {
        private IRepositoryUnitOfWork _repositoryUnitOfWork;
        public LogInOperation(IRepositoryUnitOfWork repositoryUnitOfWork)
        {
            _repositoryUnitOfWork = repositoryUnitOfWork;
        }

        public IDTO Execute(User entity)
        {
            ValidationResult validationResult = Validate(entity);
            if (validationResult.IsValid)
            {
                string passwordHasehd = BCrypt.Net.BCrypt.HashPassword(entity.Password, 50);
                if (_repositoryUnitOfWork.Users.Value.Any(user => user.Password == passwordHasehd && user.MobileNumber == entity.MobileNumber))
                {
                    entity = Authenticate(entity);
                }
                else
                {
                    return new OperationResult<Category>()
                    {
                        Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList()
                    };
                }
                entity.Password = default(string);
                return new OperationResult<User>()
                {
                    Status = Infrastructure.Enums.OperationStatus.Success,
                    Data = entity
                };
            }
            else
            {
                return new OperationResult<Category>()
                {
                    Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList()
                };
            }
        }

        public ValidationResult Validate(User entity)
        {
            return new LogInOperationValidator(_repositoryUnitOfWork).Validate(entity);
        }

        public User Authenticate(User user)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes("ASDASD@#@#$123132WWWmmm_)()(3");
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.MobileNumber)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            return user;
        }

        internal class LogInOperationValidator : AbstractValidator<User>
        {
            public LogInOperationValidator(IRepositoryUnitOfWork repositoryUnitOfWork)
            {
                RuleFor(entity => entity.MobileNumber).NotEmpty().WithMessage("Mobile Number Is Required");
                RuleFor(entity => entity.Password).NotEmpty().WithMessage("Password Is Required");
            }
        }
    }
}
