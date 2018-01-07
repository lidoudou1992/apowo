using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using FlyModel.UI.Component.PageView;
using FlyModel.Control;
using FlyModel.UI;
using FlyModel.Utils;

//public class PageView : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
public class PageView:BehaviourBase //:MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    ScrollRect rect;
    public float SensitivityLevel = 2f;
    private float effectiveValue;
    //滑动速度
    public float smooting = 5;

    List<float> pages = new List<float>();
    public int currentPageIndex = 0;
    private int oldPageIndex = -1;
    public int TempPageIndex = 0;

    //滑动的目标坐标
    float targethorizontal = 0;

    //是否拖拽结束
    bool isDrag = false;

    public Action<int> InitCallback;
    public Action<int> OnPageChanged;

    float startime = 0f;
    //float delay = 0.03f;
    float delay = 0;

    private float oldPosition = 0;

    public bool InitOver = false;

    private bool initedPagsOver;

    public Action<int> OnCreatePageItem;
    public int PageCount;

    public Dictionary<int, PageItem> pageItems = new Dictionary<int, PageItem>();

    public override void Awake()
    {
        base.Awake();

        rect = Transform.GetComponent<ScrollRect>();
        startime = Time.time;

        effectiveValue = 0.005f;
    }

    public override void Update()
    {
        if (Time.time < startime + delay) return;

        if (!isDrag && pages.Count > 0 && initedPagsOver)
        {
            //rect.horizontalNormalizedPosition = Mathf.Lerp(rect.horizontalNormalizedPosition, targethorizontal, Time.deltaTime * smooting);
            //rect.horizontalNormalizedPosition = Mathf.Lerp(rect.horizontalNormalizedPosition, targethorizontal, 0.2f);
            rect.horizontalNormalizedPosition = Mathf.Lerp(rect.horizontalNormalizedPosition, targethorizontal, 0.004f/Time.fixedDeltaTime);

            //if (Math.Abs(rect.horizontalNormalizedPosition - targethorizontal) <= 0.01)
            //{
            //    int count = pageItems.Count;
            //    if (count > 1)
            //    {
            //        for (int i = 0; i < PageCount; i++)
            //        {
            //            if (i != currentPageIndex && pageItems.ContainsKey(i))
            //            {
            //                GameObject.DestroyImmediate(pageItems[i].GameObject);
            //                pageItems.Remove(i);
            //            }
            //        }
            //    }
            //}
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;
        oldPosition = rect.horizontalNormalizedPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (rect.horizontalNormalizedPosition > 0 && rect.horizontalNormalizedPosition < 1)
        {
            if (rect.horizontalNormalizedPosition > oldPosition)
            {
                //向左
                TempPageIndex = oldPageIndex + 1;
                if (pageItems.ContainsKey(TempPageIndex) == false && currentPageIndex < PageCount - 1)
                {
                    OnCreatePageItem(TempPageIndex);
                }
            }
            else if (rect.horizontalNormalizedPosition < oldPosition)
            {
                //向右
                TempPageIndex = oldPageIndex - 1;
                if (pageItems.ContainsKey(TempPageIndex) == false && currentPageIndex > 0)
                {
                    OnCreatePageItem(TempPageIndex);
                }
            }
            else
            {
                //方向不变
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        if (initedPagsOver==false)
        {
            rect.horizontalNormalizedPosition = 0;
        }
        float currentPosX = rect.horizontalNormalizedPosition;
        float offset = Mathf.Abs(pages[Mathf.Max(0, currentPageIndex - 1)] - currentPosX);
        //effectiveValue = (pages[pages.Count - 1] - pages[0]) / pages.Count / 3f;
        
        if (currentPosX > oldPosition)
        {
            //向左
            if (offset > effectiveValue)
            {
                currentPageIndex += 1;
            }
        }
        else if (currentPosX < oldPosition)
        {
            //向右
            if (offset > effectiveValue)
            {
                currentPageIndex -= 1;
            }
        }
        else
        {
            //方向不变
        }

        currentPageIndex = Mathf.Min(pages.Count - 1, Mathf.Max(0, currentPageIndex));

        changePageMark();

        targethorizontal = pages[currentPageIndex];

        SoundUtil.PlaySound(ResPathConfig.CHANGE_PAGE_SCROLLING);
    }

    private void changePageMark()
    {
        if (currentPageIndex != oldPageIndex)
        {
            oldPageIndex = currentPageIndex;
            if (OnPageChanged != null)
            {
                OnPageChanged(currentPageIndex);
            }
        }
    }

    public void InitPages()
    {
        if (InitOver == false)
        {
            if (pages.Count != PageCount)
            {
                if (PageCount != 0)
                {
                    pages.Clear();
                    for (int i = 0; i < PageCount; i++)
                    {
                        float page = 0;
                        if (PageCount != 1)
                            page = i / ((float)(PageCount - 1));
                        pages.Add(page);
                    }
                }

                if (InitCallback != null)
                {
                    InitCallback(PageCount);
                }

                OnEndDrag(null);
                initedPagsOver = true;
            }

            InitOver = true;
        }
    }

    public void CreatePageItem(int pageIndex)
    {
        if (PageCount > 0)
        {
            currentPageIndex = pageIndex;
            TempPageIndex = currentPageIndex;
            if (pageItems.ContainsKey(currentPageIndex) == false)
            {
                OnCreatePageItem(pageIndex);
            }
        }
    }

    public void ToPrevPage()
    {
        if (currentPageIndex>0)
        {
            currentPageIndex = Mathf.Max(0, currentPageIndex - 1);
            TempPageIndex = currentPageIndex;
            if (pageItems.ContainsKey(currentPageIndex) == false)
            {
                OnCreatePageItem(currentPageIndex);
            }
            targethorizontal = pages[currentPageIndex];
            changePageMark();
        }
    }

    public void ToNextPage()
    {
        if (currentPageIndex<PageCount-1)
        {
            currentPageIndex = Mathf.Min(pages.Count-1, currentPageIndex + 1);
            TempPageIndex = currentPageIndex;
            if (pageItems.ContainsKey(currentPageIndex) == false)
            {
                OnCreatePageItem(currentPageIndex);
            }
            targethorizontal = pages[currentPageIndex];
            changePageMark();
        }
    }

    public void JumpToPage(int pageIndex, bool useAnimation=true)
    {
        currentPageIndex = pageIndex;
        TempPageIndex = currentPageIndex;
        if (pageItems.ContainsKey(currentPageIndex) == false)
        {
            OnCreatePageItem(currentPageIndex);
        }
        if (useAnimation==false)
        {
            rect.horizontalNormalizedPosition = pages[currentPageIndex];
        }
        targethorizontal = pages[currentPageIndex];
        changePageMark();
    }

    public void ClearPageItems()
    {
        foreach (var item in pageItems)
        {
            GameObject.DestroyImmediate(item.Value.GameObject);
        }
        pageItems.Clear();
    }

    public PageItem GetPageItem(int index)
    {
        if (pageItems.ContainsKey(index))
        {
            return pageItems[index];
        }
        else
        {
            return null;
        }
    }

    public PageItem GetCurrentPageItem()
    {
        return pageItems[currentPageIndex];
    }
}
