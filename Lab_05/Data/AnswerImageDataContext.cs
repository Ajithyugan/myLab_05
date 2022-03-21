using Lab_05.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab_05.Data
{
    public class AnswerImageDataContext : DbContext
    {
        public AnswerImageDataContext(DbContextOptions<AnswerImageDataContext> options) : base(options)
        {
        }
        public DbSet<AnswerImage> AnswerImage { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnswerImage>().ToTable("AnswerImage");
            /*modelBuilder.Entity<Subscription>().ToTable("Subscriptions");*//*
            modelBuilder.Entity<Subscription>().HasKey(c => new { c.ClientId, c.BrokerageId });*/


        }

    }
}
