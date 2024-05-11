using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BassNotes : MonoBehaviour
{
    public LibPdInstance patch;

    // Custom colors 
    public Color lilacColor = new Color(0.75f, 0.38f, 0.94f); 
    public Color peachColor = new Color(1f, 0.8f, 0.6f); 

    float ramp;
    float t;
    int count = 0;
    int measureCount = 0; // Counter for measures passed

    // Variable for beat duration in milliseconds (4 beats per second)
    [SerializeField] int beat = 650;

    bool chordValue = false;
    GameObject sphere; // Reference to the sphere
    int currentBassNote = -1; // track the current bass note index

    bool growing = false; // is sphere growing or shrinking

    // Start is called before the first frame update
    void Start()
    {
        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere); // Creating a sphere object
        sphere.SetActive(false); // Initially setting sphere inactive
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

            count = (count + 1) % 4;

            // Check if 4 measures have passed -- send new chords
            if (count == 0)
            {
                measureCount++;
                if (measureCount >= 2)
                {
                    // Choose a random float between 0 and 1
                    float chordFloat = chordValue ? 1f : 0f;
                    patch.SendFloat("Chord", chordFloat);

                    // Change the bass note and update the sphere
                    Updatesphere((int)chordFloat);

                    chordValue = !chordValue;
                    measureCount = 0; // Reset measure count
                }
            }
        }

        // Scale the sphere (grow or shrink)
        if (sphere.activeSelf)
        {
            float scaleChange = 0.3f * Time.deltaTime * (growing ? 1 : -1);
            sphere.transform.localScale += Vector3.one * scaleChange; // Change scale gradually

            // Check if the sphere should switch from growing to shrinking or vice versa
            if ((growing && chordValue == false) || (!growing && chordValue == true))
            {
                growing = !growing; // Switch the flag
                Updatesphere(currentBassNote); // Update sphere color and properties
            }
        }
    }

    // Function to update the sphere color and properties
    void Updatesphere(int bassNoteIndex)
    {
        sphere.SetActive(true); // Activate sphere
        sphere.transform.position = Vector3.zero; // Set position to center
        sphere.transform.localScale = Vector3.one * (growing ? 0.10f : 1.8f); // Set initial scale

        // Assigning color to the sphere
        Renderer renderer = sphere.GetComponent<Renderer>();
        if (renderer != null)
        {
            if (bassNoteIndex == 0)
            {
                renderer.material.color = peachColor; // Set material color to peach for bass note index 0
            }
            else if (bassNoteIndex == 1)
            {
                renderer.material.color = lilacColor; // Set material color to lilac for bass note index 1
            }
        }

        currentBassNote = bassNoteIndex; // Update current bass note index
    }
}

