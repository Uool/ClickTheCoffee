using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrinkSlot : MonoBehaviour
{
    public Image drinkImage;
    public Text drinkName;
    public GameObject lockBackground;
    public Button button;

    public UI_Kitchen uI_Kitchen { get; set; }
    Data.Recipe recipe;

    // 해금여부 (단 1번이라도 3개를 만들었는가?)
    public bool isLocked = true;
    // 구매여부
    public bool isBuying;


    private void OnEnable()
    {
        CheckUnlock();
    }

    void Start()
    {
        DefaultSetting();
    }

    void DefaultSetting()
    {
        drinkImage.sprite = Managers.Resource.Load<Sprite>($"Art/Cafe_Img/{gameObject.name}");
        drinkName.gameObject.SetActive(false);
        // 플레이어의 데이터를 불러온다.
        CheckUnlock();

        button.onClick.AddListener(CheckAutoDrink);

        // Find Recipe
        foreach (var KeyValuePair in Managers.Data.RecipeDict)
        {
            if (KeyValuePair.Value.engName == gameObject.name)
            {
                recipe = KeyValuePair.Value;
                break;
            }
        }
    }

    void CheckAutoDrink()
    {
        CheckUnlock();
        // 구매 된 것이라면?
        if (isBuying)
        {
            AutoMakingDrink();
            Managers.UI.ClosePopupUI();
        }
        else
        {
            UI_AutoUnlock uI_AutoUnlock = Managers.UI.ShowPopupUI<UI_AutoUnlock>();
            uI_AutoUnlock.recipe = recipe;
        }
    }

    void CheckUnlock()
    {
        // 플레이어로부터 정보를 받아온다 (해금은 됐는지? 구매를 했는지?)
        foreach (string drink in Managers.Data.Playerdata.unlockDrinkList)
        {
            if (gameObject.name == drink)
            {
                isLocked = false;
                drinkName.gameObject.SetActive(!isLocked);
                lockBackground.SetActive(isLocked);
            }
        }
        foreach (string drink in Managers.Data.Playerdata.buyingDrinkList)
        {
            if (gameObject.name == drink)
                isBuying = true;
        }         
    }

    void AutoMakingDrink()
    {
        for (int i = 0; i < recipe.engStuffList.Count; i++)
        {
            string stuffEngName = recipe.engStuffList[i];
            uI_Kitchen.currentStuff = uI_Kitchen.GetStuff(stuffEngName);

            int clickCount = (int)recipe.amountList[0] * 10;

            for (int j = 0; j < clickCount; j++)
            {
                uI_Kitchen.ClickFillButton();
                // 보험
                if (j > 10)
                    return;
            }
        }
    }
}
