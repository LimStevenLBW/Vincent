using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public Weapon weapon;
    private Player player;

    public void Buy()
    {
        int playerMoney = player.GetGold();
        int cost = weapon.cost;

        if(playerMoney >= cost)
        {
            player.weaponController.Equip(weapon);
            playerMoney -= cost;
            player.SetGold(playerMoney);
        }

    }

    public void SetItemInfo(Weapon weapon)
    {
        nameText.SetText(weapon.weaponName);

        string c = weapon.cost + " Gold";
        costText.SetText(c);

        Image image = GetComponent<Image>();
        image.sprite = weapon.sprite;
    }


    // Start is called before the first frame update
    void Start()
    {
        SetItemInfo(weapon);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
