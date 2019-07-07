using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Operations;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        OperationsUnitOfWork _operationsUnitOfWork;
        public CategoriesController(OperationsUnitOfWork operationsUnitOfWork)
        {
            _operationsUnitOfWork = operationsUnitOfWork;
        }

        [HttpPost]
        public IDTO Create(Category category)
        {
          return  _operationsUnitOfWork.CategoryOperations.Value.Create.Value.Execute(category);
        }
    }
}