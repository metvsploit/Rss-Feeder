using Feeder.Models;

namespace Feeder.Services
{
    public interface IFeederService
    {
        public ResponseFeeder<List<Tape>> GetTapes();
        public Task<ResponseFeeder<List<Item>>> GetItems(string address);
        public Task<ResponseFeeder<List<Item>>> GetAllItems();
    }
}
