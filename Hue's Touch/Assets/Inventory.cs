using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    private int sandDollar = 0;
    public TextMeshProUGUI dollarCount;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Collectible") //if what player collides w/ is tagged as collectible
        {
            Debug.Log("Coin collected");
            sandDollar++; //increase sand dollar amount
            dollarCount.text = sandDollar.ToString(); //set sandDollar count on screen = sandDollar int
            Destroy(other.gameObject); //make dollar disappear
        }
    }
}
