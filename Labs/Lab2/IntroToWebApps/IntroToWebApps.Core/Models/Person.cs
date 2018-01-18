using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IntroToWebApps.Core.Models
{
    public class Person
    {
        [Required]
        [StringLength(100)]
        public string FirstName
        {
            get;
            set;
        }

        [Required]
        [StringLength(100)]
        public string LastName
        {
            get;
            set;
        }
    }
}
