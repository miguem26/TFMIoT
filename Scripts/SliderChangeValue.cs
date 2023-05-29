using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
public class SliderChangeValue : MonoBehaviour
{
    private TextMeshPro textMesh = null;
    public void OnSliderUpdatedBrillo(SliderEventData eventData)
    {
        if (textMesh == null)
        {
            textMesh = GetComponent<TextMeshPro>();
        }

        if (textMesh != null)
        {
            textMesh.text = $"{Mathf.RoundToInt(eventData.NewValue*100)}";
        }
    }
}
