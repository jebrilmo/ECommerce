using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Interfaces
{
    public interface IEntity
    {
        long Id { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime UpdateAt { get; set; }
    }
}
