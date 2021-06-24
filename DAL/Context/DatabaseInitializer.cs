using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Context
{
    public static class DatabaseInitializer
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Positions>()
                .HasData(

                new Positions
                {
                    PositionId = 1,
                    Position = "Head"
                },
                new Positions
                {
                    PositionId = 2,
                    Position = "Director"

                },
                new Positions
                {
                    PositionId = 3,
                    Position = "Manager"
                },
                new Positions
                {
                    PositionId = 4,
                    Position = "HR"
                },
                new Positions
                {
                    PositionId = 5,
                    Position = "Programmer"
                },
                new Positions
                {
                    PositionId = 6,
                    Position = "IT"
                },
                new Positions
                {
                    PositionId = 7,
                    Position = "Cleaner"
                }
                );
            }
        }
    }
