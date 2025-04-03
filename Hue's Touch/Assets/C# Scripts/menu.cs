using UnityEngine;
using UnityEngine.SceneManagement; //gives us ability to change scenes

public class menu : MonoBehaviour
{

    //whenever "Start" button is pressed
    public void playGame()
    {
        SceneManager.LoadScene(1); // switches to scene w/ index of '1' (gameScene = 1 in build profiles)
    }

    //whenever "Quit" button is pressed
    public void quitGame()
    {
        Debug.Log("Quit"); //console statement
        Application.Quit(); //quits the game
    }
}
