using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2.Models
{
    public class Comment
    {
        public int CommentId
        {
            get;
            set;
        }

        public int BlogPostId
        {
            get;
            set;
        }

        public int UserId
        {
            get;
            set;
        }

        public string Content
        {
            get;
            set;
        }
    }
}
