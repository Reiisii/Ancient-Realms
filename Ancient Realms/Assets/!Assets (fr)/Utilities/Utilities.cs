using System;
using System.Threading.Tasks;
using UnityEngine;

public class Utilities {
        public static Sprite Texture2dToSprite(Texture2D texture){
                Sprite image = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                
                return image;
        }
        public static string FormatCultureName(CultureEnum culture)
        {
                switch (culture)
                {
                case CultureEnum.HellenisticEgyptian:
                        return FormatHellenisticEgyptian();
                default:
                        return culture.ToString();
                }
        }
        public static string FormatNumber(int number)
        {
                return string.Format("{0:N0}", number);
        }
        public static string FormatSolana(double number)
        {
                return number.ToString("F9");
        }
        public static Color GetColorForCulture(CultureEnum culture)
        {
                switch (culture)
                {
                case CultureEnum.Roman:
                        return Utilities.HexToColor("#FF0000"); // Red
                case CultureEnum.Gallic:
                        return Utilities.HexToColor("#00FF00"); // Green
                case CultureEnum.Egyptian:
                        return Utilities.HexToColor("#FFD700"); // Golden Yellow
                case CultureEnum.Greek:
                        return Utilities.HexToColor("#0000FF"); // Blue
                case CultureEnum.Germanic:
                        return Utilities.HexToColor("#8B4513"); // Brown

                case CultureEnum.HellenisticEgyptian:
                return Utilities.HexToColor("#00FFFF"); // Example color for Germanic (brownish)
                default:
                        return Color.white; // Default color
                }
        }
        public static RarityEnum GetRarityFromString(string rarityString)
        {
                if (Enum.TryParse(rarityString, true, out RarityEnum rarity))
                {
                        return rarity;
                }
                else
                {
                        throw new ArgumentException($"Invalid rarity string: {rarityString}");
                }
        }
        public static Color HexToColor(string hex)
        {
                // Remove hash if present
                hex = hex.Replace("#", "");

                // Check if the hex is valid
                if (hex.Length != 6 && hex.Length != 8)
                {
                Debug.LogError("Invalid hex color code.");
                return Color.white; // Return white as a default color for invalid codes
                }

                // Parse color components
                byte r = System.Convert.ToByte(hex.Substring(0, 2), 16);
                byte g = System.Convert.ToByte(hex.Substring(2, 2), 16);
                byte b = System.Convert.ToByte(hex.Substring(4, 2), 16);
                byte a = (hex.Length == 8) ? System.Convert.ToByte(hex.Substring(6, 2), 16) : (byte)255;

                return new Color32(r, g, b, a);
        }
        private static string FormatHellenisticEgyptian()
        {
                return "Hellenistic Egyptian";
        }
}

public class locationData{
        public string name;
        public string description;
        public string imagePath;
        public string culture;
}
public class equipmentData{
        public string name;
        public string description;
        public string imagePath;
        public string culture;
}
public class artifactsData{
        public string name;
        public string description;
        public string imagePath;
        public string culture;
}

public class triviaData{
        public string title;
        public string description;
}

public class eventsData{
        public string title;
        public string description;
}

public class nftData{
        public string name;
        public string description;
        public string imagePath;
        public string rarity;
}