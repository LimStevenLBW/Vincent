using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, Attackable
{
    public float selfDestructionTime;
    public float forceAmount;
    public float knockback;
    public float power; //Amount of damage it will do

    private Rigidbody body;
    private MeshRenderer mesh;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        body = GetComponent<Rigidbody>();
        StartCoroutine(SelfDestruction());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetForce()
    {
        return forceAmount;
    }

    IEnumerator SelfDestruction()
    {
        yield return new WaitForSeconds(selfDestructionTime);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Attackable>() != null)
        {
            Attackable attackable = collision.gameObject.GetComponent<Attackable>();
            Vector3 enemyPos = collision.gameObject.transform.position;

            Vector3 forceDirection = enemyPos - transform.position;
            attackable.Attacked(knockback, forceDirection.normalized, power);
        }
    }

    public void Attacked(float forceAmount, Vector3 forceDirection, float power)
    {
        StopAllCoroutines();
        StartCoroutine(SelfDestruction());
        body.AddForce(forceDirection * forceAmount);
    }
}
