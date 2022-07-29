using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace asdfqsf.Models
{
    public class SiteUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
    }
}
