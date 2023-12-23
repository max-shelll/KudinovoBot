namespace KudinovoBot.DAL.Configs
{
    public class Config
    {
        public string BotToken { get; set; }
        public long OwnerId { get; set; }

        public MongoConfig Mongo { get; set; }
    }
}
