using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public Text WheelsText;
    public Text TimerText;

    public void SetWheel(int nWheels)
    {
        WheelsText.text = "Wheels: " + nWheels;
    }

    public void SetTimer(float fTimer)
    {
        TimerText.text = "Timer: " + (int)fTimer;
    }
}
