using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public Text WheelsText;
    public Text TimerText;

    public GameObject StartScreen;

    public GameObject EndScreen;
    public Text EndScreen_Wheels;
    public Text EndScreen_Timer;
    public Image Star1;
    public Image Star2;
    public Image Star3;

    private AudioSource AudioSource;
    public AudioClip WowStar1;
    public AudioClip WowStar2;
    public AudioClip WowStar3;

    private int StarCount = 1;

    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    public void SetWheel(int nWheels)
    {
        WheelsText.text = nWheels.ToString();
    }

    public void SetTimer(float fTimer)
    {
        TimerText.text = "Timer: " + (int)fTimer;
    }

    public void HideStartScreen()
    {
        StartScreen.SetActive(false);
    }

    public void DisplayEndScreen(int CurrentWheels, int MaxWheels, float fTimerRemaining, float fTimer)
    {
        EndScreen.SetActive(true);
        EndScreen_Wheels.text = CurrentWheels.ToString() + " / " + MaxWheels.ToString();
        EndScreen_Timer.text = fTimerRemaining.ToString() + " / " + fTimer.ToString();

        if (fTimerRemaining > 0.0f)
        {
            StarCount = 2;

            if (fTimerRemaining > 0.6f)
            {
                StarCount = 3;
            }
        }

        StartCoroutine("AnimateEndScreen");
    }

    IEnumerator AnimateEndScreen()
    {
        EndScreen_Timer.enabled = true;
        yield return new WaitForSeconds(1.0f);

        EndScreen_Wheels.enabled = true;
        yield return new WaitForSeconds(1.0f);

        Star1.enabled = true;
        AudioSource.clip = WowStar1;
        AudioSource.Play();
        yield return new WaitForSeconds(1.0f);

        if (StarCount > 1)
        {
            Star2.enabled = true;
            AudioSource.clip = WowStar2;
            AudioSource.Play();
            yield return new WaitForSeconds(1.0f);

            if (StarCount > 2)
            {
                Star3.enabled = true;
                AudioSource.clip = WowStar3;
                AudioSource.Play();
            }
        }
    }
}
