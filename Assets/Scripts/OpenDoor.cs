using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private bool doorState = false;

    public void OpenClose()
    {
        doorState = !doorState;
        animator.SetBool("doorState", doorState);
    }
}
