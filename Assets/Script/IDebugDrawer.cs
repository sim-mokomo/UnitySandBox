using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MokomoGames
{
    namespace Debug
    {
        public interface IDebugDrawer
        {
            Vector2 GetOriginDrawScreenPos ();
            void Draw (Vector2 screenPos, string debugMessage);
        }
    }
}