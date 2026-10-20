using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class DoorFeatures : CoreFeatures
{
    [Header("Door Configs")]
    [SerializeField]
    private Transform doorPivot; //pivot rotation

    [SerializeField]
    private float maxAngle = 90.0f; //put the .0f to make scanning the scripts easier and faster

    [SerializeField]
    private float doorSpeed = 1.0f;

    [SerializeField]
    private bool reverseAngleDirection = false;

    [SerializeField]
    private bool doorOpen = false;

    [SerializeField]
    private bool MakeKinematicOnOpen = false;

    [Header("Interactions Configs")]

    [SerializeField]
    private XRSocketInteractor socketInteractor; //the key socket, triggered by "events"

    [SerializeField]
    private XRSimpleInteractable simpleInteractable; //
    

    private void Start()
    {
        //when a key gets close to a socket, add a listener to know when that connection is made
        // => is a lambda or arrow function, it is a shorthand way of coding to condense code
        // s = select enter or exit events
        // (? :) Ternam Conditional operator is a lot like an if statement        string ammoCount = (ammo>2)? "message if true" : "message if false";

        socketInteractor?.selectEntered.AddListener((s) =>  //listener catagorises it as an event thing and is now listening for the selectEntered event
        {
            OpenDoor();
        }                                              );
        socketInteractor?.selectExited.AddListener((s) =>
        {
            PlayOnEnd();
            socketInteractor.socketActive = featureUsage == FeatureUsage.Once ? false : true; //if featureUsage == FeaturUsage.Once is true, then socketInteractor = true, if not false

        }                                             );

        simpleInteractable?.selectEntered.AddListener((s) =>  //listener catagorises it as an event thing and is now listening
        {
            OpenDoor();
        }                                                );

        OpenDoor();
    }

   

    private void OpenDoor()
    {
        ///if the door is not open, Play OnStart Sound
        
        if (!doorOpen)
        {
            PlayOnStart();
            doorOpen = true;
            StartCoroutine(ProcessMotion());
        }

    }

    private IEnumerator ProcessMotion()
    {
        ///need to keep looking to confirm the door is open
        /// want a loop to check so it more frequent
        while (doorOpen)
        {
            var angle = doorPivot.localEulerAngles.y < 180 ? doorPivot.localEulerAngles.y : doorPivot.localEulerAngles.y - 360;
            angle = reverseAngleDirection ? Mathf.Abs(angle) : angle;

            if (angle <= maxAngle)
            {
                doorPivot?.Rotate(Vector3.up, doorSpeed * Time.deltaTime * (reverseAngleDirection ? -1 : 1)); //axis rotating on, speed
            }
            //when done, want to turn on the rigid body
            else
            {
                doorOpen = false;
                var featureRigidbody = GetComponent<Rigidbody>();
                if (featureRigidbody != null && MakeKinematicOnOpen)
                    featureRigidbody.isKinematic = true;
            }

            yield return null;
        }
    }

}
