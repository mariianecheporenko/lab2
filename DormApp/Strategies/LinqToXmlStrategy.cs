using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DormApp.Models;

namespace DormApp.Strategies
{
    public class LinqToXmlStrategy : IXmlParserStrategy
    {
        public List<Resident> ParseAndFind(string xmlPath, string attributeName, string attributeValue, string keyword)
        {
            var xdoc = XDocument.Load(xmlPath);
            var result = new List<Resident>();

            var nodes = xdoc.Descendants("Resident");

            foreach (var node in nodes)
            {
                var r = new Resident();
                r.Room = (string)node.Attribute("room") ?? "";
                r.MovedInDate = (string)node.Attribute("movedIn") ?? "";
                r.Faculty = (string)node.Attribute("faculty") ?? "";

                r.FullName = (string?)node.Element("FullName") ?? "";
                r.Department = (string?)node.Element("Department") ?? "";
                r.Chair = (string?)node.Element("Chair") ?? "";
                r.Course = (string?)node.Element("Course") ?? "";

                bool pass = true;
                if (!string.IsNullOrEmpty(attributeName) && !string.IsNullOrEmpty(attributeValue))
                {
                    var val = GetAttributeValueByName(r, attributeName);
                    pass = val != null && val.IndexOf(attributeValue, StringComparison.OrdinalIgnoreCase) >= 0;
                }
                if (pass && !string.IsNullOrEmpty(keyword))
                {
                    var combined = $"{r.FullName} {r.Faculty} {r.Department} {r.Chair} {r.Course} {r.Room} {r.MovedInDate}";
                    pass = combined.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;
                }
                if (pass) result.Add(r);
            }

            return result;
        }

        private string GetAttributeValueByName(Resident r, string name)
        {
            name = name.ToLower();
            return name switch
            {
                "faculty" => r.Faculty,
                "department" => r.Department,
                "chair" => r.Chair,
                "course" => r.Course,
                "room" => r.Room,
                "fullname" => r.FullName,
                "movedin" => r.MovedInDate,
                _ => null
            };
        }
    }
}
