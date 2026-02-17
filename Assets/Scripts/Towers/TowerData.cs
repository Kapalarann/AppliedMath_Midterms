using UnityEngine;

[CreateAssetMenu(
    fileName = "TowerData",
    menuName = "Tower Defense/Tower Data",
    order = 0)]
public class TowerData : ScriptableObject
{
    [Header("Projectile")]
    public string projectileTag;

    [Header("Combat Stats")]
    public float cooldown = 1f;
    public float animationSpeed = 1f;
    public float hitRadius = 0.5f;
    public float damage = 1f;
    public int pierce = 1;
    public float duration = 5f;

    [Header("Ballistics")]
    public float range = 5f;
    public float speedMult = 1f;
    public bool prefersHighAngle = false;

    [Header("Economy")]
    public int cost = 10;
}