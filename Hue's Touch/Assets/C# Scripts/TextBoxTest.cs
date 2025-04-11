using UnityEngine;
using TMPro; //allows access to text mesh assets

public class DialogueTest : MonoBehaviour
{
    [SerializeField] public TMP_Text textBox; //to reference text label ('Text TMP' in TextBox)

    
    private void Start()
    {
        //draws text to the label
        textBox.text = "Hello!\n Press 'F' to talk to bob";
    }
}
