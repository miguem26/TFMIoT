using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorConfiguration : MonoBehaviour
{
 //Variables
    public bool encendido = true;
    public int tipo = 0; //0 para movimiento 1 calidad aire 2 gas
    public Material materialOn;
    public Material materialOff;
    public int actualMaterial = 0; // 0 off 1 on
    public bool cooldown = false; // cooldown del parpadeo

    public void ResetCooldownBlink(){
        cooldown = false;
    }

    public void BlinkSensor(){
        if(cooldown == false){
            blinkLight();
            Invoke("ResetCooldownBlink",2.0f);
            cooldown = true;
        }
    }

    private void blinkLight(){
        if(actualMaterial == 0){
            Material[] materials;
            materials = gameObject.GetComponentInChildren<MeshRenderer>().materials;
            materials.SetValue(materialOn,0);
            gameObject.GetComponentInChildren<MeshRenderer>().materials = materials;

            actualMaterial = 1;
        }else{
            Material[] materials;
            materials = gameObject.GetComponentInChildren<MeshRenderer>().materials;
            materials.SetValue(materialOff,0);
            gameObject.GetComponentInChildren<MeshRenderer>().materials = materials;   

            actualMaterial = 0;  
        }
    }
}
