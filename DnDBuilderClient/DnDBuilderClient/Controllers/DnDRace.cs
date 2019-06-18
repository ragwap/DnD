using System;
using System.Collections;
using System.Collections.Generic;

namespace DnDBuilderClient.Controllers
{
    public class DnDRace
    {
        int[] ability_bonuses = new int[6];
        int index;

        public DnDRace()
        {

        }

        public int Index { get => index; set => index = value; }

        public IList<Results> Results { get; set; }
        public int[] Ability_bonuses { get => ability_bonuses; set => ability_bonuses = value; }
    }
}
