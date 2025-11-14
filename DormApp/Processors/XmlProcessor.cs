using DormApp.Models;
using DormApp.Strategies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Xsl;

namespace DormApp.Processors
{
    public class XmlProcessor
    {
        private IXmlParserStrategy _strategy = new XmlReaderStrategy();

        public void SetStrategy(IXmlParserStrategy strategy)
        {
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }

        // Викликаємо поточну стратегію
        public List<Resident> ParseAndSearch(string xmlPath, string attributeName, string attributeValue, string keyword)
        {
            return _strategy.ParseAndFind(xmlPath, attributeName, attributeValue, keyword);
        }

        // Повертає список імен атрибутів головних вузлів (Resident) у файлі
        public List<string> GetAttributeNames(string xmlPath)
        {
            var attrs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            using var reader = XmlReader.Create(xmlPath, new XmlReaderSettings { IgnoreComments = true, IgnoreWhitespace = true });
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Resident" && reader.HasAttributes)
                {
                    while (reader.MoveToNextAttribute())
                    {
                        attrs.Add(reader.Name);
                    }
                    reader.MoveToElement();
                }
            }

            // Додаємо також кілька зазвичай присутніх елементів як "атрибутів" для вибору
            attrs.Add("FullName");
            attrs.Add("Department");
            attrs.Add("Chair");
            attrs.Add("Course");
            return attrs.OrderBy(a => a).ToList();
        }

        // Повертає всі унікальні значення для атрибута attributeName у файлі
        public List<string> GetValuesForAttribute(string xmlPath, string attributeName)
        {
            var vals = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            using var reader = XmlReader.Create(xmlPath, new XmlReaderSettings { IgnoreComments = true, IgnoreWhitespace = true });
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Resident")
                {
                    if (reader.HasAttributes)
                    {
                        var found = false;
                        while (reader.MoveToNextAttribute())
                        {
                            if (reader.Name.Equals(attributeName, StringComparison.OrdinalIgnoreCase))
                            {
                                vals.Add(reader.Value);
                                found = true;
                            }
                        }
                        reader.MoveToElement();
                        if (found) continue;
                    }
                }

                // Якщо attributeName — це фактично ім'я дочірнього елемента (FullName, Department тощо)
                if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals(attributeName, StringComparison.OrdinalIgnoreCase))
                {
                    var v = reader.ReadElementContentAsString();
                    if (!string.IsNullOrEmpty(v)) vals.Add(v);
                }
            }

            return vals.OrderBy(v => v).ToList();
        }

        // Трансформує XML + XSL -> HTML (зберігає у savePath)
        public void TransformXmlToHtml(string xmlPath, string xslPath, string savePath)
        {
            var xslt = new XslCompiledTransform();
            xslt.Load(xslPath);
            using var writer = XmlWriter.Create(savePath, xslt.OutputSettings);
            xslt.Transform(xmlPath, writer);
        }
    }
}
