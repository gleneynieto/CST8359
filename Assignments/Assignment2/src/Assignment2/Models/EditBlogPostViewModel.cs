﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2.Models
{
    public class EditBlogPostViewModel
    {
        public BlogPost BlogPost
        {
            get;
            set;
        }

        public List<Photo> Photos
        {
            get;
            set;
        }
    }
}
