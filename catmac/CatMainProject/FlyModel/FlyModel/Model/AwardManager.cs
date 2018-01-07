using FlyModel.Model.Data;
using FlyModel.Proto;
using System.Collections.Generic;

namespace FlyModel.Model
{
    public class AwardManager
    {
        public static AwardManager Instance;
        public static AwardManager Initialize()
        {
            if (Instance == null)
            {
                Instance = new AwardManager();
            }

            return Instance;
        }

        public List<Data.AwardData> AwardList = new List<Data.AwardData>();

        public void AddAwards(List<Proto.AwardData> datas)
        {
            foreach (var data in datas)
            {
                AddOneAward(data);
            }
        }

        public void AddOneAward(Proto.AwardData data)
        {
            Data.AwardData award = new Data.AwardData();
            award.UpdateData(data);

            AwardList.Add(award);
        }

        public void DeleteOneAward(IDMessage data)
        {
            foreach (var award in AwardList)
            {
                if (award.ID == data.Id)
                {
                    AwardList.Remove(award);
                    break;
                }
            }
        }

        public CurrencyStruct DeleteAwards(IDListMessage datas)
        {
            CurrencyStruct currency = new CurrencyStruct();
            
            // 计算奖励中金银鱼干总数
            foreach (var award in AwardList)
            {
                if (award.AwardType == AwardType.Coin)  // 银鱼干
                {
                    currency.silver += award.Count;
                }
                else if (award.AwardType == AwardType.Dollar)
                {
                    currency.gold += award.Count;
                }
            }

            AwardList.Clear();

            return currency;
        }
    }

    // 金银鱼干
    public class CurrencyStruct
    {
        public int gold;
        public int silver;
    }
}
