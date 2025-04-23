using UnityEngine;

public class SetToConvex : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void Awake()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).gameObject.GetComponent<MeshCollider>() != null)
            {
                gameObject.transform.GetChild(i).gameObject.GetComponent<MeshCollider>().convex = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
