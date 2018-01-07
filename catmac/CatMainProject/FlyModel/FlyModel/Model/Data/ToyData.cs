using FlyModel.Proto;
using FlyModel.UI;
using FlyModel.UI.Scene.ViewObject.Data;
using System.Collections.Generic;

namespace FlyModel.Model.Data
{
    public class ToyData:BaseProp
    {
        public long RoomID;
        public List<SpreadData> PlayPointList;
        public ScenePointStruct ScenePointIndex;//从1开始
        public bool IsSmallType;

        public ToyData()
        {
            ScenePointIndex = new ScenePointStruct();
        }

        public void updateData(FurniData data)
        {
            ID = data.Id;
            Type = data.Type;
            Count = data.Count;
            Name = string.Format("ToyRoot_Toy{0}", Type);

            RoomID = data.RoomID;
            PlayPointList = data.Speaks;

            ScenePointIndex.ScenePointIndex = data.ScenePointIndex + 1;
            ScenePointIndex.PointType = data.SubPointIndex <= 0 ? EnumConfig.SubPointType.large : EnumConfig.SubPointType.small;
            ScenePointIndex.SubPointIndex = data.SubPointIndex;
            ScenePointIndex.Distribution = (EnumConfig.InteractivePointDistibution)((int)(data.RoomSectionType));
        }
    }
}
