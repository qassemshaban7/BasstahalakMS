using BasstahalakMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BasstahalakMS.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<BFile> BFiles { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Library> Libraries { get; set; }
        public DbSet<PrintType> PrintTypes { get; set; }
        public DbSet<FileBranch> FileBranches { get; set; }
        public DbSet<BfileNote> BfileNotes { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //    string SUPER_ADMIN_ID = "ecc07b18-f55e-4f6b-95bd-0e84f556135f";
            //    string ADMIN_ID = "898d9efa-cd60-4446-b9ae-e0c48dd87c49";
            //    string PREPARE_ID = "c2d7916d-74c1-4588-b2f2-6616b0e687f0";
            //    string REVIEWER_ID = "325a3e6f-b33e-43d6-8cee-f6b0ad00f620";

            //    string SUPER_ADMIN_ROLE_ID = "ba51b8f7-2a1d-45c6-9c00-68099eebd485";
            //    string ADMIN_ROLE_ID = "2cdda855-1f15-4e11-9440-cfa84493cbd6";
            //    string PREPARE_ROLE_ID = "4be32c82-c795-4db6-89ac-8cc33b11d012";
            //    string REVIEW_ROLE_ID = "f770a463-640a-43f6-b9f6-a1317fe2c227";
            string PRINTING_ROLE_ID = "488fea2d-57d4-4963-81d2-7e2e02a2b100";

        //    //seed super admin role
        //    builder.Entity<ApplicationRole>().HasData(new ApplicationRole
        //    {
        //        Name = "SuperAdmin",
        //        NormalizedName = "SUPERADMIN",
        //        Id = SUPER_ADMIN_ROLE_ID,
        //        ConcurrencyStamp = SUPER_ADMIN_ROLE_ID,
        //        ArabicRoleName = "المدير العام"
        //    });
        //    //seed admin role
        //    builder.Entity<ApplicationRole>().HasData(new ApplicationRole
        //    {
        //        Name = "Admin",
        //        NormalizedName = "ADMIN",
        //        Id = ADMIN_ROLE_ID,
        //        ConcurrencyStamp = ADMIN_ROLE_ID,
        //        ArabicRoleName = "المدير"
        //    });
        //    //seed prepare role
        //    builder.Entity<ApplicationRole>().HasData(new ApplicationRole
        //    {
        //        Name = "Prepare",
        //        NormalizedName = "PREPARE",
        //        Id = PREPARE_ROLE_ID,
        //        ConcurrencyStamp = PREPARE_ROLE_ID,
        //        ArabicRoleName = "الاعداد"
        //    });
        //    //seed Reviewer role
        //    builder.Entity<ApplicationRole>().HasData(new ApplicationRole
        //    {
        //        Name = "Review",
        //        NormalizedName = "REVIEW",
        //        Id = REVIEW_ROLE_ID,
        //        ConcurrencyStamp = REVIEW_ROLE_ID,
        //        ArabicRoleName = "المراجعة"
        //    });
        //seed Printing role
        builder.Entity<ApplicationRole>().HasData(new ApplicationRole
            {
                Name = "Printing",
                NormalizedName = "PRINTING",
                Id = PRINTING_ROLE_ID,
                ConcurrencyStamp = PRINTING_ROLE_ID,
                ArabicRoleName = "مطبعة"
            });

            //    //create user
            //    var appUser = new ApplicationUser
            //    {
            //        Id = SUPER_ADMIN_ID,
            //        Email = "mohamedsalah@gmail.com",
            //        NormalizedEmail = "MOHAMEDSALAH@GMAIL.COM",
            //        EmailConfirmed = true,
            //        FullName = "محمد صلاح",
            //        UserName = "mohamedsalah",
            //        NormalizedUserName = "MOHAMEDSALAH",
            //        PhoneNumber = "1234567890",
            //    };

            //    var appUser1 = new ApplicationUser
            //    {
            //        Id = ADMIN_ID,
            //        Email = "ehab@gmail.com",
            //        NormalizedEmail = "EHAB@GMAIL.COM",
            //        EmailConfirmed = true,
            //        FullName = "ايهاب ابراهيم ",
            //        UserName = "ehab",
            //        NormalizedUserName = "EHAB",
            //        PhoneNumber = "1234567890",
            //    };
            //    var appUser2 = new ApplicationUser
            //    {
            //        Id = PREPARE_ID,
            //        Email = "shaban@gmail.com",
            //        NormalizedEmail = "SHABAN@GMAIL.COM",
            //        EmailConfirmed = true,
            //        FullName = "شعبان ابراهيم",
            //        UserName = "shaban",
            //        NormalizedUserName = "SHABAN",
            //        PhoneNumber = "1234567890",
            //    };

            //    var appUser3 = new ApplicationUser
            //    {
            //        Id = REVIEWER_ID,
            //        Email = "malek@gmail.com",
            //        NormalizedEmail = "MALEK@GMAIL.COM",
            //        EmailConfirmed = true,
            //        FullName = "مالك ايهاب",
            //        UserName = "malek",
            //        NormalizedUserName = "MALEK",
            //        PhoneNumber = "1234567890",
            //    };

            //    //set user password
            //    PasswordHasher<ApplicationUser> ph = new PasswordHasher<ApplicationUser>();
            //    appUser.PasswordHash = ph.HashPassword(appUser, "mypassword_1?");
            //    appUser1.PasswordHash = ph.HashPassword(appUser1, "mypassword_2?");
            //    appUser2.PasswordHash = ph.HashPassword(appUser2, "mypassword_3?");
            //    appUser3.PasswordHash = ph.HashPassword(appUser3, "mypassword_4?");

            //    //seed user
            //    builder.Entity<ApplicationUser>().HasData(appUser);
            //    builder.Entity<ApplicationUser>().HasData(appUser1);
            //    builder.Entity<ApplicationUser>().HasData(appUser2);
            //    builder.Entity<ApplicationUser>().HasData(appUser3);

            //    //set user role to admin
            //    builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            //    {
            //        RoleId = SUPER_ADMIN_ROLE_ID,
            //        UserId = SUPER_ADMIN_ID
            //    });
            //    builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            //    {
            //        RoleId = ADMIN_ROLE_ID,
            //        UserId = ADMIN_ID
            //    });
            //    builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            //    {
            //        RoleId = PREPARE_ROLE_ID,
            //        UserId = PREPARE_ID
            //    });
            //    builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            //    {
            //        RoleId = REVIEW_ROLE_ID,
            //        UserId = REVIEWER_ID
            //    });
            base.OnModelCreating(builder);
        }
    }
}