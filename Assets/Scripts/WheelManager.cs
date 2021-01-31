using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelManager : MonoBehaviour
{
	public GameManager GameManager;

	public WheelLegManager LegLeftFront;
	public WheelLegManager LegLeftBack;
	public WheelLegManager LegRightFront;
	public WheelLegManager LegRightBack;

	public float fMaxLegDistanceFromTarget = 0.1f;

	private int NextLegToMove = 0; // 0:left, 1:right

	private bool bDead = false;

	public float fMoveSpeed = 0.5f;

	private float fMoveTimer = 0.0f;
	private float fDirection = 1.0f;

	void FixedUpdate()
	{
		if (!bDead)
		{
			fMoveTimer += Time.fixedDeltaTime;

			if (fMoveTimer >= 6.0f)
            {
				fDirection *= -1.0f;
				fMoveTimer = 0.0f;
			}

			transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward * (fDirection * 10.0f), fMoveSpeed * Time.deltaTime);

			if (!LegRightFront.bIsMoving
				&& NextLegToMove == 0
				&& LegLeftFront.fDistanceFromTarget >= fMaxLegDistanceFromTarget)
			{
				LegLeftFront.SetFootToInitialTarget(false);
				LegRightBack.SetFootToInitialTarget(false);
				NextLegToMove = 1;
			}

			if (!LegLeftFront.bIsMoving
				&& NextLegToMove == 1
				&& LegRightFront.fDistanceFromTarget >= fMaxLegDistanceFromTarget)
			{
				LegRightFront.SetFootToInitialTarget(false);
				LegLeftBack.SetFootToInitialTarget(false);
				NextLegToMove = 0;
			}
		}
	}

	public void OnHitByCar()
    {
		StartCoroutine("Death");
	}

	IEnumerator Death()
    {
		yield return new WaitForSeconds(3.0f);

		GameManager.PlayWheelDeathFX(transform.position);

		Destroy(gameObject);
	}
}
