using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NCD.Identity.API.Model;
using NetDevPack.Security.Jwt.Model;
using NetDevPack.Security.Jwt.Store.EntityFrameworkCore;

namespace NCD.Identity.API.Data
{
    public class IdentityAppDbContext : IdentityDbContext, ISecurityKeyContext
    {
        public IdentityAppDbContext(DbContextOptions<IdentityAppDbContext> options) : base(options) { }
        public DbSet<SecurityKeyWithPrivate> SecurityKeys { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
