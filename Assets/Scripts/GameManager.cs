using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    public Player playerPrefab;
    private Vector3 spawnPoint1 = new Vector3(-5, 2, 0);
    private Vector3 spawnPoint2 = new Vector3(5, 2, 0);

    public static GameManager instance { get; private set; }

    public int numberOfPlayers { get; private set; }
    public List<Player> playerList { get; private set; }
    private Player player1;
    private Player player2;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        playerList = new List<Player>();

        if (NetworkManager.Singleton)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += NewPlayerConnected;
        }

        base.OnNetworkSpawn();
    }

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void NewPlayerConnected(ulong playerID)
    {
        if (IsServer)
        {
            numberOfPlayers++;

            if (numberOfPlayers == 1)
            {
                player1 = Instantiate(playerPrefab, spawnPoint1, Quaternion.identity);
                player1.GetComponent<NetworkObject>().SpawnAsPlayerObject(playerID);
                playerList.Add(player1);
            }
            else if(numberOfPlayers == 2)
            {
                player2 = Instantiate(playerPrefab, spawnPoint2, Quaternion.identity);
                player2.GetComponent<NetworkObject>().SpawnAsPlayerObject(playerID);
                playerList.Add(player2);
            }
        }
    }

    public Player GetPlayer1()
    {
        return player1;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
