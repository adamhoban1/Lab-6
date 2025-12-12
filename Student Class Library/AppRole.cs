using System;
using System.Collections.Generic;
using System.Text;
// Ensure you have installed the Microsoft.AspNetCore.Identity NuGet package
using Microsoft.AspNetCore.Identity;

namespace Student_Class_Library
{
    public class AppRole : IdentityRole
    {
        public string Description { get; set; } = string.Empty;
    }
}
