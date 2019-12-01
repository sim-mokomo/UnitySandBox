using System.Collections;
using System.Collections.Generic;
using MokomoGames.Debug;
using UnityEngine;

public class DummyPlayer : MonoBehaviour
{
    PlayerInfo playerInfo = new PlayerInfo (
        Hp: 100,
        Mp: 50,
        Stamina: 120,
        Power: 20,
        Speed: 10,
        Defence: 30
    );

    PlayerInfoDebugModel playerInfoDebugModel;
    PlayerInfoDebugModel PlayerInfoDebugModel
    {
        get
        {
            if (playerInfoDebugModel == null)
                playerInfoDebugModel = new PlayerInfoDebugModel (
                    playerInfo.Hp,
                    playerInfo.Mp,
                    playerInfo.Stamina,
                    playerInfo.Power,
                    playerInfo.Defence
                );
            return playerInfoDebugModel;
        }
    }

    TransformDebugDrawer transformDebugDrawer;
    TransformDebugDrawer TransformDebugDrawer
    {
        get
        {
            if (transformDebugDrawer == null)
                transformDebugDrawer = new TransformDebugDrawer (this.transform);
            return transformDebugDrawer;
        }
    }

    public void OnGUI ()
    {
        if (Debugger.Config.DisplayPlayerData)
        {
            Debugger.DisplayDebugModel (
                PlayerInfoDebugModel,
                TransformDebugDrawer);
        }
    }

    public class PlayerInfo
    {
        public int Hp { get; }
        public int Mp { get; }
        public int Stamina { get; }
        public int Power { get; }
        public int Speed { get; }
        public int Defence { get; }

        public PlayerInfo (int Hp, int Mp, int Stamina, int Power, int Speed, int Defence)
        {
            this.Hp = Hp;
            this.Mp = Mp;
            this.Stamina = Stamina;
            this.Power = Power;
            this.Speed = Speed;
            this.Defence = Defence;
        }
    }
}