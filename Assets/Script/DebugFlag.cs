using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MokomoGames
{
    namespace Debug
    {
        public class DebugFlag
        {
            public bool Active { get; set; }
            public string Description { get; private set; }

            public DebugFlag (string description)
            {
                this.Description = description;
            }
        }

        public enum DebugFlagType
        {
            DisplayTransformPlayerData,
            DisplayFixPlayerData,
        }
    }
}