using System;
using System.Collections;
using System.Collections.Generic;

namespace DnDBuilderClient.Controllers
{
    public class DnDClass
    {
        string index;
        int hit_die;
        int count;

        public string Index { get => index; set => index = value; }
        public int Hit_die { get => hit_die; set => hit_die = value; }
        public int Count { get => count; set => count = value; }
        public IList<Results> Results { get; set; }
        public SpellCasting SpellCasting { get; set; }
    }

    public class SpellCasting
    {
        String url;

        public string Url { get => url; set => url = value; }
    }
}
