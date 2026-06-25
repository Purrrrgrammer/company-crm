using CompanyCRM.MVVM.Models;

namespace CompanyCRM.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Position position)
        {
            switch (position)
            {
                case Position.Director:
                    return "Руководитель";
                case Position.Employee:
                    return "Работник";
                default:
                    return position.ToString();
            }
        }
    }
}