using System;
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

        [Fact]
        public void attempt_to_retrieve_not_existing_value_raises_argument_exception()
        {
            var input = @"{""db"": {""connection"": ""string""}}";
            var testedObject = HermesConfiguration.ParseJson(input);

            Assert.Throws<ArgumentException>(() => testedObject["db"].GetValue<int>("port"));
        }

        [Fact]
        public void attempt_to_retrieve_not_existing_value_provided_default_value_results_in_default_value()
        {
            var input = @"{""db"": {""connection"": ""string""}}";
            var testedObject = HermesConfiguration.ParseJson(input);

            var actual = testedObject["db"].GetValue("port", 123);

            Assert.Equal(123, actual);
        }

        [Fact]
        public void attempt_to_retrieve_not_existing_config_section_results_in_argument_exception()
        {
            var input = @"{""db"": {""connection"": ""string""}}";
            var testedObject = HermesConfiguration.ParseJson(input);

            Assert.Throws<ArgumentException>(() => testedObject["dummy_section"]);
        }

        [Theory]
        [InlineData("db", true)]
        [InlineData("name", true)]
        [InlineData("blah", false)]
        public void test_if_existence_checks_work_properly(string propertyName, bool expected)
        {
            var input = @"{""name"": ""some name"",""db"": {""connection"": ""string""}}";
            var testedObject = HermesConfiguration.ParseJson(input);

            var actual = testedObject.Contains(propertyName);

            Assert.Equal(expected, actual);
        }
    }
}