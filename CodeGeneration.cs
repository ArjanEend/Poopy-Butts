using RocketWorks.CodeGeneration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexTastic
{
    class Program
    {
        static void Main(string[] args)
        {
            ContextGenerator generator = new ContextGenerator();
            if(Directory.Exists("../../Assets/Source/Generated"))
                Directory.Delete("../../Assets/Source/Generated", true);
            Directory.CreateDirectory("../../Assets/Source/Generated");
            while(!Directory.Exists("../../Assets/Source/Generated"))
            {
                //kind of a hack, but this shit works
            }
            for(int i = 0; i < generator.Builders.Count; i++)
            {
                FileStream fStream = new FileStream("../../Assets/Source/Generated/" + generator.Builders[i].Name + ".cs", FileMode.Create);
                StreamWriter writer = new StreamWriter(fStream);
                string codeString = generator.Builders[i].StringBuilder.ToString();
                //Replace newlines, there's some ambiguity but I'm using \n everywhere so this is the most safe
                codeString = codeString.Replace("\n", "\r\n");
                writer.Write(codeString);

                writer.Flush();
                writer.Dispose();
                fStream.Dispose();
            }
            while (!Console.KeyAvailable)
            {
            }
        }
    }
}
