using Infrastructure.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities
{
    public class User : IEntity
    {
        public long Id { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string VerificationCode { get; set; }
        public bool IsAvtive { get; set; }
        [NotMapped]
        public bool ReadyForVerfication => (DateTime.Now - SendVerficationCodeDate).Minutes <= 5;
        public bool IsVerfied { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime SendVerficationCodeDate { get; set; }
        public DateTime? LastSeen { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
