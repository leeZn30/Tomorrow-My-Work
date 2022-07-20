using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_Stage2 : TaskManager
{
    public override void OnStoredChanged()
    {
        List<ItemBase> items = StageInformation.instance.items;

        string[] names;
        bool flag;

        flag = true;
        names = new string[]
        {
            StageInformation.instance.prefabLists[12].name,
            StageInformation.instance.prefabLists[21].name,
            StageInformation.instance.prefabLists[32].name,
        };
        foreach (string n in names)
        {
            flag = flag && items.Find(p => p.name == n) == null;
            if (!flag) break;
        }

        GameManager.Instance.TaskComplete(1, flag);
        GameManager.Instance.TaskComplete(2, items.Find(
            p => p.name == StageInformation.instance.prefabLists[1].name
            ) != null);

        flag = true;
        names = new string[]
        {
            StageInformation.instance.prefabLists[11].name,
            StageInformation.instance.prefabLists[0].name,
            StageInformation.instance.prefabLists[3].name,
        };
        foreach (string n in names)
        {
            flag = flag && items.Find(p => p.name == n) != null;
            if (!flag) break;
        }

        GameManager.Instance.TaskComplete(3, flag);
        GameManager.Instance.TaskComplete(4, StageInformation.instance.IsStored(items.Find(
            p => p.name == StageInformation.instance.prefabLists[22].name
            )));
        GameManager.Instance.TaskComplete(5, items.Find(
            p => p.name == StageInformation.instance.prefabLists[7].name
            ) == null);
    }
}
