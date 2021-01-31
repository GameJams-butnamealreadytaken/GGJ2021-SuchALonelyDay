using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
	public GameManager GameManager;

	public LegManager LegLeft;
	public LegManager LegRight;

	public GameObject LegLeftSlidingTarget;
	public GameObject LegRightSlidingTarget;

	public Rigidbody rb;

	private bool bStopped = true;
	private bool bResetingBodyCar = false;

	private float fSlidingTime = 0.0f;

	public float fLaunchSpeed = 15000.0f;
	public float fRotationSpeed = 50.0f;

	public float fMaxLegDistanceFromTarget = 0.1f;

	private int NextLegToMove = 0; // 0:left, 1:right

    void FixedUpdate()
    {
		if (bStopped)
		{
			if (Input.GetAxis("Jump") > 0.0f)
			{
				// Launch the car
				rb.isKinematic = false;
				rb.AddForce(gameObject.transform.up * -fLaunchSpeed); // Using up*-1 because Car has a base rotation of -90
				bStopped = false;
				fSlidingTime = 0.0f;
			}
			else if (Input.GetAxis("Horizontal") != 0.0f)
			{
				transform.rotation = Quaternion.RotateTowards(transform.rotation, transform.rotation * Quaternion.Euler(Vector3.forward * Input.GetAxis("Horizontal")), fRotationSpeed * Time.deltaTime);

				if (!LegRight.bIsMoving
					&& NextLegToMove == 0
					&& LegLeft.fDistanceFromTarget >= fMaxLegDistanceFromTarget)
                {
					LegLeft.SetFootToInitialTarget(false);
					NextLegToMove = 1;
				}
				
				if (!LegLeft.bIsMoving
					&& NextLegToMove == 1
					&& LegRight.fDistanceFromTarget >= fMaxLegDistanceFromTarget)
				{
					LegRight.SetFootToInitialTarget(false);
					NextLegToMove = 0;
				}
			}
		}
		else if (!bResetingBodyCar)
        {
			fSlidingTime += Time.fixedDeltaTime;

			// Check if no more sliding
			if (fSlidingTime > 0.2f)
            {
				if (rb.velocity.sqrMagnitude < 0.1f)
				{
					rb.isKinematic = true;

					// Reset car stence to "prepare launch"
					LegLeft.SetFootToInitialTarget(true);
					LegRight.SetFootToInitialTarget(true);
					StartCoroutine("ResetBodyCar");
					bResetingBodyCar = true;
				}
                else
				{
					LegLeft.SetFootTarget(LegLeftSlidingTarget.transform.position);
					LegRight.SetFootTarget(LegRightSlidingTarget.transform.position);
				}
			}
        }
    }

	IEnumerator ResetBodyCar()
    {
		yield return new WaitForSeconds(0.5f);

		for (int i = 0; i < 25; ++i)
		{
			Vector3 vLoc = rb.transform.position;
			vLoc.y += 0.02f;
			rb.transform.position = vLoc;

			yield return null;
		}

		bStopped = true;
		bResetingBodyCar = false;
	}

    void OnCollisionEnter(Collision collision)
    {
		if (collision.gameObject.CompareTag("Wheel"))
        {
			Destroy(collision.gameObject);
			GameManager.AddCapturedWheel();
		}
    }
}
