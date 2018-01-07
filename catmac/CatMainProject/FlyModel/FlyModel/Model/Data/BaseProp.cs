using FlyModel.Proto;

namespace FlyModel.Model.Data
{
    public class BaseProp:IBaseProp
    {
        public long ID = -1;
        public string Name = "";
        public long Type;
        public int Count;
        public string Description;
        public string PicCode;
        public Currency CurrencyType;
        public int Price;
        public string AppendExplaination = "";

        public BaseProp() { }
    }
}
