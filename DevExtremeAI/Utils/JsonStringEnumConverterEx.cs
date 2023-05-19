﻿using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DevExtremeAI.Utils
{
    internal sealed class JsonStringEnumConverterEx<TEnum> 
        : JsonConverter<TEnum> where TEnum : struct, Enum
    {

        private readonly Dictionary<TEnum, string> _enumToString = new ();
        private readonly Dictionary<string, TEnum> _stringToEnum = new ();

        public JsonStringEnumConverterEx()
        {
            var type = typeof(TEnum);
            var values = Enum.GetValues<TEnum>();

            foreach (var value in values)
            {
                var enumMember = type.GetMember(value.ToString())[0];
                var attr = enumMember.GetCustomAttributes(typeof(EnumMemberAttribute), false)
                    .OfType<EnumMemberAttribute>()
                    .FirstOrDefault();

                _stringToEnum.Add(value.ToString(), value);

                if (attr?.Value != null)
                {
                    _enumToString.Add(value, attr.Value);
                    _stringToEnum.Add(attr.Value, value);
                }
                else
                {
                    _enumToString.Add(value, value.ToString());
                }
            }
        }

        public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var stringValue = reader.GetString();

            if (_stringToEnum.TryGetValue(stringValue, out var enumValue))
            {
                return enumValue;
            }

            return default;
        }

        public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(_enumToString[value]);
        }
    }
}
