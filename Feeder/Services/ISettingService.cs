using Feeder.Models;

namespace Feeder.Services
{
    public interface ISettingService
    {
        public ResponseSetting AddChannel(Tape tape);
        public ResponseSetting RemoveChannel(string name);
        public ResponseFeeder<Settings> GetSetting();
        public ResponseSetting ChangeUpdateTime(int time);
        public ResponseSetting EnabledChannel(Tape tape);
        public ResponseSetting ChangeFormatByTags(bool isFormat);
        public ResponseSetting ChangeProxyData(Proxy proxy);
        public ResponseSetting EnabledProxy(bool isEnabled);
    }
}
