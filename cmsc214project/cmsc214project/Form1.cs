using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace cmsc214project
{
    public partial class Form1 : Form
    {
        string[] tokens;                //accumulated tokens read from the code textbox
        string cToken;                  //current token
        int cIndex;                     //current index
        int cLine;                      //current line in the tokens array
        Boolean error = false;          //error flag

        public Form1()
        {
            InitializeComponent();
        }

        /*
         * Clears the code textbox
         */
        private void clear_Click(object sender, EventArgs e)
        {
            code.Text = "";
        }

        private void run_Click(object sender, EventArgs e)
        {
            int i;

            //Resets values of all variables
            resetValues();

            tokens = code.Text.TrimEnd().Split('\n');

            //Trims whitespaces in every token
            for (i = 0; i < tokens.Length; i++)
            {
                tokens[i] = Regex.Replace(tokens[i], @"^[0-9]*", "");
                tokens[i] = tokens[i].Trim(' ', '\r', '\n', '\t');
                tokens[i] = tokens[i].Replace('\t', ' ');       
            }
            
            lexer();
            analyze();
            //eval();
            //output.AppendText(cToken);
        }

        /*
         * Gets the current token per character
        */
        private void lexer()
        {
            string temp = "";
            cToken = "";

            //checks if end of line for the code
            if (cLine < tokens.Length)
            {
                //checks if the current index is still within the bounds of the current line in the code
                if (cIndex < tokens[cLine].Length)
                {
                    cToken = tokens[cLine][cIndex].ToString();
                    skipSpaces();
                    //if there's nothing in that line
                    if (cToken.Length == 0 && tokens[cLine].Length == 0)
                    {
                        cLine++;
                        cIndex = 0;
                        lexer();
                    }
                    else
                    {
                        //checks for string
                        if (cToken == "\"")
                        {
                            cIndex++;
                            if (cIndex < tokens[cLine].Length)
                            {
                                cToken = tokens[cLine][cIndex].ToString();
                            }
                            else
                            {
                                displayError("Kulang o Inaasahang \"");
                            }

                            while (cToken != "\"" && cIndex < tokens[cLine].Length)
                            {
                                temp = temp + cToken;
                                cIndex++;

                                if (cIndex < tokens[cLine].Length)
                                {
                                    cToken = tokens[cLine][cIndex].ToString();
                                }
                            }

                            //if successfully caught a string
                            if (cToken == "\"")
                            {
                                cToken = "\"" + temp + "\"";
                                // if (!parseFlag) tokenize();
                                cIndex++;
                            }
                            else
                            {
                                if (error == false)
                                {
                                    displayError("Kulang o Inaasahang \"");
                                }
                            }

                            //checks if there is a space after the string
                            temp = "";
                            if (cIndex < tokens[cLine].Length)
                            {
                                temp = tokens[cLine][cIndex].ToString();
                                if (temp == " ")
                                {
                                    cIndex++;
                                    skipSpaces();
                                }
                                else
                                {
                                    displayError("Kulang o Inaasahang sa patlang");
                                }
                            }
                        }//end of string
                        else if (cToken == "\'")
                        {
                            cIndex++;
                            if (cIndex < tokens[cLine].Length)
                            {
                                cToken = tokens[cLine][cIndex].ToString();
                                temp = cToken;
                                cIndex++;
                                if (cIndex < tokens[cLine].Length)
                                {
                                    cToken = tokens[cLine][cIndex].ToString();
                                    if (cToken == "\'")
                                    {
                                        cToken = "\'" + temp + "\'";
                                    }
                                    else
                                    {
                                        displayError("Kulang o Inaasahang \'");
                                    }
                                }
                                else
                                {
                                    displayError("Kulang o Inaasahang \'");
                                }
                            }
                            else
                            {
                                displayError("Kulang o Inaasahang \'");
                            }
                        }
                        else if (cLine < tokens.Length && cIndex < tokens[cLine].Length)
                        {
                            while (cToken != " " && cIndex < tokens[cLine].Length)
                            {
                                temp = temp + cToken;
                                cIndex++;

                                if (cIndex < tokens[cLine].Length)
                                {
                                    cToken = tokens[cLine][cIndex].ToString();
                                }
                            }
                            cToken = temp;
                            //if (!parseFlag)
                            //    token();
                        }//end of cToken
                    }
                } //end of if (cIndex < tokens[cLine].Length)
                else
                {
                    //if end of line, move to the next line

                    cIndex = 0;
                    cLine++;
                    lexer();
                }
            }
        }

        private void analyze()
        {
            if (checkVar())
            {
                output.AppendText("Variable Declaration\n");
            }
            else
            {
                displayError("Imbalidong salita");
            }
        }

        /*
         * Checks if statement is variable declaration
         */
        private Boolean checkVar()
        {
            string[] dataTypes = { "INTEDYER", "PLOWT", "BULYAN", "ISTRING", "KAR" };
            int line;
            string cType = "";
            string cVar = "";
            if (isDataType(cToken))
            {
                cType =  cToken;
                line = cLine;
                lexer();
                if (line==cLine && isVarName())
                {
                    cVar = cToken;
                    lexer();
                    if (line == cLine && cToken == "AY")
                    {
                        lexer();
                        int cValue1;
                        float cValue2;
                        if (line == cLine && cType == dataTypes[0] && int.TryParse(cToken, out cValue1))
                        {
                            //storeVar(cType, cVar, cValue1);
                            //output.Text = cType+cVar+cValue1.ToString();
                            return true;
                        }
                        else if (line == cLine && cType == dataTypes[1] && float.TryParse(cToken, out cValue2))
                        {
                            //storeVar(cType, cVar, cValue2);
                            //output.Text = cType+cVar+cValue2.ToString();
                            return true;
                        }
                        else if (line == cLine && cType == dataTypes[2] && (cToken == "totoo" || cToken == "mali"))
                        {
                            //storeVar(cType, cVar, cValue3);
                            //output.Text = cType + cVar + cToken;
                            return true;
                        }
                        else if (line == cLine && cType == dataTypes[3] && (cToken[0] == '\"' && cToken[cToken.Length-1] == '\"'))
                        {
                            //storeVar(cType, cVar, cValue3);
                            //output.Text = cType + cVar + cToken;
                            return true;
                        }
                        else if (line == cLine && cType == dataTypes[4] && (cToken[0] == '\'' && cToken[cToken.Length - 1] == '\''))
                        {
                            //storeVar(cType, cVar, cValue3);
                            //output.Text = cType + cVar + cToken;
                            return true;
                        }
                        else
                        {
                            if (line != cLine)
                            {
                                displayError("Nawawala o Inaasahang pangalan ng baryante");
                            }
                            else
                            {
                                displayError("Magkasalungat na uri");
                            }
                            
                        }

                    }
                    else
                    {
                        if(line!=cLine) return true;
                        else displayError("Nawawala o Inaasahang AY");
                    }
                }
                else
                {
                    if (line != cLine)
                    {
                        displayError("Nawawala o Inaasahang pangalan ng baryante");
                    }
                    else if (!isVarName())
                    {
                        displayError("Imbalidong pangalan ng baryante");
                    }
                    
                }
            }

            return false;
        }

        /*
         * Resets all the global variables
         */
        private void resetValues()
        {
            cToken = "";
            cLine = 0;
            cIndex = 0;
            error = false;
            output.Text = "";
        }

        /*
         * Moves the cToken to a non-whitespace character in a line
         */
        private void skipSpaces()
        {
            while (cIndex < tokens[cLine].Length)
            {
                if (cToken == " " || cToken == "\t")
                {
                    cIndex++;
                    cToken = tokens[cLine][cIndex].ToString();
                }
                else
                {
                    break;
                }
            }
        }

        /*
         * Displays an error message to the output textbox
         */
        private void displayError(String errorMessage)
        {
            int i;

            if (error == false)
            {
                i = cLine + 1;
                output.ForeColor = Color.Red;
                output.Text = "MALI SA LINYA " + i + ": " + errorMessage;
                error = true;

                //end the interpreter
                cLine = tokens.Length - 1;
                cIndex = tokens[cLine].Length;
            }
        }

        /*
         * Checks the validity of the data type
         */
        private Boolean isDataType(string cToken)
        {
            int i;
            string[] dataTypes = { "INTEDYER", "PLOWT", "BULYAN", "ISTRING", "KAR" };
            //cToken = cToken.ToUpper();

            for(i=0; i<dataTypes.Length; i++)
            {
                if(cToken == dataTypes[i]) return true;
            }
            
            return false;
        }

        /*
         * Checks the validity of the variable name
         */
        private Boolean isVarName()
        {
            Regex exp = new Regex(@"^[A-Za-z]+.*$");
            Match m = exp.Match(cToken);

            if (m.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void evaluate_code()
        {
            
        }
    }
}
