using UnityEngine;
using UnityEngine.SceneManagement; //gives us ability to change scenes
using UnityEngine.UI; //allows creation of variables that refer to the UI

public class menu : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private Slider HorizSensitivity = null;
    [SerializeField] private Slider VertSensitivity = null;

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

    //settings (control volume, sens, etc)
    //volume(0.5 by default)
    public void setVolume(float volume)
    {
        AudioListener.volume = volume; //
    }
}
