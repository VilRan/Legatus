using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Linguistics;
using Legatus.Input;
using Legatus;

namespace TestProject.Testing
{
    internal class LanguageTest : Test
    {
        private InputEventHandler Input;
        private Random RNG = new Random();
        private SpriteFont Font;
        private List<ProceduralLanguage> Languages = new List<ProceduralLanguage>();
        private StringBuilder Words = new StringBuilder();
        private StringBuilder Loanwords = new StringBuilder();
        private ProceduralLanguageFactory Factory;
        private bool VocabularyMode;
        private bool LoanwordMode;

        public LanguageTest(LegatusGame game)
        {
            Input = new InputEventHandler();
            Input.KeyboardAction += OnKeyboardAction;
            //Input.KeyReleased += new KeyReleasedDelegate(OnKeyReleased);
            Font = game.Content.Load<SpriteFont>("Graphics/DefaultFont12IPA");
            Factory = new ProceduralLanguageFactory(RNG);
            CreateLanguages();
            CreateWords();
            CreateLoanwords();
        }

        public override void Update(GameTime gameTime)
        {
            Input.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (VocabularyMode)
            {
                DrawVocabulary(spriteBatch);
            }
            else if (LoanwordMode)
            {
                spriteBatch.DrawString(Font, Loanwords, Vector2.Zero, Color.White);
            }
            else
            {
                spriteBatch.DrawString(Font, Words, Vector2.Zero, Color.White);
            }
            spriteBatch.End();
        }

        private void DrawVocabulary(SpriteBatch spriteBatch)
        {
            Vector2 position = Vector2.Zero;

            ProceduralLanguage firstLanguage = (ProceduralLanguage)Languages[0];
            foreach (string meaning in firstLanguage.Vocabulary.Keys)
            {
                spriteBatch.DrawString(Font, meaning, position, Color.White);
                position += new Vector2(0, 16);
            }
            position = new Vector2(position.X + 200, 0);

            foreach (ProceduralLanguage language in Languages)
            {
                foreach (Word word in language.Vocabulary.Values)
                {
                    spriteBatch.DrawString(Font, word.ToString(), position, Color.White);
                    position += new Vector2(0, 16);
                }

                position = new Vector2(position.X + 200, 0);
            }
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        private void CreateLanguages()
        {
            Languages.Clear();

            for (int i = 0; i < 8; i++)
            {
                Languages.Add(Factory.Generate());
            }
        }

        private void CreateWords()
        {
            Words.Clear();

            foreach (ProceduralLanguage language in Languages)
            {
                Words.Append("Language Name: " + language.Name + ", ");
                Words.Append("Vowels: ");
                for (int i = 0; i < language.Nuclei.Count; i++)
                {
                    if ( ! (language.Nuclei[i] is Diphthong))
                    {
                        Words.Append(language.Nuclei[i].Grapheme);
                        Words.Append(", ");
                    }
                }
                Words.Append("Consonants: ");
                for (int i = 0; i < language.Onsets.Count; i++)
                {
                    if ( ! (language.Onsets[i] is ConsonantCluster))
                    {
                        Words.Append(language.Onsets[i].Grapheme);
                        Words.Append(", ");
                    }
                }
                Words.Append("Word lengths: ");
                for (int i = 0; i < language.WordLengths.Count; i++)
                {
                    Words.Append(i + 1);
                    Words.Append(" - ");
                    Words.Append(Math.Round(language.WordLengths.GetWeightAt(i) / language.WordLengths.TotalWeight * 100, 2));
                    Words.Append("%, ");
                }
                Words.Append("Complexity: ");
                Words.Append(Math.Round(language.SyllableComplexity, 1));
                Words.Append("\nPhonotactics: Nucleus: ");
                Words.Append(language.Nucleus);
                Words.Append(", Initial onset: ");
                Words.Append(language.WordInitialOnset);
                Words.Append(", Medial onset: ");
                Words.Append(language.WordMedialOnset);
                Words.Append(", Medial coda: ");
                Words.Append(language.WordMedialCoda);
                Words.Append(", Final coda: ");
                Words.Append(language.WordFinalCoda);
                /*
                for (int i = 0; i < 50; i++)
                {
                    if (i % 25 == 0)
                        Words.Append("\n");
                    Words.Append(language.GenerateWord(RNG) + ", ");
                }*/
                for (int i = 0; i < 50; i++)
                {
                    if (i % 13 == 0)
                        Words.Append("\n");
                    Words.Append(language.GenerateLongName(RNG) + ", ");
                }

                Words.Append("\n\n");
            }
        }

        private void CreateLoanwords()
        {
            Loanwords.Clear();

            int words = 50;
            Word[] originals = new Word[words];
            for (int i = 0; i < words; i++)
            {
                originals[i] = Languages[0].ConstructWord(RNG);
            }

            //Word last;
            foreach (Word original in originals)
            {
                Loanwords.Append(original);// + " " + original.IPA);
                //last = original;
                for (int i = 1; i < Languages.Count; i++)
                {
                    Word loan = Languages[i].ConstructLoanword(original);
                    Loanwords.Append(", " + loan);// + " " + loan.IPA);
                    //last = loan;
                }
                Loanwords.Append("\n\n");
            }
        }

        private void OnKeyboardAction(object sender, KeyboardEventArgs keyboard)
        {
            if (keyboard.Action == KeyboardAction.Released)
                OnKeyReleased(keyboard.Key, keyboard.HeldKeys);
        }

        private void OnKeyReleased(Keys key, Keys[] allHeldKeys)
        {
            switch (key)
            {
                case Keys.L:
                    CreateLanguages();
                    CreateWords();
                    CreateLoanwords();
                    break;
                case Keys.O:
                    LoanwordMode = !LoanwordMode;
                    break;
                case Keys.W:
                    CreateWords();
                    CreateLoanwords();
                    break;
                case Keys.V:
                    VocabularyMode = !VocabularyMode;
                    break;
            }
        }
    }
}
