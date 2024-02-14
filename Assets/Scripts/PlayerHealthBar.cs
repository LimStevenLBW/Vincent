using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar : MonoBehaviour
{
    public GameObject heartPrefab;
    public int worthOfAHeart;
    private List<GameObject> hearts = new List<GameObject>();

    
    public void SetHealthBar(float health)
    {
        ClearHealthBar();

        int amountOfHeartsToAdd = (int) health / worthOfAHeart;

        for (int i = 0; i < amountOfHeartsToAdd; i++)
        {
            GameObject obj = Instantiate(heartPrefab);
            obj.transform.SetParent(gameObject.transform);
            hearts.Add(obj);
        }
    }

    void ClearHealthBar()
    {
        foreach(GameObject heart in hearts)
        {
            Destroy(heart);
        }
        hearts.Clear();
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
