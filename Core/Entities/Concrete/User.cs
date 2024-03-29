﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Concrete
{
    public class User : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public bool Status { get; set; }
        public string QRCode { get; set; }
        public DateTime? PassChangeDate { get; set; }
    }
}
