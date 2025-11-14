using System;
using System.Collections.Generic;
using System.Xml;
using DormApp.Models;

namespace DormApp.Strategies
{
    // Використовує XmlReader (forward-only) — аналог SAX у .NET
    public class XmlReaderStrategy : IXmlParserStrategy
    {
        public List<Resident> ParseAndFind(string xmlPath, string attributeName, string attributeValue, string keyword)
        {
            var result = new List<Resident>();
            using var reader = XmlReader.Create(xmlPath, new XmlReaderSettings { IgnoreComments = true, IgnoreWhitespace = true });

            Resident current = null;
            string currentElement = "";

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    currentElement = reader.Name;
                    if (reader.Name == "Resident") // наш вузол-об'єкт
                    {
                        current = new Resident();
                        // читаємо атрибути вузла Resident, якщо є
                        if (reader.HasAttributes)
                        {
                            while (reader.MoveToNextAttribute())
                            {
                                var n = reader.Name;
                                var v = reader.Value;
                                // зберігаємо деякі атрибути
                                if (n.Equals("room", StringComparison.OrdinalIgnoreCase)) current.Room = v;
                                if (n.Equals("movedIn", StringComparison.OrdinalIgnoreCase)) current.MovedInDate = v;
                                if (n.Equals("faculty", StringComparison.OrdinalIgnoreCase)) current.Faculty = v;
                                // інші атрибути можна додати
                            }
                            reader.MoveToElement(); // повернутись до елемента
                        }
                    }
                    else if (current != null && !reader.IsEmptyElement)
                    {
                        // читаємо внутрішній текст елемента наступним кроком
                    }
                }
                else if (reader.NodeType == XmlNodeType.Text)
                {
                    var txt = reader.Value.Trim();
                    if (current != null && !string.IsNullOrEmpty(txt))
                    {
                        // Зв'язуємо текст з останнім зустрінутим елементом
                        switch (currentElement.ToLower())
                        {
                            case "fullname":
                                current.FullName = txt; break;
                            case "department":
                                current.Department = txt; break;
                            case "chair":
                                current.Chair = txt; break;
                            case "course":
                                current.Course = txt; break;
                            // додати як треба
                        }
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                {
                    if (reader.Name == "Resident" && current != null)
                    {
                        // перевіряємо фільтри (attributeName / attributeValue / keyword)
                        bool pass = true;
                        if (!string.IsNullOrEmpty(attributeName) && !string.IsNullOrEmpty(attributeValue))
                        {
                            // намагаємося порівняти за відомими полями (faculty, room, course, тощо)
                            var val = GetAttributeValueByName(current, attributeName);
                            pass = val != null && val.IndexOf(attributeValue, StringComparison.OrdinalIgnoreCase) >= 0;
                        }
                        if (pass && !string.IsNullOrEmpty(keyword))
                        {
                            // простий пошук по всіх полях
                            var combined = $"{current.FullName} {current.Faculty} {current.Department} {current.Chair} {current.Course} {current.Room} {current.MovedInDate}";
                            pass = combined.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;
                        }
                        if (pass) result.Add(current);

                        current = null;
                    }
                }
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
