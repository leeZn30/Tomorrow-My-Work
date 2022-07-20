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

    public bool hasHierarchy = false;
    [SerializeField]
    private ItemBase initialStore = null;

    public bool IsContainer = false;
    public bool CanDivide = false;

    public float rotation = 0;

    public new string name;
    public string description;

    public List<Vector2Int> shape = new List<Vector2Int>();

    public List<InteractData> interacts = new List<InteractData>();
    public List<MergeData> mergeConditions = new List<MergeData>();
    public List<ItemBase> merged = new List<ItemBase>();

    public bool Interact(ItemBase item)
    {
        if(item == null && hasHierarchy)
        {
            DivideByHierarchy();
        }

        InteractData interact = null;

        if (item == null) interact = interacts.Find(p => p.target == null);
        else interact = interacts.Find(p => p.target != null && p.target.name == item.name);

        if (interact == null)
        {
            SoundManager.Instance.playGameSFX(0);
            return false;
        }

        foreach (ItemBase result in interact.result)
        {
            ItemBase MergedItem = Instantiate(result, StageInformation.instance.itemTransform);
            MergedItem.merged = new List<ItemBase>();

            MergedItem.merged.AddRange(merged);

            if (item != null) 
                MergedItem.merged.Add(item);

            MergedItem.UpdateMerge();
            MergedItem.transform.position = gameObject.transform.position;

            transform.parent = null;
            Destroy(gameObject);

            if (item != null)
            {
                item.transform.parent = null;
                Destroy(item.gameObject);
            }
        }

        SoundManager.Instance.playGameSFX(4);

        StageInformation.instance.UpdateItems();
        EventManager.Instance.OnStoredChanged.Raise();

        return true;
    }

    private bool DivideByHierarchy()
    {
        if (initialStore == null) return false;

        Instantiate(initialStore, StageInformation.instance.itemTransform).transform.position = transform.position;
        foreach (ItemBase item in merged)
        {
            ItemBase prefab = StageInformation.instance.prefabLists.Find(p => p.name == item.name);
            if (prefab == null) return false;

            Instantiate(prefab, StageInformation.instance.itemTransform).transform.position = transform.position;
        }

        gameObject.transform.parent = null;
        Destroy(gameObject);

        StageInformation.instance.UpdateItems();
        EventManager.Instance.OnStoredChanged.Raise();

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

            ItemBase result = Instantiate(condition.result, transform.parent);
            result.merged.AddRange(merged);
            result.transform.position = gameObject.transform.position;

            transform.parent = null;
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
                Mathf.RoundToInt(tx * Mathf.Cos(rad) - ty * Mathf.Sin(rad)),
                Mathf.RoundToInt(tx * Mathf.Sin(rad) + ty * Mathf.Cos(rad))
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