using System;

namespace Linguistics
{
    public abstract class Phoneme
    {
        /// <summary>
        /// The percentage of generated languages that use this phoneme.
        /// </summary>
        public readonly float Occurence;
        protected readonly string _Definition;
        protected readonly string _Grapheme;
        protected readonly string _IPA;

        public virtual string Definition { get { return _Definition; } }
        public virtual string Grapheme { get { return _Grapheme; } }
        public virtual string IPA { get { return _IPA; } }

        public Phoneme(string definition, string symbol, string ipa, float occurence)
        {
            Occurence = occurence;
            _Definition = definition;
            _Grapheme = symbol;
            _IPA = ipa;
        }

        public abstract float CalculateDistance(Phoneme other);
    }

    public class Vowel : Phoneme
    {
        public readonly Articulation Height;
        public readonly Articulation Backness;
        public readonly Articulation Roundedness;

        public virtual float HeightValue { get { return Height.Position; } }
        public virtual float BacknessValue { get { return Backness.Position; } }
        public virtual float Sonority { get { return Height.Sonority; } }

        public Vowel(Articulation height, Articulation backness, Articulation roundedness, string symbol, string ipa, float occurence)
            : base (height.Definition + backness.Definition + roundedness.Definition, symbol, ipa, occurence)
        {
            Height = height;
            Backness = backness;
            Roundedness = roundedness;
        }

        public Vowel(Vowel vowel)
            : this(vowel.Height, vowel.Backness, vowel.Roundedness, vowel.Grapheme, vowel.IPA, vowel.Occurence)
        {

        }

        public override float CalculateDistance(Phoneme other)
        {
            Vowel vowel = other as Vowel;
            if (vowel != null)
                return Math.Abs(HeightValue - vowel.HeightValue) + Math.Abs(BacknessValue - vowel.BacknessValue);

            return float.MaxValue;
        }
    }

    public class Diphthong : Vowel
    {
        public readonly bool IsLongVowel;
        private readonly Vowel Second;

        public override string Definition { get { return _Definition + " to " + Second.Definition; } }
        public override string Grapheme { get { return _Grapheme + Second.Grapheme; } }
        public override string IPA { get { return _IPA + Second.IPA; } }
        public override float BacknessValue { get { return (base.BacknessValue + Second.BacknessValue) / 2; } }
        public override float HeightValue { get { return (base.HeightValue + Second.HeightValue) / 2; } }
        public override float Sonority { get { return (base.Sonority + Second.Sonority) / 2; } }
        public float _Sonority { get { return base.Sonority; } }

        public Diphthong(Vowel first, Vowel second)
            : base (first)
        {
            IsLongVowel = (first == second);
            Second = second;
        }
    }

    public class Consonant : Phoneme
    {
        public readonly Articulation Voicing;
        public readonly Articulation Place;
        public readonly Articulation Manner;

        public virtual float PlaceValue { get { return Place.Position; } }
        public virtual float MannerValue { get { return Manner.Position; } }
        public virtual float Sonority { get { return Manner.Sonority + Voicing.Sonority; } }
        //public bool IsVoiced { get { return Voicing.Definition == "Voiced"; } }

        public Consonant(Articulation voicing, Articulation place, Articulation manner, string symbol, string ipa, float occurence)
            : base (voicing.Definition + place.Definition + manner.Definition, symbol, ipa, occurence)
        {
            Voicing = voicing;
            Place = place;
            Manner = manner;
        }

        public Consonant(Consonant consonant)
            : this(consonant.Voicing, consonant.Place, consonant.Manner, consonant.Grapheme, consonant.IPA, consonant.Occurence)
        {

        }

        public override float CalculateDistance(Phoneme other)
        {
            Consonant consonant = other as Consonant;
            if (consonant != null)
                return Math.Abs(PlaceValue - consonant.PlaceValue) + Math.Abs(MannerValue - consonant.MannerValue);

            return float.MaxValue;
        }
    }

    public class ConsonantCluster : Consonant
    {
        public readonly Consonant Second;
        public readonly bool IsGeminate;

        public override string Definition { get { return base.Definition + " to " + Second.Definition; } }
        public override string Grapheme { get { return base.Grapheme + Second.Grapheme; } }
        public override string IPA { get { return base.IPA + Second.IPA; } }
        public override float PlaceValue { get { return (base.PlaceValue); } }// + Second.PlaceValue) / 2; } }
        public override float MannerValue { get { return (base.MannerValue); } }// + Second.MannerValue) / 2; } }
        public override float Sonority { get { return (base.Sonority + Second.Sonority) / 2; } }
        public float FirstSonority { get { return base.Sonority; } }

        public ConsonantCluster(Consonant first, Consonant second)
            : base (first)
        {
            IsGeminate = (first == second);
            Second = second;
        }
    }

    public class Articulation
    {
        public readonly string Definition;
        public readonly float Position;
        public readonly float Sonority;

        public Articulation(string definition, float position, float sonority)
        {
            Definition = definition;
            Position = position;
            Sonority = sonority;
        }
    }
}
