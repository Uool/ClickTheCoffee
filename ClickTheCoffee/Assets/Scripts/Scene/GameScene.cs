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

        // Play Bgm
        Managers.Sound.Play("Bgm/Sunny Days - Anno Domini Beats", Define.Sound.Bgm);
    }

    public override void Clear()
    {
        
    }
}
