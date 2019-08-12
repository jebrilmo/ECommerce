using FluentValidation;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Operations;
using FluentValidation.Results;
using Infrastructure.Classes;
using System.Linq;
using Infrastructure.Classes.Apis;
using System;
using System.Collections.Generic;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Domain.Operations.UserOperations
{
    public class ResendCodeOperation : IExecutableOperation<User>
    {
        IRepositoryUnitOfWork _repositoryUnitOfWork;
        public ResendCodeOperation(IRepositoryUnitOfWork repositoryUnitOfWork)
        {
            _repositoryUnitOfWork = repositoryUnitOfWork;
        }

        public IDTO Execute(User entity)
        {
            var validationResult = Validate(entity);
            if (validationResult.IsValid)
            {
                entity = _repositoryUnitOfWork.Users.Value.Get(entity.Id);
                entity.VerificationCode = SendSms(entity.MobileNumber);
                entity.SendVerficationCodeDate = DateTime.Now;
                var data = _repositoryUnitOfWork.Users.Value.Update(entity);
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
            return new VerifySendCodeValidator(_repositoryUnitOfWork).Validate(entity);
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
    }

    class VerifySendCodeValidator : AbstractValidator<User>
    {
        public VerifySendCodeValidator(IRepositoryUnitOfWork repositoryUnitOfWork)
        {
            RuleFor(entity => entity.Id).NotEqual(default(long));
            RuleFor(entity => entity.Id).Must((value) => _checkUserId(repositoryUnitOfWork, value)).WithMessage("Id is not exisit");
        }

        bool _checkUserId(IRepositoryUnitOfWork repositoryUnitOfWork, long id) =>
            repositoryUnitOfWork.Users.Value.Any(user => user.Id == id);
    }
}
