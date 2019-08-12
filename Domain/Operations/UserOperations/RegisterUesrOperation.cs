using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Classes;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Operations;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;


namespace Domain.Operations.UserOperations
{
    public class RegisterUesrOperation : ICreateOperation<User>
    {
        private IRepositoryUnitOfWork _repositoryUnitOfWork;
        public RegisterUesrOperation(IRepositoryUnitOfWork repositoryUnitOfWork)
        {
            _repositoryUnitOfWork = repositoryUnitOfWork;
        }

        public IDTO Execute(User entity)
        {
            ValidationResult validationResult = Validate(entity);
            if (validationResult.IsValid)
            {
                entity.VerificationCode = SendSms(entity.MobileNumber);
                entity.SendVerficationCodeDate = DateTime.Now;
                entity.Password = BCrypt.Net.BCrypt.HashPassword(entity.Password, 50);
                entity = Authenticate(entity);
                User data = _repositoryUnitOfWork.Users.Value.Add(entity);
                data.Password = default(string);
                return new OperationResult<User>()
                {
                    Status = Infrastructure.Enums.OperationStatus.Success,
                    Data = data
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
            return new CreateCategoryValidator(_repositoryUnitOfWork).Validate(entity);
        }

        private string SendSms(string mobileNumber)
        {
            Random random = new Random();
            int code = random.Next(1, 99999);

            MessageResource message = MessageResource.Create(
                body: string.Format("Gift Registration Code {0}", code),
                from: new PhoneNumber("+15017122661"),
                to: new PhoneNumber("+" + mobileNumber)
            );
            return code.ToString();
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

        internal class CreateCategoryValidator : AbstractValidator<User>
        {
            public CreateCategoryValidator(IRepositoryUnitOfWork repositoryUnitOfWork)
            {
                RuleFor(entity => entity.Id).Equal(default(long));
                RuleFor(entity => entity.MobileNumber).Must((value) => _checkMobileNumber(repositoryUnitOfWork, value)).WithMessage("Mobile Number is used");
            }

            private bool _checkMobileNumber(IRepositoryUnitOfWork repositoryUnitOfWork, string mobileNumber)
            {
                return !
    repositoryUnitOfWork.Users.Value.Any(user => user.MobileNumber == mobileNumber);
            }
        }
    }
}
