using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class SDKTools
    {
        public TDGAAccount TDAccount;

        public void SetAccount(string id)
        {
            TDAccount = TDGAAccount.SetAccount(id);
        }

        public void SetAccountName(string name)
        {
            TDAccount.SetAccountName(name);
        }
    }
}
