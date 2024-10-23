using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerHandPoint;
    [SerializeField] private Camera PlayerCamera;
    [SerializeField] private Image crossHair;
    [SerializeField] private StashZone _stash;
    [SerializeField] private GameObject pickupTextObject;
    [SerializeField] private TMP_Text pickupText;

    private GameObject pickedItem;
    private bool isItItem = false;

    private void Update()
    {
        InteractWithObjects();
    }

    private void InteractWithObjects()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0); // центр экрана
        Ray ray = PlayerCamera.ScreenPointToRay(screenCenter);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2f))
        {
            if (hit.collider.CompareTag("Item") && !_stash.stashedItems.Contains(hit.transform))
            {
                isItItem = true;
                ChangeCrossColor(isItItem);

                pickupTextObject.SetActive(true);
                pickupText.text = "(E)\nПодобрать";
            }
            else if (hit.collider.CompareTag("Stash") && pickedItem != null)
            {
                pickupTextObject.SetActive(true);
                pickupText.text = "(E)\nПоложить";
            }
            else if (hit.collider.CompareTag("Door"))
            {
                pickupTextObject.SetActive(true);
                pickupText.text = "(E)\nОткрыть/Закрыть";
            }
            else
            {
                isItItem = false;
                ChangeCrossColor(isItItem);
                pickupTextObject.SetActive(false);
            }
        }
        else
        {
            isItItem = false;
            ChangeCrossColor(isItItem);
            pickupTextObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (hit.collider != null)
            {
                // Проверяем, взаимодействуем ли с предметом или с дверью
                if (hit.collider.CompareTag("Item"))
                {
                    HandlePickup(hit);
                }
                else if (hit.collider.CompareTag("Door"))
                {
                    HandleDoor(hit);
                }
                else if (hit.collider.CompareTag("Stash") && pickedItem != null)
                {
                    HandleStashing();
                }
            }
        }
    }

    private void HandlePickup(RaycastHit hit)
    {
        if (pickedItem == null && !_stash.stashedItems.Contains(hit.transform))
        {
            pickedItem = hit.collider.gameObject;
            pickedItem.GetComponent<Rigidbody>().isKinematic = true;
            pickedItem.transform.SetParent(playerHandPoint);

            pickedItem.transform.localPosition = Vector3.zero;
            pickedItem.transform.localRotation = Quaternion.identity;

            Debug.Log("Предмет подобран: " + hit.collider.name);
        }
        else
        {
            Debug.Log(hit.collider.name + " уже находится в грузовике, взять нельзя.");
        }
    }

    private void HandleStashing()
    {
        // Сложить предмет в грузовик
        _stash.StashItems(pickedItem);

        // Отпустить предмет
        pickedItem.GetComponent<Rigidbody>().isKinematic = false;
        pickedItem.transform.SetParent(null);
        pickedItem = null;

        Debug.Log("Предмет положен в грузовик.");
    }

    private void HandleDoor(RaycastHit hit)
    {
        OpenDoor door = hit.transform.GetComponent<OpenDoor>();
        if (door != null)
        {
            door.OpenClose();
        }
    }

    private void ChangeCrossColor(bool item)
    {
        crossHair.color = item ? Color.green : Color.white;
    }
}
