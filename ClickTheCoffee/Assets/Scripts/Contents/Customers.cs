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
            Debug.Log("UI.Info�� ã�� ���߽��ϴ�.");
            return;
        }

        // �մ��� �־��� ��Ʈ�� �����.
        GameObject _root = new GameObject { name = "@Customers" };
        _root.transform.SetParent(go.transform);
        // �մ��� �� ��Ʈ �ȿ� �� �־�д�.
        Root = _root.transform;
    }

    // ���� ���� �� ���Ḧ �ŰԺ����� �־���
    public void CheckMenu(string menuName)
    {
        foreach (Person item in _person)
        {
            // ���ᰡ ���� �´� �����
            if (item._drinkInfo.engName == menuName)
            {
                Managers.Data.Playerdata.money += item._drinkInfo.cost;
                Managers.Data.Playerdata.customers++;

                // ������ ��������
                item._Position.isUsing = false;     // �ڸ� ����
                Managers.Resource.Destroy(item.gameObject);
                _person.Remove(item);

                // ��� �Ҹ�
                Managers.Sound.Play("Effect/Sell");
                break;
            }
        }
    }
}
