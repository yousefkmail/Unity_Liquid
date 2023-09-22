using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.UIElements;
using Scrtwpns.Mixbox;

[RequireComponent(typeof(SphereCollider))]
public class Beaker : MonoBehaviour
{
    [Tooltip("Value between 0 and 1 to determine the amount of liquid in the Beaker")]
    [Range(0, 1)]
    [SerializeField] public float BeakerFillAmount = 0;

    [Tooltip("Radius of the beaker,this will determine the collider radius")]
    [Min(0.001f)]

    [SerializeField] public float BeakerRadius = 0.05f;


    [SerializeField] float drainingSpeed = 0.01f;


    [ColorUsage(true, true)]
    [SerializeField] public Color DeepLiquidColor;

    [ColorUsage(true, true)]

    [SerializeField] public Color ShallowLiquidColor;

    [Tooltip("When the Beaker receive more liquid, Should the color receive the new color of the object.")]

    [SerializeField] bool ReceiveColorChange = true;


    [Min(0)]
    [SerializeField] public float StirringSpeed = 0;

    [Min(0)]

    [SerializeField] public float StirringStrength = 0;

    [SerializeField] public ParticleSystem WaterPouring;
    [SerializeField] public GameObject BeakerLiquid;

    [SerializeField] AnimationCurve AngleToPourAmountMapping;

    [SerializeField] public bool AddEdgesGlow = false;
    [Range(0, 1)]
    [HideInInspector] public float GlowStrength = 1;


    public void PourWater()
    {

        GetComponent<Animator>().SetBool("IsPour", true);
    }

    public void StopPouring()
    {

        GetComponent<Animator>().SetBool("IsPour", false);
    }


    //called when the properties of beaker is changed in edit mode to visualize the difference instead of
    //keep entering the play mode.


    void Update()
    {
        if (AngleToPourAmountMapping.Evaluate(BeakerFillAmount) < Vector3.Angle(Vector3.up, transform.up) && BeakerFillAmount > 0)
        {
            if (!WaterPouring.isEmitting)
                WaterPouring.Play();
            BeakerFillAmount -= Time.deltaTime * drainingSpeed;
            if (BeakerFillAmount < 0) BeakerFillAmount = 0;
            BeakerLiquid.GetComponent<MeshRenderer>().material.SetFloat("_LiquidAmount", BeakerFillAmount);
        }
        else
            WaterPouring.Stop();

    }



    //handleing filling the Beaker
    void OnParticleCollision(GameObject other)
    {
        AddLiquid(other);
    }

    public void AddLiquid(GameObject other)
    {
        BeakerFillAmount += 0.002f;

        if (BeakerFillAmount > 1) BeakerFillAmount = 1;

        BeakerLiquid.GetComponent<MeshRenderer>().material.SetFloat("_LiquidAmount", BeakerFillAmount);


        if (!ReceiveColorChange) return;

        Color firstColor = other.transform.root.GetComponent<Beaker>().DeepLiquidColor;
        Color secondColor = other.transform.root.GetComponent<Beaker>().ShallowLiquidColor;
        DeepLiquidColor = Mixbox.Lerp(firstColor, DeepLiquidColor, (BeakerFillAmount - 0.002f) / BeakerFillAmount);
        ShallowLiquidColor = Mixbox.Lerp(secondColor, ShallowLiquidColor, (BeakerFillAmount - 0.002f) / BeakerFillAmount);
        BeakerLiquid.GetComponent<MeshRenderer>().material.SetColor("_DeepColor", DeepLiquidColor);
        BeakerLiquid.GetComponent<MeshRenderer>().material.SetColor("_ShallowColor", ShallowLiquidColor);

    }


}
