using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpawnPoint : NetworkBehaviour, Attackable
{
    public GameObject obj;
    public GameObject spawnArea;
    public float rateOfSpawn;
    public bool isAutomatic;


    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        if (isAutomatic) StartCoroutine(Spawn());

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    IEnumerator Spawn()
    {
       yield return new WaitForSeconds(rateOfSpawn);

        Instantiate(obj, transform.position, Quaternion.identity);
        if (obj.GetComponent<NetworkObject>())
        {
            obj.GetComponent<NetworkObject>().Spawn();
        }
        else
        {
            Debug.Log("ERROR, NETWORK OBJECT MISSING. CANNOT SPAWN");
        }


    }

    void OnTriggerEnter(Collider other)
    {
        /*
        if(other.gameObject.tag == "Player" && spawnArea)
        {
            Instantiate(obj, spawnArea.transform.position, Quaternion.identity);
        }
        */
    }

    public void Attacked(float playerForceAmount, Vector3 forceDirection, float power)
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
