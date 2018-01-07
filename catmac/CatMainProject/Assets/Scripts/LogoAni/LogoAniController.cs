using UnityEngine;
using System.Collections;
using System;

public class LogoAniController : MonoBehaviour {

    private float time = 5f;
    private float t;
    private bool lockUpdate;
    private Action callback;

	// Use this for initialization
	void Start () {
        //string SpineName = "catani005";
        //ResourceLoader.Instance.TryLoadClone(SpineName.ToLower(), SpineName, (cat) =>
        //{
        //    Debug.Log(transform);
        //    cat.transform.SetParent(transform, false);

        //    SkeletonGraphic sg = cat.transform.GetComponent<SkeletonGraphic>();
        //    sg.AnimationState.SetAnimation(0, "action0001", true);
        //});
    }

    public void SetOnTimeCallback(Action callback)
    {
        this.callback = callback;
    }
	
	// Update is called once per frame
	void Update () {
        if (lockUpdate==false)
        {
            t = t + Time.deltaTime;
            if (t>time)
            {
                lockUpdate = true;

                if (callback != null)
                {
                    callback();
                }
            }
        }
	}
}
