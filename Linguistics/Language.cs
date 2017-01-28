using System;

namespace Linguistics
{
    public abstract class Language
    {
        public string GenerateName(Random rng)
        {
            string name = GenerateWord(rng);

            if (name.Length > 0)
            {
                string first = name.Substring(0, 1);
                string upper = first.ToUpper();
                name = name.Remove(0, 1);
                name = name.Insert(0, upper);
            }

            return name;
        }

        public string GenerateLongName(Random rng)
        {
            return GenerateName(rng) + " " + GenerateName(rng);
        }

        public abstract string GenerateWord(Random rng);
    }
}
