using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StashZone : MonoBehaviour
{
	[SerializeField] private Transform[] stashPoints;

    public List<Transform> stashedItems;

	private int currentPointIndex = 0;

    public void StashItems(GameObject item)
	{
        if (stashedItems.Count < stashPoints.Length)
        {

            item.GetComponent<Rigidbody>().isKinematic = true;
            item.transform.position = stashPoints[currentPointIndex].position;

            currentPointIndex++;
            stashedItems.Add(item.transform);

            item.transform.SetParent(stashPoints[currentPointIndex - 1]);
            item.transform.localRotation = Quaternion.identity;

            Debug.Log("Предмет сложен в точку: " + currentPointIndex);
        }
        else
        {
            Debug.Log("Нет места");
        }
    }
}