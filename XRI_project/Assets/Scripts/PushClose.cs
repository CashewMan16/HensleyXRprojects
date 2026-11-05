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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
