namespace Play.Catalog.Service.Settings
{
    public class MongoDbSettings
    {
        //Not intending to change once running, so use init.
        public string Host { get; init; }
        public int Port { get; init; }

        //Interesting: is this an arrow function
        public string ConnectionString => $"mongodb://{Host}:{Port}";
    }
}