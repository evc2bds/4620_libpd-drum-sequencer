using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientMelodyVisualizer : MonoBehaviour
{
    public AmbientMelody ambientMelodyScript;
    public GameObject cubePrefab; 
    public Material lightBlueMaterial;
    public Material pinkMaterial;

    List<GameObject> cubes = new List<GameObject>(); 

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate a cube with size 2x2x3 (to leave room for bassNote anim)
        GameObject staticCube = Instantiate(cubePrefab, transform.position + Vector3.forward * 2, Quaternion.identity);
        staticCube.transform.localScale = new Vector3(2, 2, 3); //change back to 2?
        staticCube.GetComponent<Renderer>().material = lightBlueMaterial;

        // Instantiate 8 cubes corresponding to freqValues
        for (int i = 0; i < ambientMelodyScript.freqValues.Count; i++)
        {
            float size = 3 + i; // Start with a minimum size of 3
            GameObject cube = Instantiate(cubePrefab, transform.position + Vector3.forward * size, Quaternion.identity);
            cube.transform.localScale = new Vector3(size, size, size);
            cubes.Add(cube);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Change color of cubes based on chordScale
        for (int i = 0; i < cubes.Count; i++)
        {
            if (ambientMelodyScript.currentChordScaleIndex == i)
            {
                cubes[i].GetComponent<Renderer>().material = pinkMaterial;
            }
            else
            {
                cubes[i].GetComponent<Renderer>().material = lightBlueMaterial;
            }
        }
    }
}
