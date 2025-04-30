using UnityEngine;
using UnityEngine.SceneManagement; //lets us change scenes
public class GameManager : MonoBehaviour
{
    public GameObject respawnPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void returnToMenu()
    {  //attatched to "MainMenu" button on GameOver panel
        SceneManager.LoadScene(0); //load main menu scene (used in health.cs for respawn/quit screen)
    }       
    public void Respawn()
    {
        //teleport player back to starting position
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //re-load current scene
        //Or:
        //transform.position = new Vector3(0.95f, 16.57f, -4.32f); //send back to starting position
    }
    public void gameOver() //referenced in health.cs
    {
        respawnPanel.SetActive(true); //show "game over" panel
    }
}
