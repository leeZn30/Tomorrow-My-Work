using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSnapping : MonoBehaviour
{
	private enum SnapMode
	{
		None = 0x01,
		Snap = 0x10,
		Everything = 0x11
	}

	private ItemBase item;
	private SpriteRenderer spriteRenderer;

	private Vector2[,] boxPositions;

	private Vector2 originalLocation;
	private Vector2 originalMouseLocation;

	private Vector3 initialDistance;

	private bool originalStore;

	private int sizeX, sizeY;

	private bool isStartDrag = false;

	private bool coAnim = false;
	private bool isDrag = false;
	private bool isDragged = false;
	private bool isEnter = false;

	private Vector3 targetScale = Vector3.one * 0.75f;
	private Color targetColor;

	[SerializeField] private Sprite selImage;
	[SerializeField] private Sprite befImage;

	[SerializeField] private List<SnapMode> condition = new List<SnapMode>();

	private SnapMode CurrentSnapMode
	{
		set
		{
			if (!initialized) return;

			for (int i = 0; i < condition.Count; i++)
				colliders[i].enabled = (condition[i] & value) == value;
		}
	}

	private BoxCollider2D[] colliders;
	private bool initialized;

	private void Awake()
	{
		CurrentSnapMode = SnapMode.None;

		item = GetComponent<ItemBase>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		colliders = GetComponents<BoxCollider2D>();

		initialized = true;
	}

	private void Start()
	{
		sizeX = StageInformation.instance.size.x;
		sizeY = StageInformation.instance.size.y;
		boxPositions = new Vector2[sizeY, sizeX];

		for (int i = 0; i < sizeX; i++)
		{
			for (int j = 0; j < sizeY; j++)
			{
				boxPositions[j, i] = new Vector2(i - sizeX / 2, j - sizeY / 2)
				                     + (Vector2)StageInformation.instance.storeTransform.position;
			}
		}
	}

	private float snapOffset = (float)Math.Sqrt(2f) * 0.5f;

	private void OnMouseEnter()
	{
		if (coAnim) return;
		if (GameManager.Instance.isEnableMoving)
		{
			isEnter = true;
			targetScale = Vector3.one;
		}
	}

	private void OnMouseExit()
	{
		isEnter = false;
		if (!StageInformation.instance.IsStored(item))
		{
			targetScale = Vector3.one * 0.75f;
		}
	}

	private void OnMouseDown()
	{
		// if (selImage != null)
		// {
		// 	spriteRenderer.sprite = selImage;
		// }
		//
		// CurrentSnapMode = SnapMode.Snap;

		if (coAnim) return;
		if (GameManager.Instance.isEnableMoving)
		{
			originalLocation = transform.position;
			originalMouseLocation = Input.mousePosition;

			initialDistance = originalLocation;
			initialDistance -= Camera.main.ScreenToWorldPoint(originalMouseLocation);

			initialDistance.z = 0;
			isDragged = false;
		}
		// originalStore = StageInformation.instance.IsStored(item);

		// isDrag = true;
	}

	private void MouseDownFuction()
	{
		if (coAnim) return;
		if (selImage != null)
		{
			spriteRenderer.sprite = selImage;
		}

		CurrentSnapMode = SnapMode.Snap;

		// originalLocation = transform.position;
		// originalMouseLocation = Input.mousePosition;
		originalStore = StageInformation.instance.IsStored(item);

		isDrag = true;
	}

	private void OnMouseDrag()
	{
		if (!GameManager.Instance.isEnableMoving) return;
		targetScale = Vector3.one;

		if (!isStartDrag && Vector2.Distance(Input.mousePosition, originalMouseLocation) >= 0.05f)
		{
			isStartDrag = true;
			isDragged = true;

			MouseDownFuction();
		}

		if (!isStartDrag) return;

		if (StageInformation.instance.IsStored(item))
		{
			StageInformation.instance.PullItem(item);
			gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
		}

		Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5);
		Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
		transform.position = objPosition + initialDistance;
	}

	private void OnMouseUp()
	{
		targetScale = Vector3.one * 0.75f;
		transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
		
		int minPosXIdx = -1;
		int minPosYIdx = -1;
		float minDist = float.MaxValue;

		for (int i = 0; i < sizeY; i++)
		{
			for (int j = 0; j < sizeX; j++)
			{
				float dist = Vector2.Distance(transform.position,
					boxPositions[i, j]);

				if (minPosXIdx != -1 && minPosYIdx != -1)
				{
					minDist = Vector2.Distance(transform.position,
						boxPositions[minPosYIdx, minPosXIdx]);
				}

				if (dist < snapOffset && dist < minDist)
				{
					minPosYIdx = i;
					minPosXIdx = j;
				}
			}
		}

		if (minPosXIdx != -1)
		{
			targetScale = Vector3.one;
			Vector3 position =
				new Vector3(boxPositions[minPosYIdx, minPosXIdx].x,
					boxPositions[minPosYIdx, minPosXIdx].y, 0) -
				new Vector3(0, 0, -5);
			Vector2Int replace = Vector2Int.CeilToInt(position - StageInformation.instance.storeTransform.position) +
			                     StageInformation.instance.size / 2;

			if (StageInformation.instance.CanStore(replace, item))
			{
				transform.position = position;
				StageInformation.instance.PushItem(replace, item);
				gameObject.GetComponent<SpriteRenderer>().sortingOrder = -11;
				SoundManager.Instance.playGameSFX(1);
			}
			else
			{
				SoundManager.Instance.playGameSFX(0);
				//transform.position = originalLocation;
				IEnumerator CoReturn()
				{
					WaitForEndOfFrame wait = new WaitForEndOfFrame();
					coAnim = true;
					float progress = 0, duration = 0.25f;

					Vector3 currentPosition = transform.position;

					while (progress < duration)
					{
						transform.position = LineAnimation.Lerp(currentPosition, 
							originalLocation, progress / duration,
							1, 0, 1);

						yield return wait;
						progress += Time.deltaTime;
					}

					transform.position = originalLocation;

					coAnim = false;
				}

				StartCoroutine(CoReturn());

				if (originalStore)
				{
					Vector2Int origInt = Vector2Int.CeilToInt(originalLocation -
					(Vector2)StageInformation.instance.storeTransform.position) 
					                     + StageInformation.instance.size / 2;

					StageInformation.instance.PushItem(origInt, item);
				}
			}
		}

		if (selImage != null && !StageInformation.instance.IsStored(item))
		{
			spriteRenderer.sprite = befImage;
			CurrentSnapMode = SnapMode.None;
		}

		isDrag = false;
		isStartDrag = false;
	}

	private void Update()
	{
		Interact();

		transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * 20f);
		targetColor = spriteRenderer.color;
		targetColor.a = transform.localScale.x;
		spriteRenderer.color = targetColor;
	}

	private void Interact()
	{
		if (StageInformation.instance.IsStored(item)) return;

		if (!isEnter) return;
		if (!isDrag && !isDragged)
		{
			// 상호 작용
			if (Input.GetMouseButtonUp(0))
			{
				item.Interact(null);
			}

			return;
		}

		if (!Input.GetMouseButtonDown(1)) return;
		item.Rotate();
	}
}