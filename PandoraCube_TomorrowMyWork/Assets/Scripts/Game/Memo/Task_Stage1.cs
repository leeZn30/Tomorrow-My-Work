using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_Stage1 : TaskManager
{
    public override void OnStoredChanged()
    {
        List<ItemBase> items = StageInformation.instance.items;

        GameManager.Instance.TaskComplete(1, items.Find(
            p => p.name == StageInformation.instance.prefabLists[10].name
            ) != null);
        GameManager.Instance.TaskComplete(2, items.Find(
            p => p.name == StageInformation.instance.prefabLists[15].name
            ) != null);
        
        string[] names = new string[]
        {
            StageInformation.instance.prefabLists[0].name,
            StageInformation.instance.prefabLists[1].name,
            StageInformation.instance.prefabLists[2].name,
            StageInformation.instance.prefabLists[6].name,
            StageInformation.instance.prefabLists[13].name
        };
        bool flag = true;

        foreach (string n in names)
        {
            flag = flag && StageInformation.instance.IsStored(items.Find(p => p.name == n));
            if (!flag) break;
        }

        GameManager.Instance.TaskComplete(3, flag);
        GameManager.Instance.TaskComplete(4, StageInformation.instance.IsStored(items.Find(
            p => p.name == StageInformation.instance.prefabLists[5].name
            )));
    }
}
