using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewOnlineVotingApplication.Models;

    public class OnlineVotingAppContext : DbContext
    {
        public OnlineVotingAppContext (DbContextOptions<OnlineVotingAppContext> options)
            : base(options)
        {
        }

        public DbSet<NewOnlineVotingApplication.Models.UserRegistration> UserRegistration { get; set; } = default!;

    public DbSet<NewOnlineVotingApplication.Models.VoteResults>? VoteResults { get; set; }
}
