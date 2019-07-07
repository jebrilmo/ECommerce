using System;
using System.Collections.Generic;
using FluentValidation.Results;
using System.Text;

namespace Infrastructure.Interfaces
{
    public interface IExecutableOperation<Entity>
    {
        ValidationResult Validate(Entity entity);
        IDTO Execute(Entity entity);
    }
}
