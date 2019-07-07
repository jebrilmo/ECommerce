using FluentValidation;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Operations;
using FluentValidation.Results;
using Infrastructure.Classes;
using System.Linq;

namespace Domain.Operations.CategoryOperations
{
    public class CreateCategoryOperation : ICreateOperation<Category>
    {
        IRepositoryUnitOfWork _repositoryUnitOfWork;
        public CreateCategoryOperation(IRepositoryUnitOfWork repositoryUnitOfWork)
        {
            _repositoryUnitOfWork = repositoryUnitOfWork;
        }

        public IDTO Execute(Category entity)
        {
            var validationResult = Validate(entity);
            if (validationResult.IsValid)
            {
                var data = _repositoryUnitOfWork.Categories.Value.Add(entity);
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
        return new CreateCategoryValidator().Validate(entity);
    }
}

 class CreateCategoryValidator : AbstractValidator<Category>
{
    public CreateCategoryValidator()
    {
        RuleFor(Item => Item.Id).Equal(default(long));
    }
}
}

