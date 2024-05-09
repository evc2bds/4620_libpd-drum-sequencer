using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientMelody : MonoBehaviour
{
    public LibPdInstance patch;

    float ramp;
    float t;
    int count = 0;
    int measureCount = 0; // Counter for measures passed

    //variable for beat duration in milliseconds (4 beats per second)
    [SerializeField] int beat = 650; 

    List<float> freqValues = new List<float> { 261.6f, 293.6f, 329.6f, 349.2f, 391.9f, 440f, 493.8f, 523.2f };
    
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
            patch.SendBang("whole_note");
            //send chordScale to patch
            //choose random item from list
            float chordScale = freqValues[Random.Range(0, freqValues.Count)];
            patch.SendFloat("chord_scale", chordScale);
            Debug.Log("chord_scale: " + chordScale);

            //send bang each whole note
            // patch.SendBang("whole_note");
            Debug.Log("whole_note");

            

            count = (count + 1) % 4;
            Debug.Log("count :"+count);

            // check if 4 measures have passed -- send new chords
            if (count == 0)
            {
                measureCount++;
                if (measureCount >= 2)
                {

                    measureCount = 0; // Reset measure count
                }
            }

        }

    }
}
