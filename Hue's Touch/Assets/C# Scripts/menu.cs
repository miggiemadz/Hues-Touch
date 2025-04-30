using System; //for ln 59
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; //gives us ability to change scenes
using UnityEngine.UI; //allows creation of variables that refer to the UI


public class menu : MonoBehaviour{

    [Header("Levels to Load")]
    public string _newGameLevel; //is used when new game is created (file)
    private string levelToLoad; //loaded level

    [Header("UI Elements")]//allows us to attacth the UI elements to these variables
    [SerializeField] private Slider HorizSensitivity = null;
    [SerializeField] private Slider VertSensitivity = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private GameObject noSavedGamePopup = null; //pop-up that says "no save file"
    [SerializeField] private GameObject confimationPrompt = null; //pop-up for settings saves
    [SerializeField] private GameObject popupContainer = null; //have 2 activate this as well^ during coroutine
    [SerializeField] private GameObject saveContainer = null; //to make save game menu go away when "no save file" pops up



    //default settings values
    [SerializeField] public float defaultVolume = 50f;
    [SerializeField] public float defaultSensX = 50f;
    [SerializeField] public float defaultSensY = 50f;

    
    public void newGame(){ //attatched to "Yes" button in load/new game scene
        //(FIX)SceneManager.LoadScene(_newGameLevel); //loads a new game w name "_newGameLevel" (Sidney Test for now)
        SceneManager.LoadScene(1); // switches to scene w/ index of '1' (gameScene = 1 in build profiles)
    }

    public void loadGame()
    {
        if (PlayerPrefs.HasKey("SavedLevel")){ //checks if playerPrefs(saved data) has saved data file named "SavedLevel"
            levelToLoad = PlayerPrefs.GetString("SavedLevel"); //set the levelToLoad to "SavedLevel" file
            SceneManager.LoadScene(levelToLoad); //load level
            //If you wanted to access a specific saved level-->PlayerPrefs.SetString("Saved Level", whatever level u want to insert)
        }
        else{ //if "savedLevel" file isn't found/nothing available to load
            popupContainer.SetActive(true);
            noSavedGamePopup.SetActive(true);
            saveContainer.SetActive(false);
        }
    }

    //whenever "Quit" button is pressed
    public void quitGame(){
        Debug.Log("Quit"); //console statement
        Application.Quit(); //quits the game
    }

    //SETTINGS (volume, sens, etc)

    //coroutine to make confirmationPrompt appear for 2 sec
    IEnumerator FiveSecWait(){
        Debug.Log("Starting coroutine...");
        popupContainer.SetActive(true);
        confimationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        popupContainer.SetActive(false);
        confimationPrompt.SetActive(false);
        Debug.Log("Coroutine finished after 2 seconds!");
    }

    //getters
    //public void setVolume(float volume){ //attached to volume slider(sets vol on change)
    //    AudioListener.volume = volume;
    //}

    //public void setSensX(float volume){ //attached to horizSens slider
    //    PlayerPrefs.SetFloat("HorizSlider", defaultSensX); //
    //}

    //public void setSensY(float volume){ //attached to vertSens slider
    //    PlayerPrefs.SetFloat("HorizSlider", defaultSensY);
    //}


    //setters (when apply button pressed)
    public void changesApplied()
    {
        StartCoroutine(FiveSecWait());
    }

    //public void applyVolume()
    //{
    //    PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
    //    StartCoroutine(FiveSecWait());
    //}


    public void resetButton(){
        AudioListener.volume = defaultVolume;
        volumeSlider.value = defaultVolume;
        //applyVolume(); //to recursively update/change just incase

        HorizSensitivity.value = defaultSensX;
        VertSensitivity.value = defaultSensY;

        StartCoroutine(FiveSecWait());
    }
}
