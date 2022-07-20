using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundObject : MonoBehaviour
{
    [SerializeField] GameObject myPrefab;
    [SerializeField] GameObject items;

    private void OnMouseDown()
    {
        Instantiate(myPrefab, transform.position, Quaternion.identity).transform.parent = items.transform;
        Destroy(gameObject);
    }
}
