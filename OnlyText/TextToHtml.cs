using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OnlyText
{
    class TextToHtml
    {
        string myFilePath;
        public TextToHtml(string afilePath)
        {
            myFilePath = afilePath;
        }

        public void ConvertAndSave(string aText)
        {
            string[] tokens = aText.Split('\n','\r');
            Func<string, StreamWriter, bool>[] processors = { FindOneLiners, FindPreformatted, FindList, FindHeading, WriteParagraph };
            using (StreamWriter file = new StreamWriter(myFilePath))
            {
                foreach (var token in tokens)
                {
                    if (token.Length == 0)
                        continue;

                    foreach(var process in processors)
                    {
                        if (process(token, file))
                            break;
                    }
                }
            }
        }

        private bool WriteParagraph(string token, StreamWriter file)
        {
            file.WriteLine("<p>" + Html(token) + "</p>");
            return true;
        }

        private bool FindPreformatted(string token, StreamWriter file)
        {
            if(isPreformatted)
                file.WriteLine(token);
            return isPreformatted;
        }

        bool isPreformatted = false;
        private bool FindOneLiners(string pp, StreamWriter file)
        {
            string[] txt = {"{{","}}","---" };
            string[] tag = { "<pre>", "</pre>", "<hr/>" };
            string p = pp.Trim();
            for (int i = 0; i < txt.Length; i++)
            {
                if (p.Equals(txt[i]))
                {
                    if(i==0)
                    {
                        isPreformatted = true;
                    }
                    if (i==1)
                    {
                        isPreformatted = false;
                    }
                    file.WriteLine(tag[i]);
                    return true;
                }
            }
            return false;
        }

        private bool FindHeading(string token, StreamWriter file)
        {
            if (token.Length > 0 && token[0] != '!')
                return false;
            int i=0;
            while (i < 6 && i < token.Length && token[i] == '!')
            {
                i++;
            }

            file.WriteLine("<h{0}>{1}</h{0}>", i, Html(token.Substring(i)));

            return true;
        }

        int myListLevel = 0;
        StringBuilder padding = new StringBuilder("");
        private bool FindList(string token, StreamWriter file)
        {
            if (token.Length > 0 && token[0] != '#')
            {
                while(myListLevel > 0)
                {
                    file.WriteLine(padding+"</li>");
                    padding.Remove(padding.Length - 1, 1);
                    file.WriteLine("{0}{1}",padding,"</ul>");
                    myListLevel--;
                }
                return false;
            }

            int i = 0;
            while (i < 6 && i < token.Length && token[i] == '#')
            {
                i++;
            }

            if (i == myListLevel)
            {
                file.WriteLine("{0}</li>", padding);
                file.WriteLine("{0}<li>{1}", padding, Html(token.Substring(i)));
            }
            else
            {
                if(i>myListLevel)
                {
                    myListLevel++;
                    file.WriteLine("{0}<ul>", padding);
                    padding.Append('\t');
                    file.WriteLine("{0}<li>{1}", padding, Html(token.Substring(i)));
                }
                else
                {
                    myListLevel--;
                    file.WriteLine("{0}</li>", padding);
                    padding.Remove(padding.Length-1,1);
                    file.WriteLine("{0}</ul>", padding);
                    file.WriteLine("{0}</li>",padding);
                    file.WriteLine("{0}<li>{1}",padding, Html(token.Substring(i)));
                }
            }
            return true;
        }


        private string Html(string s)
        {
            return WebUtility.HtmlEncode(s);
        }
    }
}
