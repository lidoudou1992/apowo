using System;
using System.Collections.Generic;
using UnityEngine;
using CLRSharp;
using FlyModel.Control;

namespace FlyModel
{
    public static class BehaviourManager
    {
        static Dictionary<long, BehaviourBase> behaviours=new Dictionary<long,BehaviourBase>();
        static List<long> toRemoveBehaviours = new List<long>();
        static List<BehaviourBase> toAddBehaviours = new List<BehaviourBase>();
        static int addCount = 0;

        public static void Update()
        {
            //foreach (var b in behaviours.Values)
            //{
            //    b.Update();
            //}

            addCount = toAddBehaviours.Count;
            for (int i = 0; i < addCount; i++)
            {
                try
                {
                    toAddBehaviours[i].Start();
                }
                catch (Exception e)
                {
                    if (ThreadContext.activeContext != null)
                    {
                        Logger.Error.Write(e.Message + ThreadContext.activeContext.Dump());
                    }
                    else
                    {
                        Logger.Error.Write(e.Message + e.StackTrace);
                    }
                }
                behaviours[toAddBehaviours[i].BehaviourID] = toAddBehaviours[i];
                toAddBehaviours.RemoveAt(0);
                i--;
                addCount--;
            }

            foreach (var b in behaviours.Values)
            {
                try
                {
                    if (!b.Disposed)
                    {
                        b.Update();
                    }
                }
                catch (Exception e)
                {
                    if (ThreadContext.activeContext != null)
                    {
                        Logger.Error.Write(e.Message + ThreadContext.activeContext.Dump());
                    }
                    else
                    {
                        Logger.Error.Write(e.Message + e.StackTrace);
                    }
                }
            }

            foreach (var k in toRemoveBehaviours)
            {
                behaviours.Remove(k);
            }

            toRemoveBehaviours.Clear();
        }

        public static void LateUpdate()
        {
            //foreach (var b in behaviours.Values)
            //{
            //    b.LateUpdate();
            //}

            foreach (var b in behaviours.Values)
            {
                try
                {
                    if (!b.Disposed)
                    {
                        b.LateUpdate();
                    }
                }
                catch (Exception e)
                {
                    if (ThreadContext.activeContext != null)
                    {
                        Logger.Error.Write(e.Message + ThreadContext.activeContext.Dump());
                    }
                    else
                    {
                        Logger.Error.Write(e.Message + e.StackTrace);
                    }
                }
            }
        }
        public static void DrawGizmos()
        {
            foreach (var b in behaviours.Values)
            {
                try
                {
                    b.DrawGizmos();
                }
                catch (Exception e)
                {
                    Logger.Error.Write(e.Message + ThreadContext.activeContext.Dump());
                }
            }
        }

        public static void DrawGUI()
        {
            foreach (var b in behaviours.Values)
            {
                try
                {
                    b.DrawGUI();
                }
                catch (Exception e)
                {
                    Logger.Error.Write(e.Message + ThreadContext.activeContext.Dump());
                }
            }

        }

        public static void RemoveBehaviour(BehaviourBase b)
        {
            //behaviours.Remove(b.BehaviourID);
            toRemoveBehaviours.Add(b.BehaviourID);
        }

        //public static T AddGameCompoent<T>(this GameObject go) where T:BehaviourBase,new()
        //{
        //    T b = new T();
        //    b.GameObject = go;
        //    b.Awake();

        //    toAddBehaviours.Add(b);
        //    return b;
        //}

        public static BehaviourBase AddGameComponent(BehaviourBase b)
        {
            b.Awake();
            //behaviours.Add(b.BehaviourID, b);
            toAddBehaviours.Add(b);
            return b;
        }
    }
}
