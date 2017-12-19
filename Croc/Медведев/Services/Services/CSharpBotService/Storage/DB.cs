namespace CROC.EDUACATION.CSharpBotService.Storage
{

    using System.Data.Entity;


    public class Db : DbContext
    {
        // Your context has been configured to use a 'DB' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'CROC.EDUACATION.CSharpBotService.DB' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'DB' 
        // connection string in the application configuration file.
        public Db()
            : base("name=DB")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<BotLog> BotLogs{ get; set; }
        public virtual DbSet<Student> Students{ get; set; }
        public virtual DbSet<Schedule> Schedules{ get; set; }
        public virtual DbSet<Checkin> Checkins { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}