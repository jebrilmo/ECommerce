using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Entities
{
    public class User : IEntity
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public string MobileNumber { get; set; }
        public string Name { get; set; }
        public bool IsAvtive { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime? LastSeen { get; set; }
    }
}
