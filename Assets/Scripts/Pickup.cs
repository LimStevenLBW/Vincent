using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum pickupType
    {
        health,
        gold,
        weapon
    }
    public pickupType type;

    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Do nothing
        if (target == null) return;
        Vector3 direction = target.position - transform.position;
        transform.position += direction.normalized * 8 * Time.deltaTime;

    }

    public void Follow(Transform target)
    {
        this.target = target;
        Oscillate osc = GetComponent<Oscillate>();
        osc.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Player player = other.gameObject.GetComponent<Player>();
            if(type == pickupType.health)
            {
                player.ResetHealth();
                Destroy(gameObject);
            }
            else if(type == pickupType.gold)
            {
                player.AddGold(10);
                Destroy(gameObject);
            }
        }
    }
}
