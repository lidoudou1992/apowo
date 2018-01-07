using System;

namespace FlyModel.Control
{
    public class UIDataListener:BehaviourBase
    {
 
        public Action OnUpdate;
        public override void Update()
        {
            base.Update();
            if(OnUpdate!=null)
            {
                OnUpdate();
            }
        }

        public Action OnDrawGUI;
        public override void DrawGUI()
        {
            base.DrawGUI();
            if(OnDrawGUI!=null)
            {
                OnDrawGUI();
            }
        }
    }
}
