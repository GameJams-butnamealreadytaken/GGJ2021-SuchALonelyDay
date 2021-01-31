using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject CarPrefab;
    private GameObject Car; // Instance of the prefab

    public GUIManager GUIManager;

    public CameraManager CameraManager;

    public int CollectedWheels = 0;

    public float fTimer = 180.0f;
   
    void Start()
    {
        Car = Instantiate(CarPrefab, gameObject.transform);
        CameraManager.CarToFollow = Car;
        Car.GetComponent<CarManager>().GameManager = this;

        //TODO Generate wheels
    }

    void Update()
    {
        fTimer -= Time.deltaTime;

        GUIManager.SetTimer(fTimer);

        if (fTimer <= 0.0f)
        {
            //TODO Game ended
        }
    }

    public void AddCapturedWheel()
    {
        CollectedWheels += 1;
        GUIManager.SetWheel(CollectedWheels);
    }
}
