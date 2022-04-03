using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class UI_Base : MonoBehaviour
{
    // 유니티와 관련된 모든 컴포넌트 관련에는 UnityEngine.Object 라는 것으로 저장할 수 있음 (최상위 부모)
    // 현재 타입(뭐가 들어갈지 모르니까 Type)별로 저장, 그 타입의 컴포넌트 (최상위부모)
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    // JsonData
    protected Dictionary<string, Data.Stuff> stuffData = new Dictionary<string, Data.Stuff>();

    public abstract void Init();

    // Define에 있는 UI 유형으로 Type구별
    protected void Bind<T> (Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);

        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        // 컴포넌트의 타입, 그 타입 들고있는 놈들
        _objects.Add(typeof(T), objects);

        // 반복문 돌면서 맵핑 (인스펙터에 거 드래그해줘서 넣는거?)
        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, names[i], true);
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);

            if (null == objects[i])
                Debug.Log($"Failed to Bind : {names[i]}");
        }
    }

    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }

    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
    protected Text GetText(int idx) { return Get<Text>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }

    public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

        switch (type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
        }
    }
}
