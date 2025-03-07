﻿using Assets.Source.Game;
using Assets.Source.Model;

namespace Assets.Source.Events
{
    public class ChangeMapEvent : GameEvent
    {
        public string mapName;
        public int x;
        public int y;

        public override string GetNameText()
        {
            return "Mudar mapa";
        }

        public override string GetDescriptionText()
        {
            return mapName + " - {" + x + ";" + y + "}";
        }

        public override void Execute()
        {
            GameState.main.ChangeMap(mapName, x, y);

            finishedExecution = true;
        }

        public override void Update()
        {
            //TODO transition
        }

        public override PersistenceData GetData()
        {
            var data = new PersistenceData();

            data.Set("event", GetType().FullName);
            data.Set("mapName", mapName);
            data.Set("x", x);
            data.Set("y", y);

            return data;
        }

        public override void SetData(PersistenceData data)
        {
            mapName = data.Get("mapName", mapName);
            x = data.Get("x", x);
            y = data.Get("y", y);
        }

        public override string GetEventType()
        {
            return "changeMap";
        }
    }
}
