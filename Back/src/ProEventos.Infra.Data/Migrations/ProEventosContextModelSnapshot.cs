﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProEventos.Infra.Data.Context;

#nullable disable

namespace ProEventos.Infra.Data.Migrations
{
    [DbContext(typeof(ProEventosContext))]
    partial class ProEventosContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ProEventos.Domain.Entities.Evento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("DataEvento")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ImagemURL")
                        .HasColumnType("longtext");

                    b.Property<string>("Local")
                        .HasColumnType("longtext");

                    b.Property<int>("Lote")
                        .HasColumnType("int");

                    b.Property<int>("QtdPessoas")
                        .HasColumnType("int");

                    b.Property<string>("Telefone")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Tema")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("eventos");
                });

            modelBuilder.Entity("ProEventos.Domain.Entities.Lote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("DataFim")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DataInicio")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("EventoId")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<decimal>("Preco")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("Quantidade")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EventoId");

                    b.ToTable("lotes");
                });

            modelBuilder.Entity("ProEventos.Domain.Entities.Palestrante", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ImagemUrl")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("MiniCurriculo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Telefone")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("palestrantes");
                });

            modelBuilder.Entity("ProEventos.Domain.Entities.PalestranteEvento", b =>
                {
                    b.Property<int>("EventoId")
                        .HasColumnType("int");

                    b.Property<int>("PalestranteId")
                        .HasColumnType("int");

                    b.HasKey("EventoId", "PalestranteId");

                    b.HasIndex("PalestranteId");

                    b.ToTable("palestrantes_eventos");
                });

            modelBuilder.Entity("ProEventos.Domain.Entities.RedeSocial", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("EventoId")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("PalestranteId")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("EventoId");

                    b.HasIndex("PalestranteId");

                    b.ToTable("redes_sociais");
                });

            modelBuilder.Entity("ProEventos.Domain.Entities.Lote", b =>
                {
                    b.HasOne("ProEventos.Domain.Entities.Evento", "Evento")
                        .WithMany("Lotes")
                        .HasForeignKey("EventoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Evento");
                });

            modelBuilder.Entity("ProEventos.Domain.Entities.PalestranteEvento", b =>
                {
                    b.HasOne("ProEventos.Domain.Entities.Evento", "Evento")
                        .WithMany("PalestrantesEventos")
                        .HasForeignKey("EventoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProEventos.Domain.Entities.Palestrante", "Palestrante")
                        .WithMany("PalestrantesEventos")
                        .HasForeignKey("PalestranteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Evento");

                    b.Navigation("Palestrante");
                });

            modelBuilder.Entity("ProEventos.Domain.Entities.RedeSocial", b =>
                {
                    b.HasOne("ProEventos.Domain.Entities.Evento", "Evento")
                        .WithMany("RedesSociais")
                        .HasForeignKey("EventoId");

                    b.HasOne("ProEventos.Domain.Entities.Palestrante", "Palestrante")
                        .WithMany("RedesSociais")
                        .HasForeignKey("PalestranteId");

                    b.Navigation("Evento");

                    b.Navigation("Palestrante");
                });

            modelBuilder.Entity("ProEventos.Domain.Entities.Evento", b =>
                {
                    b.Navigation("Lotes");

                    b.Navigation("PalestrantesEventos");

                    b.Navigation("RedesSociais");
                });

            modelBuilder.Entity("ProEventos.Domain.Entities.Palestrante", b =>
                {
                    b.Navigation("PalestrantesEventos");

                    b.Navigation("RedesSociais");
                });
#pragma warning restore 612, 618
        }
    }
}
