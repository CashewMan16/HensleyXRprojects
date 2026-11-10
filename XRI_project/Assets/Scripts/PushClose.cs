using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using Unity.VisualScripting;

public class PushClose : CoreFeatures
{
    [Header("Push")]
    [SerializeField] private Transform pushableObject;
    [SerializeField] private Grabbable grabbable;
    [SerializeField] private float snapDistance = 0.05f; //distance it closes itself
    private float closedZ;
    private Bounds bounds; //size of mesh, where is front side
    private PokeInteractor[] fingees; //shoulf work for controllers allegedly
    private readonly Dictionary<PokeInteractor, bool> wasInFront = new Dictionary<PokeInteractor, bool>(); //if in front of the object, creates a copy of the values, readonly because we dont want to write over it


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if (pushableObject == null) pushableObject = transform; //autofilled fields
        if (grabbable == null) grabbable = pushableObject.GetComponent<Grabbable>();
        closedZ = pushableObject.localPosition.z; //saves the starting point
        bounds = pushableObject.GetComponent<MeshFilter>().sharedMesh.bounds; //sends the side of the mesh that we want to detect movement

        fingees = FindObjectsByType<PokeInteractor>(FindObjectsInactive.Include, FindObjectsSortMode.None); //find the fingers even while inactive, dont sort them because that takes up a lot of processing power and we dont need it

    }

    private void LateUpdate()
    {

        if (grabbable != null && grabbable.SelectingPointsCount > 0)
        {
            //if any hand is grabbing the object, do nothing, will have a stroke if you are grabing and pushing it
            wasInFront.Clear();
            return;
        }

        float frontFace = bounds.max.z;
        Vector3 pos = pushableObject.localPosition;
        bool isOpen = pos.z > closedZ + 0.01f;

        foreach (var finger in fingees)
        {
            if (!finger.isActiveAndEnabled) continue;

            Vector3 tip = pushableObject.InverseTransformPoint(finger.Origin); //origin is the middle of your finger, different for everyone
            float radius = finger.Radius / pushableObject.lossyScale.z;
            float tipBack = tip.z - radius;

            bool lineUp = tip.x > bounds.min.x && tip.x < bounds.max.x && 
                          tip.y > bounds.min.y && tip.y < bounds.max.y;
                     //&& tip.z > bounds.min.z && tip.z < bounds.max.z //if you want z

            wasInFront.TryGetValue(finger, out bool inFront);

            if (lineUp && inFront && tipBack < frontFace)
            {
                pos.z -= (frontFace - tipBack) * pushableObject.localScale.z;
                tipBack = frontFace;
            }

            wasInFront[finger] = lineUp && tipBack >= frontFace - 0.01f;
            pos.z = Mathf.Max(pos.z, closedZ);
            if (pos.z - closedZ < snapDistance) pos.z = Mathf.MoveTowards(pos.z, closedZ, 0.6f * Time.deltaTime); //close from pos to closed pos, speed

            pushableObject.localPosition = pos;

            if (isOpen && pos.z <= closedZ + 0.01f) PlayOnEnd();

        }
    }

}
