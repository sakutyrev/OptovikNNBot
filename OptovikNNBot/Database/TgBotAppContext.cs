using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptovikNNBot.Database
{
    internal class TgBotAppContext :DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Admin> Admins => Set<Admin>();
        public DbSet<DiscountsSubscription> DiscSubscriptions => Set<DiscountsSubscription>();

        public DbSet<UserState> UserStates => Set<UserState>();
        public DbSet<Remains> Remains => Set<Remains>();
        public TgBotAppContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Настройка подключения к БД SQLite
            var builder = new ConfigurationBuilder();
            // установка пути к текущему каталогу
            builder.SetBasePath(Directory.GetCurrentDirectory());
            // получаем конфигурацию из файла appsettings.json
            builder.AddJsonFile("Properties/appsettings.json");
            // создаем конфигурацию
            var config = builder.Build();
            // получаем строку подключения
            string? connectionString = config.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlite(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Настройка таблицы пользователей
            modelBuilder.Entity<User>().HasKey(u => u.TgUserId).HasName("TgUserId");
            modelBuilder.Entity<User>()
                .HasOne(u => u.DiscountsSubscription)
                .WithOne(ds => ds.User)
                .HasForeignKey<DiscountsSubscription>(ds => ds.Tg_user_id);
            
            //Настройка таблицы админов
            modelBuilder.Entity<Admin>().HasKey(u => u.TgUserId).HasName("AdminId");
            modelBuilder.Entity<Admin>().HasData(new Admin { 
                TgUserId = Convert.ToInt64(Environment.GetEnvironmentVariable("adminId"))}); //Инициализация начальным значением id и имени администратора
            
            //Настройка таблицы подписок на рассылку
            modelBuilder.Entity<DiscountsSubscription>().ToTable("DiscSubscriptions");

            modelBuilder.Entity<UserState>().HasKey(u => u.TgUserId);
            
            //Настройка таблицы остатков пряжи
            modelBuilder.Entity<Remains>().Property(y => y.Name).IsRequired();
            //modelBuilder.Entity<Remains>().HasIndex(y => y.Name);
        }


    }
}
