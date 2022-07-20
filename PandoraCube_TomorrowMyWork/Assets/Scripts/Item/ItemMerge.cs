using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemMerge : MonoBehaviour
{
	private bool isMouseUp = false;
	private ItemBase item;

	private Dictionary<ItemBase, Collider2D> colliders;

	private void Awake()
	{
		item = GetComponent<ItemBase>();
		colliders = new Dictionary<ItemBase, Collider2D>();
	}

	private void OnMouseDown()
	{
		isMouseUp = false;
		if (currentCoroutine != null)
		{
			StopCoroutine(currentCoroutine);
			currentCoroutine = null;
		}
		currentCoroutine = StartCoroutine(CheckMouse());
	}

	private void OnMouseUp()
	{
		isMouseUp = true;
	}

	private void OnMouseExit()
	{
		isMouseUp = false;
	}

	private void OnTriggerStay2D(Collider2D col)
	{
		/*
		if (isMouseUp)
		{
			ItemBase colitem = col.gameObject.GetComponent<ItemBase>();
			if (!StageInformation.instance.IsStored(colitem)
			    && !StageInformation.instance.IsStored(item))
			{
				if (item.IsContainer)
					item.Interact(colitem);
				else colitem.Interact(item);
			}
			isMouseUp = false;
		}
		*/

		if (currentCoroutine == null) return;
		colliders[col.gameObject.GetComponent<ItemBase>()] = col;
	}

	Coroutine currentCoroutine;

	private IEnumerator CheckMouse()
    {
		//yield return wait;
		WaitForFixedUpdate frame = new WaitForFixedUpdate();
		while(!isMouseUp)
        {
			colliders = new Dictionary<ItemBase, Collider2D>();
			yield return frame;
		}

		List<ItemBase> items = new List<ItemBase>();
		foreach (ItemBase item in colliders.Keys)
			items.Add(item);

        items.Sort(
			(a, b) => 
			Vector3.Distance(
				item.transform.position, 
				a.transform.position)
			.CompareTo(Vector3.Distance(
				item.transform.position, 
				b.transform.position)));

		foreach (ItemBase item in items)
		{
			if (gameObject == null) break;
			if (StageInformation.instance.IsStored(this.item)) continue;
			if (StageInformation.instance.IsStored(item)) continue;

			bool flag = false;
			if (item == null || this.item == null) break;

			if (item.IsContainer)
				flag = item.Interact(this.item);
			else flag = this.item.Interact(item);

			if (flag) break;
		}
	}
}