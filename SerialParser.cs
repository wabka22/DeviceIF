using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DeviceIF
{
    class SerialParser
    {
        Regex reg;
        string expression;
        public Dictionary<string, double> ParsingMatch(string input)
        {
            Dictionary<string, double> matchesTable = new Dictionary<string, double>();
            return matchesTable;
        }
    }
}
