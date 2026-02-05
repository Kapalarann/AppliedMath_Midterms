using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public Transform targetPoint;
    public Vector3 velocity = Vector3.zero;
}
