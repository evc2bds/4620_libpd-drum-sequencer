
//_________________________________________________________________________________________________________________________
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class GenerativeDrums : MonoBehaviour
// {
//     public LibPdInstance patch;
//     float ramp;
//     float t;
//     int count = 0;
//     int measureCount = 0; // Counter for measures passed

//     [SerializeField] List<bool> kick;
//     [SerializeField] List<bool> snare;
//     [SerializeField] List<bool> sticks;
//     [SerializeField] List<bool> toms;
//     public List<AudioClip> sounds;
//     string[] drum_type = new string[] { "Kick", "Snare", "Sticks", "Toms" };
//     List<float> envelopes;
//     List<bool>[] gates = new List<bool>[4];
//     Vector4 adsr_params;

//     GameObject[] StepsObjs;
//     GameObject[] kickStepsObjs;
//     [SerializeField] List<bool> Steps;

//     //variable for beat duration in milliseconds (4 beats per second)
//     [SerializeField] int beat = 250; 

//     void Start()
//     {
//         envelopes = new List<float>();

//         // Setting up sounds and envelopes
//         for (int i = 0; i < sounds.Count; i++)
//         {
//             string name = sounds[i].name + ".wav";
//             patch.SendSymbol(drum_type[i], name);
//             envelopes.Add(0);
//         }

//         GeneratePatterns(); // Generate initial random patterns

//         adsr_params = new Vector4(100, 50, 0.4f, 200); //ADSR Parameters

//         StepsObjs = new GameObject[Steps.Count]; 
//         kickStepsObjs = new GameObject[Steps.Count]; 

//         //create a game object for each step 
//         for (int i = 0; i < Steps.Count; i++)
//         {
//             StepsObjs[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//             StepsObjs[i].transform.position = new Vector3(i, 0, 0);
//             StepsObjs[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
//         }

//         //create a game object for each kick step 
//         for (int i = 0; i < Steps.Count; i++)
//         {
//             kickStepsObjs[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//             kickStepsObjs[i].transform.position = new Vector3(i, 8, 0);
//             kickStepsObjs[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
//         }
//     }

//     void Update()
//     {
//         t += Time.deltaTime;
//         int dMs = Mathf.RoundToInt(Time.deltaTime * 1000);
//         bool trig = ramp > ((ramp + dMs) % beat); // check if beat has occurred
//         ramp = (ramp + dMs) % beat;

//         if (trig)
//         {
//             // Send bangs to Pure Data based on patterns
//             if (kick[count])
//             {
//                 patch.SendBang("kick_bang");
//             }
//             if (snare[count])
//             {
//                 patch.SendBang("snare_bang");
//             }
//             if (sticks[count])
//             {
//                 patch.SendBang("sticks_bang");
//             }
//             if (toms[count])
//             {
//                 patch.SendBang("toms_bang");
//             }

//             count = (count + 1) % kick.Count;

//             // check if 4 measures have passed -- regenerate patterns
//             if (count == 0)
//             {
//                 measureCount++;
//                 if (measureCount >= 4)
//                 {
//                     GeneratePatterns();
//                     measureCount = 0; // Reset measure count
//                 }
//             }
//         }

        
//         for (int i = 0; i < sounds.Count; i++)
//         {
//             envelopes[i] = ControlFunctions.ADSR(ramp / (float)beat, gates[i][count], adsr_params); // Adjusting the ADSR time scale
//             if (gates[i][count])
//             {
//                 StepsObjs[count].transform.position = new Vector3(count, envelopes[i], 0);
//             }
//             if (gates[0][count]) {
//                 kickStepsObjs[count].transform.position = new Vector3(count, envelopes[i]+3, 0);
//             }
//         }
//     }

//     private void GeneratePatterns()
//     {
//         //probabilities for drum matterns -- must be length 8
//         float[] kickProbabilities = { 1.0f, 0.8f, 0.5f, 0.45f, 1.0f, 0.7f, 0.5f, 0.6f };
//         float[] snareProbabilities = { 0.3f, 0.2f, 0.5f, 0.6f, 0.4f, 0.3f, 0.2f, 0.1f };
//         float[] sticksProbabilities = { .01f, 0.01f, 1.0f, 0.65f, 0.01f, 0.01f, 1.0f, 0.5f };
//         float[] tomsProbabilities = { 0.02f, 0.04f, 0.3f, 0.05f, 0.06f, 0.4f, 0.07f, 0.3f };

//         //generate random patterns based on probabilities (probability of a bang at each step)
//         kick = GenerateRandomPattern(kickProbabilities);
//         snare = GenerateRandomPattern(snareProbabilities);
//         sticks = GenerateRandomPattern(sticksProbabilities);
//         toms = GenerateRandomPattern(tomsProbabilities);

//         gates[0] = kick;
//         gates[1] = snare;
//         gates[2] = sticks;
//         gates[3] = toms;
//     }

//     private List<bool> GenerateRandomPattern(float[] probabilities)
//     {
//         if (probabilities.Length != 8) //probabilities array is wrong length
//         {
//             return null;
//         }

//         List<bool> pattern = new List<bool>();

//         //generate random pattern based on probabilities
//         for (int i = 0; i < 8; i++)
//         {
//             pattern.Add(UnityEngine.Random.value < probabilities[i]);
//         }
//         return pattern;
//     }
// }

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
    GameObject[] kickStepsObjs;
    GameObject[] snareStepsObjs;
    GameObject[] sticksStepsObjs;
    GameObject[] highTomStepsObjs;
    [SerializeField] List<bool> Steps;

    //variable for beat duration in milliseconds (4 beats per second)
    [SerializeField] int beat = 250; 

    public Material yellowMaterial; // Assign this in the Unity Editor

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

        // StepsObjs = new GameObject[Steps.Count]; 
        kickStepsObjs = new GameObject[Steps.Count]; 
        snareStepsObjs = new GameObject[Steps.Count]; 
        sticksStepsObjs = new GameObject[Steps.Count]; 
        highTomStepsObjs = new GameObject[Steps.Count]; 

        //create a game object for each step 
        for (int i = 0; i < Steps.Count; i++)
        {
            // StepsObjs[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // StepsObjs[i].transform.position = new Vector3(i, 0, 0);
            // StepsObjs[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            //create gameObject for each stick sound step: 
            sticksStepsObjs[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            float yVar = i - 3.5f;
            sticksStepsObjs[i].transform.position = new Vector3(-7.4f, yVar*2.1f, 10);
            sticksStepsObjs[i].transform.localScale = new Vector3(2.1f, 2.1f, 2.1f);

            //create gameObject for each highTom sound step: 
            highTomStepsObjs[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            highTomStepsObjs[i].transform.position = new Vector3(7.4f, yVar*2.1f, 10);
            highTomStepsObjs[i].transform.localScale = new Vector3(2.1f, 2.1f, 2.1f);

            //create gameObject for each kick sound step: 
            kickStepsObjs[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            float xVar = i - 3.5f;
            kickStepsObjs[i].transform.position = new Vector3(xVar*2.1f, 7.4f, 10);
            kickStepsObjs[i].transform.localScale = new Vector3(2.1f, 2.1f, 2.1f);

            //create gameObject for each snare sound step: 
            snareStepsObjs[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            snareStepsObjs[i].transform.position = new Vector3(xVar*2.1f, -7.4f, 10);
            snareStepsObjs[i].transform.localScale = new Vector3(2.1f, 2.1f, 2.1f);
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

                // Change the material to yellow when kick is played
                Material originalMaterial = kickStepsObjs[count].GetComponent<Renderer>().material;
                kickStepsObjs[count].GetComponent<Renderer>().material = yellowMaterial;
                StartCoroutine(ResetMaterialAfterDelay(kickStepsObjs[count], originalMaterial, beat));
                if (count == 0) {
                    sticksStepsObjs[7].GetComponent<Renderer>().material = yellowMaterial;
                    StartCoroutine(ResetMaterialAfterDelay(sticksStepsObjs[7], originalMaterial, beat));
                }
                if (count == 7) {
                    highTomStepsObjs[7].GetComponent<Renderer>().material = yellowMaterial;
                    StartCoroutine(ResetMaterialAfterDelay(highTomStepsObjs[7], originalMaterial, beat));
                }
            }
            if (snare[count])
            {
                patch.SendBang("snare_bang");
                // Change the material to yellow when snare is played
                Material originalMaterial = snareStepsObjs[count].GetComponent<Renderer>().material;
                snareStepsObjs[count].GetComponent<Renderer>().material = yellowMaterial;
                StartCoroutine(ResetMaterialAfterDelay(snareStepsObjs[count], originalMaterial, beat));
                if (count == 0) {
                    sticksStepsObjs[0].GetComponent<Renderer>().material = yellowMaterial;
                    StartCoroutine(ResetMaterialAfterDelay(sticksStepsObjs[0], originalMaterial, beat));
                }
                if (count == 7) {
                    highTomStepsObjs[0].GetComponent<Renderer>().material = yellowMaterial;
                    StartCoroutine(ResetMaterialAfterDelay(highTomStepsObjs[0], originalMaterial, beat));
                }

            }
            if (sticks[count])
            {
                patch.SendBang("sticks_bang");
                // Change the material to yellow when sticks is played
                Material originalMaterial = sticksStepsObjs[count].GetComponent<Renderer>().material;
                sticksStepsObjs[count].GetComponent<Renderer>().material = yellowMaterial;
                StartCoroutine(ResetMaterialAfterDelay(sticksStepsObjs[count], originalMaterial, beat));

            }
            if (toms[count])
            {
                patch.SendBang("toms_bang");
                // Change the material to yellow when sticks is played
                Material originalMaterial = highTomStepsObjs[count].GetComponent<Renderer>().material;
                highTomStepsObjs[count].GetComponent<Renderer>().material = yellowMaterial;
                StartCoroutine(ResetMaterialAfterDelay(highTomStepsObjs[count], originalMaterial, beat));
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

        
        // for (int i = 0; i < sounds.Count; i++)
        // {
        //     envelopes[i] = ControlFunctions.ADSR(ramp / (float)beat, gates[i][count], adsr_params); // Adjusting the ADSR time scale
        //     if (gates[i][count])
        //     {
        //         StepsObjs[count].transform.position = new Vector3(count, envelopes[i], 0);
        //     }
        // }
    }

    private void GeneratePatterns()
    {
        //probabilities for drum matterns -- must be length 8
        float[] kickProbabilities = { 1.0f, 0.8f, 0.5f, 0.45f, 1.0f, 0.7f, 0.5f, 0.6f };
        float[] snareProbabilities = { 0.3f, 0.2f, 0.5f, 0.6f, 0.4f, 0.3f, 0.2f, 0.1f };
        float[] sticksProbabilities = { 0.0f, 0.1f, 1.0f, 0.65f, 0.01f, 0.01f, 1.0f, 0.0f };
        float[] tomsProbabilities = { 0.0f, 0.4f, 0.3f, 0.05f, 0.06f, 0.4f, 0.07f, 0.0f };

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

    IEnumerator ResetMaterialAfterDelay(GameObject obj, Material originalMaterial, int delayMs)
    {
        yield return new WaitForSeconds(delayMs / 1000f); // Convert milliseconds to seconds
        obj.GetComponent<Renderer>().material = originalMaterial;
    }
}
