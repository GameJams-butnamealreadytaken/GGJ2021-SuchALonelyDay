using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject CarPrefab;
    private GameObject Car; // Instance of the prefab

    public GUIManager GUIManager;

    public CameraManager CameraManager;

    public int CollectedWheels = 0;

    public float fStartTimer = 180.0f;
    private float fTimer;

    public ParticleSystem[] WheelDeathFX;

    public GameObject[] WheelList;
    private int WheelNumber;

    public bool bGameIsOver = false;

    public bool bStartScreen = true;

    void Start()
    {
        fTimer = fStartTimer;

        //TODO Generate wheels
        // + set this to their gamemanager
        WheelNumber = WheelList.Length;
    }

    void Update()
    {
        if (bGameIsOver)
            return;

        if (bStartScreen)
        {
            if (Input.GetAxis("Jump") > 0.0f)
            {
                CreateCar();
                bStartScreen = false;
                GUIManager.HideStartScreen();
            }

            return;
        }

        fTimer -= Time.deltaTime;

        GUIManager.SetTimer(fTimer);

        if (fTimer <= 0.0f || CollectedWheels >= WheelNumber)
        {
            GUIManager.DisplayEndScreen(CollectedWheels, WheelNumber, fTimer, fStartTimer);
            bGameIsOver = true;
        }

        if (Input.GetAxis("ResetCar") > 0.0f)
        {
            Destroy(Car);
            CreateCar();
        }

        if (Input.GetAxis("ReloadLevel") > 0.0f)
        {
            SceneManager.LoadScene(0);
        }
    }

    private void CreateCar()
    {
        Car = Instantiate(CarPrefab, gameObject.transform);
        CameraManager.CarToFollow = Car;
        Car.GetComponent<CarManager>().GameManager = this;
    }

    public void AddCapturedWheel()
    {
        CollectedWheels += 1;
        GUIManager.SetWheel(CollectedWheels);
    }

    public void PlayWheelDeathFX(Vector3 vWheelLocation)
    {
        Instantiate(WheelDeathFX[Random.Range(0, WheelDeathFX.Length - 1)], vWheelLocation, Quaternion.identity);
    }
}
