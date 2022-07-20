using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInformation : MonoBehaviour
{
    public static StageInformation instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if(instance != this)
        {
            Destroy(instance.gameObject);

            instance = this;
        }

        DontDestroyOnLoad(gameObject);

        storedData = new ItemBase[size.y, size.x];
        storedItemPositions = new Dictionary<ItemBase, Vector2Int>();
    }

    public Vector2Int size; // ���� ũ��
    private ItemBase[,] storedData; // ������ ������ ������ 

    public List<ItemBase> items; // ������ ���� ���
    public Dictionary<ItemBase, Vector2Int> storedItemPositions; // ������ ������ ��ġ

    public Transform itemTransform;
    public Transform storeTransform;

    public List<ItemBase> prefabLists;

    private void Start()
    {
        UpdateItems();
    }

    public void UpdateItems()
    {
        items = new List<ItemBase>();
        foreach (Transform child in itemTransform)
            items.Add(child.GetComponent<ItemBase>());
    }

    /// <summary>
    /// position�� item�� ������Ű�� �Լ�
    /// </summary>
    public void PushItem(Vector2Int position, ItemBase item)
    {
        if (!CanStore(position, item)) return;
        if (IsStored(item)) return;

        for (int i = 0; i < item.shape.Count; i++)
        {
            Vector2Int p = position + item.shape[i];
            storedData[p.y, p.x] = item;
        }

        storedItemPositions.Add(item, position);
        EventManager.Instance.OnStoredChanged.Raise();

        if (IsClear())
        {
            //GameManager.Instance.GameSuccess();
            GameManager.Instance.CallGameSuccess();
        }
    }

    /// <summary>
    /// �����ߴ� item�� ���� �Լ�
    /// </summary>
    public void PullItem(ItemBase item)
    {
        if (!IsStored(item)) return;

        Vector2Int position = storedItemPositions[item];
        for(int i = 0; i < item.shape.Count; i++)
        {
            Vector2Int p = position + item.shape[i];
            if (storedData[p.y, p.x] == item)
                storedData[p.y, p.x] = null;
        }

        storedItemPositions.Remove(item);
        EventManager.Instance.OnStoredChanged.Raise();
    }

    /// <summary>
    /// �̹� ������ item�̸� true, �׷��� ������ false
    /// </summary>
    public bool IsStored(ItemBase item)
    {
        if (item == null) return false;
        return storedItemPositions.ContainsKey(item);
    }

    /// <summary>
    /// item�� �ش� position�� ������ �� ������ true, �׷��� ������ false
    /// </summary>
    public bool CanStore(Vector2Int position, ItemBase item)
    {
        for (int i = 0; i < item.shape.Count; i++)
        {
            Vector2Int p = position + item.shape[i];

            if (p.x < 0 || p.x >= size.x || p.y < 0 || p.y >= size.y) return false;
            if (storedData[p.y, p.x] != null) return false;
        }

        return true;
    }

    /// <summary>
    /// Ŭ���� ������ true, �׷��� ������ false
    /// </summary>
    public bool IsClear()
    {
        UpdateItems();
        EventManager.Instance.OnStoredChanged.Raise();

        if (storedItemPositions.Count < items.Count) return false;

        for (int y = 0; y < size.y; y++)
            for (int x = 0; x < size.x; x++)
                if (storedData[y, x] == null)
                    return false;

        if (!GameManager.Instance.GetTaskComplete) return false;

        return true;
    }
}
