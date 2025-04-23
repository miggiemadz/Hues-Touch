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
            //print console statement
            print("Dialouge Started!");
            textBox.text = "Hello!\n I'm Bob!";

            //(TO DO: update UI or add speech bubble above player?)
        }
        
    }

    //using OnTrigger methods
    //If player collides w sphere
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "NPCdetector")//check if colliding asset is our player
        {
            playerDetection = true;
        }
    }

    //when player isn't colliding w sphere
    private void OnTriggerExit(Collider other)
    {
        playerDetection = false;
    }
}
