using System;
using System.Collections.Generic;
using System.IO;

namespace Linguistics
{
    public class ListedLanguage : Language
    {
        public List<string> Words;

        public ListedLanguage(string filePath)
        {
            StreamReader streamReader = new StreamReader(filePath);
            string data = streamReader.ReadToEnd();
            streamReader.Close();

            Words = new List<string>();
            foreach (string word in data.Split(','))
            {
                Words.Add(word.Trim());
            }
        }

        public override string GenerateWord(Random rng)
        {
            return Words[rng.Next(Words.Count)];
        }
    }
}
