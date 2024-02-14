using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private Player player;

    //Audio
    private AudioSource source;

    public AudioClip fireballClip;
    public AudioClip attackClip;
    public AudioClip tookDamageClip;
    public AudioClip diedClip;
    public AudioClip jumpClip;
    public AudioClip footstepClip;
    
    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.GetComponent<Player>();
        source = GetComponent<AudioSource>();
        StartCoroutine(FootstepCoroutine());
    }

    IEnumerator FootstepCoroutine()
    {
        while (true)
        {
            //If they are moving and are on the ground, play the clip
            if(player.GetIsMovingState() && player.GetGroundedState()) source.PlayOneShot(footstepClip);
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void PlayAudio(string audioName)
    {
        if (audioName == "attack") source.PlayOneShot(attackClip);
        else if (audioName == "fireball") source.PlayOneShot(fireballClip);
        else if (audioName == "tookDamage") source.PlayOneShot(tookDamageClip);
        else if (audioName == "died") source.PlayOneShot(diedClip);
        else if (audioName == "jump") source.PlayOneShot(jumpClip);
       // else if (audioName == "attack") source.PlayOneShot(attackClip);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
