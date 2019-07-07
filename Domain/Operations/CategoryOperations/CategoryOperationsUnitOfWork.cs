using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Operations.CategoryOperations
{
    public class CategoryOperationsUnitOfWork
    {
        public Lazy<CreateCategoryOperation> Create { get; set; }
        public Lazy<UpdateCategoryOperation> Update { get; set; }
        public CategoryOperationsUnitOfWork(IRepositoryUnitOfWork repositoryUnitOfWork)
        {
            Create = new Lazy<CreateCategoryOperation>(() => new CreateCategoryOperation(repositoryUnitOfWork));
            Update = new Lazy<UpdateCategoryOperation>(() => new UpdateCategoryOperation(repositoryUnitOfWork));
        }
    }
}
