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

    // �رݿ��� (�� 1���̶� 3���� ������°�?)
    public bool isLocked = true;
    // ���ſ���
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
        // �÷��̾��� �����͸� �ҷ��´�.
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
        // ���� �� ���̶��?
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
        // �÷��̾�κ��� ������ �޾ƿ´� (�ر��� �ƴ���? ���Ÿ� �ߴ���?)
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
                // ����
                if (j > 10)
                    return;
            }
        }
    }
}
