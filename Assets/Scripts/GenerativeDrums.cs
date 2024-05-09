using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerativeDrums : MonoBehaviour
{
    public LibPdInstance patch;
    float ramp;
    float t;
    int count = 0;
    int measureCount = 0; // Counter for measures passed

    [SerializeField] List<bool> kick;
    [SerializeField] List<bool> snare;
    [SerializeField] List<bool> sticks;
    [SerializeField] List<bool> toms;
    public List<AudioClip> sounds;
    string[] drum_type = new string[] { "Kick", "Snare", "Sticks", "Toms" };
    List<float> envelopes;
    List<bool>[] gates = new List<bool>[4];
    Vector4 adsr_params;

    GameObject[] StepsObjs;
    [SerializeField] List<bool> Steps;

    //variable for beat duration in milliseconds (4 beats per second)
    [SerializeField] int beat = 250; 

    void Start()
    {
        envelopes = new List<float>();

        // Setting up sounds and envelopes
        for (int i = 0; i < sounds.Count; i++)
        {
            string name = sounds[i].name + ".wav";
            patch.SendSymbol(drum_type[i], name);
            envelopes.Add(0);
        }

        GeneratePatterns(); // Generate initial random patterns

        adsr_params = new Vector4(100, 50, 0.4f, 200); //ADSR Parameters

        StepsObjs = new GameObject[Steps.Count]; 

        //create a game object for each step 
        for (int i = 0; i < Steps.Count; i++)
        {
            StepsObjs[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            StepsObjs[i].transform.position = new Vector3(i, 0, 0);
            StepsObjs[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }

    void Update()
    {
        t += Time.deltaTime;
        int dMs = Mathf.RoundToInt(Time.deltaTime * 1000);
        bool trig = ramp > ((ramp + dMs) % beat); // check if beat has occurred
        ramp = (ramp + dMs) % beat;

        if (trig)
        {
            // Send bangs to Pure Data based on patterns
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

            // check if 4 measures have passed -- regenerate patterns
            if (count == 0)
            {
                measureCount++;
                if (measureCount >= 4)
                {
                    GeneratePatterns();
                    measureCount = 0; // Reset measure count
                }
            }
        }

        
        for (int i = 0; i < sounds.Count; i++)
        {
            envelopes[i] = ControlFunctions.ADSR(ramp / (float)beat, gates[i][count], adsr_params); // Adjusting the ADSR time scale
            if (gates[i][count])
            {
                StepsObjs[count].transform.position = new Vector3(count, envelopes[i], 0);
            }
        }
    }

    private void GeneratePatterns()
    {
        //probabilities for drum matterns -- must be length 8
        float[] kickProbabilities = { 1.0f, 0.8f, 0.5f, 0.45f, 1.0f, 0.7f, 0.5f, 0.6f };
        float[] snareProbabilities = { 0.3f, 0.2f, 0.5f, 0.6f, 0.4f, 0.3f, 0.2f, 0.1f };
        float[] sticksProbabilities = { .01f, 0.01f, 1.0f, 0.65f, 0.01f, 0.01f, 1.0f, 0.5f };
        float[] tomsProbabilities = { 0.02f, 0.04f, 0.3f, 0.05f, 0.06f, 0.4f, 0.07f, 0.3f };

        //generate random patterns based on probabilities (probability of a bang at each step)
        kick = GenerateRandomPattern(kickProbabilities);
        snare = GenerateRandomPattern(snareProbabilities);
        sticks = GenerateRandomPattern(sticksProbabilities);
        toms = GenerateRandomPattern(tomsProbabilities);

        gates[0] = kick;
        gates[1] = snare;
        gates[2] = sticks;
        gates[3] = toms;
    }

    private List<bool> GenerateRandomPattern(float[] probabilities)
    {
        if (probabilities.Length != 8) //probabilities array is wrong length
        {
            return null;
        }

        List<bool> pattern = new List<bool>();

        //generate random pattern based on probabilities
        for (int i = 0; i < 8; i++)
        {
            pattern.Add(UnityEngine.Random.value < probabilities[i]);
        }
        return pattern;
    }
}