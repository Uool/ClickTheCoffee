using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_Instance;
    static Managers Instance { get { Init(); return s_Instance; } }

    // Managers
    DataManager _data = new DataManager();
    PoolManager _pool = new PoolManager();
    ResourceManager _resource = new ResourceManager();
    SceneManagerEx _scene = new SceneManagerEx();
    SoundManager _sound = new SoundManager();
    UIManager _ui = new UIManager();

    public static DataManager Data { get { return Instance._data; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static UIManager UI { get { return Instance._ui; } }


    protected void Start()
    {
        Init();
    }

    static void Init()
    {
        if (null == s_Instance)
        {
            GameObject go = GameObject.Find("@Managers");
            if (null == go)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }
            DontDestroyOnLoad(go);
            s_Instance = go.GetComponent<Managers>();

            s_Instance._data.Init();
            s_Instance._pool.Init();
            s_Instance._sound.Init();

            // Frame Control
            Application.targetFrameRate = 60;
        }
    }

    public static void Clear()
    {
        Scene.Clear();
        Sound.Clear();
        UI.Clear();

        Pool.Clear();
    }
}
