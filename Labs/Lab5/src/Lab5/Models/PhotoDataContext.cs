﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Lab5.Models
{
    public class PhotoDataContext : DbContext
    {
        public PhotoDataContext(DbContextOptions<PhotoDataContext> options)
            : base(options)
        {

        }

        public DbSet<Photo> Photos { get; set; }
    }
}
