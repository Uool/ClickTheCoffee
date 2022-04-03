using UnityEngine;

public class ResourceManager
{
    public T Load<T> (string path) where T : Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');
            if (index >= 0)
                name = name.Substring(index + 1);

            GameObject go = Managers.Pool.GetOriginal(name);
            if (null != go)
                return go as T;
        }

        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (null == original)
        {
            Debug.Log($"Failed to Load Prefab : {path}");
            return null;
        }

        if (null != original.GetComponent<Poolable>())
            return Managers.Pool.Pop(original, parent).gameObject;

        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;

        return go;
    }

    public void Destroy(GameObject go, float time = 0f)
    {
        if (null == go)
            return;

        Poolable poolable = go.GetComponent<Poolable>();
        if (null != poolable)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go, time);
    }
}
