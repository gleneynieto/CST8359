using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2.Models
{
    public class Photo
    {
        public int PhotoId
        {
            get;
            set;
        }

        public int BlogPostId
        {
            get;
            set;
        }

        public string FileName
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }
    }
}
