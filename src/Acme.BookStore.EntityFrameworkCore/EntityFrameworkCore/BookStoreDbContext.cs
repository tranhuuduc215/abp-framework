﻿using Microsoft.EntityFrameworkCore;
using Acme.BookStore.Users;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Acme.BookStore.Books;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;

namespace Acme.BookStore.EntityFrameworkCore
{
    /* This is your actual DbContext used on runtime.
     * It includes only your entities.
     * It does not include entities of the used modules, because each module has already
     * its own DbContext class. If you want to share some database tables with the used modules,
     * just create a structure like done for AppUser.
     *
     * Don't use this DbContext for database migrations since it does not contain tables of the
     * used modules (as explained above). See BookStoreMigrationsDbContext for migrations.
     */
    [ConnectionStringName("Default")]
    public class BookStoreDbContext : AbpDbContext<BookStoreDbContext>
    {
        public DbSet<AppUser> Users { get; set; }

        public DbSet<Book> Books { get; set; }

        /* Add DbSet properties for your Aggregate Roots / Entities here.
         * Also map them inside BookStoreDbContextModelCreatingExtensions.ConfigureBookStore
         */

        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Include modules to your migration db context */

            builder.ConfigurePermissionManagement();
            /* Configure your own tables/entities inside here */

            builder.Entity<Book>(b =>
            {
                b.ToTable(BookStoreConsts.DbTablePrefix + "Books",
                    BookStoreConsts.DbSchema);
                b.ConfigureByConvention(); //auto configure for the base class props
                b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            });
        }
    }
}
