using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class UI_Base : MonoBehaviour
{
    // ����Ƽ�� ���õ� ��� ������Ʈ ���ÿ��� UnityEngine.Object ��� ������ ������ �� ���� (�ֻ��� �θ�)
    // ���� Ÿ��(���� ���� �𸣴ϱ� Type)���� ����, �� Ÿ���� ������Ʈ (�ֻ����θ�)
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    // JsonData
    protected Dictionary<string, Data.Stuff> stuffData = new Dictionary<string, Data.Stuff>();

    public abstract void Init();

    // Define�� �ִ� UI �������� Type����
    protected void Bind<T> (Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);

        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        // ������Ʈ�� Ÿ��, �� Ÿ�� ����ִ� ���
        _objects.Add(typeof(T), objects);

        // �ݺ��� ���鼭 ���� (�ν����Ϳ� �� �巡�����༭ �ִ°�?)
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
