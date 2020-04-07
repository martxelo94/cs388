using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCreator : MonoBehaviour
{
    public GameObject sphere_prefab;
    public float offset = 1.0f;
    public bool mobile = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)) {
            if (mobile && Input.touchCount == 1)
            {
                GameObject obj = GameObject.Instantiate(sphere_prefab);
                obj.transform.position = new Vector3(Random.Range(-offset, offset), 0.0f, Random.Range(-offset, offset)) + gameObject.transform.position;
            }
        }
    }
}
