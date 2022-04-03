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
        canvas.overrideSorting = true;      // ĵ���� �ȿ� �ٸ� ĵ������ �ִ��� ���� sort order�� �������ڴ�.

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        // �˾�â�� �ƴѰ͵�(�⺻������ ����� UI)
        else
        {
            canvas.sortingOrder = 0;
        }

    }

    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        // �̸��� ���� ���, T�� �̸��� �޾ƿ´�
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");
        // ui�� Setparent�� ���� �θ� ������ �ؾ� �Ѵ�.
        if (null != parent)
            go.transform.SetParent(parent);

        return go.GetComponent<T>();
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        // �̸��� ���� ���, T�� �̸��� �޾ƿ��ڴ�
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        // �����鿡 ����Ǿ��ִ� �� ����
        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI = go.GetOrAddComponent<T>();
        _sceneUI = sceneUI;

        go.transform.SetParent(Root.transform);

        return sceneUI;
    }

    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        // �̸��� ���� ���, T�� �̸��� �޾ƿ��ڴ�
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        // �����鿡 ����Ǿ��ִ� �� ����
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

        // �ŰԺ����� �� ���� ���� �������� �ִ� ���ΰ���?
        if (popup != _popupStack.Peek())
        {
            Debug.Log("Close Popup Failed");
            return;
        }

        ClosePopupUI();
    }

    // ���� �������� ������ ���� (���� ���� ��)
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
