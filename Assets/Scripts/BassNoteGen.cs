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

    //variable for beat duration in milliseconds (4 beats per second)
    [SerializeField] int beat = 650; 

    bool chordValue = false;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        int dMs = Mathf.RoundToInt(Time.deltaTime * 1000);
        bool trig = ramp > ((ramp + dMs) % beat); // check if beat has occurred
        ramp = (ramp + dMs) % beat;

        if (trig) {
            //send bang each whole note
            patch.SendBang("whole_time");
            Debug.Log("whole_time Bang");

            

            count = (count + 1) % 4;
            Debug.Log("count :"+count);

            // check if 4 measures have passed -- send new chords
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

                    measureCount = 0; // Reset measure count
                }
            }

        }

    }
}
