using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Linguistics
{
    public class Syllable : List<Phoneme>
    {
        public Syllable()
        {

        }

        public Syllable(Syllable syllableToCopy)
        {
            foreach (Phoneme phoneme in syllableToCopy)
            {
                Add(phoneme);
            }
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (Phoneme phoneme in this)
            {
                stringBuilder.Append(phoneme.Grapheme);
            }

            return stringBuilder.ToString();
        }

        public string ToIPAString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (Phoneme phoneme in this)
            {
                stringBuilder.Append(phoneme.IPA);
            }

            return stringBuilder.ToString();
        }
    }

    public class Word : List<Syllable>
    {
        public string IPA { get { return ToIPAString(); } }
        public int TotalLength { get { return this.Sum(s => s.Count); } }

        public Word()
        {

        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            
            Syllable lastSyllable = null;
            Phoneme lastPhoneme = null;
            foreach (Syllable currentSyllable in this)
            {
                int positionInSyllable = 0;
                foreach (Phoneme currentPhoneme in currentSyllable)
                {
                    if (lastSyllable != null && positionInSyllable == 0 && currentPhoneme is Vowel)
                    {
                        stringBuilder.Append("'");
                    }

                    stringBuilder.Append(currentPhoneme.Grapheme);
                    lastPhoneme = currentPhoneme;
                    positionInSyllable++;
                }
                lastSyllable = currentSyllable;
            }
           
            /*
            Syllable currentSyllable = null,
                //lastSyllable = null,
                nextSyllable = null;
            for (int syllableIndex = 0; syllableIndex < this.Count; syllableIndex++)
            {
                currentSyllable = this[syllableIndex];
                if (syllableIndex - 1 < this.Count)
                    nextSyllable = this[syllableIndex + 1];

                for (int phonemeIndex = 0; phonemeIndex < currentSyllable.Count; phonemeIndex++)
                {
                    Phoneme currentPhoneme = currentSyllable[phonemeIndex];

                    if (phonemeIndex == 0 && syllableIndex > 0 && currentPhoneme is Vowel)
                    {
                        stringBuilder.Append("'");
                    }

                    if ( ! (phonemeIndex == currentSyllable.Count - 1 && currentPhoneme == nextSyllable[0] && currentPhoneme is Consonant) )
                    {
                        stringBuilder.Append(currentPhoneme.Symbol);
                    }
                }

                //lastSyllable = currentSyllable;
                nextSyllable = null;
            }
            */

            return stringBuilder.ToString();
        }

        public string ToIPAString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("/");

            foreach (Syllable currentSyllable in this)
            {
                foreach (Phoneme currentPhoneme in currentSyllable)
                {
                    stringBuilder.Append(currentPhoneme.IPA);
                }
            }

            stringBuilder.Append("/");

            return stringBuilder.ToString();
        }
    }
}
