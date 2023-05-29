using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;


public class GameController : MonoBehaviour
{
    void Start()
    {
        //Para que siempre este el puntero de la cabeza
        PointerUtils.SetGazePointerBehavior(PointerBehavior.AlwaysOn);
    }

    public void CloseApp(){
        Application.Quit();
    }
}
