using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FlyModel.UI.Panel.LoadingPanel
{
    /// <summary>
    /// 预加载资源
    /// </summary>
     public class LoadingQueue
    {
        private Stack<LoadingInfo> queue = new Stack<LoadingInfo>();

        //ResourceLoader中缓存了资源，所以这里不要缓存了
        //private Dictionary<string, GameObject> results = new Dictionary<string, GameObject>();

        private int totalInfos;
        private int completeNums;

        private Action completeCallbcak;

        public LoadingQueue()
        {

        }

        public void AddLoadingInfo(LoadingInfo info)
        {
            if (queue.Contains(info))
            {
                Debug.Log(string.Format("{0} 已在队列中，不重复加载", info.AssetName));
            }
            else
            {
                queue.Push(info);
            }
        }

        public void StartLoad()
        {
            totalInfos = queue.Count;
            completeNums = 0;

            if (totalInfos > 0)
            {
                LoadingInfo temp = queue.Pop();
                loadOne(temp);
            }
        }

        public void RegisterCompleteCallback(Action complete)
        {
            completeCallbcak = complete;
        }

        private void loadOne(LoadingInfo info)
        {
            //ResourceLoader.Instance.TryLoadAssetBundle(info.AssetName.ToLower(), info.AssetName, (go) =>
            //{
            //    completeNums++;
            //    //results.Add(info.AssetName, go);
            //    if (info.LoadOverCallback != null)
            //    {
            //        info.LoadOverCallback(go);
            //    }

            //    if (completeNums >= totalInfos && completeCallbcak != null)
            //    {
            //        completeCallbcak();
            //    }

            //    if (queue.Count > 0)
            //    {
            //        loadOne(queue.Pop());
            //    }
            //}, typeof(GameObject));

            ResourceLoader.Instance.TryLoadAssetBundle(info.AssetName.ToLower(), info.AssetName, (go) =>
            {
                completeNums++;
                //results.Add(info.AssetName, go);
                if (info.LoadOverCallback != null)
                {
                    info.LoadOverCallback(go);
                }

                if (completeNums >= totalInfos && completeCallbcak != null)
                {
                    completeCallbcak();
                }

                if (queue.Count > 0)
                {
                    loadOne(queue.Pop());
                }
            }, info.AssetType);

            //ResourceLoader.Instance.TryLoadClone(info.AssetName.ToLower(), info.AssetName, (go) =>
            //{
            //    completeNums++;
            //    //results.Add(info.AssetName, go);
            //    if (info.LoadOverCallback != null)
            //    {
            //        info.LoadOverCallback(go as GameObject);
            //    }

            //    if (completeNums >= totalInfos && completeCallbcak != null)
            //    {
            //        completeCallbcak();
            //    }

            //    if (queue.Count > 0)
            //    {
            //        loadOne(queue.Pop());
            //    }
            //});
        }

        public int GetLength()
        {
            return queue.Count;
        }
    }
}
