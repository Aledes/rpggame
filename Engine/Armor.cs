using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Armor : Item
    {
        public int MinDamageReduction { get; set; }
        public int MaxDamageReduction { get; set; }
        public int Evasion { get; set; } //int % out of 100?

        public Armor(int id, string name, string namePlural,
            int minDamageReduction, int maxDamageReduction, int evasion)
            : base(id, name, namePlural)
        {
            MinDamageReduction = minDamageReduction;
            MaxDamageReduction = maxDamageReduction;
            Evasion = evasion;
        }
    }
}
