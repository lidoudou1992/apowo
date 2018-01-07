using FlyModel.Model.Data;
using FlyModel.UI;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.Model
{
    public class RoomManager
    {
        public static RoomManager Instance;

        public static RoomManager Initialize()
        {
            if (Instance == null)
            {
                Instance = new RoomManager();
            }
            return Instance;
        }

        public List<Model.Data.RoomData> RoomsList = new List<Data.RoomData>();

        public void UpdateRooms(List<Proto.RoomData> rooms)
        {
            foreach (var room in rooms)
            {
                UpdateRoom(room);
            }
        }

        public void UpdateRoom(Proto.RoomData data)
        {
            foreach (var roomData in RoomsList)
            {
                if (roomData.Type == data.Type)
                {
                    roomData.UpdateData(data);
                    return;
                }
            }

        }

        public void InitConfigs()
        {
            ResourceLoader.Instance.TryLoadTextAsset(ResPathConfig.Rooms_CONFIG, (textAssert) => {
                string text = ((TextAsset)textAssert).text;
                JsonData rooms = JsonMapper.ToObject(text);

                Model.Data.RoomData tempData;
                foreach (var room in rooms)
                {
                    tempData = new RoomData();
                    tempData.UpdateData(room as JsonData);
                    RoomsList.Add(tempData);
                }
            });
        }

        public bool HasRoom(int type)
        {
            foreach (var room in RoomsList)
            {
                if (room.Type == type)
                {
                    return room.own;

                }
            }

            return false;
        }

        public int GetScenePrice()
        {
            int ownCount = 0;

            foreach (var room in RoomsList)
            {
                if (room.own)
                {
                    ownCount++;
                }
            }

            if (ownCount == 1)
            {
                return 100;
            }else if (ownCount == 2)
            {
                return 200;
            }

            return 200;
        }

        public bool isFirstBuyTime()
        {
            int t = 0;
            foreach (var room in RoomsList)
            {
                if (room.own)
                {
                    t++;
                }
            }

            return t <= 1;
        }
    }
}
