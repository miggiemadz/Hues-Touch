using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    private int sandDollar = 0;
    
    [Header("Icon Assets")]
    public TextMeshProUGUI dollarCount; //to alter on-screen coin count
    [SerializeField] public GameObject bagIcon; //access the tutorial icon
    [SerializeField] public GameObject inventoryPage; //access the hidden tutorial canvas (when H is pressed, enable or disable)

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V)){ //when player presses "V" key
            ToggleInventoryPanel(); //take text & text away, then enable player control list/visa versa
        }
    }

    //public int sandDollar { get; private set; } //can be referenced outside but only set in this script
    //public void diamondCollected()
    //{
    //    sandDollar++;
    //}
    
    private void OnTriggerEnter(Collider other) //or: attatch this 2 every dollar & use ^ function
    {
        if(other.CompareTag("Collectible")) //if colliding obj has tag of "Collectible"
        {
            Debug.Log("Coin collected");
            sandDollar++; //increase sand dollar amount
            dollarCount.text = sandDollar.ToString(); //set sandDollar count on screen euqal to sandDollar int
            Destroy(other.gameObject); //make dollar disappear
        }
    }

    public void ToggleInventoryPanel(){ //to take bag icon away then enable inventory panel
        bool isPageActive = inventoryPage.activeSelf; //bool for control list (false @ start)

        // Toggle everything based on the panel's current state
        inventoryPage.SetActive(!isPageActive); //set page to be opposite of current state
        bagIcon.SetActive(isPageActive); // Show icon when panel is hidden
    }
}
