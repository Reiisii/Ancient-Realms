using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

public class Utilities {
        private static System.Random random = new System.Random();
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
        public static bool CheckRequirements(QuestSO quest){
                List<QuestSO> playerCompletedQuest = PlayerStats.GetInstance().completedQuests;
                List<bool> arrayCompletion = new List<bool>();
                if(quest.requirements.Count < 1) return true;
                foreach(string questFinished in quest.requirements){
                QuestSO questComplete = playerCompletedQuest.FirstOrDefault(q => q.questID.Equals(questFinished));
                arrayCompletion.Add(questComplete != null && questComplete.isCompleted && !questComplete.isActive);
                }

                return arrayCompletion.All(b => b);
        }
        public static bool CheckRequirements(string[] questRequired){
                List<QuestSO> playerCompletedQuest = PlayerStats.GetInstance().completedQuests;
                List<bool> arrayCompletion = new List<bool>();
                if(questRequired.Length < 1) return true;
                foreach(string val in questRequired){
                        QuestSO q = playerCompletedQuest.FirstOrDefault(que => que.questID == val);
                        arrayCompletion.Add(q !=null ? true : false);
                }

                return arrayCompletion.All(b => b);
        }
        public static string FormatNumber(int number)
        {
                return string.Format("{0:N0}", number);
        }
        public static string FormatSolana(double number)
        {
                return number.ToString("F9");
        }
        public static bool npcHasQuest(NPCData npc){
                return npc.giveableQuest.Count > 0;
        }
        public static string GenerateUuid()
        {
            Guid uuid = Guid.NewGuid();
            return uuid.ToString();
        }
        public static string ValidateName(string name)
        {
                // Step 1: Trim spaces from start and end
                name = name.Trim();

                // Step 2: Check if the string is empty or only contains spaces
                if (string.IsNullOrEmpty(name))
                {
                return "Name cannot be empty!";
                }

                // Step 3: Check if the name exceeds 27 characters
                if (name.Length > 27)
                {
                return "Name cannot be longer than 27 characters!";
                }

                // Step 4: Remove multiple spaces between words
                name = Regex.Replace(name, @"\s{2,}", " ");

                // Return the cleaned name if all checks pass
                return name;
        }
        public static TriviaSO GetRandomNumberFromList(List<TriviaSO> numbers)
        {
                if (numbers == null || numbers.Count == 0)
                {
                throw new ArgumentException("The list is either null or empty.");
                }

                // Get a random index from the list
                int randomIndex = random.Next(numbers.Count);

                // Return the element at that random index
                return numbers[randomIndex];
        }
        public static string FormatTimeRemaining(TimeSpan timeRemaining)
        {
                 if (timeRemaining.TotalMinutes >= 1 && timeRemaining.TotalMinutes <= 10)
                {
                        int minutesRemaining = Mathf.FloorToInt((float)timeRemaining.TotalMinutes);
                        return $"{minutesRemaining} minute{(minutesRemaining > 1 ? "s" : "")}";
                }
                // If less than 1 minute remains, show it in seconds
                else if (timeRemaining.TotalSeconds < 60)
                {
                        int secondsRemaining = Mathf.FloorToInt((float)timeRemaining.TotalSeconds);
                        return $"{secondsRemaining} second{(secondsRemaining > 1 ? "s" : "")}";
                }
                return "Fetching...";
        }
        public static bool CheckIfLateBy10Minutes(DateTime dateTime)
        {
                // Convert scheduled time to local time zone
                DateTime localScheduledTime = dateTime.ToLocalTime();
                
                // Get current local time
                DateTime currentLocalTime = DateTime.Now;
                // Calculate the difference in minutes
                double minutesLate = (currentLocalTime - localScheduledTime).TotalMinutes;

                // Check if it is late by 10 minutes
                if (minutesLate > 10)
                {
                        return true;
                }
                else
                {
                        return false;
                }
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