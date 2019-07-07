using Infrastructure.Entities;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Classes.DB
{
    public class RepositoryUnitOfWork : IRepositoryUnitOfWork
    {
        public RepositoryUnitOfWork(DataBaseContext dataBaseContext)
        {
            Users = new Lazy<Repository<User>>(() => new Repository<User>(dataBaseContext));
            Categories = new Lazy<Repository<Category>>(() => new Repository<Category>(dataBaseContext));
            SubCategories = new Lazy<Repository<SubCategory>>(() => new Repository<SubCategory>(dataBaseContext));
            Carts = new Lazy<Repository<Cart>>(() => new Repository<Cart>(dataBaseContext));
            Items = new Lazy<Repository<Item>>(() => new Repository<Item>(dataBaseContext));
        }

        public Lazy<Repository<User>> Users { get; set; }
        public Lazy<Repository<Category>> Categories { get; set; }
        public Lazy<Repository<SubCategory>> SubCategories { get; set; }
        public Lazy<Repository<Cart>> Carts { get; set; }
        public Lazy<Repository<Item>> Items { get; set; }
    }
}
