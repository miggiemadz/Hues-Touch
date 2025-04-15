using UnityEngine;

public class Rangedattack : MonoBehaviour
{
    private Rigidbody rb;
    [Header("Projectile Settings")]
    [SerializeField] private GameObject Projectile;
    
    [SerializeField] private float Damage = 20f;
    [SerializeField] private float ProjectileSpeed = 5f;
    [SerializeField] private float Lifetime = 2f;
    [Header("Enemy Settings")]
    public float ShootRadius;
    [SerializeField] private float TimeBetweenAttacks = 1f;
    private bool PlayerInAttackRange;
    private LayerMask WhatIsPlayer;

    public void Initialize(Enemyai2 enemyai)
    {
        
        ShootRadius = 20f;
        WhatIsPlayer = enemyai.p;

        Debug.Log("Init was ran! ShootRadius set to: " + ShootRadius + " | Instance ID: " + GetInstanceID());
    }
    
    private void OnEnable() { Debug.Log("Rangedattack Enabled, InstanceID: " + GetInstanceID());  }
private void OnDisable() { Debug.Log("Rangedattack Disabled"); }

    /*private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Projectile is missing a Rigidbody!");
        }

    }*/

    private void Update()
    {
        //PlayerInAttackRange = Physics.CheckSphere(transform.position, ShootRadius, WhatIsPlayer);
        if (ShootRadius == 0)
    {
        Debug.LogError("ShootRadius is unexpectedly 0!");
    }
    }
}
