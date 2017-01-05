using System;
using Newtonsoft.Json.Linq;

namespace Armada
{
    public static class HermesConfiguration
    {
        public static IHermesConfiguration ParseJson(string jsonContent)
        {
            return new JTokenHermesConfiguration(JObject.Parse(jsonContent));
        }
    }

    public interface IHermesConfiguration
    {
        T GetValue<T>(string propertyName);
        
        T GetValue<T>(string propertyName, T defaultValue);

        bool Contains(string propertyOrSectionName);

        IHermesConfiguration this[string configSectionName] { get; }
    }

    internal class JTokenHermesConfiguration : IHermesConfiguration
    {
        private readonly JToken _element;
        public JTokenHermesConfiguration(JToken element)
        {
            _element = element;
        }

        public IHermesConfiguration this[string configSectionName]
        {
            get
            {
                if (Contains(configSectionName) == false)
                    throw new ArgumentException("Config section not found: " + configSectionName, nameof(configSectionName));

                return new JTokenHermesConfiguration(_element.SelectToken(configSectionName));
            }
        }

        public bool Contains(string propertyOrSectionName)
        {
            return _element[propertyOrSectionName] != null;
        }

        public T GetValue<T>(string propertyName)
        {
            if (_element[propertyName] == null)
                throw new ArgumentException("Property doesn't exist: " + propertyName, nameof(propertyName));

            return _element.Value<T>(propertyName);
        }

        public T GetValue<T>(string propertyName, T defaultValue)
        {
            if (_element[propertyName] == null)
                return defaultValue;

            return _element.Value<T>(propertyName);
        }
    }
}