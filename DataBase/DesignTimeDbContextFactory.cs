using DataBaseManager;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBase
{
    /// <summary>
    /// Фабрика для создания миграций. Запускается из Package Manager Console в контексте этого проекта
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<VkDbContext>
    {
        /// <summary>
        /// Для того что бы добавить миграцию выполнить Add-Migration [Имя миграции]
        /// Использует стандартное подключение к БД. Оно нужно для формирования классов создания объектов в БД
        /// Указывается явно. Лучше после использования закомментировать
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public VkDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<VkDbContext>();
            var conStr = new SqliteConnectionStringBuilder();
            conStr.DataSource = "VKParserDB";
            optionsBuilder.UseSqlite(conStr.ConnectionString);

            return new VkDbContext(optionsBuilder.Options);
        }
    }
}
