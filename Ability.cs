using System;
using System.Collections.Generic;
using System.Text;

namespace PokeAPI
{
    public class AbilityEntry
    {
        public AbilityDetail ability { get; set; }
    }

    public class AbilityDetail
    {
        public string name { get; set; }
    }
}