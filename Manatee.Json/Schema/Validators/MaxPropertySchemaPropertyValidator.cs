﻿/***************************************************************************************

	Copyright 2016 Greg Dennis

	   Licensed under the Apache License, Version 2.0 (the "License");
	   you may not use this file except in compliance with the License.
	   You may obtain a copy of the License at

		 http://www.apache.org/licenses/LICENSE-2.0

	   Unless required by applicable law or agreed to in writing, software
	   distributed under the License is distributed on an "AS IS" BASIS,
	   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	   See the License for the specific language governing permissions and
	   limitations under the License.
 
	File Name:		MaxPropertySchemaPropertyValidator.cs
	Namespace:		Manatee.Json.Schema.Validators
	Class Name:		MaxPropertySchemaPropertyValidator
	Purpose:		Validates schema with a "maxProperties" property.

***************************************************************************************/
namespace Manatee.Json.Schema.Validators
{
	internal class MaxPropertySchemaPropertyValidator : IJsonSchemaPropertyValidator
	{
		public bool Applies(JsonSchema schema, JsonValue json)
		{
			return schema.MaxProperties.HasValue && json.Type == JsonValueType.Object;
		}
		public SchemaValidationResults Validate(JsonSchema schema, JsonValue json, JsonValue root)
		{
			if (json.Object.Count > schema.MaxProperties)
				return new SchemaValidationResults(string.Empty, $"Expected: <= {schema.MaxProperties} properties; Actual: {json.Object.Count} properties.");
			return new SchemaValidationResults();
		}
	}
}
