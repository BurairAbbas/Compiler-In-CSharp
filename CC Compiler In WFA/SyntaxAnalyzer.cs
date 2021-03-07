using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Compiler_In_WFA
{
    class SyntaxAnalyzer
    {
        List<string> token;
        BreakToken btk;
        int index;
        List<int> errorLineNumber;

        public SyntaxAnalyzer(string[] code)
        {
            LexicalAnalysis la = new LexicalAnalysis(code);
            this.token = la.GetToken;
            btk = new BreakToken(this.token);
            index = 0;
            errorLineNumber = new List<int>();
            ProcessNonTerminals();
        }

        private void ProcessNonTerminals()
        {
            bool lineClear;
            while (index < token.Count)
            {
                lineClear = false;
                if (btk.CP[index] == "$")
                {
                    lineClear = true;
                    break;
                }
                // put NT here...
                else if (DEC() || INIT3() || Print_St())
                {
                    lineClear = true;
                    continue;
                }                
                else if (SST())
                {
                    lineClear = true;
                    continue;
                }
                else if (OE3())
                {
                    lineClear = true;

                    continue;
                }
                else if (Funct_St())
                {
                    lineClear = true;

                    continue;
                }
                else if (Abstract_St())
                {
                    lineClear = true;

                    continue;
                }
                else if (Arr_St())
                {
                    lineClear = true;

                    continue;
                }
                else if (ArrList_St())
                {
                    lineClear = true;

                    continue;
                }
                else if (Class_St())
                {
                    lineClear = true;

                    continue;
                }
                else if (Poly_St())
                {
                    lineClear = true;

                    continue;
                }
                else if (Interface_St())
                {
                    lineClear = true;

                    continue;
                }
                else
                {
                    if (!lineClear)
                    {
                        if (index >= token.Count)
                        {
                            if (!errorLineNumber.Contains(Convert.ToInt32(btk.LN[index - 1])))
                                errorLineNumber.Add(Convert.ToInt32(btk.LN[index - 1]));
                        }
                        else 
                        {
                            if (!errorLineNumber.Contains(Convert.ToInt32(btk.LN[index])))
                                errorLineNumber.Add(Convert.ToInt32(btk.LN[index]));
                        }
                    }
                    else
                    {
                        // if line number is not in list than add it.
                        if (!errorLineNumber.Contains(Convert.ToInt32(btk.LN[index - 1])))
                            errorLineNumber.Add(Convert.ToInt32(btk.LN[index - 1]));
                    }
                    index++;
                }
            }
        }

        public string CheckSyntax()
        {
            string allErrors = null;
            if (errorLineNumber.Count == 0)
            {
                return "No Syntax Error";
            }
            else
            {
                foreach (int item in errorLineNumber)
                {
                    allErrors += String.Format("Syntax Error At Line: {0}{1}", item, Environment.NewLine);
                }
            }
            return allErrors;
        }

        protected bool DEC()
        {
            int tempIndex = index;
            if (btk.CP[index] == "DT")
            {
                index++;
                if (btk.CP[index] == "ID")
                {
                    index++;
                    if (INIT())
                    {
                        if (LIST())
                        {
                            return true;
                        }
                    }
                }
            }
            index = tempIndex;
            return false;
        }

        protected bool INIT()
        {
            int tempIndex = index;
            if (btk.CP[index] == "=")
            {
                index++;
                if (INIT2())
                {
                    return true;
                }
            }
            else if (btk.CP[index] == "," || btk.CP[index] == ";")
            {
                return true;
            }
            index = tempIndex;
            return false;
        }

        protected bool INIT2()
        {
            int tempIndex = index;
            if (btk.CP[index] == "int-const" || btk.CP[index] == "str-const" ||
                btk.CP[index] == "bool-const" || btk.CP[index] == "char-const" ||
                btk.CP[index] == "double-const")
            {
                index++;
                return true;
            }
            else if (btk.CP[index] == "ID")
            {
                index++;
                if (INIT())
                {
                    return true;
                }
            }
            index = tempIndex;
            return false;
        }

        protected bool INIT3()
        {

            int tempIndex = index;
            if (btk.CP[index] == "ID")
            {
                index++;
                if (INIT())
                {
                    if (LIST())
                    {
                        return true;
                    }
                }
            }
            index = tempIndex;
            return false;
        }
        protected bool LIST()
        {
            int tempIndex = index;
            if (btk.CP[index] == ";")
            {
                index++;
                return true;
            }
            else if (btk.CP[index] == ",")
            {
                index++;
                if (btk.CP[index] == "ID")
                {
                    index++;
                    if (INIT())
                    {
                        if (LIST())
                        {
                            return true;
                        }
                    }
                }
            }
            index = tempIndex;
            return false;
        }


        protected bool For_St()
        {
            if (btk.CP[index] == "for")
            {
                index++;
                if (btk.CP[index] == ":")
                {
                    index++;
                    if (C1())
                    {
                        if (C2())
                        {
                            if (btk.CP[index] == ";")
                            {
                                index++;
                                if (C3())
                                {
                                    if (btk.CP[index] == ":")
                                    {
                                        index++;
                                        if (Body())
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        protected bool C1()
        {
            if (DEC())
            {
                return true;
            }
            else if (Asgn_St())
            {
                return true;
            }
            else if (btk.CP[index] == ";")
            {
                index++;
                return true;
            }
            return false;
        }

        protected bool C2()
        {
            if (OE())
            {
                return true;
            }
            else if (btk.CP[index] == ";")
            {
                return true;
            }
            return false;
        }

        protected bool C3()
        {
            int tempIndex = index;
            if (btk.CP[index] == "ID")
            {
                index++;
                if (X())
                {
                    if (C4())
                    {
                        return true;
                    }
                }
            }
            else if (btk.CP[index] == "inc_dec")
            {
                index++;
                if (btk.CP[index] == "ID")
                {
                    index++;
                    if (X())
                    {
                        return true;
                    }
                }
            }
            else if (btk.CP[index] == ":")
            {
                return true;
            }
            index = tempIndex;
            return false;
        }

        protected bool C4()
        {
            int tempIndex = index;
            if (Asgn_OP())
            {
                if (E())
                {
                    return true;
                }
            }
            else if (btk.CP[index] == "inc_dec")
            {
                index++;
                return true;
            }
            index = tempIndex;
            return false;
        }

        protected bool Body()
        {
            if (SST())
            {
                return true;
            }
            else if (btk.CP[index] == "(")
            {
                index++;
                if (MST())
                {
                    if (btk.CP[index] == ")")
                    {
                        index++;
                        return true;
                    }
                }
            }
            else if (btk.CP[index] == ";")
            {
                index++;
                return true;
            }
            return false;
        }

        protected bool SST()
        {
            int tempIndex = index;
            if (For_St() || While_St() || Foreach_St() || If_Else() || ArrList_St())
            {
                return true;
            }
            else if (btk.CP[index] == "DT")
            {
                index++;
                if (SST03())
                {
                    if (btk.CP[index] == ";")
                    {
                        index++;
                        return true;
                    }
                }

            }
            else if (btk.CP[index] == "inc_dec")
            {
                index++;
                if (btk.CP[index] == "ID")
                {
                    index++;
                    if (X())
                    {
                        if (btk.CP[index] == ";")
                        {
                            index++;
                            return true;
                        }
                    }
                }

            }
            else if (btk.CP[index] == "ID")
            {
                tempIndex = index;
                index++;
                if (SST01())
                {
                    return true;
                }
                index = tempIndex;
                
            }

            return false;
        }

        protected bool SST01()
        {
            int tempIndex = index;
            if (btk.CP[index] == "ID")
            {
                index++;
                if (btk.CP[index] == "=")
                {
                    index++;
                    if (btk.CP[index] == "new")
                    {
                        index++;
                        if (btk.CP[index] == "ID")
                        {
                            index++;
                            if (btk.CP[index] == ":")
                            {
                                index++;
                                if (Argu_St())
                                {
                                    if (btk.CP[index] == ":")
                                    {
                                        index++;
                                        if (btk.CP[index] == ";")
                                        {
                                            index++;
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (Y())
            {
                if (btk.CP[index] == ":")
                {
                    index++;
                    if (Argu_St())
                    {
                        if (btk.CP[index] == ":")
                        {
                            index++;
                            if (btk.CP[index] == ";")
                            {
                                index++;
                                return true;
                            }
                        }
                    }
                }
            }
            else if (X())
            {
                if (SST02())
                {
                    if (btk.CP[index] == ":")
                    {
                        index++;
                        return true;
                    }
                }
            }
            index = tempIndex;
            return false;
        }

        protected bool SST02()
        {
            int tempIndex = index;
            if (Asgn_OP())
            {
                if (E())
                {
                    return true;
                }
            }
            else if (btk.CP[index] == "inc_dec")
            {
                index++;
                return true;
            }
            index = tempIndex;
            return false;
        }


        protected bool SST03()
        {
            int tempIndex = index;
            if (btk.CP[index] == "ID")
            {
                index++;
                if (INIT())
                {
                    if (LIST())
                    {
                        return true;
                    }
                }
            }
            else if (btk.CP[index] == "[")
            {
                index++;
                if (btk.CP[index] == "]")
                {
                    index++;
                    if (btk.CP[index] == "ID")
                    {
                        index++;
                        if (List3())
                        {
                            return true;
                        }
                    }
                }
            }
            index = tempIndex;
            return false;
        }
        protected bool MST()
        {
            if (SST())
            {
                if (MST())
                {
                    return true;
                }
            }
            else if (btk.CP[index] == ")")
            {
                return true;
            }
            return false;
        }

        protected bool Obj_Dec()
        {
            int tempIndex = index;
            if (btk.CP[index] == "ID")
            {
                index++;
                if (btk.CP[index] == "ID")
                {
                    index++;
                    if (btk.CP[index] == "=")
                    {
                        index++;
                        if (btk.CP[index] == "ID")
                        {
                            index++;
                            if (btk.CP[index] == ":")
                            {
                                index++;
                                if (Argu_St())
                                {
                                    if (btk.CP[index] == ":")
                                    {
                                        index++;
                                        if (btk.CP[index] == ";")
                                        {
                                            index++;
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            index = tempIndex;
            return false;
        }
        protected bool Argu_St()
        {
            if (OE())
            {
                if (Argu_St2())
                {
                    return true;
                }
            }
            else if (btk.CP[index] == ":")
            {
                return true;
            }
            return false;
        }
        protected bool Argu_St2()
        {
            int tempIndex = index;
            if (btk.CP[index] == ",")
            {
                index++;
                if (OE())
                {
                    if (Argu_St2())
                    {
                        return true;
                    }
                }
            }
            else if (btk.CP[index] == ":")
            {
                return true;
            }
            index = tempIndex;
            return false;
        }

        protected bool ID_Const()
        {
            int tempIndex = index;
            if (btk.CP[index] == "ID" || Const())
            {
                index++;
                return true;
            }
            index = tempIndex;
            return false;
        }

        protected bool Print_St()
        {
            int tempIndex = index;

            if (btk.CP[index] == "print")
            {
                index++;
                if (btk.CP[index] == "(")
                {
                    index++;
                    if (ID_Const())
                    {
                        if (btk.CP[index] == ")")
                        {
                            index++;
                            if (btk.CP[index] == ";")
                            {
                                index++;
                                return true;
                            }
                        }
                    }
                }
            }
            index = tempIndex;
            return false;
        }

        protected bool Const()
        {
            if (btk.CP[index] == "int-const" || btk.CP[index] == "str-const" ||
                btk.CP[index] == "bool-const" || btk.CP[index] == "char-const" ||
                btk.CP[index] == "double-const")
            {
                return true;
            }
            return false;
        }

        protected bool Funct_Call()
        {
            int tempIndex = index;
            if (btk.CP[index] == "ID")
            {
                index++;
                if (Y())
                {
                    if (btk.CP[index] == ":")
                    {
                        index++;
                        if (Argu_St())
                        {
                            if (btk.CP[index] == ":")
                            {
                                index++;
                                if (btk.CP[index] == ";")
                                {
                                    index++;
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            index = tempIndex;
            return false;
        }

        protected bool Y()
        {
            int tempIndex = index;
            if (btk.CP[index] == ".")
            {
                index++;
                if (btk.CP[index] == "ID")
                {
                    index++;
                    if (Y())
                    {
                        return true;
                    }
                }
            }
            else if (btk.CP[index] == ":")
            {
                return true;
            }
            index = tempIndex;
            return false;
        }

        protected bool Asgn_St()
        {
            int tempIndex = index;
            if (btk.CP[index] == "ID")
            {
                index++;
                if (X())
                {
                    if (Asgn_OP())
                    {
                        if (E())
                        {
                            if (btk.CP[index] == ";")
                            {
                                index++;
                                return true;
                            }
                        }
                    }
                }
            }
            index = tempIndex;
            return false;
        }
        protected bool Asgn_OP()
        {
            int tempIndex = index;
            if (btk.CP[index] == "=")
            {
                index++;
                return true;
            }
            else if (CoP())
            {
                return true;
            }
            index = tempIndex;
            return false;
        }
        protected bool CoP()
        {
            int tempIndex = index;
            if (btk.CP[index] == "RO" || btk.CP[index] == "AO")
            {
                index++;
                return true;
            }
            index = tempIndex;
            return false;
        }

        protected bool X()
        {
            if (Handle_Array())
            {
                if (Handle_Dot())
                {
                    return true;
                }
            }
            else if (btk.CP[index] == "=" || btk.CP[index] == "RO" || btk.CP[index] == "AO" || btk.CP[index] == "inc_dec")
            {
                return true;
            }
            return false;
        }
        protected bool Handle_Array()
        {
            int tempIndex = index;
            if (btk.CP[index] == "[")
            {
                index++;
                if (btk.CP[index] == "int-const")
                {
                    index++;
                    if (btk.CP[index] == "]")
                    {
                        index++;
                        return true;
                    }
                }
            }
            else if (btk.CP[index] == "=" || btk.CP[index] == "RO" || btk.CP[index] == "AO")
            {
                return true;
            }
            index = tempIndex;
            return false;
        }
        protected bool Handle_Dot()
        {
            int tempIndex = index;
            if (btk.CP[index] == ".")
            {
                index++;
                if (btk.CP[index] == "ID")
                {
                    index++;
                    if (X2())
                    {
                        return true;
                    }
                }
            }
            else if (btk.CP[index] == "=" || btk.CP[index] == "RO" || btk.CP[index] == "AO")
            {
                return true;
            }
            index = tempIndex;
            return false;
        }

        protected bool X2()
        {
            int tempIndex = index;
            if (X())
            {
                return true;
            }
            else if (Argu_St())
            {
                if (btk.CP[index] == ":")
                {
                    index++;
                    if (Handle_Dot())
                    {
                        return true;
                    }
                }
            }
            index = tempIndex;
            return false;
        }


        protected bool OE()
        {
            int tempIndex = index;

            if (AE())
            {
                if (OE2())
                {
                    return true;
                }
            }
            else if (btk.CP[index] == ";")
            {
                return true;
            }
            index = tempIndex;
            return false;
        }

        protected bool OE3()
        {
            int tempIndex = index;

            if (AE())
            {
                if (OE2())
                {
                    return true;
                }
            }
            else if (btk.CP[index] == ";")
            {
                index++;
                return true;
            }
            index = tempIndex;
            return false;
        }

        protected bool OE2()
        {
            if (btk.CP[index] == "||")
            {
                index++;
                if (AE())
                {
                    if (OE2())
                    {
                        return true;
                    }
                }

            }
            else if (btk.CP[index] == "$" || btk.CP[index] == ":" || btk.CP[index] == ")" || btk.CP[index] == "]")
            {
                return true;
            }
            return false;
        }
        protected bool AE()
        {
            if (RE())
            {
                if (AE2())
                {
                    return true;
                }
            }
            return false;
        }
        protected bool AE2()
        {
            if (btk.CP[index] == "&&")
            {
                index++;
                if (RE())
                {
                    if (AE2())
                    {
                        return true;
                    }
                }
            }
            else if (btk.CP[index] == "||" || btk.CP[index] == "$" || btk.CP[index] == ":" || btk.CP[index] == ")" || btk.CP[index] == "]")
            {
                return true;
            }
            return false;
        }
        protected bool RE()
        {
            if (E())
            {
                if (RE2())
                {
                    return true;
                }
            }
            return false;
        }
        protected bool RE2()
        {
            if (btk.CP[index] == "RO")
            {
                index++;
                if (E())
                {
                    if (RE())
                    {
                        return true;
                    }
                }
            }
            else if (btk.CP[index] == "&&" || btk.CP[index] == "||" || btk.CP[index] == "$" || btk.CP[index] == ":" || btk.CP[index] == ")" || btk.CP[index] == "]")
            {
                return true;
            }
            return false;
        }
        protected bool E()
        {
            if (T())
            {
                if (E2())
                {
                    return true;
                }
            }
            return false;
        }

        protected bool E2()
        {
            int tempIndex = index;
            if (btk.CP[index] == "PM")
            {
                index++;
                if (T())
                {
                    if (E2())
                    {
                        return true;
                    }
                }
            }
            else if (btk.CP[index] == "RO" || btk.CP[index] == "&&" || btk.CP[index] == "||" || btk.CP[index] == "$" || btk.CP[index] == ":" || btk.CP[index] == ")" || btk.CP[index] == "]" || btk.CP[index] == ";")
            {
                return true;
            }
            index = tempIndex;
            return false;
        }
        protected bool T()
        {
            if (F())
            {
                if (T2())
                {
                    return true;
                }
            }
            return false;
        }
        protected bool T2()
        {
            if (btk.CP[index] == "MDM")
            {
                index++;
                if (F())
                {
                    if (T2())
                    {
                        return true;
                    }
                }
            }
            else if (btk.CP[index] == "PM" || btk.CP[index] == "RO" || btk.CP[index] == "&&" || btk.CP[index] == "||" || btk.CP[index] == "$" || btk.CP[index] == ":" || btk.CP[index] == ")" || btk.CP[index] == "]" || btk.CP[index] == ";")
            {
                return true;
            }
            return false;
        }
        protected bool F()
        {
            if (TH())
            {
                if (btk.CP[index] == "ID")
                {
                    index++;
                    if (X())
                    {
                        return true;
                    }
                }
            }
            else if (Const())
            {
                index++;
                return true;
            }
            else if (btk.CP[index] == "(")
            {
                index++;
                if (OE())
                {
                    if (btk.CP[index] == ")")
                    {
                        index++;
                        return true;
                    }
                }
            }
            else if (btk.CP[index] == "inc_dec")
            {
                index++;
                if (btk.CP[index] == "ID")
                {
                    index++;
                    return true;
                }
            }
            else if (btk.CP[index] == "!")
            {
                index++;
                if (F())
                {
                    return true;
                }
            }
            return false;
        }

        protected bool TH()
        {
            if (btk.CP[index] == "this")
            {
                index++;
                if (btk.CP[index] == ".")
                {
                    index++;
                    return true;
                }
            }
            else if (btk.CP[index] == "ID")
            {
                return true;
            }
            return false;
        }


        protected bool Foreach_St()
        {
            int tempIndex = index;
            if (btk.CP[index] == "foreach")
            {
                index++;
                if (btk.CP[index] == ":")
                {
                    index++;
                    if (btk.CP[index] == "DT")
                    {
                        index++;
                        if (btk.CP[index] == "ID")
                        {
                            index++;
                            if (btk.CP[index] == "in")
                            {
                                index++;
                                if (DS())
                                {
                                    if (btk.CP[index] == ":")
                                    {
                                        index++;
                                        if (Body())
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            index = tempIndex;
            return false;
        }
        protected bool DS()
        {
            if (Arr_St() || ArrList_St())
            {
                return true;
            }
            return false;
        }
        protected bool While_St()
        {
            int tempIndex = index;
            if (btk.CP[index] == "while")
            {
                index++;
                if (btk.CP[index] == ":")
                {
                    index++;
                    if (OE())
                    {
                        if (btk.CP[index] == ":")
                        {
                            index++;
                            if (Body())
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            index = tempIndex;
            return false;
        }

        protected bool If_Else()
        {
            int tempIndex = index;
            if (btk.CP[index] == "if")
            {
                index++;
                if (btk.CP[index] == ":")
                {
                    index++;
                    if (OE())
                    {
                        if (btk.CP[index] == ":")
                        {
                            index++;
                            if (Body())
                            {
                                if (Elif_St())
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            index = tempIndex;
            return false;
        }
        protected bool Elif_St()
        {
            int tempIndex = index;
            if (btk.CP[index] == "elif")
            {
                index++;
                if (btk.CP[index] == ":")
                {
                    index++;
                    if (OE())
                    {
                        if (btk.CP[index] == ":")
                        {
                            index++;
                            if (Body())
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            else if (btk.CP[index] == "$" || btk.CP[index] == ":")
            {
                return true;
            }
            else if (Else())
            {
                return true;
            }
            index = tempIndex;
            return false;
        }
        protected bool Else()
        {
            int tempIndex = index;
            if (btk.CP[index] == "else")
            {
                index++;
                if (Body())
                {
                    return true;
                }
            }
            else if (btk.CP[index] == "$" || btk.CP[index] == ":")
            {
                return true;
            }
            index = tempIndex;
            return false;
        }


        protected bool Funct_St()
        {

            int tempIndex = index;

            if (Static_St())
            {
                if (DT_Void())
                {
                    if (btk.CP[index] == "ID")
                    {
                        index++;
                        if (btk.CP[index] == ":")
                        {
                            index++;
                            if (Param())
                            {
                                if (btk.CP[index] == ":")
                                {
                                    index++;
                                    if (Body())
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            index = tempIndex;
            return false;
        }
        protected bool Static_St()
        {
            int tempIndex = index;
            if (btk.CP[index] == "static")
            {
                index++;
                return true;
            }
            else if (btk.CP[index] == "DT" || btk.CP[index] == "void" || btk.CP[index] == "class")
            {
                return true;
            }
            index = tempIndex;
            return false;
        }
        protected bool DT_Void()
        {
            int tempIndex = index;
            if (btk.CP[index] == "void" || btk.CP[index] == "DT")
            {
                index++;
                return true;
            }
            index = tempIndex;
            return false;
        }
        protected bool Param()
        {
            int tempIndex = index;
            if (btk.CP[index] == "DT")
            {
                index++;
                if (btk.CP[index] == "ID")
                {
                    index++;
                    if (MultiParam())
                    {
                        return true;
                    }
                }
            }
            else if (btk.CP[index] == ":")
            {
                return true;
            }
            index = tempIndex;
            return false;
        }
        protected bool MultiParam()
        {
            int tempIndex = index;
            if (btk.CP[index] == ",")
            {
                index++;
                if (btk.CP[index] == "DT")
                {
                    index++;
                    if (btk.CP[index] == "ID")
                    {
                        index++;
                        if (MultiParam())
                        {
                            return true;
                        }
                    }
                }
            }
            else if (btk.CP[index] == ":")
            {
                return true;
            }
            index = tempIndex;
            return false;
        }
        protected bool Return_st()
        {
            int tempIndex = index;
            if (btk.CP[index] == "ret")
            {
                index++;
                if (Const())
                {
                    index++;
                    if (btk.CP[index] == ";")
                    {
                        index++;
                        return true;
                    }
                }
            }
            else if (btk.CP[index] == "$")
            {
                return true;
            }
            index = tempIndex;
            return false;
        }

        protected bool Abstract_St()
        {
            int tempIndex = index;

            if (AccessModifier2())
            {
                if (btk.CP[index] == "abstract")
                {
                    index++;
                    if (DT_Void())
                    {
                        if (btk.CP[index] == "ID")
                        {
                            index++;
                            if (btk.CP[index] == ":")
                            {
                                index++;
                                if (Param())
                                {
                                    if (btk.CP[index] == ":")
                                    {
                                        index++;
                                        if (btk.CP[index] == ";")
                                        {
                                            index++;
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            index = tempIndex;
            return false;
        }

        protected bool Arr_St()
        {
            int tempIndex = index;

            if (btk.CP[index] == "DT")
            {
                index++;
                if (btk.CP[index] == "[")
                {
                    index++;
                    if (btk.CP[index] == "]")
                    {
                        index++;

                        if (btk.CP[index] == "ID")
                        {
                            index++;
                            if (List3())
                            {
                                if (btk.CP[index] == ";")
                                {
                                    index++;
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            index = tempIndex;
            return false;
        }
        protected bool List3()
        {
            int tempIndex = index;
            if (btk.CP[index] == "=")
            {
                index++;
                if (List2())
                {
                    return true;
                }
            }
            else if (btk.CP[index] == ";")
            {
                return true;
            }
            index = tempIndex;
            return false;
        }
        protected bool List2()
        {
            int tempIndex = index;
            if (btk.CP[index] == "new")
            {
                index++;
                if (btk.CP[index] == "DT")
                {
                    index++;
                    if (btk.CP[index] == "[")
                    {
                        index++;
                        if (btk.CP[index] == "int-const")
                        {
                            index++;
                            if (btk.CP[index] == "]")
                            {
                                index++;
                                return true;
                            }
                        }
                    }
                }
            }
            else if (btk.CP[index] == "{")
            {
                index++;
                if (Const())
                {
                    index++;
                    if (Multi_Value())
                    {
                        if (btk.CP[index] == "}")
                        {
                            index++;
                            return true;
                        }
                    }
                }
            }
            index = tempIndex;
            return false;
        }
        protected bool Multi_Value()
        {
            int tempIndex = index;
            if (btk.CP[index] == ",")
            {
                index++;
                if (Const())
                {
                    index++;
                    if (Multi_Value())
                    {
                        return true;
                    }
                }
            }
            else if (btk.CP[index] == "}")
            {
                return true;
            }
            index = tempIndex;
            return false;
        }


        protected bool ArrList_St()
        {
            int tempIndex = index;

            if (btk.CP[index] == "ArrayList")
            {
                index++;
                if (btk.CP[index] == "ID")
                {
                    index++;
                    if (List4())
                    {
                        if (btk.CP[index] == ";")
                        {
                            index++;
                            return true;
                        }
                    }
                }
            }
            index = tempIndex;
            return false;
        }
        protected bool List4()
        {
            int tempIndex = index;
            if (btk.CP[index] == "=")
            {
                index++;
                if (btk.CP[index] == "new")
                {
                    index++;
                    if (btk.CP[index] == "ArrayList()")
                    {
                        index++;
                        if (btk.CP[index] == "(")
                        {
                            index++;
                            if (btk.CP[index] == ")")
                            {
                                index++;
                                if (List5())
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            else if (btk.CP[index] == ";")
            {
                return true;
            }
            index = tempIndex;
            return false;
        }
        protected bool List5()
        {
            int tempIndex = index;
            if (btk.CP[index] == "{")
            {
                index++;
                if (Const())
                {
                    index++;
                    if (Multi_Value())
                    {
                        if (btk.CP[index] == "}")
                        {
                            index++;
                            return true;
                        }
                    }
                }
            }
            else if (btk.CP[index] == ";")
            {
                return true;
            }
            index = tempIndex;
            return false;
        }


        protected bool Class_St()
        {
            int tempIndex = index;

            if (AccessModifier())
            {
                if (Static_Abstract())
                {
                    if (btk.CP[index] == "class")
                    {
                        index++;
                        if (btk.CP[index] == "ID")
                        {
                            index++;
                            if (Inherit_Inter())
                            {
                                if (Body2())
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            index = tempIndex;
            return false;
        }
        protected bool AccessModifier()
        {
            int tempIndex = index;
            if (btk.CP[index] == "AccessModifier")
            {
                index++;
                return true;
            }
            else if (btk.CP[index] == "static" || btk.CP[index] == "abstract" || btk.CP[index] == "class")
            {
                return true;
            }
            index = tempIndex;
            return false;
        }
        protected bool Static_Abstract()
        {
            int tempIndex = index;
            if (btk.CP[index] == "static" || btk.CP[index] == "abstract")
            {
                index++;
                return true;
            }
            else if (btk.CP[index] == "class")
            {
                return true;
            }
            index = tempIndex;
            return false;
        }
        protected bool Body2()
        {
            int tempIndex = index;
            if (btk.CP[index] == "{")
            {
                index++;
                if (MST2())
                {
                    if (btk.CP[index] == "}")
                    {
                        index++;
                        return true;
                    }
                }
            }
            index = tempIndex;
            return false;
        }
        protected bool SST2()
        {
            if (Body() || Funct_St() || Class_St())
            {
                return true;
            }
            else if (btk.CP[index] == "{")
            {
                return true;
            }
            return false;
        }
        protected bool MST2()
        {
            if (SST2())
            {
                if (MST2())
                {
                    return true;
                }
            }
            else if (btk.CP[index] == "}")
            {
                return true;
            }
            return false;
        }
        protected bool Inherit_Inter()
        {
            if (Inherit_St())
            {
                if (Interface_Imple())
                {
                    return true;
                }
            }
            if (Interface_Imple())
            {
                return true;
            }
            else if (btk.CP[index] == "{")
            {
                return true;
            }
            return false;
        }
        protected bool Inherit_St()
        {
            int tempIndex = index;
            if (btk.CP[index] == ":")
            {
                index++;
                if (btk.CP[index] == "ID")
                {
                    index++;
                    return true;
                }
            }
            else if (btk.CP[index] == "," || btk.CP[index] == ":" || btk.CP[index] == ":")
            {
                return true;
            }
            index = tempIndex;
            return false;
        }
        protected bool Poly_St()
        {
            int tempIndex = index;

            if (AccessModifier2())
            {
                if (btk.CP[index] == "VO")
                {
                    index++;
                    if (Funct_St2())
                    {
                        return true;
                    }
                }
            }
            index = tempIndex;
            return false;
        }
        protected bool AccessModifier2()
        {
            int tempIndex = index;
            if (btk.CP[index] == "public" || btk.CP[index] == "protected")
            {
                index++;
                return true;
            }
            index = tempIndex;
            return false;
        }
        protected bool Funct_St2()
        {
            int tempIndex = index;
            if (DT_Void())
            {
                if (btk.CP[index] == "ID")
                {
                    index++;
                    if (btk.CP[index] == ":")
                    {
                        index++;
                        if (Param())
                        {
                            if (btk.CP[index] == ":")
                            {
                                index++;
                                if (Body())
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            index = tempIndex;
            return false;
        }


        protected bool Interface_Imple()
        {
            int tempIndex = index;
            if (Col_Comma())
            {
                if (btk.CP[index] == "ID")
                {
                    index++;
                    if (Interface_Imple())
                    {
                        return true;
                    }
                }
            }
            else if (btk.CP[index] == "{")
            {
                return true;
            }
            index = tempIndex;
            return false;
        }
        protected bool Col_Comma()
        {
            int tempIndex = index;
            if (btk.CP[index] == ":" || btk.CP[index] == ",")
            {
                index++;
                return true;
            }
            index = tempIndex;
            return false;
        }


        protected bool Interface_St()
        {
            int tempIndex = index;

            if (btk.CP[index] == "interface")
            {
                index++;
                if (btk.CP[index] == "ID")
                {
                    index++;
                    if (Body3())
                    {
                        return true;
                    }
                }
            }
            index = tempIndex;
            return false;
        }
        protected bool Body3()
        {
            int tempIndex = index;
            if (btk.CP[index] == "{")
            {
                index++;
                if (MST3())
                {
                    if (btk.CP[index] == "}")
                    {
                        return true;
                    }
                }
            }
            index = tempIndex;
            return false;
        }
        protected bool MST3()
        {
            if (SST3())
            {
                if (MST3())
                {
                    return true;
                }
            }
            else if (btk.CP[index] == "}")
            {
                return true;
            }
            return false;
        }
        protected bool SST3()
        {
            if (Funct_St3())
            {
                return true;
            }
            else if (btk.CP[index] == "{")
            {
                return true;
            }
            return false;
        }
        protected bool Funct_St3()
        {
            int tempIndex = index;
            if (DT_Void())
            {
                if (btk.CP[index] == "ID")
                {
                    index++;
                    if (btk.CP[index] == ":")
                    {
                        index++;
                        if (Param())
                        {
                            if (btk.CP[index] == ":")
                            {
                                index++;
                                if (btk.CP[index] == ";")
                                {
                                    index++;
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            index = tempIndex;
            return false;
        }



        public void PrintToken()
        {
            foreach (var item in btk.CP)
            {
                Console.WriteLine(item);
            }
            foreach (var item in btk.VP)
            {
                Console.WriteLine(item);
            }
            foreach (var item in btk.LN)
            {
                Console.WriteLine(item);
            }
        }

    }

    class BreakToken
    {
        public List<string> CP { get; set; }
        public List<string> VP { get; set; }
        public List<string> LN { get; set; }

        public BreakToken(List<string> token)
        {
            CP = new List<string>();
            VP = new List<string>();
            LN = new List<string>();

            for (int i = 0; i < token.Count; i++)
            {
                int indexOfComma = token[i].IndexOf(", ");
                CP.Add(token[i].Substring(1, indexOfComma - 1));
                int indexOfComma2 = token[i].IndexOf(", ", indexOfComma + 1);
                VP.Add(token[i].Substring(indexOfComma + 2, indexOfComma2 - (indexOfComma + 2)));
                LN.Add(token[i].Substring(indexOfComma2 + 2, token[i].Length - indexOfComma2 - 3));
            }
            CP.Add("$");
        }
    }


}
