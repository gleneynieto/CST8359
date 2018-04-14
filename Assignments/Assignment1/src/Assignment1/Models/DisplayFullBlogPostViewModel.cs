using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment1.Models
{
    public class DisplayFullBlogPostViewModel
    {
        public BlogPost BlogPost
        {
            get;
            set;
        }

        public User User
        {
            get;
            set;
        }

        public List<Comment> Comments
        {
            get;
            set;
        }
    }
}
