﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SolveChess.DAL.Model;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231108160744_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("SolveChess.DAL.Model.Game", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("BlackPlayerId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("CastlingRightBlackKingSide")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("CastlingRightBlackQueenSide")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("CastlingRightWhiteKingSide")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("CastlingRightWhiteQueenSide")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("EnpassantSquareFile")
                        .HasColumnType("int");

                    b.Property<int?>("EnpassantSquareRank")
                        .HasColumnType("int");

                    b.Property<string>("Fen")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("FullMoveNumber")
                        .HasColumnType("int");

                    b.Property<int>("HalfMoveClock")
                        .HasColumnType("int");

                    b.Property<int>("SideToMove")
                        .HasColumnType("int");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<string>("WhitePlayerId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("BlackPlayerId");

                    b.HasIndex("WhitePlayerId");

                    b.ToTable("Game");
                });

            modelBuilder.Entity("SolveChess.DAL.Model.Move", b =>
                {
                    b.Property<string>("GameId")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int>("Side")
                        .HasColumnType("int");

                    b.Property<string>("Notation")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("GameId", "Number", "Side");

                    b.ToTable("Move");
                });

            modelBuilder.Entity("SolveChess.DAL.Model.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("User");
                });

            modelBuilder.Entity("SolveChess.DAL.Model.Game", b =>
                {
                    b.HasOne("SolveChess.DAL.Model.User", "BlackPlayer")
                        .WithMany()
                        .HasForeignKey("BlackPlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SolveChess.DAL.Model.User", "WhitePlayer")
                        .WithMany()
                        .HasForeignKey("WhitePlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BlackPlayer");

                    b.Navigation("WhitePlayer");
                });

            modelBuilder.Entity("SolveChess.DAL.Model.Move", b =>
                {
                    b.HasOne("SolveChess.DAL.Model.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });
#pragma warning restore 612, 618
        }
    }
}
