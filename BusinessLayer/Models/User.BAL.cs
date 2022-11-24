﻿using Microsoft.AspNetCore.Identity;

namespace BusinessLayer.Models
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
    }
}
