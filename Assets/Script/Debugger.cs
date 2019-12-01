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

            public static Config config = new Config ();

            public class Config
            {
                public bool GetFlagActive (DebugFlagType flagType)
                {
                    if (DebugFlags.TryGetValue (flagType, out DebugFlag flag))
                    {
                        return flag.Active;
                    }
                    return false;
                }

                Dictionary<DebugFlagType, DebugFlag> debugFlags;
                public IReadOnlyDictionary<DebugFlagType, DebugFlag> DebugFlags => debugFlags;

                public Config ()
                {
                    debugFlags = new Dictionary<DebugFlagType, DebugFlag> ();
                    debugFlags.Add (DebugFlagType.DisplayTransformPlayerData, new DebugFlag ("プレイヤー詳細情報表示(追従)"));
                    debugFlags.Add (DebugFlagType.DisplayFixPlayerData, new DebugFlag ("プレイヤー詳細情報表示(固定)"));
                }
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