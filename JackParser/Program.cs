using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;

namespace JackParser
{
    class Program
    {
        public static List<string> keywordList = new List<string>();
        public static List<string> symbolList = new List<string>();
        public static List<string> jackToParse = new List<string>();
        public static int integerConstant = 0;
        public static string stringConstant = "";
        public static string identifier = "";
        public static string[] jackFileSplit;
        //Best sorting pattern EU
        public static string pattern = "([(])|([)])|[ ]|([;])|([{])|([}])|[\\t]";


        public static string lineToRead = "";


        static void Main(string[] args)
        {
            FillKeyWord();
            FillSymbol();

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("   ");
            settings.CloseOutput = true;
            settings.OmitXmlDeclaration = true;

            string path = Console.ReadLine();
            string jackFile;

            foreach (string file in Directory.GetFiles(path, "*.jack"))
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    while ((jackFile = sr.ReadLine()) != null)
                    {
                        //Work with this, basicly look at what you did in the other script!"!!!"!!"#""!!"!"#"!""!#!"#"!#!"#!"#!"#!"#
                        jackFile = jackFile.Split('/')[0];
                        if (jackFile.StartsWith(""))
                        {

                        }
                        else
                        {
                            string[] part = Regex.Split(jackFile, pattern);
                            foreach (string entry in part)
                            {
                                jackToParse.Add(entry);
                            }
                        }

                        
                    }

                }
            }

            jackFileSplit = jackToParse.ToArray();

            using (XmlWriter writer = XmlWriter.Create($"simpletokentest.xml", settings))
            {
                writer.WriteStartElement("tokens");
                while (jackFileSplit.Length != 0)
                {
                    //Check for nothing and Comments
                    if (jackFileSplit[0] == "" || jackFileSplit[0].StartsWith("//"))
                    {
                        string toRemove = jackFileSplit[0];
                        List<string> tmp = new List<string>(jackFileSplit);
                        tmp.Remove(toRemove);
                        jackFileSplit = tmp.ToArray();
                    }

                    //Check if its a keyword
                    else if (WriteKeyWord(jackFileSplit) != null)
                    {
                        writer.WriteElementString("keyword", WriteKeyWord(jackFileSplit));
                        string toRemove = jackFileSplit[0];
                        List<string> tmp = new List<string>(jackFileSplit);
                        tmp.Remove(toRemove);
                        jackFileSplit = tmp.ToArray();
                    }

                    //Check if its a symbol
                    else if (WriteSymbol(jackFileSplit) != null)
                    {
                        writer.WriteElementString("symbol", WriteSymbol(jackFileSplit));
                        string toRemove = jackFileSplit[0];
                        List<string> tmp = new List<string>(jackFileSplit);
                        tmp.Remove(toRemove);
                        jackFileSplit = tmp.ToArray();
                    }

                    //Check if its an integer
                    else if (int.TryParse(jackFileSplit[0], out integerConstant))
                    {
                        writer.WriteElementString("integerConstant", integerConstant.ToString());
                        string toRemove = jackFileSplit[0];
                        List<string> tmp = new List<string>(jackFileSplit);
                        tmp.Remove(toRemove);
                        jackFileSplit = tmp.ToArray();
                    }

                    else if (jackFileSplit[0].StartsWith("\""))
                    {
                        stringConstant = jackFileSplit[0].Split('"')[1];
                        writer.WriteElementString("stringConstant", stringConstant);
                        string toRemove = jackFileSplit[0];
                        List<string> tmp = new List<string>(jackFileSplit);
                        tmp.Remove(toRemove);
                        jackFileSplit = tmp.ToArray();
                    }

                    //Else it gotta be an Identifier
                    else
                    {
                        identifier = jackFileSplit[0];
                        writer.WriteElementString("identifier", identifier);
                        string toRemove = jackFileSplit[0];
                        List<string> tmp = new List<string>(jackFileSplit);
                        tmp.Remove(toRemove);
                        jackFileSplit = tmp.ToArray();
                    }
                }





                writer.WriteEndElement();
                writer.Flush();
            }
        }

        public static string WriteKeyWord(string[] file)
        {
            foreach (string keyword in keywordList)
            {
                if (keyword == file[0])
                {
                    return keyword;
                }
            }
            return null;
        }

        public static string WriteSymbol(string[] file)
        {
            foreach (string symbol in symbolList)
            {
                if (symbol == file[0])
                {
                    if (symbol == "<")
                    {
                        return "&lt;";
                    }
                    else if (symbol == ">")
                    {
                        return "&gt";
                    }
                    else if (symbol == "&")
                    {
                        return "&amp";
                    }
                    else
                    {
                        return symbol;
                    }
                }
            }
            return null;
        }

        //public static void DefineClass()
        //{
        //    string definedclass = "class " + identifier + "{" + DefineClassVarDec();
        //}

        //public static string DefineClassVarDec()
        //{
        //    string staticOrField = "";
        //    string varName = "";
        //    string definedClassVarDec = "";

        //    if (jackFile.Contains("static"))
        //    {
        //        foreach (string keyword in keywordList)
        //        {
        //            if (keyword == "static")
        //            {
        //                staticOrField = "static";
        //            }
        //        }
        //    }
        //    else
        //    {
        //        staticOrField = "field";
        //    }

        //    definedClassVarDec = $"({staticOrField})";
        //    return null;

        //}

        //public static string DefineVarName()
        //{
        //    return identifier;
        //}

        public static void FillSymbol()
        {
            symbolList.Add("{");
            symbolList.Add("}");
            symbolList.Add("(");
            symbolList.Add(")");
            symbolList.Add("[");
            symbolList.Add("]");
            symbolList.Add(".");
            symbolList.Add(",");
            symbolList.Add(";");
            symbolList.Add("+");
            symbolList.Add("-");
            symbolList.Add("*");
            symbolList.Add("/");
            symbolList.Add("&");
            symbolList.Add("|");
            symbolList.Add("<");
            symbolList.Add(">");
            symbolList.Add("=");
            symbolList.Add("~");
        }

        public static void FillKeyWord()
        {
            keywordList.Add("class");
            keywordList.Add("constructor");
            keywordList.Add("function");
            keywordList.Add("method");
            keywordList.Add("field");
            keywordList.Add("static");
            keywordList.Add("var");
            keywordList.Add("int");
            keywordList.Add("char");
            keywordList.Add("boolean");
            keywordList.Add("void");
            keywordList.Add("true");
            keywordList.Add("false");
            keywordList.Add("null");
            keywordList.Add("this");
            keywordList.Add("let");
            keywordList.Add("do");
            keywordList.Add("if");
            keywordList.Add("else");
            keywordList.Add("while");
            keywordList.Add("return");
        }
    }
}
