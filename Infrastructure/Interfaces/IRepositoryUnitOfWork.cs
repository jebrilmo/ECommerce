using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Interfaces
{
    public interface IRepositoryUnitOfWork
    {
        Lazy<Repository<User>> Users { get; set; }
        Lazy<Repository<Category>> Categories { get; set; }
        Lazy<Repository<SubCategory>> SubCategories { get; set; }
        Lazy<Repository<Cart>> Carts { get; set; }
        Lazy<Repository<Item>> Items { get; set; }
    }
}
