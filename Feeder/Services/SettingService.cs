using Feeder.Models;
using System.Xml.Linq;

namespace Feeder.Services
{
    public class SettingService:ISettingService
    {
        public ResponseSetting AddChannel(Tape tape)
        {
            var response = new ResponseSetting();
            try
            {
                
                XDocument xdoc = XDocument.Load("settings.xml");
                XElement root = xdoc.Element("settings").Element("tapes");
                if(root != null)
                {
                    if(root.Elements("tape").FirstOrDefault(
                        t => t.Attribute("name")?.Value == tape.Channel) != null)
                    {
                        response.Message = "Канал с таким названием уже существует";
                        response.Success = false;
                        return response;
                    }

                    root.Add(new XElement("tape",
                        new XAttribute("name", tape.Channel),
                        new XElement("address", tape.Address),
                        new XElement("enabled", "true")
                        ));
                    response.Message = "Канал успешно добавлен";
                    response.Success = true;
                    xdoc.Save("settings.xml");
                }
                else
                {
                    response.Message = "Неверный файл конфигурации";
                    response.Success= false;
                }
                return response;

            }
            catch (Exception ex)
            {
                return new ResponseSetting { Success = false, Message = ex.Message };
            }
        }

        public ResponseSetting RemoveChannel(string name)
        {
            var response = new ResponseSetting();
            try
            {
                XDocument xdoc = XDocument.Load("settings.xml");
                XElement root = xdoc.Element("settings").Element("tapes");
                if (root != null)
                {
                    var tape = root.Elements("tape")
                        .FirstOrDefault(t => t.Attribute("name")?.Value == name);

                    if(tape != null)
                    {
                        tape.Remove();
                        xdoc.Save("settings.xml");
                        response.Message = "Канал успешно удален";
                        response.Success = true;
                        return response;
                    }

                    response.Success = false;
                    response.Message = "Канал не найден";

                }
                else
                {
                    response.Message = "Неверный файл конфигурации";
                    response.Success = false;
                }
                return response;

            }
            catch (Exception ex)
            {
                return new ResponseSetting { Success = false, Message = ex.Message };
            }
        }

        public ResponseSetting ChangeUpdateTime(int time)
        {
            var response = new ResponseSetting();
            try
            {
                XDocument xdoc = XDocument.Load("settings.xml");
                XElement updateTime = xdoc.Element("settings").Element("update");
                updateTime.Value = time.ToString();
                xdoc.Save("settings.xml");

                response.Success = true;
                response.Message = "Время обновления ленты изменено";
                return response;
            }
            catch(Exception ex)
            {
                return new ResponseSetting { Message = ex.Message, Success = false };
            }
        }

        public ResponseSetting EnabledChannel(Tape tape)
        {
            var response = new ResponseSetting();
            try
            {
                XDocument xdoc = XDocument.Load("settings.xml");
                XElement root = xdoc.Element("settings").Element("tapes");

                var channel = root.Elements("tape")
                       .FirstOrDefault(t => t.Attribute("name")?.Value == tape.Channel);

                if(channel == null)
                {
                    response.Success = false;
                    response.Message = "Канал не найден";
                    return response;
                }

                channel.Element("enabled").Value = tape.Enabled.ToString();
                xdoc.Save("settings.xml");
                response.Success = true;
                if (tape.Enabled)
                    response.Message = "Канал будет включён в ленту после обновления";
                else
                    response.Message = "Канал будет выключён после обновления";
                return response;
            }
            catch (Exception ex)
            {
                return new ResponseSetting { Message= ex.Message, Success = false };
            }
        }

        public ResponseSetting ChangeFormatByTags(bool isFormat)
        {
            var response = new ResponseSetting();
            try
            {
                XDocument xdoc = XDocument.Load("settings.xml");
                XElement format = xdoc.Element("settings").Element("format");
                format.Value = isFormat.ToString();
                xdoc.Save("settings.xml");
                if (isFormat)
                    response.Message = "Форматирование по тегам включён";
                else
                    response.Message = "Форматирование по тегам выключен";
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                return new ResponseSetting { Success = false, Message = ex.Message };
            }
        }

        public ResponseFeeder<Settings> GetSetting()
        {
            var tapeList = new List<Tape>();
            var response = new ResponseFeeder<Settings>();
            var settings = new Settings();
            try
            {
                XDocument xdoc = XDocument.Load("settings.xml");
                XElement root = xdoc.Element("settings");
                var proxy = root.Element("proxy");
                settings.Proxy = new Proxy();
                settings.Proxy.Url = proxy.Element("address").Value;
                settings.Proxy.UserName = proxy.Element("username").Value;
                settings.Proxy.Password = proxy.Element("password").Value;
                settings.Proxy.IsEnabled = Convert.ToBoolean(proxy.Attribute("enabled").Value);
                settings.IsFormat = Convert.ToBoolean(root.Element("format").Value);
                settings.UpdateTime = int.Parse(root.Element("update").Value);
                foreach(var tape in root.Element("tapes").Elements("tape"))
                {
                    var item = new Tape
                    {
                        Channel = tape.Attribute("name").Value,
                        Address = tape.Element("address").Value,
                        Enabled = Convert.ToBoolean(tape.Element("enabled").Value)
                    };
                    tapeList.Add(item);
                }
                settings.Tapes = tapeList;
                response.Data = settings;
                response.Success = true;
                response.isFormat = settings.IsFormat;
                return response;
            }
            catch (Exception ex)
            {
                return new ResponseFeeder<Settings> { Message = ex.Message, Success=false };
            }
        }

        public ResponseSetting ChangeProxyData(Proxy proxy)
        {
            var response = new ResponseSetting();

            try
            {
                XDocument xdoc = XDocument.Load("settings.xml");
                XElement root = xdoc.Element("settings").Element("proxy");
              
                root.Element("address").Value = proxy.Url;
                root.Element("username").Value = proxy.UserName;
                root.Element("password").Value = proxy.Password;
                xdoc.Save("settings.xml");

                response.Success = true;
                response.Message = "Данные прокси сервера обновлены";
                return response;
            }
            catch(Exception ex)
            {
                return new ResponseSetting { Message = ex.Message, Success = false };
            }
        }

        public ResponseSetting EnabledProxy(bool isEnabled)
        {
            var response = new ResponseSetting();

            try
            {
                XDocument xdoc = XDocument.Load("settings.xml");
                XElement root = xdoc.Element("settings").Element("proxy");
                root.Attribute("enabled").Value = isEnabled.ToString();
                xdoc.Save("settings.xml");

                if (isEnabled)
                    response.Message = "Прокси сервер включён";
                else
                    response.Message = "Прокси сервер выключён";
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                return new ResponseSetting { Success = false, Message = ex.Message };
            }
        }
    }
}
