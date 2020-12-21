using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  private IRoomTrigger roomTrigger = null;
  private Transform m_transform = null;

  private void Awake() => m_transform = transform;
  private void OnEnable() => PlayerMover.OnMovePlayer += MoveToDestiantion;
  private void OnDisable() => PlayerMover.OnMovePlayer -= MoveToDestiantion;

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.TryGetComponent<IRoomTrigger>(out var trigger))
      roomTrigger = trigger;
  }

  private void OnTriggerExit2D(Collider2D collision)
  {
    if (collision.TryGetComponent<IRoomTrigger>(out var _))
      roomTrigger = null;
  }

  private void MoveToDestiantion(Vector2 destination) => LeanTween.move(gameObject, destination, 0.25f).setEaseOutSine().setOnComplete(InteractWithRoom);

  private void InteractWithRoom()
  {
    if (roomTrigger != null)
      roomTrigger.TriggerRoom(m_transform);
  }
}
