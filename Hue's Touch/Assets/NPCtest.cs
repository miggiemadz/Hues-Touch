using UnityEngine;
using TMPro;

public class NPCtest : MonoBehaviour
{
    //might be bad practice? (repetition)
    [SerializeField] public TMP_Text textBox; //to reference text label ('Text TMP' in TextBox)

    bool playerDetection = false;


    // Update is called once per frame
    void Update()
    {
        //if player presses "F" while in range
        if(playerDetection && Input.GetKeyDown(KeyCode.F)){
            print("Dialouge Started!"); //print console statement
            textBox.text = "Hello!\n I'm Bob!";

            //(TO DO: update UI or add speech bubble above player?)
        }
    }

    //using OnTrigger methods
    //If player collides w sphere
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))//check if colliding asset has npc tag (other.name not working)
        {
            playerDetection = true;
            //(TO DO: set canvas w "F" key above player instead of textbox text)
        }
    }

    //when player isn't colliding w sphere
    private void OnTriggerExit(Collider other)
    {
        playerDetection = false;
        //take away canvas w "F" key above player? 
    }
}
