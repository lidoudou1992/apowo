using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class PageView : MonoBehaviour
{
    public Transform itemPanel;
    public GameObject itemPrefab;

    private int _maxPage;
    private int _nowPage;
    private List<PageItem> _newItemList;
    private List<PageItem> _oldItemList;
    private bool _isPlaying = false;    //标记是否正在播放缓动动画

    void Start()
    {
        _maxPage = 5;
        _nowPage = 1;
        _newItemList = new List<PageItem>();
        CreateItemGrid(0);
    }

    private void CreateItemGrid(int offset)
    {
        _newItemList.Clear();
        int index = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                GameObject go = GameTools.AddChild(itemPanel, itemPrefab);
                go.name = "item " + (index < 10 ? "0" + index : index.ToString());
                go.transform.localPosition = new Vector3(j * 100 - 100 + offset, 100 - i * 100, 0);
                _newItemList.Add(go.GetComponent<PageItem>());
                index++;
            }
        }
    }

    public void OnPageButtonCLick(string name)
    {
        if (_isPlaying) return;


        int offset = 0;
        switch (name)
        {
            case "Prev":
                if (_nowPage == 1)
                {
                    Debug.LogError("已经是第一页了，没有上一页");
                    return;
                }
                Debug.LogError("上一页");
                _nowPage--;
                offset = -300;
                break;
            case "Next":
                if (_nowPage == _maxPage)
                {
                    Debug.LogError("已经是最后页了，没有下一页");
                    return;
                }
                Debug.LogError("下一页");
                _nowPage++;
                offset = 300;
                break;
        }
        _isPlaying = true;
        _oldItemList = new List<PageItem>(_newItemList);
        CreateItemGrid(offset);
        int num = offset / 300;
        for (int i = _oldItemList.Count - 1; i >= 0; i--)
        {
            GameObject item = _oldItemList[i].gameObject;
            Tweener tween = item.transform.DOLocalMoveX(-offset, 0.5f);
            tween.SetRelative();
            tween.SetEase(Ease.InOutCubic);

            float delay = (i % 3) * num + (1 - num) * 2f;
            //乘以0.02f就是减少每一排的切换间隔
            tween.SetDelay(delay * 0.02f);
        }
        for (int i = _newItemList.Count - 1; i >= 0; i--)
        {
            GameObject item = _newItemList[i].gameObject;
            Tweener tween = item.transform.DOLocalMoveX(-offset, 0.5f);
            tween.SetRelative();
            tween.SetEase(Ease.InOutCubic);

            tween.OnComplete(OnTweenComplete);

            float delay = (i % 3) * num + (1 - num) * 2f;
            tween.SetDelay((delay + 5) * 0.02f);
        }
    }

    private int index = 0;
    private void OnTweenComplete()
    {
        index++;
        //当index累计到9，也就是所有缓动完成后，证明当次背包切换完成
        if (index == 9)
        {
            while (_oldItemList.Count > 0)
            {
                GameTools.Destroy(_oldItemList[0].gameObject);
                _oldItemList.RemoveAt(0);
            }
            _oldItemList.Clear();
            index = 0;
            _isPlaying = false;
        }
    }
}
