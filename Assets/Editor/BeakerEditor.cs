using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Unity.VisualScripting;


[CustomEditor(typeof(Beaker))]
public class BeakerEditor : Editor
{


    SerializedProperty AddEdgeGlow;
    SerializedProperty EdgeGlowStrength;


    void OnEnable()
    {
        AddEdgeGlow = serializedObject.FindProperty(nameof(Beaker.AddEdgesGlow));
        EdgeGlowStrength = serializedObject.FindProperty(nameof(Beaker.GlowStrength));

    }

    public override void OnInspectorGUI()
    {
        Debug.Log(((Beaker)target).WaterPouring.transform.position);
        ((Beaker)target).WaterPouring.transform.localPosition = new Vector3(0, 0.06f, ((Beaker)target).BeakerRadius);

        base.OnInspectorGUI();
        UpdateGlow(AddEdgeGlow.boolValue);
        if (AddEdgeGlow.boolValue)
        {

            EditorGUILayout.PropertyField(EdgeGlowStrength);
        }
        UpdateVisuals((Beaker)target);
        serializedObject.ApplyModifiedProperties();
    }

    private void UpdateGlow(bool IsGlowing)
    {
        if (IsGlowing)
            ((Beaker)target).BeakerLiquid.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_GlowColor", Color.Lerp(Color.black, Color.white, EdgeGlowStrength.floatValue));
        else
            ((Beaker)target).BeakerLiquid.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_GlowColor", Color.black);

    }

    public void UpdateVisuals(Beaker beaker)
    {
        UpdateMaterials(beaker);
        beaker.GetComponent<SphereCollider>().radius = beaker.BeakerRadius;
    }

    private void UpdateMaterials(Beaker beaker)
    {
        beaker.BeakerLiquid.GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_LiquidAmount", beaker.BeakerFillAmount);
        beaker.BeakerLiquid.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_DeepColor", beaker.DeepLiquidColor);
        beaker.BeakerLiquid.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_ShallowColor", beaker.ShallowLiquidColor);
        beaker.BeakerLiquid.GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_Stirringspeed", beaker.StirringSpeed);
        beaker.BeakerLiquid.GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_StirringValue", beaker.StirringStrength);
        beaker.WaterPouring.GetComponent<ParticleSystemRenderer>().sharedMaterial.SetColor("_BaseColor", beaker.ShallowLiquidColor);


        // Determine wether the beaker is horizontal or vertical
        //If Vertical the liquid offset will be higher
        float BeakerAngleCosin = Mathf.Abs(Mathf.Cos(Vector3.Angle(beaker.transform.up, Vector3.up) * Mathf.Deg2Rad));
        // we have the base offset which is when the beaker is at its minimum height
        // then we offset it by a number based on its angle
        float value2 = 0.06F + (BeakerAngleCosin * 0.02f);
        Vector2 vector2 = new Vector2(-value2, value2);
        beaker.BeakerLiquid.GetComponent<MeshRenderer>().sharedMaterial.SetVector("_LiquidHeightOffest", vector2);

    }

}
