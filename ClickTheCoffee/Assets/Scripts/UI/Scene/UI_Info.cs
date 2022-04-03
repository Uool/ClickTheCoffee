using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Info : UI_Scene
{
    enum Texts
    {
        CoinText,
        CustomerText,
        //DayText,
    }

    enum Buttons
    {
        OptionButton,
    }

    Data.PlayerStat _player;

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        _player = Managers.Data.Playerdata;

        GetText((int)Texts.CoinText).text = $"{_player.money}";
        GetText((int)Texts.CustomerText).text = $"{_player.customers}";
        //GetText((int)Texts.DayText).text = $"Day {_player.day}";

        BindEvent(GetButton((int)Buttons.OptionButton).gameObject, (PointerEventData data) => { Managers.UI.ShowPopupUI<UI_Option>(); Time.timeScale = 0f; });
    }

    // Update is called once per frame
    void Update()
    {
        GetText((int)Texts.CoinText).text = $"{_player.money}";
        GetText((int)Texts.CustomerText).text = $"{_player.customers}";
        //GetText((int)Texts.DayText).text = $"Day {_player.day}";
    }
}
