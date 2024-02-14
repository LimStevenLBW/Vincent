using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Stats : NetworkBehaviour
{
    public Player player;
    private float originalHealth;
    private float originalSpeed;
    private float originalJumpSpeed;
    private float originalPower;

    [SerializeField] private NetworkVariable<float> health = new NetworkVariable<float>(100);
    [SerializeField] private NetworkVariable<float> speed = new NetworkVariable<float>(5);
    [SerializeField] private NetworkVariable<float> jumpSpeed = new NetworkVariable<float>(12);
    [SerializeField] private NetworkVariable<float> power = new NetworkVariable<float>(5);
    [SerializeField] private NetworkVariable<int> goldCount = new NetworkVariable<int>(100);

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            health.OnValueChanged += OnHealthValueChanged;
            speed.OnValueChanged += OnSpeedValueChanged;
            jumpSpeed.OnValueChanged += OnJumpSpeedValueChanged;
            power.OnValueChanged += OnPowerValueChanged;
        }

        base.OnNetworkSpawn();
    }

    private void OnHealthValueChanged(float previous, float newValue)
    {
        if(health.Value <= 0)
        {
            //player.Gameover()
        }
    }

    private void OnSpeedValueChanged(float previous, float newValue)
    {

    }
    private void OnJumpSpeedValueChanged(float previous, float newValue)
    {

    }
    private void OnPowerValueChanged(float previous, float newValue)
    {

    }


    public int GetGold()
    {
        return goldCount.Value;
    }

    public void AddGold(int gold)
    {
        goldCount.Value += gold;
    }

    public void SetGold(int goldCount)
    {
        this.goldCount.Value = goldCount;
    }

    public float GetHealth()
    {
        return health.Value;
    }

    public void TakeDamage(float damage)
    {
        health.Value -= damage;
    }

    public float GetSpeed()
    {
        return speed.Value;
    }

    public void SetSpeed(float speed)
    {
        this.speed.Value = speed;
    }

    public float GetJumpSpeed()
    {
        return jumpSpeed.Value;
    }

    public void SetJumpSpeed(float jumpSpeed)
    {
        this.jumpSpeed.Value = jumpSpeed;
    }

    public void ResetHealth()
    {
        health.Value = originalHealth;
    }
    public void ResetSpeed()
    {
        speed.Value = originalSpeed;
    }
    public void ResetJumpSpeed()
    {
        jumpSpeed.Value = originalJumpSpeed;
    }
    public float GetPower()
    {
        return power.Value;
    }

    public void SetPower(float power)
    {
        this.power.Value = power;
    }


    // Start is called before the first frame update
    void Start()
    {
        originalHealth = health.Value;
        originalSpeed = speed.Value;
        originalJumpSpeed = jumpSpeed.Value;
        originalPower = power.Value;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
