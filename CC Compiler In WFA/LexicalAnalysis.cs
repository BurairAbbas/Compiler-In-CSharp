using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace CC_Compiler_In_WFA
{
    class LexicalAnalysis
    {
        List<string> token;
        string[] words = null;
        int lineNumber = 0;

        public List<string> GetToken
        {
            get
            {
                return token;
            }
        }

        public LexicalAnalysis(string Filepath)
        {
           token = new List<string>();
           FileReader(Filepath);
        }

        public LexicalAnalysis(string[] code)
        {
            token = new List<string>();
            Separator(code);
        }

        // Used this method from Console. Change private into public and call it.
        private void Generate_Token()
        {
            foreach (var temp in this.token)
            {
                Console.WriteLine(temp);
            }
        }

        private void FileReader(string Filepath)
        {
           if (File.Exists(Filepath))
            {
                string[] lineInTextFile = File.ReadAllLines(Filepath);
                Separator(lineInTextFile);
            }
           else
           {
               Console.WriteLine("File Not Found. Please check your file path.");
           }
        }

        // Separator control the work follow!
        private void Separator(string[] code)
        {
            StringBuilder word = new StringBuilder();
            List<string> wordList = new List<string>();

            this.words = new string[code.Length];
            for (int i = 0; i < code.Length; i++)
            {
                char[] characters = code[i].ToCharArray();
                for (int j = 0; j < characters.Length; j++)
                {
                    // Put any condition for word breaking here...
                    char ch = characters[j];
                    if (ch == ';')
                    {
                        AddWordInListAndClearIt(word, wordList);
                        wordList.Add(ch.ToString()); // To Add ';' in list
                    }
                    else if (j + 1 < characters.Length && ch == '/' && characters[j +1] == '/')
                    {
                        break;
                    }
                    else if (ch == '(' || ch == ')')
                    {
                        AddWordInListAndClearIt(word, wordList);
                        wordList.Add(ch.ToString());
                    }
                    else if (ch == '[' || ch == ']') 
                    {
                        AddWordInListAndClearIt(word, wordList);
                        wordList.Add(ch.ToString());
                    }
                    else if (ch == '{' || ch == '}')
                    {
                        AddWordInListAndClearIt(word, wordList);
                        wordList.Add(ch.ToString());
                    }
                    else if (j + 1 < characters.Length && IsCompoundOperator(ch, characters[j + 1]))
                    {
                        AddWordInListAndClearIt(word, wordList);
                        string op = ch.ToString() + characters[j + 1].ToString();
                        wordList.Add(op);
                        j += 1;
                    }
                    else if (ch == '+')
                    {
                        if (j + 1 < characters.Length) // For Increment
                        {
                            if (characters[j + 1] == '+')
                            {
                                AddWordInListAndClearIt(word, wordList);
                                wordList.Add("++");
                                j += 1;
                            }
                            else
                            {
                                AddWordInListAndClearIt(word, wordList);
                                wordList.Add(ch.ToString());
                            }
                        }
                        else
                        {
                            AddWordInListAndClearIt(word, wordList);
                            wordList.Add(ch.ToString());
                        }
                    }
                    else if (ch == '-')
                    {
                        if (j + 1 < characters.Length) // For Decrement
                        {
                            if (characters[j + 1] == '-')
                            {
                                AddWordInListAndClearIt(word, wordList);
                                wordList.Add("--");
                                j += 1;
                            }
                            else
                            {
                                AddWordInListAndClearIt(word, wordList);
                                wordList.Add(ch.ToString());
                            }
                        }
                        else
                        {
                            AddWordInListAndClearIt(word, wordList);
                            wordList.Add(ch.ToString());
                        }
                    }
                    else if (ch == '=')
                    {
                        AddWordInListAndClearIt(word, wordList);
                        wordList.Add(ch.ToString());
                    }
                    else if (ch == '.') 
                    {
                        string x = characters[j - 1].ToString();
                        int temp;
                        if (!(int.TryParse(x, out temp)))
                        {
                            AddWordInListAndClearIt(word, wordList);
                            wordList.Add(ch.ToString());
                        }
                        else 
                        {
                            word.Append(ch);
                        }
                    }                    
                     else if (ch == ',')
                    {
                        AddWordInListAndClearIt(word, wordList);
                        wordList.Add(ch.ToString());
                    }
                    else if (ch == '\"')
                    {
                        string word2 = DoubleQuotation_Condition(code[i].Substring(j));
                        wordList.Add(word2);
                        // escape those character which is added in 'DQ' by adding their length 
                        // in index(j) of charArray
                        j += word2.Length - 1;                        
                    }
                    else if (ch != ' ')
                    {
                        word.Append(ch);
                    }
                    else
                    {
                        AddWordInListAndClearIt(word, wordList);
                    }
                }

                AddWordInListAndClearIt(word, wordList);

                this.words = wordList.Select(j => j.ToString()).ToArray();
                this.lineNumber = i + 1;

                Process_Word(this.words, this.lineNumber);
                wordList.Clear();
            }
        }

        private void AddWordInListAndClearIt(StringBuilder word, List<string> wordList)
        {
            if (word.Length != 0)   // if word is empty don't insert in list
                wordList.Add(word.ToString());
            word.Clear();
        }
        private string DoubleQuotation_Condition(string lineWithDQ)
        {
            StringBuilder word = new StringBuilder();
            word.Append("\"");

            for (int i = 1; i < lineWithDQ.Length; i++)
            {
                word.Append(lineWithDQ[i].ToString());
                if (lineWithDQ[i - 1] != '\\' && lineWithDQ[i] == '\"')
                {
                    break;
                }
            }
            return word.ToString();
        }
        private void Process_Word(string[] words, int lineNumber)
        {
            foreach (string temp in words)
            {
                if (IsIdentifier(temp))
                {
                    if (GetKeyword(temp) != null)
                    {
                        if (GetKeyword(temp) == "virtual" || GetKeyword(temp) == "override")
                        {
                            token.Add(string.Format("(VO, {0}, {1})", temp, lineNumber));

                        }
                        else
                            token.Add(string.Format("({0}, {1}, {2})", GetKeyword(temp), temp, lineNumber));
                    }
                    else if (IsAccessModifier(temp))
                    {
                        token.Add(string.Format("(Access Modifier, {0}, {1})", temp, lineNumber));

                    }
                    else if (IsDataType(temp))
                    {
                        token.Add(string.Format("(DT, {0}, {1})", temp, lineNumber));
                    }
                    // Bool condition here. So, boolean not take as ID
                    else if (new Regex("^(true|false)$", RegexOptions.IgnoreCase).IsMatch(temp))
                    {
                        token.Add(string.Format("(bool, {0}, {1})", temp, lineNumber));
                    }
                    else
                    {
                        token.Add(string.Format("(ID, {0}, {1})", temp, lineNumber));
                    }
                }
                else if (IsDigit(temp))
                {
                    // Integer
                    if (new Regex(@"^-?[0-9]{1,16}$").IsMatch(temp))
                        token.Add(string.Format("(int, {0}, {1})", temp, lineNumber));
                    else if (new Regex(@"^-?[0-9]+(.[0-9]+)?(e)?(-)?([0-9]+)?$").IsMatch(temp)) // double
                        token.Add(string.Format("(double, {0}, {1})", temp, lineNumber));

                }
                else if (GetOperator(temp) != null)
                {
                    token.Add(string.Format("({0}, {1}, {2})", GetOperator(temp), temp, lineNumber));
                }
                else if (GetValueDT(temp) != null)
                {
                    token.Add(string.Format("({0}, {1}, {2})", GetValueDT(temp), temp, lineNumber));
                }
                else if (temp.Length == 1 && GetPunctuator(temp) != null)
                {
                    token.Add(string.Format("({0}, {1}, {2})", GetPunctuator(temp), temp, lineNumber));
                }
                else
                {
                    token.Add(string.Format("(InValid, {0}, {1})", temp, lineNumber));
                }
            }
        }
        private bool IsIdentifier(string temp)
        {
            return new Regex(@"^[_a-zA-Z][_a-zA-Z0-9]*").IsMatch(temp);
        }
        private string GetKeyword(string temp)
        {
            // Use dictionary if any keyword lie in same class.
            List<string> kw = new List<string>();

            //Condition
            kw.Add("if");
            kw.Add("elif");
            kw.Add("else");

            //Loop
            kw.Add("for");
            kw.Add("while");
            kw.Add("foreach");

            //Function
            kw.Add("ret");
            kw.Add("void");

            //General
            kw.Add("break");
            kw.Add("continue");
            kw.Add("print");

             //Data Structure
            kw.Add("ArrayList");

            //OOP
            kw.Add("new");
            kw.Add("class");
            kw.Add("abstract");
            kw.Add("static");
            kw.Add("this");     
            kw.Add("virtual");  
            kw.Add("override"); 

            return kw.Contains(temp) ? kw[kw.IndexOf(temp)] : null;
        }
        private bool IsDataType(string temp)
        {
            List<string> dt = new List<string>();
            dt.Add("int");
            dt.Add("char");
            dt.Add("str");
            dt.Add("bool");
            dt.Add("double");

            return dt.Contains(temp);
        }
        private bool IsAccessModifier(string temp) 
        {
            List<string> ac = new List<string>();
            ac.Add("public");
            ac.Add("private");
            ac.Add("protected");

            return ac.Contains(temp);
        }
        private bool IsDigit(string temp)
        {
            return new Regex(@"^-?[0-9]+(.[0-9]+)?(e)?(-)?([0-9]+)?$").IsMatch(temp);
        }
        private string GetOperator(string temp)
        {
            Dictionary<string, string> op = new Dictionary<string, string>();
            // Arithematics Operator
            op.Add("+", "AM");
            op.Add("-", "AM");
            op.Add("*", "AM");
            op.Add("/", "AM");
            op.Add("%", "AM");

            // Rational Operators
            op.Add("<", "RO");
            op.Add("<=", "RO");
            op.Add(">", "RO");
            op.Add(">=", "RO");
            op.Add("==", "RO");
            op.Add("!=", "RO");

            //Logical Operator
            op.Add("&&", "LO");
            op.Add("||", "LO");


            //Assignment Operator
            op.Add("=", "AO");
            op.Add("+=", "AO");
            op.Add("-=", "AO");
            op.Add("/=", "AO");
            op.Add("%=", "AO");

            // Increment and Decrement
            op.Add("++", "ic");
            op.Add("--", "dc");

            return op.ContainsKey(temp) ? op[temp] : null;
        }
        public string GetValueDT(string temp)
        {
            if (new Regex("^\"[\\s\\w.]*\"$").IsMatch(temp))
            {
                return "str";
            }
            else if (new Regex("^'.{1,2}'$").IsMatch(temp))
            {
                return "char";
            }
            else
                return null;
        }
        private string GetPunctuator(string temp)
        {
            Dictionary<char, string> pc = new Dictionary<char, string>();
            pc.Add('?', "PC");
            pc.Add(';', "PC");
            pc.Add(',', "PC");
            pc.Add(':', "PC");
            pc.Add('(', "PC");
            pc.Add(')', "PC");
            pc.Add('.', "PC");
            pc.Add('[', "PC");
            pc.Add(']', "PC");
            pc.Add('{', "PC");
            pc.Add('}', "PC");

            return pc.ContainsKey(Convert.ToChar(temp)) ? pc[Convert.ToChar(temp)] : null;
        }
    }
}
