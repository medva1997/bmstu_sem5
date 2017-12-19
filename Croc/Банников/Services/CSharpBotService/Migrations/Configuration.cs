namespace CROC.Education.CSharpBotService.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CROC.Education.CSharpBotService.Storage.DB>
    {
        /// <summary>
        /// Конструктор конфигурации миграции
        /// </summary>
        public Configuration()
        {
            // Разрешение автоматической миграции
            AutomaticMigrationsEnabled = true;
            // Разрешение миграции с потерей данных
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "CROC.Education.CSharpBotService.Storage.DB";
        }

        protected override void Seed(CROC.Education.CSharpBotService.Storage.DB context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
