// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class BassNoteGen : MonoBehaviour
// {
//     public LibPdInstance patch;

//     float ramp;
//     float t;
//     int count = 0;
//     int measureCount = 0; // Counter for measures passed

//     //variable for beat duration in milliseconds (4 beats per second)
//     [SerializeField] int beat = 650; 

//     bool chordValue = false;
    
//     // Start is called before the first frame update
//     void Start()
//     {

//     }

//     // Update is called once per frame
//     void Update()
//     {
//         t += Time.deltaTime;
//         int dMs = Mathf.RoundToInt(Time.deltaTime * 1000);
//         bool trig = ramp > ((ramp + dMs) % beat); // check if beat has occurred
//         ramp = (ramp + dMs) % beat;

//         if (trig) {
//             //send bang each whole note
//             patch.SendBang("whole_time");
//             Debug.Log("whole_time Bang");

            

//             count = (count + 1) % 4;
//             Debug.Log("count :"+count);

//             // check if 4 measures have passed -- send new chords
//             if (count == 0)
//             {
//                 measureCount++;
//                 if (measureCount >= 2)
//                 {
//                     // Choose a random float between 0 and 1
//                     float chordFloat = chordValue ? 1f : 0f;
//                     patch.SendFloat("Chord", chordFloat);
//                     Debug.Log("Chord: " + chordFloat);

//                     chordValue = !chordValue;

//                     measureCount = 0; // Reset measure count
//                 }
//             }

//         }

//     }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BassNoteGen : MonoBehaviour
{
    public LibPdInstance patch;

    float ramp;
    float t;
    int count = 0;
    int measureCount = 0; // Counter for measures passed

    // Variable for beat duration in milliseconds (4 beats per second)
    [SerializeField] int beat = 650; 

    bool chordValue = false;
    GameObject[] capsules; // Array to hold the capsules
    int currentBassNote = -1; // Variable to track the current bass note index
    Color[] colors = { Color.red, Color.blue, Color.green, Color.yellow }; // Array of colors for capsules

    // Start is called before the first frame update
    void Start()
    {
        capsules = new GameObject[4]; // Creating an array of 4 capsules
        for (int i = 0; i < 4; i++)
        {
            capsules[i] = GameObject.CreatePrimitive(PrimitiveType.Capsule); // Creating capsule objects
            capsules[i].SetActive(false); // Initially setting them inactive
        }
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        int dMs = Mathf.RoundToInt(Time.deltaTime * 1000);
        bool trig = ramp > ((ramp + dMs) % beat); // check if beat has occurred
        ramp = (ramp + dMs) % beat;

        if (trig)
        {
            // Send bang each whole note
            patch.SendBang("whole_time");
            Debug.Log("whole_time Bang");

            count = (count + 1) % 4;
            Debug.Log("count :" + count);

            // Check if 4 measures have passed -- send new chords
            if (count == 0)
            {
                measureCount++;
                if (measureCount >= 2)
                {
                    // Choose a random float between 0 and 1
                    float chordFloat = chordValue ? 1f : 0f;
                    patch.SendFloat("Chord", chordFloat);
                    Debug.Log("Chord: " + chordFloat);

                    chordValue = !chordValue;

                    // Change the bass note and activate its corresponding capsule
                    int newBassNote = Random.Range(0, 4); // Generating a random bass note index
                    if (newBassNote != currentBassNote)
                    {
                        if (currentBassNote != -1)
                            capsules[currentBassNote].SetActive(false); // Deactivate previous capsule

                        capsules[newBassNote].SetActive(true); // Activate new capsule
                        capsules[newBassNote].transform.position = Vector3.zero; // Set position to center
                        capsules[newBassNote].transform.localScale = Vector3.one * 1.1f; // Set initial scale

                        // Assigning color to the capsule
                        Renderer renderer = capsules[newBassNote].GetComponent<Renderer>();
                        if (renderer != null)
                        {
                            renderer.material.color = colors[newBassNote];
                        }

                        currentBassNote = newBassNote; // Update current bass note index
                    }

                    measureCount = 0; // Reset measure count
                }
            }
        }

        // Increase size of active capsule
        if (currentBassNote != -1 && capsules[currentBassNote].activeSelf)
        {
            capsules[currentBassNote].transform.localScale += Vector3.one * 0.5f * Time.deltaTime; // Increase scale gradually
        }
    }
}
