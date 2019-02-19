using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace S7_Dimat.Class
{
    class InputFormatter
    {
        private string Adrr;
        private Boolean Ok;

        private Boolean nbit;
        private Boolean nbyte;
        private Boolean nword;
        private Boolean ndouble;

        private Boolean dbbit;
        private Boolean dbbyte;
        private Boolean dbword;
        private Boolean dbdouble;

        public Boolean IsValid
        {
            get {
                return Ok;
            }
        }

        public Boolean IsBit {
            get
            {
                return nbit | dbbit;
            }
        }

        public Boolean IsByte
        {
            get
            {
                return nbyte | dbbyte;
            }
        }

        public Boolean IsWord
        {
            get
            {
                return nword | dbword;
            }
        }

        public Boolean IsDouble
        {
            get
            {
                return ndouble | dbdouble;
            }
        }

        public InputFormatter(string input)
        {
            Adrr = input.ToUpper().Trim();
            CheckAdrr();
        }

        private void CheckAdrr()
        {
            Regex NormalBit = new Regex(@"[I,Q,M]\d+[.][0-7]$", RegexOptions.IgnoreCase);
            Regex NormalByte = new Regex(@"[I,Q,M][B]\d+$", RegexOptions.IgnoreCase);
            Regex NormalWord = new Regex(@"[I,Q,M][W]\d+$", RegexOptions.IgnoreCase);
            Regex NormalDouble = new Regex(@"[I,Q,M][D]\d+$", RegexOptions.IgnoreCase);

            Regex DBBit = new Regex(@"\DB\d+.DBX\d+[.][0-7]$", RegexOptions.IgnoreCase);
            Regex DBByte = new Regex(@"\DB\d+.DB[B]\d+$", RegexOptions.IgnoreCase);
            Regex DBWord = new Regex(@"\DB\d+.DB[W]\d+$", RegexOptions.IgnoreCase);
            Regex DBDouble = new Regex(@"\DB\d+.DB[D]\d+$", RegexOptions.IgnoreCase);
            
            if (NormalBit.IsMatch(Adrr)) { nbit = true; Ok = true; return; }
            if (NormalByte.IsMatch(Adrr)) { nbyte = true; Ok = true; return; }
            if (NormalWord.IsMatch(Adrr)) { nword = true; Ok = true; return; }
            if (NormalDouble.IsMatch(Adrr)) { ndouble = true; Ok = true; return; }

            if (DBBit.IsMatch(Adrr)) { dbbit = true; Ok = true; return; }
            if (DBByte.IsMatch(Adrr)) { dbbyte = true; Ok = true; return; }
            if (DBWord.IsMatch(Adrr)) { dbword = true; Ok = true; return; }
            if (DBDouble.IsMatch(Adrr)) { dbdouble = true; Ok = true; return; }

        }

    }
}
