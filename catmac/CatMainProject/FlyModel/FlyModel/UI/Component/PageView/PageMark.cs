using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using FlyModel.Control;

public class PageMark
{
    public PageView scrollPage;
    public ToggleGroup toggleGroup;
    public GameObject togglePrefab;

    public List<Toggle> toggleList = new List<Toggle>();

    public GameObject GameObject;

    public void Init(PageView pageView, GameObject pageMarkPrefab)
    {
        scrollPage = pageView;
        scrollPage.OnPageChanged = OnScrollPageChanged;
        scrollPage.InitCallback = initCallback;

        togglePrefab = pageMarkPrefab;
    }

    public void OnScrollPageChanged(int currentPageIndex)
    {
        if (currentPageIndex >= 0)
        {
            toggleList[currentPageIndex].isOn = true;
        }
    }

    private void initCallback(int pagesCount)
    {
        if (pagesCount != toggleList.Count)
        {
            if (pagesCount > toggleList.Count)
            {
                //int cc = pagesCount - toggleList.Count;
                int startIndex = toggleList.Count;
                for (int i = startIndex; i < pagesCount; i++)
                {
                    toggleList.Add(createToggle(i));
                }
            }
            else if (pagesCount < toggleList.Count)
            {
                while (toggleList.Count > pagesCount)
                {
                    Toggle t = toggleList[toggleList.Count - 1];
                    toggleList.Remove(t);
                    GameObject.DestroyImmediate(t.gameObject);
                }
            }
        }
    }

    private Toggle createToggle(int index)
    {
        GameObject Instance = GameObject.Instantiate(togglePrefab);
        Instance.SetActive(true);
        Instance.transform.SetParent(GameObject.transform, false);
        Instance.transform.Find("Index").gameObject.GetComponent<Text>().text = (index+1).ToString();

        Toggle t = Instance.GetComponent<Toggle>();
        t.group = GameObject.transform.GetComponent<ToggleGroup>();
        return t;
    }

    public int GetCurrentPageIndex()
    {
        for (int i = 0; i < toggleList.Count; i++)
        {
            if (toggleList[i].isOn)
            {
                return i;
            }
        }

        return 0;
    }

    public void SelectedPage(int pageIndex)
    {
        for (int i = 0; i < toggleList.Count; i++)
        {
            toggleList[i].isOn = i == pageIndex;
        }
    }
}
