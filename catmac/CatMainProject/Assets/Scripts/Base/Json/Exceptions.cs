﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Json;

 
    public class JsonException : Exception
    {
        public JsonException(string message) : base(message) { }

        public JsonException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    public class JsonFormatException : JsonException
    {
        public JsonFormatException(string jsonString, Exception innerException)
            : base("Invalid JSON format.", innerException)
        {
            this.JsonString = jsonString;
        }

        public string JsonString { get; private set; }
    }

    public class MappingException : JsonException
    {
        public MappingException(string message) : base(message) { }

        public MappingException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    public class KeyNotFoundException : MappingException
    {
        public KeyNotFoundException(JsonObject jsonObj, string key) :
            base(BuildMessage(jsonObj, key))
        {
            this.Object = jsonObj;
            this.Key = key;
        }

        public JsonObject Object { get; private set; }

        public string Key { get; private set; }

        private static string BuildMessage(JsonObject jsonObj, string key)
        {
            return String.Format(
                "Cannot find the required key '{0}' in {1} ", key, jsonObj);
        }
    }

    public class ContractMissingException : MappingException
    {
        public ContractMissingException(object value) :
            base(BuildMessage(value))
        {
            this.Value = value;
        }

        public object Value { get; private set; }

        private static string BuildMessage(object value)
        {
            return String.Format(
                "Missing the contract when serializing {0}",
                value);
        }
    }

    public class ConversionException : MappingException
    {
        public PropertyInfo PropertyInfo { get; private set; }

        public object Value { get; private set; }

        public ConversionException(PropertyInfo propertyInfo, object value, Exception innerException)
            : base(BuildMessage(propertyInfo, value), innerException)
        {
            this.PropertyInfo = propertyInfo;
            this.Value = value;
        }

        private static string BuildMessage(PropertyInfo propertyInfo, object value)
        {
            return String.Format(
                "Error occurred when converting value {0} for {1}", value, propertyInfo);
        }
    }
 
