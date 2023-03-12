﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagmentApp.DAL.Models.Account;

namespace TaskManagmentApp.DAL.Configuration
{
    public class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.HasKey(r => r.Id);

            builder.HasData
                (
                    new AppRole
                    {
                        Id = 1,
                        Name = "Admin",
                        NormalizedName = "ADMIN",
                        
                    },

                    new AppRole
                    {
                        Id = 2,
                        Name = "Employee",
                        NormalizedName = "EMPLOYEE",
                    }
                );
        }
    }
}
