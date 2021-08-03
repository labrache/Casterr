using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Casterr.Data
{
    public class AppUser: IdentityUser
    {
        public AppUser() : base()
        {
            uKey = Guid.NewGuid().ToString();
            mailKey = Guid.NewGuid().ToString();
        }
        [MaxLength(36)]
        public string uKey { get; private set; }
        [MaxLength(36)]
        public string mailKey { get; private set; }
        public Boolean mailSubscribe { get; set; }
    }
}
