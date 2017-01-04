using Xunit;

namespace Armada.Tests
{
    public class HermesConfigurationTests
    {
        [Fact]
        public void valid_json_is_correctly_parsed_as_configuration()
        {
            var input = "{}";

            var actual = HermesConfiguration.ParseJson(input);

            Assert.NotNull(actual);
        }

        [Fact]
        public void top_level_string_properties_are_accessible()
        {
            var input = @"{""name"": ""value""}";
            var testedObject = HermesConfiguration.ParseJson(input);

            var actual = testedObject.GetValue<string>("name");

            Assert.Equal("value", actual);
        }

        [Fact]
        public void top_level_int_properties_are_accessible()
        {

            var input = @"{""name"": 1}";
            var testedObject = HermesConfiguration.ParseJson(input);

            var actual = testedObject.GetValue<int>("name");

            Assert.Equal(1, actual);
        }

        [Fact]
        public void nested_configurations_are_accessible()
        {

            var input = @"{""db"": {""connection"": ""string""}}";
            var testedObject = HermesConfiguration.ParseJson(input);

            var actual = testedObject["db"].GetValue<string>("connection");

            Assert.Equal("string", actual);
        }
    }
}