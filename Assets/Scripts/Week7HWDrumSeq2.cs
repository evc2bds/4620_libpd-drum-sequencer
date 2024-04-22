using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Week7HWDrumSeq2 : MonoBehaviour
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
    [SerializeField]
    List<bool> toms;
    public List<AudioClip> sounds;
    string[] drum_type = new string[] { "Kick", "Snare", "Sticks", "Toms" };
    List<float> envelopes;
    List<bool>[] gates = new List<bool>[4];
    Vector4 adsr_params;

    GameObject[] StepsObjs;
    // [SerializeField] List<bool> Steps;

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
        gates[3] = toms;
        adsr_params = new Vector4(100, 50, 0.4f, 200);

        //generate objects for each of the array elements 
        StepsObjs = new GameObject[sounds.Count];


        for (int i=0; i < sounds.Count; i++) {
            StepsObjs[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            StepsObjs[i].transform.position = new Vector3(i, 0, 0);
            //reduce scale?
            StepsObjs[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
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
            if (toms[count])
            {
                patch.SendBang("toms_bang");
            }

            count = (count + 1) % kick.Count;
        }

        for (int i = 0; i < sounds.Count; i++)
        {
            envelopes[i] = ControlFunctions.ADSR(ramp / 1000, gates[i][count], adsr_params);

            // Adjust the Y-position of the StepsObjs based on the ADSR envelope
            StepsObjs[i].transform.position = new Vector3(i, envelopes[i]*5, 0);

        }
    }
}
