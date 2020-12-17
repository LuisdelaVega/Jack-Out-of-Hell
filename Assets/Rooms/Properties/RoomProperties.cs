using UnityEngine;

[CreateAssetMenu(fileName = "New Room Properties", menuName = "RoomProperties")]
public class RoomProperties : ScriptableObject
{
    [Header("Properties")]
    public Color groundColor = Color.white;
}
