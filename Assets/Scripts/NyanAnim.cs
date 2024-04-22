using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NyanAnim : MonoBehaviour
{
    float t;
    [SerializeField] float speed = 0.2f;

    Material mat; 


    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime * speed;
        float frame = Mathf.Floor(t % 6);
        mat.SetFloat("_frameCount", frame);
        
    }
}
