using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Framework.DomainModel.Entities.Mapping
{
    public class TemplateMap : QuickspatchEntityTypeConfiguration<Template>
    {
        public TemplateMap()
        {
            Property(t => t.Title).IsRequired().HasMaxLength(200);
            Property(t => t.Content).IsRequired();

            ToTable("Template");
            Property(t => t.Title).HasColumnName("Title");
            Property(t => t.Content).HasColumnName("Content");
            Property(t => t.TemplateType).HasColumnName("TemplateType");
            Property(t => t.ReportType).HasColumnName("ReportType");

        }
    }
}
