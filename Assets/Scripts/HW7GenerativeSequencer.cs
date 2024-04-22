using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HW7GenerativeSequencer : MonoBehaviour
{
    public LibPdInstance patch;
    float ramp;
    float t;
    int count = 0;

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

    //variable for beat duration in milliseconds
    [SerializeField] int beat = 250; // Default value is 1000 milliseconds (1 second)

    void Start()
    {
        envelopes = new List<float>();

        for (int i = 0; i < sounds.Count; i++)
        {
            string name = sounds[i].name + ".wav";
            patch.SendSymbol(drum_type[i], name);
            envelopes.Add(0);
        }

        float[] kickProbabilities = { 1.0f, 0.8f, 0.5f, 0.45f, 1.0f, 0.7f, 0.5f, 0.6f };
        float[] snareProbabilities = { 0.3f, 0.2f, 0.5f, 0.6f, 0.4f, 0.3f, 0.2f, 0.1f };
        float[] sticksProbabilities = { .01f, 0.01f, 1.0f, 0.65f, 0.01f, 0.01f, 1.0f, 0.5f };
        float[] tomsProbabilities = { 0.2f, 0.4f, 0.3f, 0.5f, 0.6f, 0.4f, 0.7f, 0.3f };

        // Initialize drum patterns with specified probabilities
        kick = GenerateRandomPattern(kickProbabilities);
        snare = GenerateRandomPattern(snareProbabilities);
        sticks = GenerateRandomPattern(sticksProbabilities);
        toms = GenerateRandomPattern(tomsProbabilities);

        gates[0] = kick;
        gates[1] = snare;
        gates[2] = sticks;
        gates[3] = toms;

        adsr_params = new Vector4(100, 50, 0.4f, 200);

        StepsObjs = new GameObject[Steps.Count];

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
        bool trig = ramp > ((ramp + dMs) % beat); // Using the 'beat' variable here
        ramp = (ramp + dMs) % beat;

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
            envelopes[i] = ControlFunctions.ADSR(ramp / (float)beat, gates[i][count], adsr_params); // Adjusting the ADSR time scale
            if (gates[i][count])
            {
                StepsObjs[count].transform.position = new Vector3(count, envelopes[i], 0);
            }
        }
    }

    private List<bool> GenerateRandomPattern(float[] probabilities)
    {
        if (probabilities.Length != 8)
        {
            Debug.LogError("Invalid probabilities array length. Please provide an array of length 8.");
            return null;
        }

        List<bool> pattern = new List<bool>();

        for (int i = 0; i < 8; i++)
        {
            pattern.Add(UnityEngine.Random.value < probabilities[i]);
        }
        return pattern;
    }
}
