using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasterrLib.classes
{
    public class AuthenticateRes
    {
        public Boolean success { get; set; }
        public string Message { get; set; }
    }
    public class AuthenticateRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
