using System;

namespace Linguistics
{
    /// <summary>
    /// Generates random names based on the English language, using an Adjective-Noun or Noun-Noun format.
    /// </summary>
    public class EnglishLanguage : Language
    {
        private static string[] Starts = { 
                                             "ale", 
                                             "axe", 

                                             "bear", 
                                             "black", 
                                             "blue", 
                                             "boat", 
                                             "boot", 
                                             "bright", 
                                             "bronze", 

                                             "cat", 
                                             "calm", 
                                             "cheese", 
                                             "copper", 
                                             "corpse", 
                                             "crater", 

                                             "dagger", 
                                             "dark", 
                                             "day", 
                                             "dawn", 
                                             "dead", 
                                             "death",
                                             "deep", 
                                             "dog", 
                                             "dusk", 

                                             "east", 
                                             "elk", 
                                             "end", 
                                             "eye", 

                                             "feast", 
                                             "flesh", 
                                             "fog", 
                                             "fool", 
                                             "foot",
                                             "full", 
                                             "fun", 

                                             "ghost", 
                                             "gloom", 
                                             "god", 
                                             "gold", 
                                             "grace", 
                                             "green", 

                                             "hammer", 
                                             "hand", 
                                             "hag", 
                                             "hat", 
                                             "haste", 
                                             "head", 
                                             "high", 
                                             "hog", 
                                             "holy", 
                                             "horse", 

                                             "iron", 

                                             "joy", 

                                             "lazy", 
                                             "life", 
                                             "low", 
                                             "lush", 

                                             "mad", 
                                             "man", 
                                             "mist", 
                                             "moon", 
                                             "mouse", 
                                             "mule", 
                                             "murder", 

                                             "nail", 
                                             "night", 
                                             "north", 

                                             "plague", 

                                             "rage", 
                                             "rain", 
                                             "red", 
                                             "rock", 
                                             "rust", 

                                             "sage", 
                                             "salt", 
                                             "sane", 
                                             "shoe", 
                                             "silver", 
                                             "slow", 
                                             "song", 
                                             "sorrow", 
                                             "soul", 
                                             "south", 
                                             "star", 
                                             "steel", 
                                             "still", 
                                             "stone", 
                                             "sun", 
                                             "sweet", 
                                             "sword", 

                                             "tooth", 
                                             "true", 

                                             "war", 
                                             "white", 
                                             "wise", 
                                             "west", 

                                             "yellow"
                                         };
        private static string[] Ends = { 
                                           "bottom", 
                                           "bridge", 
                                           "brook", 
                                           "bury", 
                                           "castle", 
                                           "cave", 
                                           "church",
                                           "coast", 
                                           "dale", 
                                           "field", 
                                           "ford", 
                                           "fort", 
                                           "forest", 
                                           "garden", 
                                           "gate", 
                                           "hill", 
                                           "isle", 
                                           "lake", 
                                           "land", 
                                           "manor", 
                                           "mark", 
                                           "market", 
                                           "mount", 
                                           "park", 
                                           "port", 
                                           "river", 
                                           "shire", 
                                           "tower", 
                                           "town", 
                                           "vale", 
                                           "village", 
                                           "ville", 
                                           "water", 
                                           "well", 
                                           "wood" 
                                       };

        public EnglishLanguage()
        {

        }

        public override string GenerateWord(Random rng)
        {
            string word = "";

            word += Starts[rng.Next(Starts.Length)];
            word += Ends[rng.Next(Ends.Length)];

            return word;
        }
    }
}
