using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTempConfiguration : MonoBehaviour
{
    //Variables
    public bool encendido = true;
    public bool autoEncendido = false;
    public int tipo = 0; //0 para calor 1 para frio
    public int temp = 22;
    public int modo = 0; // 0 manual 1 automatico
    //Configurable
    public bool checkMan = false;
    public int tempMan = 22;
    public bool checkNoche = false;
    public int tempNoche = 22;
    public Material materialOn;
    public Material materialOff;

    
    public bool cooldown = false; // cooldown del parpadeo

    public void ResetCooldownBlink(){
        cooldown = false;
    }

    public void resetTime(){
        if(cooldown == false){
            Invoke("ResetCooldownBlink",5.0f);
            cooldown = true;
        }
    }
}
