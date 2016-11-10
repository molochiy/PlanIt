namespace PlanIt.Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class PlanIt : DbContext
    {
        public PlanIt()
            : base("name=PlanIt")
        {
        }

        public virtual DbSet<C__RefactorLog> C__RefactorLog { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Image> Image { get; set; }
        public virtual DbSet<Plan> Plan { get; set; }
        public virtual DbSet<PlanItem> PlanItem { get; set; }
        public virtual DbSet<Profile> Profile { get; set; }
        public virtual DbSet<SharedPlanItemUser> SharedPlanItemUser { get; set; }
        public virtual DbSet<SharedPlanUser> SharedPlanUser { get; set; }
        public virtual DbSet<SharingStatus> SharingStatus { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Plan>()
                .HasMany(e => e.PlanItem)
                .WithRequired(e => e.Plan)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Plan>()
                .HasMany(e => e.SharedPlanUser)
                .WithRequired(e => e.Plan)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PlanItem>()
                .HasMany(e => e.SharedPlanItemUser)
                .WithRequired(e => e.PlanItem)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Profile>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Profile>()
                .HasMany(e => e.User)
                .WithRequired(e => e.Profile)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SharingStatus>()
                .HasMany(e => e.SharedPlanItemUser)
                .WithRequired(e => e.SharingStatus)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SharingStatus>()
                .HasMany(e => e.SharedPlanUser)
                .WithRequired(e => e.SharingStatus)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Status>()
                .HasMany(e => e.Plan)
                .WithRequired(e => e.Status)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Status>()
                .HasMany(e => e.PlanItem)
                .WithRequired(e => e.Status)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Plan)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.SharedPlanItemUser)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.SharedPlanUser)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
        }
    }
}
