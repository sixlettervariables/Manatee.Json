using System.IO;
using Manatee.Json.Internal;

namespace Manatee.Json.Parsing
{
	internal class ObjectParser : IJsonParser
	{
		public bool Handles(char c)
		{
			return c == '{';
		}
		public string TryParse(string source, ref int index, out JsonValue value, bool allowExtraChars)
		{
			var obj = new JsonObject();
			value = obj;
			var length = source.Length;
			index++;
			while (index < length)
			{
				char c;
				var message = source.SkipWhiteSpace(ref index, length, out c);
				if (message != null) return message;
				// check for empty object
				if (c == '}')
					if (obj.Count == 0)
					{
						index++;
						break;
					}
					else return "Expected key.";
				// get key
				JsonValue keyValue = null;
				while (keyValue == null && message == null)
					message = JsonParser.Parse(source, ref index, out keyValue);
				if (keyValue == null || keyValue.Type != JsonValueType.String) return "Expected key.";
				if (message != null) return message;
				var key = keyValue.String;
				// check for colon
				message = source.SkipWhiteSpace(ref index, length, out c);
				if (message != null) return message;
				if (c != ':')
				{
					obj.Add(key, null);
					return "Expected ':'.";
				}
				index++;
				// get value (whitespace is removed in Parse)
				JsonValue item = null;
				while (item == null && message == null)
					message = JsonParser.Parse(source, ref index, out item);
				obj.Add(key, item);
				if (message != null) return message;
				message = source.SkipWhiteSpace(ref index, length, out c);
				if (message != null) return message;
				// check for end or separator
				index++;
				if (c == '}')
				{
					break;
				}
				if (c != ',') return "Expected ','.";
			}
			return null;
		}
		public string TryParse(StreamReader stream, out JsonValue value)
		{
			var obj = new JsonObject();
			value = obj;
			while (!stream.EndOfStream)
			{
				stream.Read(); // waste the '{' or ','
				char c;
				var message = stream.SkipWhiteSpace(out c);
				if (message != null) return message;
				// check for empty object
				if (c == '}')
					if (obj.Count == 0)
					{
						stream.Read(); // waste the '}'
						break;
					}
					else return "Expected key.";
				// get key
				JsonValue keyValue = null;
				while (keyValue == null && message == null)
					message = JsonParser.Parse(stream, out keyValue);
				if (keyValue == null || keyValue.Type != JsonValueType.String) return "Expected key.";
				if (message != null) return message;
				var key = keyValue.String;
				// check for colon
				message = stream.SkipWhiteSpace(out c);
				if (message != null) return message;
				if (c != ':')
				{
					obj.Add(key, null);
					return "Expected ':'.";
				}
				stream.Read(); // waste the ':'
				// get value (whitespace is removed in Parse)
				JsonValue item;
				message = JsonParser.Parse(stream, out item);
				obj.Add(key, item);
				if (message != null) return message;
				message = stream.SkipWhiteSpace(out c);
				if (message != null) return message;
				// check for end or separator
				if (c == '}')
				{
					stream.Read(); // waste the '}'
					break;
				}
				if (c != ',') return "Expected ','.";
			}
			return null;
		}
	}
}