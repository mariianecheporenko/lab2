using DormApp.Models;
using System.Collections.Generic;

namespace DormApp.Strategies
{
    // Strategy pattern: кожна стратегія реалізує цей інтерфейс.
    public interface IXmlParserStrategy
    {
        // Парсить XML та повертає список Residents, які відповідають фільтру:
        // attributeName != "" -> шукати по атрибуту; attributeValue - бажане значення
        // keyword - додатковий текстовий фільтр (шукає в елементах чи атрибутах)
        List<Resident> ParseAndFind(string xmlPath, string attributeName, string attributeValue, string keyword);
    }
}
