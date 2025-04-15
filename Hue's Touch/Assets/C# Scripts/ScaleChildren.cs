using System.Data.Common;
using UnityEngine;

public class ScaleChildren : MonoBehaviour
{
    public GameObject parentObject;
    public GameObject[] allChildren;
    public bool scaled;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        allChildren = new GameObject[parentObject.transform.childCount];
        for(int i = 0; i < allChildren.Length; i++)
        {
            allChildren[i] = parentObject.transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(scaled == true)
        {
            //Debug.Log("Space");
            for(int i = 0; i < allChildren.Length; i++)
            {
                allChildren[i].transform.localScale = new Vector3(2f, 2f, 2f);
            }
        }
    }
}
