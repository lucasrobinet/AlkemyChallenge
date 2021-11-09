﻿// <auto-generated />
using System;
using AhoraSi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AhoraSi.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    [Migration("20210915224146_UpdateImageFile")]
    partial class UpdateImageFile
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AhoraSi.Models.Character", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("History")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Weight")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Character");
                });

            modelBuilder.Entity("AhoraSi.Models.CharacterMoS", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CharacterId")
                        .HasColumnType("int");

                    b.Property<int>("MovieOrSerieId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CharacterId");

                    b.HasIndex("MovieOrSerieId");

                    b.ToTable("CharacterOfShow");
                });

            modelBuilder.Entity("AhoraSi.Models.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte>("Image")
                        .HasColumnType("tinyint");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Genre");
                });

            modelBuilder.Entity("AhoraSi.Models.GenreMoS", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GenreId")
                        .HasColumnType("int");

                    b.Property<int>("MovieOrSerieId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GenreId");

                    b.HasIndex("MovieOrSerieId");

                    b.ToTable("GenreOfShow");
                });

            modelBuilder.Entity("AhoraSi.Models.MovieOrSerie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateBirth")
                        .HasColumnType("datetime2");

                    b.Property<byte>("Image")
                        .HasColumnType("tinyint");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Valoration")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("MovieOrSerie");
                });

            modelBuilder.Entity("AhoraSi.Models.CharacterMoS", b =>
                {
                    b.HasOne("AhoraSi.Models.Character", "Character")
                        .WithMany("CharacterMoS")
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AhoraSi.Models.MovieOrSerie", "MovieOrSerie")
                        .WithMany("CharacterMoS")
                        .HasForeignKey("MovieOrSerieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Character");

                    b.Navigation("MovieOrSerie");
                });

            modelBuilder.Entity("AhoraSi.Models.GenreMoS", b =>
                {
                    b.HasOne("AhoraSi.Models.Genre", "Genre")
                        .WithMany("GenreMoS")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AhoraSi.Models.MovieOrSerie", "MovieOrSerie")
                        .WithMany("GenreMoS")
                        .HasForeignKey("MovieOrSerieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genre");

                    b.Navigation("MovieOrSerie");
                });

            modelBuilder.Entity("AhoraSi.Models.Character", b =>
                {
                    b.Navigation("CharacterMoS");
                });

            modelBuilder.Entity("AhoraSi.Models.Genre", b =>
                {
                    b.Navigation("GenreMoS");
                });

            modelBuilder.Entity("AhoraSi.Models.MovieOrSerie", b =>
                {
                    b.Navigation("CharacterMoS");

                    b.Navigation("GenreMoS");
                });
#pragma warning restore 612, 618
        }
    }
}
