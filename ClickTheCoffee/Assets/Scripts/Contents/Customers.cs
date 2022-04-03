using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customers
{
    public List<Person> _person = new List<Person>();
    public Transform Root;

    public void Init()
    {
        GameObject go = GameObject.Find("UI_Info");
        if (null == go)
        {
            Debug.Log("UI.Info를 찾지 못했습니다.");
            return;
        }

        // 손님을 넣어줄 루트를 만든다.
        GameObject _root = new GameObject { name = "@Customers" };
        _root.transform.SetParent(go.transform);
        // 손님을 이 루트 안에 다 넣어둔다.
        Root = _root.transform;
    }

    // 내가 만든 그 음료를 매게변수에 넣어줌
    public void CheckMenu(string menuName)
    {
        foreach (Person item in _person)
        {
            // 음료가 만약 맞는 경우라면
            if (item._drinkInfo.engName == menuName)
            {
                Managers.Data.Playerdata.money += item._drinkInfo.cost;
                Managers.Data.Playerdata.customers++;

                // 맞으면 지워야지
                item._Position.isUsing = false;     // 자리 비우기
                Managers.Resource.Destroy(item.gameObject);
                _person.Remove(item);

                // 계산 소리
                Managers.Sound.Play("Effect/Sell");
                break;
            }
        }
    }
}
