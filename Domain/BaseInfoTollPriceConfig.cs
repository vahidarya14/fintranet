namespace congestion.calculator.Domain
{
    public class BaseInfoTollPriceConfig
    {
        public int Hour { get; set; }
        public int FromMinute { get; set; }
        public int ToMinute { get; set; }
        public int Fee { get; set; }
    }

}