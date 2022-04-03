using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    // sort order
    int _order = 1;
    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    UI_Scene _sceneUI = null;

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (null == root)
                root = new GameObject { name = "@UI_Root" };
            return root;
        }
    }

    public void SetCanvas (GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;      // 캔버스 안에 다른 캔버스가 있더라도 나의 sort order을 가져가겠다.

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        // 팝업창이 아닌것들(기본적으로 깔고가는 UI)
        else
        {
            canvas.sortingOrder = 0;
        }

    }

    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        // 이름이 없는 경우, T의 이름을 받아온다
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");
        // ui라서 Setparent를 통해 부모 설정을 해야 한다.
        if (null != parent)
            go.transform.SetParent(parent);

        return go.GetComponent<T>();
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        // 이름이 없는 경우, T의 이름을 받아오겠다
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        // 프리펩에 저장되어있는 놈 생성
        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI = go.GetOrAddComponent<T>();
        _sceneUI = sceneUI;

        go.transform.SetParent(Root.transform);

        return sceneUI;
    }

    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        // 이름이 없는 경우, T의 이름을 받아오겠다
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        // 프리펩에 저장되어있는 놈 생성
        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");

        T popup = Util.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);

        go.transform.SetParent(Root.transform);

        return popup;
    }

    public void ClosePopupUI(UI_Popup popup)
    {
        if (0 == _popupStack.Count)
            return;

        // 매게변수로 온 놈이 가장 마지막에 있는 놈인가요?
        if (popup != _popupStack.Peek())
        {
            Debug.Log("Close Popup Failed");
            return;
        }

        ClosePopupUI();
    }

    // 가장 마지막에 띄운놈을 삭제 (변수 없는 판)
    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;

        UI_Popup popup = _popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        popup = null;
    }

    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }

    public void Clear()
    {
        CloseAllPopupUI();
        _sceneUI = null;
    }
}
