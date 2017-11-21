using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V0._1
{
    abstract class Team
    {
        private string name;
        private string league;

        public Team(string Name, string league = "")
        {
            this.name = Name;
            if(league != "")
            {
                this.league = league;
            }  
        }
    }

    class InternationalTeam : Team
    {
        private int pot;

        public InternationalTeam(string Name, string League, int Pot) : base(Name, League)
        {
            this.pot = Pot;
        }               
    }
}
