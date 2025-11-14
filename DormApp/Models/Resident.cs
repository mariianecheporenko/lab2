using System;

namespace DormApp.Models
{
    public class Resident
    {
        public string FullName { get; set; } = "";
        public string Faculty { get; set; } = "";
        public string Department { get; set; } = "";
        public string Chair { get; set; } = "";
        public string Course { get; set; } = "";
        public string Room { get; set; } = "";
        public string MovedInDate { get; set; } = "";

        // Можна додати інші атрибути за потреби
        public string ToReadableString()
        {
            return $"П.І.П.: {FullName}\nФакультет: {Faculty}\nКафедра: {Chair}\nКурс: {Course}\nМісце: {Room}\nДата поселення: {MovedInDate}";
        }
    }
}
