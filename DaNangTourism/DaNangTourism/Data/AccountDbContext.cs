using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DaNangTourism.Models;

namespace DaNangTourism.Data
{
    public class AccountDbContext : DbContext
    {
        public AccountDbContext (DbContextOptions<AccountDbContext> options)
            : base(options)
        {
        }

        public DbSet<DaNangTourism.Models.Account> Account { get; set; } = default!;
    }
}
