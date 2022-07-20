using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    [System.Serializable]
    public class InteractData
    {
        public ItemBase target = null;
        public List<ItemBase> result;
    }

    [System.Serializable]
    public class MergeData
    {
        public List<ItemBase> condition;
        public ItemBase result;
    }

    public bool IsContainer = false;

    public float rotation = 0;

    public new string name;
    public string description;

    public List<Vector2Int> shape = new List<Vector2Int>();

    public List<InteractData> interacts = new List<InteractData>();
    public List<MergeData> mergeConditions = new List<MergeData>();

    [System.NonSerialized]
    public List<ItemBase> merged = new List<ItemBase>();

    public bool Interact(ItemBase item)
    {
        InteractData interact = null;

        if (item == null) interact = interacts.Find(p => p.target == null);
        else interact = interacts.Find(p => p.target != null && p.target.name == item.name);

        if (interact == null) return false;

        foreach (ItemBase result in interact.result)
        {
            ItemBase MergedItem = Instantiate(result, transform.parent);

            MergedItem.merged.AddRange(merged);
            MergedItem.merged.Add(item);
            MergedItem.UpdateMerge();

            MergedItem.transform.position = gameObject.transform.position;

            Destroy(gameObject);
            if(item != null) Destroy(item.gameObject);
        }

        return true;
    }

    public void UpdateMerge()
    {
        foreach(MergeData condition in mergeConditions)
        {
            if (merged.Count != condition.condition.Count) return;
            bool flag = true;
            foreach (ItemBase item in merged)
                if (condition.condition.Find(p => p.name == item.name) == null)
                    flag = false;
            if (!flag) continue;

            Instantiate(condition.result, transform.parent).transform.position = gameObject.transform.position;
            Destroy(gameObject);
        }
    }

    /// <summary>물건 회전</summary>
    /// <param name="delta">시계 방향 1, 반시계 방향 -1</param>
    public void Rotate(int delta = 1)
    {
        if (Mathf.Abs(delta) != 1) return;

        rotation += delta;
        if (rotation < 0) rotation += 4;
        rotation %= 4;

        float rad = Mathf.PI * delta * 0.5f;
        for (int i = 0; i < shape.Count; i++)
        {
            int tx = shape[i].x, ty = shape[i].y;
            Vector2Int position = new Vector2Int(
                (int)(tx * Mathf.Cos(rad) - ty * Mathf.Sin(rad)),
                (int)(tx * Mathf.Sin(rad) + ty * Mathf.Cos(rad))
            );

            shape[i] = position;
            //Debug.Log(shape[i]);
        }

        transform.eulerAngles = new Vector3(0, 0, rotation * 90);
    }

    //private void Update()
    //{
        //if (Input.GetMouseButtonDown(1))
        //    Rotate();
    //}


}