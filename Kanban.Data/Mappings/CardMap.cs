using Kanban.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kanban.Data.Mappings
{
    public class CardMap : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder.ToTable("Card", "dbo");

            builder.HasKey(pk => pk.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Titulo)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(p => p.Conteudo)
                .HasMaxLength(2000)
                .IsRequired();

            builder.Property(p => p.Lista)
                .HasMaxLength(2000)
                .IsRequired();
        }
    }
}
