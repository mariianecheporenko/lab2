using System;
using System.Collections.Generic;
using System.Xml;
using DormApp.Models;

namespace DormApp.Strategies
{
    public class XmlDocumentStrategy : IXmlParserStrategy
    {
        public List<Resident> ParseAndFind(string xmlPath, string attributeName, string attributeValue, string keyword)
        {
            var result = new List<Resident>();
            var doc = new XmlDocument();
            doc.Load(xmlPath);

            var nodes = doc.SelectNodes("//Resident");
            if (nodes == null) return result;

            foreach (XmlNode node in nodes)
            {
                var r = new Resident();
                // атрибути вузла Resident
                if (node.Attributes != null)
                {
                    var roomAttr = node.Attributes["room"];
                    if (roomAttr != null) r.Room = roomAttr.Value;

                    var movedInAttr = node.Attributes["movedIn"];
                    if (movedInAttr != null) r.MovedInDate = movedInAttr.Value;

                    var facultyAttr = node.Attributes["faculty"];
                    if (facultyAttr != null) r.Faculty = facultyAttr.Value;
                }

                // дочірні елементи
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (child.NodeType != XmlNodeType.Element) continue;
                    var name = child.Name.ToLower();
                    var val = child.InnerText.Trim();
                    switch (name)
                    {
                        case "fullname": r.FullName = val; break;
                        case "department": r.Department = val; break;
                        case "chair": r.Chair = val; break;
                        case "course": r.Course = val; break;
                    }
                }

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
