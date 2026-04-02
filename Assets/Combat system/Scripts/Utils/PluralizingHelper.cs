namespace NueGames.NueDeck.Scripts.Utils
{
    public class PluralizingHelper
    {
        public static string GetPluralizingString(int value, string singleSuffix, string pluralizingSuffix)
        {
            return value > 1 ? pluralizingSuffix : singleSuffix;
        }
    }
}