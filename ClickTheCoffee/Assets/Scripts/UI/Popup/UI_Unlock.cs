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
        GetText((int)Texts.UnlockStuffText).text = $"���ο� ��� : {_stuffName}";
        GetText((int)Texts.UnlockCostText).text = $"���Ÿ� ���� ��� : {_cost}";
        GetImage((int)Images.UnlockImage).sprite = _imageSprite;

    }

    void ClickOkButton()
    {
        // ���� ������ Ǯ���ְ� ������ �ȵ�.
        if (Managers.Data.Playerdata.money >= _cost)
        {
            Managers.Data.Playerdata.money -= _cost;
            GetText((int)Texts.StateText).text = "���� �Ϸ�!";
            GetButton((int)Buttons.OKButton).interactable = false;
            // �ر�
            _currentStuff.isLocked = false;
            _currentStuff.IconSetting();

            // �÷��̾� �����Ϳ� ����
            Managers.Data.Playerdata.unlockStuffList.Add(_currentStuff.stuffEngName);
            // �մԵ��� �ֹ����� List ����
            Managers.Data.RenewRecipe();
        }
        else
            GetText((int)Texts.StateText).text = "���� �����ϴ�..";
        
    }

    void ClickCancelButton()
    {
        ClosePopupUI();
    }

}