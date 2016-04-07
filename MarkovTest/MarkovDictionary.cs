using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkovTest
{
    public class MarkovDictionary
    {
        public List<MarkovWord> Words = new List<MarkovWord>();

        public static char[] Punctuation = { '.', ',', ':', '!', '?', ';' };
        public static char[] NonTerminating = { ',', ':', ';' };
        public static char[] Delimiters = { ' ', '\t', '\n', '"', '\r' };

        public MarkovDictionary()
        {
            foreach (char c in Punctuation)
            {
                AddWord(Convert.ToString(c));
            }
        }

        public void BuildDictionary(string _input)
        {
            List<string> tokens = Tokenize(_input);
            foreach (string s in tokens)
            {
                Console.Write(s + "\t");
            }
            Console.WriteLine("\n====================");
            IndexWords(tokens);
            foreach (MarkovWord m in Words)
            {
                Console.Write(m.ToString() + "\t");
            }
            Console.WriteLine("\n====================");
            BuildChain(tokens);
            Console.WriteLine("\n====================");
        }

        public string GenerateSentence(int _seed)
        {
            string result = "";

            Random RNG = new Random(_seed);

            result = BuildSentence("", RNG);

            result = CleanSentence(result);

            return result;
        }

        private string BuildSentence(string _previous, Random _RNG)
        {
            string nextWord = "";

            if (_previous == "" || GetWord(_previous) == -1)//If this is the start
            {
                nextWord = Words[Punctuation.Length + _RNG.Next(Words.Count - Punctuation.Length)].Text;
                Console.Write(">");
            }
            else if (_previous.Length <= 1 && Punctuation.Contains(_previous[0]) && !NonTerminating.Contains(_previous[0]))//If the previous iteration ended the sentence
            {
                Console.Write("<\n");
                return "";
            }
            else
            {
                int selectedIndex = -1;
                float randomNumber = _RNG.Next(0, 1000) / 1000.0f;
                foreach (KeyValuePair<int, float> p in Words[GetWord(_previous)].NextWords)
                {
                    if (randomNumber < p.Value)
                    {
                        selectedIndex = p.Key;
                        break;
                    }

                    randomNumber = randomNumber - p.Value;
                }
                nextWord = Words[selectedIndex].Text;
                Console.Write("|");
            }

            //Console.Write(nextWord + "\t");
            return nextWord + " " + BuildSentence(nextWord, _RNG);
        }

        private string CleanSentence(string _sentence)
        {
            string result = "";
            int parenCount = 0;
            //Capitalize first letter
            result += _sentence[0].ToString().ToUpper();

            for (int i = 1; i < _sentence.Length - 1; i++)
            {
                if (!(_sentence[i] == ' ' && Punctuation.Contains(_sentence[i + 1])))//If it is not a space before a punctuation
                {
                    if (_sentence[i] == '(') { parenCount++; }//If it is an open paren, increment counter
                    if (_sentence[i] == ')' && parenCount > 0) { parenCount--; }
                    if (!(_sentence[i] == ')' && parenCount <= 0))
                    {
                        result += _sentence[i];
                    }
                }
            }

            result += _sentence[_sentence.Length - 1];

            return result;
        }

        public List<string> Tokenize(string _input)
        {
            List<string> tokens = _input.Split(Delimiters).ToList();

            for (int i = 1; i < tokens.Count; i++)
            {
                if (tokens[i].Length > 0)
                {
                    char last = tokens[i][tokens[i].Length - 1];
                    if (tokens[i].Length > 1 && Punctuation.Contains(last))
                    {
                        tokens[i] = tokens[i].Substring(0, tokens[i].Length - 1);
                        tokens.Insert(i + 1, last.ToString());
                    }
                }
                else
                {
                    tokens.RemoveAt(i);
                    i--;
                }
            }

            return tokens;
        }

        public void IndexWords(List<string> _input)
        {
            foreach (string s in _input)
            {
                bool isNew = true;
                foreach (MarkovWord w in Words)
                {
                    if (s == w.Text)
                    {
                        isNew = false;
                    }
                }

                if (isNew)
                {
                    AddWord(s);
                }
            }
        }

        public void BuildChain(List<string> _tokens)
        {
            foreach (MarkovWord m in Words)
            {
                Console.Write(m.Text + ": ");
                int uses = 0;
                for (int i = 0; i < (_tokens.Count - 1); i++)
                {
                    if (_tokens[i] == m.Text)
                    {
                        uses++;
                        string nextToken = _tokens[i + 1];
                        bool seenBefore = false;
                        for (int j = 0; j < m.NextWords.Count; j++)
                        {
                            if (m.NextWords[j].Key == GetWord(nextToken))
                            {
                                seenBefore = true;
                                m.NextWords[j] = new KeyValuePair<int, float>(m.NextWords[j].Key, m.NextWords[j].Value + 1.0f);
                            }
                        }
                        if (seenBefore == false)
                        {
                            m.NextWords.Add(new KeyValuePair<int, float>(GetWord(nextToken), 1.0f));
                        }
                    }
                }
                Console.Write(uses + "\t");

                //Normalize weights
                float sum = 0.0f;
                for (int i = 0; i < m.NextWords.Count; i++)
                {
                    sum += m.NextWords[i].Value;
                }
                for (int i = 0; i < m.NextWords.Count; i++)
                {
                    m.NextWords[i] = new KeyValuePair<int, float>(m.NextWords[i].Key, m.NextWords[i].Value / sum);
                }
            }
        }

        public int GetWord(string _word)
        {
            int result = -1;

            for (int i = 0; i < Words.Count; i++)
            {
                if (Words[i].Text == _word)
                {
                    return i;
                }
            }

            return result;
        }

        public void AddWord(string _word)
        {
            string word = _word;
            if (_word.Length > 1)
            {
                char last = word[word.Length - 1];
                if (Punctuation.Contains(last))
                {
                    word = word.Substring(0, word.Length - 1);
                }
            }
            MarkovWord temp = new MarkovWord(_word);
            Words.Add(temp);
        }
    }

    public class MarkovWord
    {
        public string Text;
        public List<KeyValuePair<int, float>> NextWords = new List<KeyValuePair<int, float>>();

        public MarkovWord(string _text)
        {
            Text = _text;
        }

        override public string ToString()
        {
            return Text;
        }
    }
}