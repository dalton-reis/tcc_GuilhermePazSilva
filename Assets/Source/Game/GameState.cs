﻿using Assets.Source.Model;
using UnityEngine;

namespace Assets.Source.Game
{
    public class GameState
    {
        public static GameState main = new GameState();

        public GameMap currentMap;

        // event execution
        public GameEntity currentExecutedEntity;
        public int currentExecutedEventIndex;

        public void ExecuteEntity(GameEntity interactionEntity)
        {
            currentExecutedEntity = interactionEntity;
            currentExecutedEventIndex = 0;

            foreach (var ev in currentExecutedEntity.events)
            {
                ev.startedExecution = false;
                ev.finishedExecution = false;
            }
        }

        public void ChangeMap(string mapName, int x, int y)
        {
            GameMap curr = null;
            foreach (var map in Global.game.maps)
            {
                if (map.name == mapName)
                {
                    curr = map;
                    break;
                }
            }

            if (curr == null)
                return;

            Global.currentMap = curr;

            GameMasterBehaviour.main.InstantiateMap(Global.currentMap);
            GameMasterBehaviour.main.player.transform.localPosition = new Vector3(x, Global.currentMap.height - y - 1, GameMasterBehaviour.main.player.transform.localPosition.z);
            GameMasterBehaviour.main.player.CenterCamera();
        }
    }
}
