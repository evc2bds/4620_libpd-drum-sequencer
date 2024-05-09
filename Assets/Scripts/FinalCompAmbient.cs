using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCompAmbient : MonoBehaviour
{
    public LibPdInstance patch;
    int ramp;
    float t;
    [SerializeField] int beat;
    int[] scaleNotes;
    int measureCount = 0;
    int chordMeasureInterval = 2;
    int wholeNoteCount = 0;
    int wholeNoteInterval = 4; // Send a bang "whole_note" every 4 measures (whole notes)
    int[] cMajorScale = { 60, 62, 64, 65, 67, 69, 71, 72 }; // C major scale MIDI notes

    void Start()
    {
        patch.SendBang("ON");
        scaleNotes = ControlFunctions.PitchArray(0, new Vector2Int(48, 60), new int[] { 2, 1, 2, 2, 2, 1 });
    }

    void Update()
    {
        t += Time.deltaTime;
        int dMs = Mathf.RoundToInt(Time.deltaTime * 1000);
        float lfo = ControlFunctions.Sin(t, 0.1522f, 0);

        int pitch_ind = Mathf.RoundToInt((lfo * 0.5f + 0.5f) * (scaleNotes.Length - 1));
        bool trig = ramp > ((ramp + dMs) % beat);
        ramp = (ramp + dMs) % beat;

        if (trig)
        {
            // Send a MIDI note with message "Scale" every measure
            patch.SendList("Scale", scaleNotes[pitch_ind]);
            patch.SendMidiNoteOn(0, scaleNotes[pitch_ind], 60);
            Debug.Log(scaleNotes[pitch_ind]);
        }

        // Check if it's time to send a chord message
        if (measureCount % chordMeasureInterval == 0 && ramp == 0)
        {
            // Send a bang with message "Chord" every two measures
            patch.SendBang("Chord");
        }

        // Check if it's time to send a bang for a whole note
        if (measureCount % wholeNoteInterval == 0 && ramp == 0)
        {
            // Send a bang with message "whole_note" every four measures (whole notes)
            patch.SendBang("whole_note");
            patch.SendBang("whole_time");
            wholeNoteCount++;
        }

        // Increment measure count
        if (ramp == 0)
        {
            measureCount++;
        }
    }
}
