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
            for(int i = 0; i < 10; i++)
            {
                FileStream fStream = new FileStream("../../Assets/Source/Generated/" + i + ".cs", FileMode.Create);
                StreamWriter writer = new StreamWriter(fStream);
                writer.Write(new ClassBuilder().StringBuilder.ToString());

                writer.Flush();
                writer.Dispose();
                fStream.Dispose();
            }
        }
    }
}
