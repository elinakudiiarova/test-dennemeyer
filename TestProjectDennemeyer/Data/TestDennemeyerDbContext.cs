using Microsoft.EntityFrameworkCore;
using TestProjectDennemeyer.Data.Entities;

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
            .HasForeignKey(pp => pp.PartyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProposalParty>()
            .HasOne(pp => pp.Proposal)
            .WithMany(p => p.ProposalParties)
            .HasForeignKey(pp => pp.ProposalId)
            .OnDelete(DeleteBehavior.Restrict);

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
            .HasOne(p => p.Item)
            .WithMany(i => i.Proposals)
            .HasForeignKey(p => p.ItemId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Proposal>()
            .HasOne(p => p.Creator)
            .WithMany()
            .HasForeignKey(p => p.CreatorId)
            .OnDelete(DeleteBehavior.SetNull);
        
        modelBuilder.Entity<Proposal>()
            .HasMany(p => p.ProposalParties)
            .WithOne(pp => pp.Proposal)
            .HasForeignKey(pp => pp.ProposalId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Party>().HasData(
            new Party
            {
                Id = 1,
                Name = "Alpha Co",
                CreationDate = new DateTime(2023, 1, 10, 0,0,0, DateTimeKind.Utc)
            },
            new Party
            {
                Id = 2,
                Name = "Beta Inc",
                CreationDate = new DateTime(2023, 1, 10, 0, 0, 0, DateTimeKind.Utc)
            },
            new Party
            {
                Id = 3,
                Name = "Gamma LLC",
                CreationDate = new DateTime(2023, 2, 5,0,0,0, DateTimeKind.Utc)
            },
            new Party
            {
                Id = 4,
                Name = "Delta Group",
                CreationDate = new DateTime(2023, 3, 1,0,0,0, DateTimeKind.Utc)
            }
        );

        modelBuilder.Entity<User>().HasData(
            new User {Id = 1, Name = "John", Surname = "Smith", PartyId = 1},
            new User {Id = 2, Name = "Alice", Surname = "Brown", PartyId = 1},
            new User {Id = 3, Name = "Bob", Surname = "Miller", PartyId = 2},
            new User {Id = 4, Name = "Diana", Surname = "Jones", PartyId = 2},
            new User {Id = 5, Name = "Evan", Surname = "White", PartyId = 3},
            new User {Id = 6, Name = "Carol", Surname = "Blake", PartyId = 3},
            new User {Id = 7, Name = "Frank", Surname = "Green", PartyId = 4},
            new User {Id = 8, Name = "Hannah", Surname = "Young", PartyId = 4}
        );
        
        modelBuilder.Entity<Item>().HasData(
            new Item
            {
                Id = 1,
                Name = "Golden Ring",
                Value = 1000m,
                CreationDate = new DateTime(2023, 2, 10,0, 0, 0, DateTimeKind.Utc),
                OwnerPartyId = 1
            },
            new Item
            {
                Id = 2,
                Name = "Silver Necklace",
                Value = 500m,
                CreationDate = new DateTime(2023, 2, 15, 0,0,0, DateTimeKind.Utc),
                OwnerPartyId = 2
            },
            new Item
            {
                Id = 3,
                Name = "Diamond Bracelet",
                Value = 2000m,
                CreationDate = new DateTime(2023, 3, 5, 0,0,0, DateTimeKind.Utc),
                OwnerPartyId = 3
            }
        ); 
        
        modelBuilder.Entity<Proposal>().HasData(
        new Proposal
        {
            Id = 1,
            CreatedDate = new DateTime(2023, 3, 10, 0,0,0, DateTimeKind.Utc),
            Comment = "Proposal #1: Golden Ring (Alpha's item)",
            CreatorId = 1,
            ItemId = 1,
            Closed = false
        },
        new Proposal
        {
            Id = 2,
            CreatedDate = new DateTime(2023, 3, 12,0,0,0, DateTimeKind.Utc),
            Comment = "Proposal #2: Silver Necklace (Beta's item)",
            CreatorId = 3,
            ItemId = 2,
            Closed = false
        });

        modelBuilder.Entity<ProposalParty>().HasData(
            new ProposalParty
            {
                Id = 1,
                PartyId = 1,
                ProposalId = 1,
                Accepted = true,
                DecisionUserId = 2,
                Amount = 700m,
                Percentage = 70m
            },
            new ProposalParty
            {
                Id = 2,
                PartyId = 2,
                ProposalId = 1,
                Accepted = false,
                Amount = 300m,
                Percentage = 30m
            },
            new ProposalParty
            {
                Id = 3,
                PartyId = 2,
                ProposalId = 2,
                Accepted = true,
                DecisionUserId = 4,
                Amount = 400m,
                Percentage = 80m
            },
            new ProposalParty
            {
                Id = 4,
                PartyId = 1,
                ProposalId = 2,
                Accepted = null,
                DecisionUserId = null,
                Amount = 100m,
                Percentage = 20m
            },
            new ProposalParty
            {
                Id = 5,
                PartyId = 3,
                ProposalId = 2,
                Accepted = true,
                DecisionUserId = 6,
                Amount = 1500m,
                Percentage = 75m
            });
    }
}