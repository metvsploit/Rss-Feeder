namespace Feeder.Models
{
    public class Settings
    {
        public Proxy Proxy { get; set; }
        public int UpdateTime { get; set; }
        public bool IsFormat { get; set; }
        public List<Tape> Tapes { get; set; }
    }
}
