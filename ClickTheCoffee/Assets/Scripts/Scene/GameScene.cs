using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Game;
        // UI
        Managers.UI.ShowPopupUI<UI_Menu>();
        Managers.UI.ShowSceneUI<UI_Info>();

        // UI만들어지고 세팅 된 후, 매니저 세팅
        Managers.Customers.Init();

        // 손님들 오는거 조절하기
        StartCoroutine(CoCreateCustomer());
        // 배경음악 틀기
        Managers.Sound.Play("Bgm/Meeka - Steve Adams", Define.Sound.Bgm);
    }

    IEnumerator CoCreateCustomer()
    {
        while(true)
        {
            float timer = 10.3f - (0.3f * Managers.Data.Playerdata.popLevel);

            if (Managers.Customers._person.Count < 5)
            {
                Managers.Resource.Instantiate("Customer", Managers.Customers.Root);
                Managers.Sound.Play("Effect/ring");
            }

            yield return new WaitForSeconds(timer);
        }
        
    }

    public override void Clear()
    {
        StopCoroutine(CoCreateCustomer());
    }
}
