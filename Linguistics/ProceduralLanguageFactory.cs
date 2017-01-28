using System;
using System.Collections.Generic;
using System.Xml;
using System.Globalization;
using System.Reflection;
using Legatus.Collections;

namespace Linguistics
{
    public enum Phonotactics { Forbidden, Optional, Required }

    public class ProceduralLanguageFactory
    {
        public Random RNG;
        //public Dictionary<Tuple<Vowel, Vowel>, Diphthong> Diphthongs = new Dictionary<Tuple<Vowel,Vowel>,Diphthong>();
        public Dictionary<Vowel, Dictionary<Vowel, Diphthong>> DiphthongList = new Dictionary<Vowel, Dictionary<Vowel, Diphthong>>();
        public Dictionary<Consonant, Dictionary<Consonant, ConsonantCluster>> ConsonantClusterList = new Dictionary<Consonant, Dictionary<Consonant, ConsonantCluster>>();
        public Dictionary<string, Articulation> ArticulationList = new Dictionary<string, Articulation>();
        public List<Vowel> VowelList = new List<Vowel>();
        public List<Consonant> ConsonantList = new List<Consonant>();
        public List<string> WordKeyList = new List<string>();
        public WeightedList<Phonotactics> WordInitialOnset = new WeightedList<Phonotactics>();
        public WeightedList<Phonotactics> WordMedialOnset = new WeightedList<Phonotactics>();
        public WeightedList<Phonotactics> Nucleus = new WeightedList<Phonotactics>();
        public WeightedList<Phonotactics> WordMedialCoda = new WeightedList<Phonotactics>();
        public WeightedList<Phonotactics> WordFinalCoda = new WeightedList<Phonotactics>();
        public WeightedList<Phonotactics> Diphthongs = new WeightedList<Phonotactics>();
        public WeightedList<Phonotactics> ConsonantClusters = new WeightedList<Phonotactics>();
        public WeightedList<Phonotactics> Geminates = new WeightedList<Phonotactics>();

        public ProceduralLanguageFactory(Random rng)
        {
            RNG = rng;
            ReadArticulations("Linguistics/Articulation.xml");
            ReadPhonemes("Linguistics/Phonemes.xml");
            ReadPhonotactics("Linguistics/Phonotactics.xml");
            ReadWords("Linguistics/Words.xml");
            GenerateDiphthongs();
            GenerateConsonantClusters();
        }

        public IEnumerable<Vowel> SelectRandomVowels()
        {
            foreach (Vowel vowel in VowelList)
                if (RNG.NextDouble() < vowel.Occurence)
                    yield return vowel;
        }

        public IEnumerable<Diphthong> SelectMatchingDiphthongs(IEnumerable<Vowel> vowels)
        {
            foreach (Vowel first in vowels)
                foreach (Vowel second in vowels)
                    yield return DiphthongList[first][second];
        }

        public IEnumerable<Consonant> SelectRandomConsonants()
        {
            foreach (Consonant consonant in ConsonantList)
                if (RNG.NextDouble() < consonant.Occurence)
                    yield return consonant;
        }
        /*
        public IEnumerable<ConsonantCluster> SelectMatchingGeminates(IEnumerable<Consonant> consonants)
        {
            foreach (Consonant consonant in consonants)
                yield return ConsonantClusterList[consonant][consonant];
        }
        */
        public IEnumerable<ConsonantCluster> SelectMatchingClusters(IEnumerable<Consonant> consonants)
        {
            foreach (Consonant first in consonants)
                foreach (Consonant second in consonants)
                    yield return ConsonantClusterList[first][second];
        }

        private void ReadArticulations(string filePath)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(filePath);
            foreach (XmlNode node in xml.SelectNodes("Articulations/Articulation"))
            {
                string definition = node.Attributes.GetNamedItem("Definition").Value;
                float position = float.Parse(node.Attributes.GetNamedItem("Position").Value, CultureInfo.InvariantCulture);
                float sonority = float.Parse(node.Attributes.GetNamedItem("Sonority").Value, CultureInfo.InvariantCulture);

                ArticulationList.Add(definition, new Articulation(definition, position, sonority));
            }
        }

        private void ReadPhonemes(string filePath)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(filePath);
            foreach (XmlNode node in xml.SelectNodes("Phonemes/Vowels/Vowel"))
            {
                string height = node.Attributes.GetNamedItem("Height").Value;
                string backness = node.Attributes.GetNamedItem("Backness").Value;
                string roundedness = node.Attributes.GetNamedItem("Roundedness").Value;
                string grapheme = node.Attributes.GetNamedItem("Grapheme").Value;
                string ipa = node.Attributes.GetNamedItem("IPA").Value;
                float occurence = float.Parse(node.Attributes.GetNamedItem("Occurence").Value, CultureInfo.InvariantCulture);

                VowelList.Add(new Vowel(ArticulationList[height], ArticulationList[backness], ArticulationList[roundedness], grapheme, ipa, occurence));
            }

            foreach (XmlNode node in xml.SelectNodes("Phonemes/Consonants/Consonant"))
            {
                string voicing = node.Attributes.GetNamedItem("Voicing").Value;
                string place = node.Attributes.GetNamedItem("Place").Value;
                string manner = node.Attributes.GetNamedItem("Manner").Value;
                string grapheme = node.Attributes.GetNamedItem("Grapheme").Value;
                string ipa = node.Attributes.GetNamedItem("IPA").Value;
                float frequency = float.Parse(node.Attributes.GetNamedItem("Occurence").Value, CultureInfo.InvariantCulture);

                ConsonantList.Add(new Consonant(ArticulationList[voicing], ArticulationList[place], ArticulationList[manner], grapheme, ipa, frequency));
            }
        }

        private void ReadPhonotactics(string filePath)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(filePath);
            foreach (XmlNode node in xml.SelectNodes("Phonotactics/Constraint"))
            {
                string definition = node.Attributes.GetNamedItem("Definition").Value;
                FieldInfo field = GetType().GetField(definition);
                if (field != null)
                {
                    WeightedList<Phonotactics> constraint = (WeightedList<Phonotactics>)field.GetValue(this);
                    double forbiddenWeight = double.Parse(node.Attributes.GetNamedItem("Forbidden").Value, CultureInfo.InvariantCulture);
                    double optionalWeight = double.Parse(node.Attributes.GetNamedItem("Optional").Value, CultureInfo.InvariantCulture);
                    double requiredWeight = double.Parse(node.Attributes.GetNamedItem("Required").Value, CultureInfo.InvariantCulture);
                    constraint.Add(Phonotactics.Forbidden, forbiddenWeight);
                    constraint.Add(Phonotactics.Optional, optionalWeight);
                    constraint.Add(Phonotactics.Required, requiredWeight);
                }
                else
                {
                    Console.WriteLine("Error: " + definition + " not implemented");
                }
            }
        }

        private void ReadWords(string filePath)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(filePath);
            foreach (XmlNode node in xml.SelectNodes("Words/Word"))
            {
                string meaning = node.Attributes.GetNamedItem("Meaning").Value;
                WordKeyList.Add(meaning);
            }
        }

        private void GenerateDiphthongs()
        {
            foreach (Vowel first in VowelList)
            {
                DiphthongList.Add(first, new Dictionary<Vowel, Diphthong>());
                foreach (Vowel second in VowelList)
                {
                    DiphthongList[first].Add(second, new Diphthong(first, second));
                    //Diphthongs.Add(new Tuple<Vowel, Vowel>(first, second), new Diphthong(first, second));
                }
            }
        }

        private void GenerateConsonantClusters()
        {
            foreach (Consonant first in ConsonantList)
            {
                ConsonantClusterList.Add(first, new Dictionary<Consonant, ConsonantCluster>());
                foreach (Consonant second in ConsonantList)
                {
                    ConsonantClusterList[first].Add(second, new ConsonantCluster(first, second));
                }
            }
        }

        public ProceduralLanguage Generate()
        {
            return new ProceduralLanguage(this);
        }
    }
}
