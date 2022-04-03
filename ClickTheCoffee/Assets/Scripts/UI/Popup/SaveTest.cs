using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTest : MonoBehaviour
{
    public void Save()
    {
        Managers.Data.SaveJson<Data.PlayerStat>(Managers.Data.Playerdata);
    }
}
