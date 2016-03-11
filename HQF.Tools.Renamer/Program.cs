using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HQF.Tools.Renamer
{
    class Program
    {
        static void Main(string[] args)
        {
            var fixer = new FileNameFixer();
            if (args.Length > 0)
            {
                fixer.FixAll(args[0]);
            }
        }


      
    }
}
