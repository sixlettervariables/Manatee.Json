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
 
	File Name:		IJsonSchemaPropertyValidator.cs
	Namespace:		Manatee.Json.Schema.Validators
	Class Name:		IJsonSchemaPropertyValidator
	Purpose:		Defines functionality required to validate JSON in accordance with
					a schema.

***************************************************************************************/

namespace Manatee.Json.Schema.Validators
{
	internal interface IJsonSchemaPropertyValidator
	{
		bool Applies(JsonSchema schema, JsonValue json);
		SchemaValidationResults Validate(JsonSchema schema, JsonValue json, JsonValue root);
	}
}
