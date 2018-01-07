using System;

namespace FlyModel.Control
{
    public class BehaviourHandle : BehaviourBase
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
