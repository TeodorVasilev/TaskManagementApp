using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagmentApp.DAL.Models.Account;
using TaskManagmentApp.DAL.Models.Assignments;

namespace TaskManagmentApp.DAL.Configuration
{
    public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> builder)
        {
            builder.HasKey(a => a.Id);

            builder
                .HasOne<AppUser>(a => a.User)
                .WithMany(u => u.Assignments)
                .HasForeignKey(a => a.UserId);
        }
    }
}
