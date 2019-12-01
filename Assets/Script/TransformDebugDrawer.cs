using System.Collections;
using System.Collections.Generic;
using MokomoGames.Debug;
using UnityEngine;

namespace MokomoGames
{
    namespace Debug
    {
        public class TransformDebugDrawer : IDebugDrawer
        {
            Transform transform;

            public TransformDebugDrawer (Transform transform)
            {
                this.transform = transform;
            }

            public void Draw (Vector2 screenPos, string debugMessage)
            {
                GUI.Label (
                    new Rect (
                        screenPos.x,
                        screenPos.y,
                        300f,
                        30f
                    ),
                    debugMessage
                );
            }

            public Vector2 GetOriginDrawScreenPos ()
            {
                var originWorldPos = transform.position;
                var originScreenPos = Camera.main.WorldToScreenPoint (originWorldPos);
                return originScreenPos;
            }
        }
    }
}