using Microsoft.EntityFrameworkCore;
using PhoneRedirectory.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneRedirectory.Models.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public virtual DbSet<FullViewObject> vw_People_PhoneType_No_Country { get; set; }

        #region old
        //public virtual DbSet<dynamic> vw_People_PhoneType_No_Country { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<dynamic>(entity =>
        //    {
        //        entity.HasNoKey();
        //    });

        //    base.OnModelCreating(modelBuilder);
        //}
        #endregion

        public virtual DbSet<DirectoryTypes> DirectoryTypes { get; set; }
        public virtual DbSet<TypeFilter> TypeFilter { get; set; }
        public virtual DbSet<Filters> Filters { get; set; }
        public virtual DbSet<FilterType> FilterType { get; set; }
        public virtual DbSet<PhoneType> PhoneType { get; set; }
        public virtual DbSet<Country> Country { get; set; }
    }
}
