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
                return new JTokenHermesConfiguration(_element.SelectToken(configSectionName));
            }
        }

        public T GetValue<T>(string propertyName)
        {
            return _element.Value<T>(propertyName);
        }
    }
}