using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AmbientMelody : MonoBehaviour
{
   public LibPdInstance patch;
   public int currentChordScaleIndex = 0; // Track the index of the current chord scale




   float ramp;
   float t;
   int count = 0;


   //variable for beat duration in milliseconds (4 beats per second)
   [SerializeField] int beat = 650;


   public List<float> freqValues = new List<float> { 261.6f, 293.6f, 329.6f, 349.2f, 391.9f, 440f, 493.8f, 523.2f };
  
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
           //choose random item from list
           currentChordScaleIndex = Random.Range(0, freqValues.Count); // Update currentChordScaleIndex


           float chordScale = freqValues[currentChordScaleIndex];
           patch.SendFloat("chord_scale", chordScale);        


           count = (count + 1) % 4;


       }


   }
}
