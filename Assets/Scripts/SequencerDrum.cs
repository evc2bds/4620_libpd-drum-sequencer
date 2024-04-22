using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencerDrum : MonoBehaviour
{
    public LibPdInstance patch;
    float ramp;
    float t;
    int[] mode;
    int count = 0;
    

    [SerializeField]
    List<bool> kick;
    [SerializeField]
    List<bool> snare;
    [SerializeField]
    List<bool> sticks;
    public List<AudioClip> sounds;
    string[] drum_type = new string[] { "Kick", "Snare", "Sticks" };
    List<float> envelopes;
    List<bool>[] gates = new List<bool>[3];
    Vector4 adsr_params;

    void Start()
    {
        envelopes = new List<float>();

        for(int i = 0; i < sounds.Count; i++)
        {
            //send sound files names to patch
            //add .wav
            //drum type is both the name of receive obj 
            //and of Drums folder subdirectory for sound
            string name = sounds[i].name + ".wav"; //get the name of the sound and add .wav for PD
            patch.SendSymbol(drum_type[i], name);
            //build list of envelopes
            envelopes.Add(0); //add new value for each one of the sounds
        }
        gates[0] = kick;
        gates[1] = snare;
        gates[2] = sticks;
        adsr_params = new Vector4(100, 50, 0.4f, 200);
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        bool trig = ramp > (ramp + Time.deltaTime) % 1;
        ramp = (ramp + Time.deltaTime) % 1;

        if (trig)
        {
            if (kick[count])
            {
                patch.SendBang("kick_bang");
            } 
            if (snare[count])
            {
                patch.SendBang("snare_bang");
            }
            if (sticks[count])
            {
                patch.SendBang("sticks_bang");
            }
            count = (count + 1) % kick.Count;
        }

        for (int i = 0; i < sounds.Count; i++)
        {
            envelopes[i] = ControlFunctions.ADSR(ramp / 1000, gates[i][count], adsr_params);

            // // Set the y position of the object using the envelopes
            // transform.position = new Vector3(transform.position.x, envelopes[i], transform.position.z);
        }
    }
}
