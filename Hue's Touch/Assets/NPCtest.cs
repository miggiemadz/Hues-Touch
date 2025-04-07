using UnityEngine;

public class NPCtest : MonoBehaviour
{
    //
    bool playerDetection = false;


    // Update is called once per frame
    void Update()
    {
        //if player presses "F" while in range
        if(playerDetection && Input.GetKeyDown(KeyCode.F))
        {
            //print console statement
            print("Dialouge Started!");

            //(TO DO: update UI or add speech bubble above player?)
        }
        
    }

    //using OnTrigger methods
    //If player collides w sphere
    private void OnTriggerEnter(Collider other)
    {
        //check if colliding asset is our player
        if (other.name == "TestPlayer")
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
