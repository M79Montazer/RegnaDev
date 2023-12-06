using Microsoft.EntityFrameworkCore;
using Regna.Core.Models;

namespace Regna.Core.Context
{
    public class RegnaContext : DbContext
    {
        public RegnaContext(DbContextOptions options)
    : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        //public DbSet<Post> Posts { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<OCard> OCards { get; set; }
        public DbSet<Condition> Conditions { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Mechanic> Mechanics { get; set; }
        public DbSet<OVariable> OVariables { get; set; }
        public DbSet<Variable> Variables { get; set; }
        public DbSet<ListenerMech> ListenerMechs { get; set; }
        public DbSet<MechOCard> MechOCards { get; set; }
        public DbSet<Match> Matches{ get; set; }
        public DbSet<GenericVariable> GenericVariables { get; set; }
        public DbSet<CardInDeck> Deck { get; set; }
    }
}
