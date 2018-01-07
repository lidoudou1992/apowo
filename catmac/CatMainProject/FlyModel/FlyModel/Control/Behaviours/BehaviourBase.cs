using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.Control
{
    public abstract class BehaviourBase:DisposeObject
    {
        static long maxID=1000;
    
        public long BehaviourID
        {
            get;
            private set;
        }

        public GameObject GameObject
        {
            get;
            set;
        }

        Transform _transform;

        public Transform Transform
        {
            get 
            { 
                if(_transform==null&&GameObject!=null)
                {
                    _transform = GameObject.transform;
                }
                return _transform; 
            }
          
        }

        public BehaviourBase()
        {
            BehaviourID = ++maxID;
        }

        public virtual void Awake()
        {

        }

        public virtual void Start()
        {

        }

        public virtual void Update()
        {
            //if(Transform==null)
            //{
            //    Dispose();
            //}
        }

        public virtual void DrawGizmos()
        {

        }

        public virtual void LateUpdate()
        {

        }

        public virtual void OnEnable()
        {

        }

        public virtual void OnDisable()
        {

        }

        public virtual void DrawGUI()
        {

        }

        public bool Disposed
        {
            get;
            set;
        }

        public override void Dispose()
        {
            Disposed = true;
            //BehaviourManager.RemoveBehaviour(this);
            _transform = null;
            GameObject = null;
            
        }
    }
}
