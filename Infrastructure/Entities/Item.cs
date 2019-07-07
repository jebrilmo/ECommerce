using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Entities
{
    public class Item : IEntity
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public SubCategory SubCategory { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }
}
