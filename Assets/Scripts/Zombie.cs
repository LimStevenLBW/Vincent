using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Zombie : NetworkBehaviour, Attackable
{
    public Stats zombieStats;
    public AudioSource source;
    public AudioClip tookDamageClip;
    public AudioClip diedClip;
    public SkinnedMeshRenderer mesh;
    public Material atlas;
    public Material hitMaterial;
    public float forceAmount;
    public Pickup goldPrefab;

    private bool isActive;
    private Coroutine isDyingCoroutine;
    private Animator animator;
    private Player target;
    private Vector3 direction;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        animator = GetComponent<Animator>();
        isActive = true;

        StartCoroutine(UpdateTargetsRoutine());
    }

    void SearchForClosestPlayer(List<Player> playerList)
    {
        float closestDistance = 9999999;
        foreach (Player player in playerList)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);

            //Found a target that is closer
            if (distance < closestDistance)
            {
                target = player;
                closestDistance = distance;
            }
        }
    }

    IEnumerator UpdateTargetsRoutine()
    {
        while (true)
        {
            List<Player> playerList = GameManager.instance.playerList;

            yield return new WaitForSeconds(3);

            if (playerList.Count == 0) continue;

            SearchForClosestPlayer(playerList);
         
        }
    }

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsServer) return;
        if (isActive == false) return;

        if(zombieStats.GetHealth() <= 0)
        {
            isActive = false;

            if (isDyingCoroutine == null) isDyingCoroutine = StartCoroutine(Dedge());

        }

        if (target == null) return;

        direction = target.transform.position - transform.position;
        transform.position += direction.normalized * zombieStats.GetSpeed() * Time.deltaTime;

        if (direction.normalized.x > 0 || direction.normalized.y > 0 || direction.normalized.z > 0) animator.SetBool("IsWalking", true);
        else { animator.SetBool("IsWalking", false); }
        //Storing the rotation towards the player
        Vector3 rotation = Quaternion.LookRotation(target.transform.position - transform.position).eulerAngles;

        rotation.x = 0;
        rotation.z = 0;
        transform.rotation = Quaternion.Euler(rotation);
    }

    IEnumerator Dedge()
    {
        yield return new WaitForSeconds(1);

        Vector3 pickupSpawnPos = transform.position;
        pickupSpawnPos.y = 3;
        Instantiate(goldPrefab, pickupSpawnPos, Quaternion.identity);
        Destroy(gameObject);
    }

    void FixedUpdate()
    {

    }

    public bool GetIsActive()
    {
        return isActive;
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Player" && isActive)
        {
            Rigidbody body = other.gameObject.GetComponent<Rigidbody>();
            Vector3 liftOffset = new Vector3(0, 0.2f, 0);

            Vector3 impactDirection = target.transform.position - transform.position;

            body.AddForce((impactDirection + liftOffset) * forceAmount);
        }

    }

    //This is what happens when the zombie is attacked
    public void Attacked(float playerForceAmount, Vector3 forceDirection, float power)
    {
        //Zombie takes damage
        zombieStats.TakeDamage(power);
        source.PlayOneShot(tookDamageClip);

        StartCoroutine(AttackedCoroutine());

        Rigidbody body = GetComponent<Rigidbody>();
        Vector3 liftOffset = new Vector3(0, 0.2f, 0);
        body.AddForce((forceDirection + liftOffset) * playerForceAmount);
    }

    IEnumerator AttackedCoroutine()
    {
        mesh.material = hitMaterial;

        yield return new WaitForSeconds(0.5f);
        //mesh.materials[1] = null;
    }
}
