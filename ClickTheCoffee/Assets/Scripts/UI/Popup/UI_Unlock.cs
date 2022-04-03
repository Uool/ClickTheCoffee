using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Unlock : UI_Popup
{
    enum Buttons
    {
        OKButton,
        CancelButton,
    }
    enum Texts
    {
        UnlockStuffText,
        UnlockCostText,
        StateText,
    }
    enum Images
    {
        UnlockImage,
    }

    string _stuffName;
    int _cost;
    Sprite _imageSprite;
    Stuff _currentStuff;

    private void Awake()
    {
        Init();
    }
    void Start()
    {
        
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));
    }

    public void DefaultSetting(Stuff stuff)
    {
        _currentStuff = stuff;
        _stuffName = stuff.stuffName;
        _cost = stuff.cost;
        _imageSprite = Managers.Resource.Load<Sprite>($"Art/ButtonIcon/{stuff.stuffEngName}");
        if (null == _imageSprite)
            Debug.Log($"Image load failed : {stuff.stuffEngName}");

        BindEvent(GetButton((int)Buttons.OKButton).gameObject, (PointerEventData data) => { ClickOkButton(); });
        BindEvent(GetButton((int)Buttons.CancelButton).gameObject, (PointerEventData data) => { ClickCancelButton(); });

        // Stuff Setting
        GetText((int)Texts.UnlockStuffText).text = $"새로운 재료 : {_stuffName}";
        GetText((int)Texts.UnlockCostText).text = $"구매를 위한 요금 : {_cost}";
        GetImage((int)Images.UnlockImage).sprite = _imageSprite;

    }

    void ClickOkButton()
    {
        // 돈이 있으면 풀어주고 없으면 안됨.
        if (Managers.Data.Playerdata.money >= _cost)
        {
            Managers.Data.Playerdata.money -= _cost;
            GetText((int)Texts.StateText).text = "구매 완료!";
            GetButton((int)Buttons.OKButton).interactable = false;
            // 해금
            _currentStuff.isLocked = false;
            _currentStuff.IconSetting();

            // 플레이어 데이터에 저장
            Managers.Data.Playerdata.unlockStuffList.Add(_currentStuff.stuffEngName);
            // 손님들의 주문가능 List 갱신
            Managers.Data.RenewRecipe();
        }
        else
            GetText((int)Texts.StateText).text = "돈이 없습니다..";
        
    }

    void ClickCancelButton()
    {
        ClosePopupUI();
    }

}