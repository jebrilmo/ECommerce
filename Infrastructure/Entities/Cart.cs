using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Entities
{
    public class Cart : IEntity
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public User User { get; set; }
        public List<Item> Items { get; set; }
        public double Price { get { return Items.Sum(item => item.Price); } }
    }
}
