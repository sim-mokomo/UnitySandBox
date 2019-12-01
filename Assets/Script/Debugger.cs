using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MokomoGames
{
    namespace Debug
    {
        public class Debugger
        {
            public static void DisplayDebugModel (IDebugModel model, IDebugDrawer drawer)
            {
                var debugMessages = model.GetDebugMessages ();
                var originScreenPos = drawer.GetOriginDrawScreenPos ();

                for (int i = 0; i < debugMessages.Length; i++)
                {
                    var labelScreenPos = CalcScreenPosWithSpan (originScreenPos, drawOrder : i);
                    var message = debugMessages[i];
                    drawer.Draw (labelScreenPos, message);
                }
            }

            public static class Config
            {
                public static bool DisplayPlayerData;
                public static bool DisplayFixPlayerData;
            }

            static Vector2 CalcScreenPosWithSpan (Vector2 screenPos, int drawOrder)
            {
                screenPos.y += 15f * drawOrder;
                screenPos.y = Screen.height - screenPos.y;
                return screenPos;
            }
        }
    }
}