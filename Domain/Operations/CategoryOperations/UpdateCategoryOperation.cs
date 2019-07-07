using FluentValidation;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Operations;
using FluentValidation.Results;
using Infrastructure.Classes;
using System.Linq;

namespace Domain.Operations.CategoryOperations
{
    public class UpdateCategoryOperation : IUpdateOperation<Category>
    {
        IRepositoryUnitOfWork _repositoryUnitOfWork;
        public UpdateCategoryOperation(IRepositoryUnitOfWork repositoryUnitOfWork)
        {
            _repositoryUnitOfWork = repositoryUnitOfWork;
        }
        public IDTO Execute(Category entity)
        {
            var validationResult = Validate(entity);
            if (validationResult.IsValid)
            {
                var data = _repositoryUnitOfWork.Categories.Value.Update(entity);
                return new OperationResult<Category>()
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

        public ValidationResult Validate(Category entity)
        {
            return new UpdateCategoryValidator().Validate(entity);
        }
    }

    public class UpdateCategoryValidator : AbstractValidator<Category>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(Item => Item.Id).NotEqual(default(long));
        }
    }
}

