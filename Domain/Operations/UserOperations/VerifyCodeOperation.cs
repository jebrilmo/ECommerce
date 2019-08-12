using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Classes;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Operations.UserOperations
{
    public class VerifyCodeOperation : IExecutableOperation<User>
    {
        private IRepositoryUnitOfWork _repositoryUnitOfWork;
        public VerifyCodeOperation(IRepositoryUnitOfWork repositoryUnitOfWork)
        {
            _repositoryUnitOfWork = repositoryUnitOfWork;
        }

        public IDTO Execute(User entity)
        {
            ValidationResult validationResult = Validate(entity);
            if (validationResult.IsValid)
            {
                string code = _repositoryUnitOfWork.Users.Value.Get(entity.Id).VerificationCode;
                if (code == entity.VerificationCode)
                {
                    entity.IsVerfied = true;
                    _repositoryUnitOfWork.Users.Value.Update(entity);
                    return new OperationResult<User>()
                    {
                        Status = Infrastructure.Enums.OperationStatus.Success,
                    };
                }

                return new OperationResult<User>()
                {
                    Errors = new List<string>() { "Code is not valid" }
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
            return new VerifyCodeValidator(_repositoryUnitOfWork).Validate(entity);
        }
    }

    internal class VerifyCodeValidator : AbstractValidator<User>
    {
        public VerifyCodeValidator(IRepositoryUnitOfWork repositoryUnitOfWork)
        {
            RuleFor(entity => entity.Id).NotEqual(default(long));
            RuleFor(entity => entity.Id).Must((value) => _checkUserId(repositoryUnitOfWork, value)).WithMessage("Id is not exisit");
            RuleFor(entity => entity.VerificationCode).NotEqual(default(string));
            RuleFor(entity => entity.ReadyForVerfication).Equal(true).WithMessage("Time for code is expired");
        }

        private bool _checkUserId(IRepositoryUnitOfWork repositoryUnitOfWork, long id)
        {
            return repositoryUnitOfWork.Users.Value.Any(user => user.Id == id);
        }
    }
}
