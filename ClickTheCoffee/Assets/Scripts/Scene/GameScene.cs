using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Game;

        Managers.UI.ShowSceneUI<UI_Counter>();

        // 배경음악 틀기
        //Managers.Sound.Play("Bgm/Meeka - Steve Adams", Define.Sound.Bgm);
    }

    public override void Clear()
    {
        
    }
}
