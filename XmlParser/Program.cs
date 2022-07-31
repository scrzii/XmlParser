using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlParserTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var xml = new XmlParser();
            var textData = File.ReadAllText(@".\Data.xml");
            Console.WriteLine(xml.GetAttribute(textData, "somethingText", "attrib"));
        }
    }
}
