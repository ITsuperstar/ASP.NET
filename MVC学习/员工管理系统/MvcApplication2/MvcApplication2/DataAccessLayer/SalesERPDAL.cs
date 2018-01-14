using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MvcApplication2.Models;

namespace MvcApplication2.DataAccessLayer
{
    public class SalesERPDAL:DbContext
    {
    protected override void OnModelCreating(DbModelBuilder modelBuilder)
   {
        modelBuilder.Entity<Employee>().ToTable("TblEmployee");
         base.OnModelCreating(modelBuilder);
     }
     public DbSet<Employee> Employees{get;set;}
    }

}