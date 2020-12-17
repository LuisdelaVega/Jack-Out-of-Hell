using System;
using UnityEngine;

public class ExitRoomKey : MonoBehaviour
{
    public static event Action OnKeyPickedUp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnKeyPickedUp?.Invoke();
            // TODO: Play some key pickup sound
            Destroy(gameObject);
        }
    }
}
