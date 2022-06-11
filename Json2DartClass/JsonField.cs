using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Json2DartClass
{
    public abstract class JsonField
    {
        public abstract string dartTypeName { get; }
        public const string indentation = "   ";


        protected void AppendMany(StringBuilder sb, string s, int items)
        {
            for (int i = 0; i < items; i++)
            {
                sb.Append(s);
            }
        }

       
        public string DartFieldDeclaration(string name, bool notNull = false)
        {
            return $"\t{dartTypeName}{(notNull ? "" : "?")} {name};";
        }

        public abstract string DartMapAssignment(string name);

        public string DartConstuctorParams(string name, bool notNull, object value)
        {
            var str = $"this.{name}";
            if (notNull)
            {
                str += $"={(string.IsNullOrEmpty(value.ToString()) ? "\"\"": value) }";
            }
            return $"{str}, ";
        }

        public abstract string DartMapFetching(string name);


        public abstract void toString(StringBuilder sb, int? indents = null);

        public string toString(int? indent = null)
        {
            var sb = new StringBuilder();
            toString(sb, indent);
            return sb.ToString();
        }

        public override string ToString()
        {
            return toString();
        }
    }
}
