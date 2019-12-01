using System.Collections;
using System.Collections.Generic;
using MokomoGames.Debug;
using UnityEngine;

namespace MokomoGames
{
    namespace Debug
    {
        public class PlayerInfoDebugModel : IDebugModel
        {
            string[] debugMessages = new string[5];

            public int Hp { get; }
            public int Mp { get; }
            public int Stamina { get; }
            public int Power { get; }
            public int Defence { get; }

            public PlayerInfoDebugModel (int Hp, int Mp, int Stamina, int Power, int Defence)
            {
                this.Hp = Hp;
                this.Mp = Mp;
                this.Stamina = Stamina;
                this.Power = Power;
                this.Defence = Defence;
            }

            public string[] GetDebugMessages ()
            {
                debugMessages[0] = $"Hp:{Hp}";
                debugMessages[1] = $"Mp:{Mp}";
                debugMessages[2] = $"Stamina:{Stamina}";
                debugMessages[3] = $"Power:{Power}";
                debugMessages[4] = $"Defence:{Defence}";
                return debugMessages;
            }
        }
    }
}