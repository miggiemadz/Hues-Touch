using TMPro;
using UnityEngine;

public class tutorialToggle : MonoBehaviour{
    [Header("Icon Assets")]
    [SerializeField] public GameObject tutIcon; //access the tutorial icon
    [SerializeField] public GameObject tutPage; //access the hidden tutorial canvas (when H is pressed, enable or disable)
    //[SerializeField] public TMP_Text tutText; // access text above tut icon


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //tutPage.SetActive(false);... player list hidden by default(manually)
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)){ //when player presses "H" key
            ToggleControlsPanel(); //take "?" & text away, then enable player control list/visa versa
        }
    }

    public void ToggleControlsPanel()  //to take "?" and header away then enable player control list
    {
        bool isPageActive = tutPage.activeSelf; //bool for control list (false @ start)

        // Toggle everything based on the panel's current state
        tutPage.SetActive(!isPageActive); //set page to be opposite of current state
        tutIcon.SetActive(isPageActive); // Show icon when panel is hidden
        //tutText.gameObject.SetActive(isPageActive); // Show text when panel is hidden
    }
}