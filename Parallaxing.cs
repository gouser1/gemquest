using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour
{

    public Transform[] backgrounds;             // Array of back- and foregrounds to be parallaxed
    private float[] parallaxScales;             // The amount of camera movement to move the background by
    public float smoothing;                     // Smoothness of parallax

    private Transform cam;                      // Main camera transform reference
    private Vector3 previousCamPos;             // Position of camera in previous frame

    //Called before Start()
    void Awake()
    {
        //Camera reference setup
        cam = Camera.main.transform;
    }

    // Use this for initialization
    void Start()
    {
        // Storing previous frame
        previousCamPos = cam.position;

        // Asigning coresponding parallaxScales
        parallaxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }

    }

    // Update is called once per frame
    void Update()
    {

        // For each background
        for (int i = 0; i < backgrounds.Length; i++)
        {
            // Parallax is the oposite of the camera movement because previous frame multiplied by the scale
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];

            //Set a target x position which is the current position + the parallax
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            // Create a targat positon which is the background's current position with it's target x position
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            // Fade between current position and the target position using lerp

            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        // Set the previousCamPos to the camera's position at the end of the grame

        previousCamPos = cam.position;

    }
}
