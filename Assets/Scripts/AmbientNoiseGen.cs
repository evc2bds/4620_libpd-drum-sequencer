// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class AmbientNoiseGen : MonoBehaviour
// {
//     public LibPdInstance patch;
//     float ramp;
//     float t;
//     int count = 0;
//     int measureCount = 0; // Counter for measures passed

//     // BPM and beat duration
//     [SerializeField] int bpm = 60;
//     float beatDuration;

//     // List of midi notes for C major scale
//     int[] cMajorScale = { 60, 62, 64, 65, 67, 69, 71, 72 }; // MIDI note numbers for C major scale

//     // Duration of a measure in seconds
//     float measureDuration;

//     void Start()
//     {
//         // Calculate beat duration and measure duration
//         beatDuration = 60f / bpm;
//         measureDuration = beatDuration * 4;

//         // Start generating ambient noise
//         StartCoroutine(GenerateAmbientNoise());
//     }

//     IEnumerator GenerateAmbientNoise()
//     {
//         while (true)
//         {
//             yield return new WaitForSeconds(measureDuration);

//             // Send "Chord" every two measures
//             if (measureCount % 2 == 0)
//             {
//                 patch.SendBang("Chord");
//             }

//             // Send "Scale" every measure
//             int randomNote = Random.Range(0, 8); // Generate random index for C major scale
//             int midiNote = cMajorScale[randomNote]; // Get the MIDI note from C major scale
//             patch.SendFloat("Scale", midiNote);

//             measureCount++;
//         }
//     }
// }
