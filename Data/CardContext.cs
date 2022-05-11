
using Cards.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Cards.API.Data
{
    public class CardContext : DbContext 
    {
        public CardContext(DbContextOptions<CardContext> options) : base(options)
        {     
                  
        }
        public DbSet<Card>? Cards {get; set;}
    }
}