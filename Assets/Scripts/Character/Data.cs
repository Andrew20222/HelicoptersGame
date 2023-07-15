using System;
using System.Collections.Generic;

namespace Dates
{
    [Serializable]
    public class Data
    {
        public string CurrentCharacter = "Airplane big";
        public List<string> HaveCharacters = new() { "Airplane big" };
        public int Money;
    }
}