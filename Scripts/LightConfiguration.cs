using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightConfiguration : MonoBehaviour
{
    //Contiene todas las variables configurables de la luz
    public bool encendido = true;
    public bool autoEncendido = false;
    public float colorRed = 1.0f;
    public float colorGreen = 1.0f;
    public float colorBlue = 0.0f;
    public float intensity = 0.5f;
    //Configurable
    public bool morning = false;
    public bool evening = false;
    public bool night = false;

}
