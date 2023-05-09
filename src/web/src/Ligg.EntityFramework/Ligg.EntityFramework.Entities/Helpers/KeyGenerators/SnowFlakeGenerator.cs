
namespace Ligg.EntityFramework.Entities.Helpers
{
    public class SnowFlakeGenerator
    {
        private int SnowFlakeWorkerId =1;

        private TwitterSnowflake snowflake;

        private static readonly SnowFlakeGenerator instance = new SnowFlakeGenerator();

        private SnowFlakeGenerator()
        {
            snowflake = new TwitterSnowflake(SnowFlakeWorkerId, 0, 0);
        }
        public static SnowFlakeGenerator Instance
        {
            get
            {
                return instance;
            }
        }
        public long GetId()
        {
            return snowflake.NextId();
        }
    }
}
