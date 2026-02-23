using System.Collections.Generic;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Utils
{
    public class GetRandom
    {
        public static List<T> GetRandomItems<T>(List<T> source, int count)
    {
        List<T> copy = new(source);
        List<T> result = new();
    
        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, copy.Count);
            result.Add(copy[index]);
        }

        return result;
    }
    }
}
