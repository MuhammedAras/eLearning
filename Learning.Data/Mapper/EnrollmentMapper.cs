using Learning.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Data.Mapper
{
    class EnrollmentMapper : EntityTypeConfiguration<Enrollments>
    {
        public EnrollmentMapper()
        {
            this.ToTable("Enrollments");

            this.HasKey(c=>c.Id);
            this.Property(c => c.Id).IsRequired();
            this.Property(c => c.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            this.Property(c => c.EnrollmentDate).IsRequired();
            this.Property(c => c.EnrollmentDate).HasColumnType("smalldatetime");

            this.HasOptional(c => c.Course).WithMany(e => e.Enrollments).Map(s=>s.MapKey("CourseId")).WillCascadeOnDelete(false);
            this.HasOptional(c => c.Student).WithMany(e => e.Enrollments).Map(s => s.MapKey("StudentId")).WillCascadeOnDelete(false) ;

        }
    }
}
