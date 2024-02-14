using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Netcode;
public class PlayerCanvas : NetworkBehaviour
{
    public GameOverScreen gameOverScreen;
    public PlayerHealthBar healthBar;
    public TextMeshProUGUI goldCounter;
    public TextMeshProUGUI playersOnlineText;

    private NetworkVariable<int> playerCount = new NetworkVariable<int>(
        0, 
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Server
    );

    public Shop shop;

    // Start is called before the first frame update
    void Start()
    {
        playerCount.OnValueChanged += OnPlayersOnlineChanged;
    }

    void OnPlayersOnlineChanged(int previous, int newValue)
    {
        playersOnlineText.SetText(newValue.ToString() + " Players Online");
    }

    // Update is called once per frame
    void Update()
    {
        playerCount.Value = GameManager.instance.numberOfPlayers;
    }

    public void ShowShop()
    {
        shop.gameObject.SetActive(true);
    }

    public void HideShop()
    {
        shop.gameObject.SetActive(false);
    }

    public void SetHealthBar(float health)
    {
        healthBar.SetHealthBar(health);
    }


    public void ShowGameOverScreen()
    {
        gameOverScreen.gameObject.SetActive(true);
    }

    public void UpdateGoldCounterText(int goldCount)
    {
        goldCounter.SetText(goldCount.ToString());
    }
}
