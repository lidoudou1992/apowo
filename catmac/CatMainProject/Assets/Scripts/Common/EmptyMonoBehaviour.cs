using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Common
{

    public class EmptyMonoBehaviour:MonoBehaviour
    {
        public Action UpdateHandler;

        void Update()
        {
            if (UpdateHandler != null)
            {
                UpdateHandler();
            }
        }
    }
}
