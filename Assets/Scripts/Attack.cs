using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float forceAmount;

    private bool active;
    private MeshRenderer mesh;
    private float power;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        active = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate(float power)
    {
        this.power = power;
       // mesh.enabled = true;
        active = true;
    }

    public void Deactivate()
    {
      //  mesh.enabled = false;
        active = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Attackable>() != null && active)
        {
            Attackable attackable = other.gameObject.GetComponent<Attackable>();

            attackable.Attacked(forceAmount, transform.forward, power);
   
            Deactivate();
        }

    }
}
