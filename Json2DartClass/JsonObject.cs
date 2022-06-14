using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Json2DartClass
{
    public class JsonObject : JsonField
    {

        List<NameValue> nameValues = new List<NameValue>();

        public JsonObject(){}

        public JsonObject(params NameValue[] pairs)
        {
            this.nameValues.AddRange(pairs);
        }

		public JsonObject add(params NameValue[] pairs)
        {
            this.nameValues.AddRange(pairs);
			return this;
        }

		public JsonObject add(NameValue pair)
        {
            this.nameValues.Add(pair);
			return this;
        }

		public JsonObject add(string name, JsonField value)
        {
			getOrNew(name).value = value;
			return this;
        }

		public JsonObject add(string name, string value)
        {
			var nv = getOrNew(name);

            if (value != null)
            {
                nv.value = new JsonString(value);
                nv.initValue = "";
                nv.notNull = true;
            }
            else
            {
                nv.value = new JsonNull();
                nv.notNull = true;
            }
			return this;
        }

		public JsonObject add(string name, double value)
        {
            var obj = getOrNew(name);
            obj.value = new JsonNumber(value);
            obj.initValue = "0";
            obj.notNull = true;
            return this;
        }

        public JsonObject add(string name, bool value)
        {
            var obj = getOrNew(name);
            obj.value= new JsonBoolean(value);
            obj.initValue = "false";
            obj.notNull = true;

            return this;
        }

		/// <summary>
		/// gets the name-value pair with this name if exists, or adds a new one and return it
		/// </summary>
		NameValue getOrNew(string name)
		{
			NameValue nv = nameValues.FirstOrDefault(n => n.name == name);
			if (nv == null)
			{
				nv = new NameValue(name);
				nameValues.Add(nv);
			}
			return nv;
		}

        public int Length { get { return nameValues.Count; } }

        public NameValue[] getNameVlaues()
        {
            return nameValues.ToArray();
        }

        public JsonField getValue(int index)
        {
            return nameValues.ElementAt(index).value;
        }

        public JsonField getValue(string name)
        {
            return nameValues.First(nv => nv.name == name).value;
        }

        public string getString(string name)
        {
            JsonString tmp = (JsonString)getValue(name);
            return tmp.value;
        }

        public double getDouble(string name)
        {
            JsonNumber tmp = (JsonNumber)getValue(name);
            return tmp.value;
        }

        public int getInt(string name)
        {
            JsonNumber tmp = (JsonNumber)getValue(name);
            return (int)tmp.value;
        }

        public bool getBool(string name)
        {
            JsonBoolean tmp = (JsonBoolean)getValue(name);
            return tmp.value;
        }

		public JsonObject getObject(string name)
		{
			return (JsonObject)getValue(name);
		}

		public JsonArray getArray(string name)
		{
			return (JsonArray)getValue(name);
		}

        

		public override void toString(StringBuilder sb, int? indents)
        {
            
            if (indents != null)
            {
                //appendMany(sb, indentation, (int)indents); 
                sb.Append("{");sb.Append("\r\n");
                
                for (int i = 0; i < this.nameValues.Count; i++ )
                {
                    AppendMany(sb, indentation, (int)indents + 1); 
                    nameValues[i].render(sb, indents+1);
                    if (i < nameValues.Count - 1)
                    {
                        sb.AppendLine(",");
                    }
                    else
                    {
                        sb.AppendLine();
                    }
                }
                AppendMany(sb, indentation, (int)indents);
                sb.Append("}");
            }
            else
            {
                sb.Append("{");
                foreach (NameValue item in this.nameValues)
                {
                    item.render(sb); sb.Append(",");
                }
                sb.Remove(sb.Length - 1, 1);//remove the last comma
                sb.Append("}");
            }
        }


        public override string dartTypeName => className ?? throw new Exception("set class name first");


        public override string DartMapAssignment(string name)
        {
            return $"\t\tif (this.{name} != null) {{\r\n" +
                   $"\t\t\tdata['{name}'] = this.{name}!.toMap();\r\n"+
                   $"\t\t}}";
        }

        public override string DartMapFetching(string name)
        {
            return $"\t\t{name}: map['{name}'] != null ? ({dartTypeName}.fromMap(map['{name}'])) : null, ";
        }

        string className;
        public void setClassName(string name) => className = name;

        public string createDartClass(StringBuilder sb)
        {
            sb.AppendLine($"import 'dart:convert';");
            sb.AppendLine($"class {dartTypeName} {{");

            // declarations
            foreach (var item in this.nameValues)
            {
                if(item.value is JsonObject)
                {
                    setInnerType(item.value as JsonObject, item.name);
                }
                //
                if(item.value is JsonArray && (item.value as JsonArray).Length > 0 && (item.value as JsonArray).getValue(0) is JsonObject)
                {
                    setInnerType((item.value as JsonArray).getValue(0) as JsonObject, item.name);
                }
                sb.AppendLine(item.value.DartFieldDeclaration(item.name, item.notNull));
            }

            sb.AppendLine();

            // constructor
            sb.Append($"\t{dartTypeName}({{");
            foreach (var item in this.nameValues)
            {
                sb.Append(item.value.DartConstuctorParams(item.name, item.notNull, item.initValue));
            }
            sb.AppendLine("});");
            sb.AppendLine();

            // from map method
            sb.AppendLine($"\tfactory {dartTypeName}.fromMap(Map<String, dynamic> map) {{");
            sb.AppendLine($"\t\treturn {dartTypeName}(");
            foreach (var item in this.nameValues)
            {
                sb.AppendLine("\t\t\t" + item.value.DartMapFetching(item.name));
            }
            sb.AppendLine("\t\t);");
            sb.AppendLine("\t}");
            sb.AppendLine();

            // to map method
            sb.AppendLine("\tMap<String, dynamic> toMap() {");
            sb.AppendLine("\t\tfinal Map<String, dynamic> data = <String, dynamic>{};");
            //
            foreach (var item in this.nameValues)
            {
                sb.AppendLine(item.value.DartMapAssignment(item.name));
            }
            //
            sb.AppendLine("\t\treturn data;");
            sb.AppendLine("\t}");
            sb.AppendLine();

            // to json method
            sb.AppendLine("\tString toJson() => json.encode(toMap());");
            sb.AppendLine();

            // from json method
            sb.AppendLine($"\tfactory {dartTypeName}.fromJson (String source) => {dartTypeName}.fromMap(json.decode(source));");
            sb.AppendLine();

            // operator method
            sb.AppendLine("\t@override");
            sb.AppendLine("\tbool operator ==(Object other) {");
            sb.AppendLine("\t\tif (identical(this, other)) return true;");
            sb.AppendLine($"\t\treturn other is {dartTypeName} &&");
            foreach (var item in this.nameValues)
            {
                sb.Append($"\t\tother.{item.name} == {item.name}");
                if(nameValues.IndexOf(item) < this.nameValues.Count - 1)
                {
                    sb.AppendLine(" &&");
                }
                else
                {
                    sb.AppendLine(";");
                    
                }
            }
            sb.AppendLine("\t}");
            sb.AppendLine();

            // hashCode 
            sb.AppendLine("\t@override");
            sb.Append("\tint get hashCode => ");
            foreach (var item in this.nameValues)
            {
                sb.Append($"{item.name}.hashCode");
                if (this.nameValues.IndexOf(item) < this.nameValues.Count - 1)
                {
                    sb.Append(" ^ ");
                }
                else
                {
                    sb.AppendLine(";");
                    
                }
            }

            sb.AppendLine();

            sb.AppendLine("}");
            sb.AppendLine("\t");
            //
            return sb.ToString();
        }

        private void setInnerType(JsonObject jsonObject, string name)
        {
            jsonObject.setClassName(capitalizeFirstLetter(name));
            DartClassGenerator.innerObjects.Enqueue(jsonObject);
        }

        public static string capitalizeFirstLetter(string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            char[] letters = source.ToCharArray();
            letters[0] = char.ToUpper(letters[0]);
            return new string(letters);
        }

    }


    public class NameValue
    {
        public string name;
        public JsonField value;
        public bool notNull;
        public string initValue;

        public NameValue(string name)
        {
            this.name = name;
        }

        public NameValue(string name, JsonField value)
        {
            this.name = name;
            this.notNull = false;
            this.value = value;
        }

        public NameValue(string name, string value)
        {
            this.name = name;
            this.notNull = value != null;
            if (notNull)
            {
                this.value = new JsonString(value);
            }
            else
            {
                this.value = new JsonNull();
            }
        }

        public NameValue(string name, bool value)
        {
            this.name = name;
            this.notNull = true;
            this.value = new JsonBoolean(value);
        }

        public NameValue(string name, double value)
        {
            this.name = name;
            this.notNull = true;
            this.value = new JsonNumber(value);
        }

        internal void render(StringBuilder sb, int? indents= null)
        {
			sb.AppendFormat("\"{0}\":", name);
			value.toString(sb, indents);
		}


        private void appendMany(StringBuilder sb, string s, int times)
        {
            for (int i = 0; i < times; i++)
            {
                sb.Append(s);
            }
        }

	}
}
