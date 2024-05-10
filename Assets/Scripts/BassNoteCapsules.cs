using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BassNoteCapsules : MonoBehaviour
{
    public LibPdInstance patch;

    // Custom colors for lilac and peach
    public Color lilacColor = new Color(0.75f, 0.38f, 0.94f); // Example lilac color
    public Color peachColor = new Color(1f, 0.8f, 0.6f); // Example peach color

    float ramp;
    float t;
    int count = 0;
    int measureCount = 0; // Counter for measures passed

    // Variable for beat duration in milliseconds (4 beats per second)
    [SerializeField] int beat = 650;

    bool chordValue = false;
    GameObject capsule; // Reference to the capsule
    int currentBassNote = -1; // Variable to track the current bass note index

    bool growing = false; // Flag to indicate whether the capsule is growing or shrinking

    // Start is called before the first frame update
    void Start()
    {
        // capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule); // Creating a capsule object
        capsule = GameObject.CreatePrimitive(PrimitiveType.Sphere); // Creating a capsule object
        capsule.SetActive(false); // Initially setting it inactive
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

                    // Change the bass note and update the capsule
                    UpdateCapsule((int)chordFloat);

                    chordValue = !chordValue;
                    measureCount = 0; // Reset measure count
                }
            }
        }

        // Scale the capsule (grow or shrink)
        if (capsule.activeSelf)
        {
            float scaleChange = 0.3f * Time.deltaTime * (growing ? 1 : -1);
            capsule.transform.localScale += Vector3.one * scaleChange; // Change scale gradually

            // Check if the capsule should switch from growing to shrinking or vice versa
            if ((growing && chordValue == false) || (!growing && chordValue == true))
            {
                growing = !growing; // Switch the flag
                UpdateCapsule(currentBassNote); // Update capsule color and properties
            }
        }
    }

    // Function to update the capsule color and properties based on the bass note index and chord float
    void UpdateCapsule(int bassNoteIndex)
    {
        capsule.SetActive(true); // Activate capsule
        capsule.transform.position = Vector3.zero; // Set position to center
        capsule.transform.localScale = Vector3.one * (growing ? 0.10f : 1.8f); // Set initial scale

        // Assigning color to the capsule
        Renderer renderer = capsule.GetComponent<Renderer>();
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

