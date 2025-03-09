using Microsoft.EntityFrameworkCore;
using TestProjectDennemeyer.Models;

namespace TestProjectDennemeyer.Data;

public class TestDennemeyerDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Proposal> Proposals { get; set; }
    public DbSet<Party> Parties { get; set; }
    public DbSet<ProposalParty> ProposalParties { get; set; }
    
    public TestDennemeyerDbContext(DbContextOptions<TestDennemeyerDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProposalParty>()
            .HasKey(pp => new { pp.Id });

        modelBuilder.Entity<ProposalParty>()
            .HasOne(pp => pp.Party)
            .WithMany(p => p.ProposalParties)
            .HasForeignKey(pp => pp.PartyId);

        modelBuilder.Entity<ProposalParty>()
            .HasOne(pp => pp.Proposal)
            .WithMany(p => p.ProposalParties)
            .HasForeignKey(pp => pp.ProposalId);

        modelBuilder.Entity<ProposalParty>()
            .HasOne(pp => pp.DecisionUser)
            .WithMany(u => u.ProposalParties)
            .HasForeignKey(pp => pp.DecisionUserId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Party)
            .WithMany(p => p.Users)
            .HasForeignKey(u => u.PartyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Item>()
            .HasOne(i => i.OwnerParty)
            .WithMany(p => p.Items)
            .HasForeignKey(i => i.OwnerPartyId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Proposal>()
            .HasOne(i => i.Item)
            .WithOne()
            .HasForeignKey<Proposal>(p => p.ItemId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Proposal>()
            .HasOne(p => p.Creator)
            .WithMany()
            .HasForeignKey(p => p.CreatorId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}