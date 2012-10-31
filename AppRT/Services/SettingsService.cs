using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Composition;
using System.IO;
using System.Linq;
using System.Text;
using Windows.Storage;

namespace AppRT.Services
{
    [Export]
    [Shared]
    public class SettingsService
    {
        private SettingsSet _local;
        private SettingsSet _roaming;

        public SettingsSet Local
        {
            get { return _local ?? (_local = new WindowsSettingsSet(ApplicationData.Current.LocalSettings)); }
        }

        public SettingsSet Roaming
        {
            get { return _roaming ?? (_roaming = new WindowsSettingsSet(ApplicationData.Current.RoamingSettings)); }
        }
    }

    public abstract class SettingsSet
    {
        private JsonSerializer _serializer = JsonSerializer.Create(new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All
        });

        public virtual string GetString(string key) { return GetString(key, null); }
        public abstract string GetString(string key, string defaultValue);
        public abstract void SetString(string key, string value);

        public virtual T Get<T>(string key) { return Get<T>(key, default(T)); }
        public virtual T Get<T>(string key, T defaultValue)
        {
            return (T)Get(key, defaultValue);
        }

        public virtual object Get(string key) { return Get(key, null); }
        public virtual object Get(string key, object defaultValue)
        {
            string str = GetString(key);
            if (String.IsNullOrEmpty(str))
            {
                return defaultValue;
            }

            using (var reader = new JsonTextReader(new StringReader(str)))
            {
                return _serializer.Deserialize(reader);
            }
        }

        public virtual void Set<T>(string key, T value)
        {
            StringBuilder sb = new StringBuilder();
            using (var writer = new StringWriter(sb))
            {
                _serializer.Serialize(writer, value);
            }
            SetString(key, sb.ToString());
        }

        public abstract void Delete(string key);
    }

    public class WindowsSettingsSet : SettingsSet
    {
        private ApplicationDataContainer _dataContainer;

        public WindowsSettingsSet(ApplicationDataContainer dataContainer)
        {
            _dataContainer = dataContainer;
        }

        public override string GetString(string key, string defaultValue)
        {
            return (string)_dataContainer.Values[key] ?? defaultValue;
        }

        public override void SetString(string key, string value)
        {
            _dataContainer.Values[key] = value;
        }

        public override void Delete(string key)
        {
            _dataContainer.Values.Remove(key);
        }
    }
}
