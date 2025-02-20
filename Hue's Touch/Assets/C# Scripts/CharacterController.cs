using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private float playerMass;

    [SerializeField] private Transform playerPosition;
    [SerializeField] private Transform feetPosition;
    [SerializeField] private Rigidbody rb;

    private Vector3 gravityVector;
    private Vector3 groundCheckVector = new Vector3(0,-.5f, 0);

    private void Awake()
    {
        playerPosition = gameObject.GetComponent<Transform>();
        rb = gameObject.GetComponent<Rigidbody>();
        gravityVector = new Vector3(0, -9.8f * playerMass, 0);
        feetPosition = transform.GetChild(0).GetComponent<Transform>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!isGrounded())
        {
            rb.MovePosition(transform.position + gravityVector * Time.fixedDeltaTime);
        }
    }

    private bool isGrounded()
    {
        RaycastHit hit;
        return Physics.Raycast(feetPosition.position, transform.TransformDirection(Vector3.down), out hit, .2f);
    }
}
