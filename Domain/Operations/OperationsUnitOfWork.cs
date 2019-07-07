using Domain.Operations.CategoryOperations;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Operations
{
    public class OperationsUnitOfWork
    {
        public Lazy<CategoryOperationsUnitOfWork> CategoryOperations { get; set; }
        public OperationsUnitOfWork(IRepositoryUnitOfWork repositoryUnitOfWork)
        {
            CategoryOperations = new Lazy<CategoryOperationsUnitOfWork>(() => new CategoryOperationsUnitOfWork(repositoryUnitOfWork));
        }
    }
}
