using UnityEngine;
using UnityEngine.InputSystem;

public class NPCDialogue : MonoBehaviour {
    [Header("Interaction Settings")]
    [SerializeField] private float interactionDistance = 3f;
    
    [TextArea(2, 5)]
    public string[] dialogueLines;

 
    private Transform player;
    private int currentLine = 0;
    private bool isTalking = false;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update() {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= interactionDistance && Keyboard.current.eKey.wasPressedThisFrame) {
            if (!isTalking) {
                StartDialogue();
            } else {
                ContinueDialogue();
            }
        }

        // Optional: End dialogue if player walks away
        if (isTalking && distance > interactionDistance) {
            EndDialogue();
        }
    }

    private void StartDialogue() {
        isTalking = true;
        currentLine = 0;
        Debug.Log(dialogueLines[currentLine]);
    }

    private void ContinueDialogue() {
        currentLine++;
        if (currentLine >= dialogueLines.Length) {
            EndDialogue();
        } else {
            Debug.Log(dialogueLines[currentLine]);
        }
    }

    private void EndDialogue() {
        isTalking = false;
        Debug.Log("End of dialogue");
    }
    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}
