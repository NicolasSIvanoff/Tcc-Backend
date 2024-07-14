﻿using System.ComponentModel.DataAnnotations;

namespace TccBackend.DTOs
{
    public class LoginModel
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
