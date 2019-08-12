using Infrastructure.Classes;
using Infrastructure.Classes.SearchFilters;
using Infrastructure.Entities;
using Infrastructure.Extensions;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Operations;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Operations.CategoryOperations
{
    public class SearchCategoryOperation : ISearchOperation<SearchCategory>
    {
        private IRepositoryUnitOfWork _repositoryUnitOfWork;
        public SearchCategoryOperation(IRepositoryUnitOfWork repositoryUnitOfWork)
        {
            _repositoryUnitOfWork = repositoryUnitOfWork;
        }
        public IDTO Execute(SearchCategory filter)
        {
            IQueryable<Infrastructure.Entities.Category> data = _repositoryUnitOfWork.Categories.Value.GetAll();

            if (filter.Name != default(string))
            {
                data = data.Where(item => item.Name == filter.Name);
            }

            return new OperationResult<IEnumerable<Category>>()
            {
                Status = Infrastructure.Enums.OperationStatus.Success,
                Data = data.Paginate(filter.PageNumber, filter.PageSize)
            };
        }
    }
}
