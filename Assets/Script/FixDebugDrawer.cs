using System.Collections;
using System.Collections.Generic;
using MokomoGames.Debug;
using UnityEngine;

public class FixDebugDrawer : IDebugDrawer
{
    Vector2 DrawScreenPos { get; set; }

    public FixDebugDrawer (Vector2 DrawScreenPos)
    {
        this.DrawScreenPos = DrawScreenPos;
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
        return DrawScreenPos;
    }
}