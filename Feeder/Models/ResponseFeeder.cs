namespace Feeder.Models
{
    public class ResponseFeeder<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public bool isFormat { get; set; }
    }
}
