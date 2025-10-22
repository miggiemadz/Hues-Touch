using UnityEngine;

public class Billboard : MonoBehaviour
{

    public Transform cam;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward); //to point canvas torward cam pos. (.forward to make it +1 unit frm where cam is facing)
    }
}
