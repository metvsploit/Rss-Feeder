using Feeder.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Xml;
using System.Xml.Linq;

namespace Feeder.Services
{
    public class FeederService : IFeederService
    {
        private readonly HttpClient _httpClient;
        private readonly WebProxy _webProxy;
        private IMemoryCache _cache;
        public Settings _settings;

        public FeederService(IMemoryCache cache)
        {
            var tapeList = new List<Tape>();
            _cache = cache;
            _settings = new Settings();
            XDocument settings = XDocument.Load("settings.xml");
            var root = settings.Element("settings");
            _settings.IsFormat = Convert.ToBoolean(root?.Element("format")?.Value);
            _settings.UpdateTime = int.Parse(root.Element("update").Value);
            _settings.Proxy = new Proxy();
            _settings.Proxy.Url = root.Element("proxy").Element("address").Value;
            _settings.Proxy.UserName = root.Element("proxy").Element("username").Value;
            _settings.Proxy.Password = root.Element("proxy").Element("password").Value;
            _settings.Proxy.IsEnabled = Convert.ToBoolean(root.Element("proxy").Attribute("enabled").Value);

            foreach (var item in root.Element("tapes").Elements("tape"))
            {
                var tape = new Tape
                {
                    Channel = item.Attribute("name")?.Value,
                    Address = item.Element("address")?.Value,
                    Enabled = Convert.ToBoolean(item.Element("enabled")?.Value),
                };
                tapeList.Add(tape);
            }

            _webProxy = new WebProxy(_settings.Proxy.Url);
            _webProxy.UseDefaultCredentials = false;
            _webProxy.Credentials = new NetworkCredential(_settings.Proxy.UserName, _settings.Proxy.Password);

            var httpHandler = new HttpClientHandler { Proxy = _webProxy, PreAuthenticate = true, UseDefaultCredentials = false };
            _httpClient = _settings.Proxy.IsEnabled ? new HttpClient(httpHandler) : new HttpClient();
            _settings.Tapes = tapeList;
        }

        public async Task<ResponseFeeder<List<Item>>> GetAllItems()
        {
            var response = new ResponseFeeder<List<Item>>();
            var itemsList = new List<Item>();

            try
            {
                if (_cache.TryGetValue(1, out List<Item> items))
                {
                    response.Success = true;
                    response.Data = items;
                    response.isFormat = _settings.IsFormat;
                    return response;
                }

                foreach(var tape in _settings.Tapes)
                {
                    if (!tape.Enabled)
                        continue;

                    string address = tape.Address;
                    var result = await _httpClient.GetAsync(address);
                    var xmlResponse = await result.Content.ReadAsStringAsync();
                    if(xmlResponse != null)
                    {
                            foreach (var item in ResponseParse(xmlResponse))
                                itemsList.Add(item);

                            _cache.Set(1, itemsList,
                                 new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(_settings.UpdateTime)));
                            response.Data = itemsList;
                    }
                    else
                    {
                        response.Success = false;
                        return response;
                    }
                }
                response.isFormat = _settings.IsFormat;
                response.Success = true;
                response.Data = itemsList;
                return response;
            }
            catch (Exception ex)
            {
                return new ResponseFeeder<List<Item>> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ResponseFeeder<List<Item>>> GetItems(string address)
        {
            var response = new ResponseFeeder<List<Item>>();

            try
            {
                XDocument settings = XDocument.Load("settings.xml");

                var result = await _httpClient.GetAsync(address);
                var xmlResponse = await result.Content.ReadAsStringAsync();
                if(xmlResponse != null)
                {
                    var itemsList = new List<Item>();
                    XDocument xdoc = XDocument.Parse(xmlResponse);
                    var root = xdoc.Element("rss")?.Element("channel");
                    foreach (var item in ResponseParse(xmlResponse))
                        itemsList.Add(item);

                    response.Success = true;
                    response.Data = itemsList;
                    response.isFormat = _settings.IsFormat;
                }
                else
                    response.Success=false;
                    
                return response;

            }
            catch (Exception ex)
            {
                return new ResponseFeeder<List<Item>> { Success = false, Message = ex.Message };
            }
        }

        public ResponseFeeder<List<Tape>> GetTapes()
        {
            var response = new ResponseFeeder<List<Tape>>();
            var tapes = new List<Tape>();
            try
            {
                response.Success = true;
                response.Data = _settings.Tapes;
                return response;
            }

            catch(Exception ex)
            {
                return new ResponseFeeder<List<Tape>> { Message = $"Error: {ex.Message}", Success = false};
            }
        }

        private IEnumerable<Item> ResponseParse(string xmlResponse)
        {
            var itemsList = new List<Item>();
            XDocument xdoc = XDocument.Parse(xmlResponse);
            var root = xdoc.Element("rss")?.Element("channel");
            foreach (var element in root.Elements("item"))
            {
                var item = new Item
                {
                    Title = element.Element("title")?.Value.Replace("![CDATA[ ", ""),
                    Description = element.Element("description")?.Value,
                    Published = element.Element("pubDate")?.Value.Substring(5),
                    Address = element.Element("guid")?.Value,
                };
                yield return item;
            }
        }
    }
}
