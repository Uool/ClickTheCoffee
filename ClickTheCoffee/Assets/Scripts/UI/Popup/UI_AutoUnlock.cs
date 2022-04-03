using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_AutoUnlock : UI_Popup
{
    enum Buttons
    {
        OkButton,
        CloseButton,
    }
    enum Texts
    {
        DrinkNameText,
        UnlockText,
        CostText
    }
    enum Images
    {
        DrinkImage,
    }

    public Data.Recipe recipe { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Binding();
        DefaultSetting();
    }

    void Binding()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));

        BindEvent(GetButton((int)Buttons.OkButton).gameObject ,(PointerEventData data) => { ChoiceButton(Buttons.OkButton); });
        BindEvent(GetButton((int)Buttons.CloseButton).gameObject, (PointerEventData data) => { ChoiceButton(Buttons.CloseButton); });
    }

    void DefaultSetting()
    {
        if (null == recipe)
        {
            Debug.Log($"{gameObject.name}의 recipe가 없습니다.");
            return;
        }

        GetText((int)Texts.DrinkNameText).text = recipe.korName;
        GetText((int)Texts.CostText).text = $"가격 : {recipe.unlockCost}원";
        GetImage((int)Images.DrinkImage).sprite = Managers.Resource.Load<Sprite>($"Art/Cafe_Img/{recipe.engName}");
    }

    void ChoiceButton(Buttons button)
    {
        switch (button)
        {
            case Buttons.OkButton:
                if (recipe.unlockCost > Managers.Data.Playerdata.money)
                    GetText((int)Texts.UnlockText).text = "돈이 모자랍니다.";
                else
                {
                    GetText((int)Texts.UnlockText).text = "구매하였습니다.";
                    Managers.Data.Playerdata.money -= recipe.unlockCost;
                    Managers.Data.Playerdata.buyingDrinkList.Add(recipe.engName);
                }
                GetButton((int)Buttons.OkButton).interactable = false;
                break;
            case Buttons.CloseButton:
                ClosePopupUI();
                break;
            default:
                break;
        }
    }
}
