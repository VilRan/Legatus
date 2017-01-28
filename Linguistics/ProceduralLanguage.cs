using Legatus.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Linguistics
{
    public class ProceduralLanguage : Language
    {
        private const double MinimumComplexity = 100;

        public Dictionary<string, Word> Vocabulary = new Dictionary<string, Word>();
        public WeightedList<Consonant> Onsets = new WeightedList<Consonant>();
        public WeightedList<Vowel> Nuclei = new WeightedList<Vowel>();
        public WeightedList<Consonant> Codae = new WeightedList<Consonant>();
        public WeightedList<int> WordLengths = new WeightedList<int>();
        public Phonotactics Geminates;

        public string Name;
        public double WordInitialOnsetChance, WordMedialOnsetChance, NucleusChance, WordMedialCodaChance, WordFinalCodaChance;

        /* //For testing:
        public List<Phoneme> PhonemeList
        {
            get
            {
                return AllPhonemes.ToList();
            }
        }
        */
        public IEnumerable<Phoneme> AllPhonemes
        {
            get
            {
                foreach (Vowel vowel in Nuclei)
                    yield return vowel;
                foreach (Consonant consonant in AllConsonants)
                    yield return consonant;
            }
        }
        public IEnumerable<Consonant> AllConsonants
        {
            get
            {
                //IEnumerable<Consonant> consonants = Onsets.Union(Codae);
                foreach (Consonant consonant in Onsets.Union(Codae))
                    yield return consonant;
            }
        }

        public Phonotactics Nucleus { get { return GetPhonotacticsFromChance(NucleusChance); } }
        public Phonotactics WordInitialOnset { get { return GetPhonotacticsFromChance(WordInitialOnsetChance); } }
        public Phonotactics WordMedialOnset { get { return GetPhonotacticsFromChance(WordMedialOnsetChance); } }
        public Phonotactics WordMedialCoda { get { return GetPhonotacticsFromChance(WordMedialCodaChance); } }
        public Phonotactics WordFinalCoda { get { return GetPhonotacticsFromChance(WordFinalCodaChance); } }
        private Phonotactics GetPhonotacticsFromChance(double chance)
        {
            if (chance == 0)
                return Phonotactics.Forbidden;
            if (chance == 1)
                return Phonotactics.Required;
            return Phonotactics.Optional;
        }

        public double SyllableComplexity
        {
            get
            {
                return (1 + Onsets.Count) * (1 + Nuclei.Count) * (1 + Codae.Count)
                    * (NucleusChance + WordInitialOnsetChance + WordMedialOnsetChance * WordMedialCodaChance + WordFinalCodaChance) / 4;
            }
        }
        public bool AllowsOnset { get { return WordInitialOnset != Phonotactics.Forbidden || WordMedialOnset != Phonotactics.Forbidden; } }
        public bool AllowsCoda { get { return WordMedialCoda != Phonotactics.Forbidden || WordFinalCoda != Phonotactics.Forbidden; }  }


        public ProceduralLanguage(ProceduralLanguageFactory factory)
        {
            do
            {
                SelectPhonotactics(factory);
                SelectPhonemes(factory);
            }
            while (SyllableComplexity < MinimumComplexity);
            SelectWordLengths(factory);
            GenerateVocabulary(factory);

            Name = GenerateLongName(factory.RNG);
        }

        private void SelectPhonotactics(ProceduralLanguageFactory factory)
        {
            Random rng = factory.RNG;
            ImplementPhonotactics(factory.Nucleus.SelectRandom(), ref NucleusChance, 0.25, rng);
            ImplementPhonotactics(factory.WordInitialOnset.SelectRandom(), ref WordInitialOnsetChance, 0.25, rng);
            ImplementPhonotactics(factory.WordMedialOnset.SelectRandom(), ref WordMedialOnsetChance, 0.75, rng);
            ImplementPhonotactics(factory.WordMedialCoda.SelectRandom(), ref WordMedialCodaChance, 0.25, rng);
            WordFinalCodaChance = WordMedialCodaChance;
            Geminates = factory.Geminates.SelectRandom();
        }

        private void ImplementPhonotactics(Phonotactics phonotactics, ref double chance, double minChance, Random rng)
        {
            switch (phonotactics)
            {
                case Phonotactics.Forbidden:
                    chance = 0.0;
                    break;
                case Phonotactics.Optional:
                    //chance = minChance + (rng.NextDouble() + rng.NextDouble()) / 2 * (1 - minChance);
                    chance = minChance + rng.NextDouble() * (1 - minChance);
                    break;
                case Phonotactics.Required:
                    chance = 1.0;
                    break;
            }
        }

        private void SelectPhonemes(ProceduralLanguageFactory factory)
        {
            Random rng = factory.RNG;

            Onsets.Clear();
            Nuclei.Clear();
            Codae.Clear();

            Phonotactics diphthongs = factory.Diphthongs.SelectRandom();
            IEnumerable<Vowel> vowels = factory.SelectRandomVowels();
            switch (diphthongs)
            {
                case Phonotactics.Forbidden:
                    AddNuclei(vowels, 0.2, 1, rng);
                    break;
                case Phonotactics.Optional:
                    AddNuclei(vowels, 0.2, 1, rng);
                    AddNuclei(factory.SelectMatchingDiphthongs(vowels), 0.01, 0.1, rng);
                    break;
                case Phonotactics.Required:
                    AddNuclei(factory.SelectMatchingDiphthongs(vowels), 0.01, 0.1, rng);
                    break;
            }

            Phonotactics clusters = factory.ConsonantClusters.SelectRandom();
            IEnumerable<Consonant> consonants = factory.SelectRandomConsonants();
            switch (clusters)
            {
                case Phonotactics.Forbidden:
                    if (AllowsOnset)
                        AddOnsets(consonants, 0.2, 1, rng);
                    if (AllowsCoda)
                        AddCodae(consonants, 0.2, 1, rng);
                    break;
                case Phonotactics.Optional:
                    if (AllowsOnset)
                    {
                        AddOnsets(consonants, 0.2, 1, rng);
                        AddOnsetClusters(factory.SelectMatchingClusters(consonants), 0.01, 0.1, rng);
                    }
                    if (AllowsCoda)
                    {
                        AddCodae(consonants, 0.2, 1, rng);
                        AddCodaClusters(factory.SelectMatchingClusters(consonants), 0.01, 0.1, rng);
                    }
                    break;
                case Phonotactics.Required:
                    if (AllowsOnset)
                    {
                        AddOnsetClusters(factory.SelectMatchingClusters(consonants), 0.01, 0.1, rng);
                    }
                    if (AllowsCoda)
                    {
                        AddCodaClusters(factory.SelectMatchingClusters(consonants), 0.01, 0.1, rng);
                    }
                    break;
            }
        }

        private void AddNuclei(IEnumerable<Vowel> vowels, double baseWeight, double weightMultiplier, Random rng)
        {
            foreach (Vowel vowel in vowels)
            {
                double weight = baseWeight + rng.NextDouble() * weightMultiplier;
                Nuclei.Add(vowel, weight);
            }
        }

        private void AddOnsets(IEnumerable<Consonant> consonants, double baseWeight, double weightMultiplier, Random rng)
        {
            foreach (Consonant consonant in consonants)
            {
                double weight = baseWeight + rng.NextDouble() * weightMultiplier;
                Onsets.Add(consonant, weight);
            }
        }

        private void AddCodae(IEnumerable<Consonant> consonants, double baseWeight, double weightMultiplier, Random rng)
        {
            foreach (Consonant consonant in consonants)
            {
                double weight = baseWeight + rng.NextDouble() * weightMultiplier;
                Codae.Add(consonant, weight);
            }
        }

        private void AddOnsetClusters(IEnumerable<ConsonantCluster> clusters, double baseWeight, double weightMultiplier, Random rng)
        {
            foreach (ConsonantCluster cluster in clusters)
            {
                if (cluster.FirstSonority < cluster.Second.Sonority)
                {
                    double weight = baseWeight + rng.NextDouble() * weightMultiplier;
                    Onsets.Add(cluster, weight);
                }
            }
        }

        private void AddCodaClusters(IEnumerable<ConsonantCluster> clusters, double baseWeight, double weightMultiplier, Random rng)
        {
            foreach (ConsonantCluster cluster in clusters)
            {
                if (cluster.FirstSonority > cluster.Second.Sonority)
                {
                    double weight = baseWeight + rng.NextDouble() * weightMultiplier;
                    Codae.Add(cluster, weight);
                }
            }
        }

        private void SelectWordLengths(ProceduralLanguageFactory factory)
        {
            Random rng = factory.RNG;

            double complexityModifier = Math.Log(SyllableComplexity);
            double bias = 20 / complexityModifier * ( 1 + ( rng.NextDouble() - rng.NextDouble() / 4 ) );

            for (int i = 1; i < 6; i++)
            {
                WordLengths.Add(i, 1 / (0.5 + Math.Pow(Math.Abs(bias - i), 1.1)));
            }
        }

        private void GenerateVocabulary(ProceduralLanguageFactory factory)
        {
            Random rng = factory.RNG;
            foreach (string word in factory.WordKeyList)
            {
                Vocabulary.Add(word, ConstructWord(rng));
            }
        }

        public override string GenerateWord(Random rng)
        {
            Word word = ConstructWord(rng);
            return word.ToString();// +" " + word.ToIPAString();
        }

        public Word ConstructWord(Random rng)
        {
            Word word = new Word();
            /*
            int syllableCount = WordLengths.SelectRandom();

            if (syllableCount == 1)
            {
                Syllable syllable = new Syllable();
                int attempts = 0;
                do
                {
                    syllable.Clear();
                    syllable = ConstructSyllable(rng, true, true);
                    attempts++;
                }
                while (syllable.Count < 2 && attempts < 10);
                word.Add(syllable);
            }
            else
            {
                for (int i = 1; i <= syllableCount; i++)
                {
                    Syllable syllable = ConstructSyllable(rng, (i == 1), (i == syllableCount));

                    if (syllable.Count > 0)
                        word.Add(syllable);
                }
            }
            */

            int length = WordLengths.SelectRandom();

            bool isFirst = true;
            do
            {
                Syllable final = ConstructSyllable(rng, isFirst, true);

                if (word.TotalLength + final.Count >= length)
                {
                    word.Add(final);
                }
                else
                {
                    Syllable nonFinal = ConstructSyllable(rng, isFirst, false);

                    word.Add(nonFinal);
                }
                isFirst = false;
            }
            while (word.TotalLength < length);

            return word;// RemoveForbiddenElements(word);
        }

        private Syllable ConstructSyllable(Random rng, bool isInitial, bool isFinal)
        {
            Syllable syllable = new Syllable();

            if (isInitial)
            {
                if (rng.NextDouble() < WordInitialOnsetChance)
                    syllable.Add(Onsets.SelectRandom());
            }
            else
            {
                if (rng.NextDouble() < WordMedialOnsetChance)
                    syllable.Add(Onsets.SelectRandom());
            }

            if (rng.NextDouble() < NucleusChance)
                syllable.Add(Nuclei.SelectRandom());

            if (isFinal)
            {
                if (rng.NextDouble() < WordFinalCodaChance)
                    syllable.Add(Codae.SelectRandom());
            }
            else
            {
                if (rng.NextDouble() < WordMedialCodaChance)
                    syllable.Add(Codae.SelectRandom());
            }

            return syllable;
        }
        /*
        private Word RemoveForbiddenElements(Word word)
        {
            for (int syllableIndex = 0; syllableIndex < word.Count; syllableIndex++)
            {
                Syllable currentSyllable = word[syllableIndex];
                Syllable nextSyllable = null;
                if (syllableIndex + 1 < word.Count)
                    nextSyllable = word[syllableIndex + 1];

                Syllable newSyllable = new Syllable(currentSyllable);
                for (int phonemeIndex = 0; phonemeIndex < currentSyllable.Count; phonemeIndex++)
                {
                    Phoneme currentPhoneme = currentSyllable[phonemeIndex];

                    if (currentPhoneme is Consonant)
                        if (nextSyllable != null && phonemeIndex == currentSyllable.Count - 1)
                            if (currentPhoneme == nextSyllable[0])
                                if (Geminates == Phonotactics.Forbidden)
                                {
                                    newSyllable.Remove(currentPhoneme);
                                }
                }

                word[syllableIndex] = newSyllable;
            }

            return word;
        }
         */

        public Word ConstructLoanword(Word wordToLoan)
        {
            Word newWord = new Word();

            foreach (Syllable syllable in wordToLoan)
            {
                Syllable newSyllable = new Syllable();

                foreach (Phoneme phoneme in syllable)
                {
                    newSyllable.Add(SelectClosestPhoneme(phoneme));
                }

                newWord.Add(newSyllable);
            }

            return newWord;
        }

        private Phoneme SelectClosestPhoneme(Phoneme phonemeToMatch)
        {
            Phoneme closestSoFar = null;
            float closestDistance = float.MaxValue;
            foreach (Phoneme phonemeToTest in AllPhonemes)
            {
                if (phonemeToTest == phonemeToMatch)
                    return phonemeToTest;

                float distance = phonemeToMatch.CalculateDistance(phonemeToTest);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestSoFar = phonemeToTest;
                }
            }

            return closestSoFar;
        }
    }
}
