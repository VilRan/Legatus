using System;
using System.Collections.Generic;

namespace Linguistics
{
    class SimpleLanguage : Language
    {
        private static readonly string[] Consonants = { "b", "c", "ch", "d", "dz", "dh", "f", "g", "h", "j", "k", "l", "m", "n", "ng", "p", "q", "r", "s", "t", "ts", "th", "v", "w", "x", "y", "z" };
        private static readonly string[] Vowels = { "a", "ä", "e", "ë", "i", "ï", "o", "ö", "u", "ü" };

        public List<string> Onsets = new List<string>();
        public List<string> Nuclei = new List<string>();
        public List<string> Codae = new List<string>();
        public int WordMaxSyllables;

        public SimpleLanguage(Random rng)
        {
            int consonants = rng.Next(6, Consonants.Length + 1);
            for (int i = 0; i < consonants; i++)
            {
                Onsets.Add(Consonants[rng.Next(Consonants.Length)]);
                Codae.Add(Consonants[rng.Next(Consonants.Length)]);
            }

            int vowels = rng.Next(3, Vowels.Length + 1);
            for (int i = 0; i < vowels; i++)
            {
                Nuclei.Add(Vowels[rng.Next(Vowels.Length)]);
            }

            float complexity = Onsets.Count + Nuclei.Count;
            WordMaxSyllables = 2;//(int)Math.Round(50 / complexity);
        }

        public override string GenerateWord(Random rng)
        {
            string word = "";

            int syllables = rng.Next(1, WordMaxSyllables + 1);
            for (int i = 0; i < syllables; i++)
            {
                word += ConstructSyllable(rng);
            }
            
            return word;
        }

        private string ConstructSyllable(Random rng)
        {
            string syllable = "";

            if (rng.Next(100) < 50)
            {
                syllable += Onsets[rng.Next(Onsets.Count)];
            }

            syllable += Nuclei[rng.Next(Nuclei.Count)];

            if (rng.Next(100) < 50)
            {
                syllable += Codae[rng.Next(Onsets.Count)];
            }

            return syllable;
        }
    }
}
