using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_MakeList : UI_Popup
{
    enum Buttons
    {
        ExitButton,
    }

    public Transform slotParent;
    public UI_Kitchen uI_Kitchen { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));

        BindEvent(GetButton((int)Buttons.ExitButton).gameObject, (PointerEventData data) => { ClickCloseButton(); });

        // 현재 음료의 총 개수만큼 오브잭트를 생산해야됨
        foreach (var pair in Managers.Data.RecipeDict)
        {
            Data.Recipe recipe = pair.Value;

            GameObject go = Managers.Resource.Instantiate("UI/DrinkSlot", slotParent);
            go.name = recipe.engName;
            go.GetComponent<DrinkSlot>().drinkName.text = recipe.korName;
            go.GetComponent<DrinkSlot>().uI_Kitchen = uI_Kitchen;
        }
    }

    void ClickCloseButton()
    {
        gameObject.SetActive(false);
    }
}
