using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Start;
        Managers.UI.ShowSceneUI<UI_Start>();
    }

    public override void Clear()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
