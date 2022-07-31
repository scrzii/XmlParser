using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlParserTest
{
    public class XmlParser
    {
        private class Border
        {
            public int Start { get; set; }
            public int End { get; set; }
            public int Length
            {
                get
                {
                    return End - Start + 1;
                }
            }

            public Border(int start, int end)
            {
                Start = start;
                End = end;
            }
        }

        private class XmlAttributeBorders
        {
            public XmlAttributeBorders(Border attributeName, Border value)
            {
                AttributeName = attributeName;
                Value = value;
            }

            public Border AttributeName { get; set; }
            public Border Value { get; set; }
        }

        private HashSet<char> AvailableChars { get; set; }
        private Dictionary<string, string> EscapeChars { get; set; }

        public XmlParser()
        {
            AvailableChars = new HashSet<char> { ':', '_' };
            AvailableChars.UnionWith(
                Enumerable.Range(0, 123)  // chr('z') = 122
                    .Select(_ => (char)_)
                    .Where(_ => char.IsLetterOrDigit(_))
            );

            EscapeChars = new Dictionary<string, string>
            {
                ["&quot;"] = "\"",
                ["&apos;"] = "\'",
                ["&lt;"] = "<",
                ["&gt;"] = ">",
                ["&amp;"] = "&",
                ["&#34;"] = "\"",
                ["&#39;"] = "\'",
                ["&#60;"] = "<",
                ["&#62;"] = ">",
                ["&#38;"] = "&"
            };
        }

        public string GetAttribute(string input, string elementName, string attribute)
        {
            var index = 0;
            while (index < input.Length)
            {
                var elStart = input.IndexOf("<", index);
                var elEnd = input.IndexOf(">", elStart);
                var nameBorder = GetNameBorder(input, elStart, elEnd);
                var name = input.Substring(nameBorder.Start, nameBorder.Length);
                if (name != elementName)
                {
                    index = elEnd + 1;
                    continue;
                }
                index = nameBorder.End + 1;

                while (true)
                {
                    var currAttrib = GetNextAttribute(input, index, elEnd);
                    if (currAttrib == null)
                    {
                        break;
                    }
                    index = currAttrib.Value.End + 2;
                    var currAttribName = input.Substring(currAttrib.AttributeName.Start, currAttrib.AttributeName.Length);
                    if (currAttribName != attribute)
                    {
                        continue;
                    }
                    return ReplaceEscapeChars(input.Substring(currAttrib.Value.Start, currAttrib.Value.Length));
                }
            }

            return null;
        }

        private string ReplaceEscapeChars(string data)
        {
            foreach (var sym in EscapeChars)
            {
                data = data.Replace(sym.Key, sym.Value);
            }
            return data;
        }

        private Border GetNameBorder(string input, int start, int end)
        {
            while (!AvailableChars.Contains(input[start]))
            {
                start++;
            }
            for (int i = start; i < end; i++)
            {
                if (!AvailableChars.Contains(input[i]))
                {
                    return new Border(start, i - 1);
                }
            }
            return new Border(start, end - 1);
        }

        private XmlAttributeBorders GetNextAttribute(string input, int start, int end)
        {
            while (!AvailableChars.Contains(input[start]))
            {
                start++;
                if (start == end)
                {
                    return null;
                }
            }
            var name = new Border(start, start);
            while (AvailableChars.Contains(input[name.End + 1]))
            {
                name.End++;
            }
            var startValue = name.End + 2;
            while (input[startValue] != '"' && input[startValue] != '\'')
            {
                startValue++;
            }
            var value = new Border(startValue + 1, startValue + 2);
            while (input[value.End + 1] != '"' && input[value.End + 1] != '\'')
            {
                value.End++;
            }
            return new XmlAttributeBorders(name, value);
        }
    }
}
