using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DitzelGames.FastIK;

public class LegManager : MonoBehaviour
{
	public GameObject InitialTarget;
	
    public FastIKFabric IKFabric;
    public GameObject IKFoot;
	
	public bool bIsMoving = false;

    public float fDistanceFromTarget;

    private Vector3 vGroundCastedTarget;

    void FixedUpdate()
    {
        // Project Initial Target to the ground - 9 is Leg layer id we want to ignore
        RaycastHit hit;
        if (Physics.Raycast(InitialTarget.transform.position, -Vector3.up, out hit, Mathf.Infinity, 9))
        {
            Debug.DrawRay(InitialTarget.transform.position, -Vector3.up * hit.distance, Color.yellow);

            // Compute distance to current foot location
            fDistanceFromTarget = Vector3.Distance(IKFoot.transform.position, hit.point);
           
            // Keep ground casted location updated
            vGroundCastedTarget = hit.point;
        }

    }
	
    public void SetFootTarget(Vector3 vNewTargetLocation)
    {
        IKFabric.Target.position = vNewTargetLocation;
    }

	public void SetFootToInitialTarget(bool bReset)
    {
        bIsMoving = true;

        if (bReset)
        {
            StartCoroutine("ResetIKTarget");
        }
        else
        {
            StartCoroutine("Move");
        }
	}
	

    IEnumerator ResetIKTarget()
    {
        while (0.01f < Vector3.Distance(IKFoot.transform.position, vGroundCastedTarget))
        {
            IKFabric.Target.position = Vector3.MoveTowards(IKFoot.transform.position, vGroundCastedTarget, 10.0f * Time.deltaTime);
            yield return null;
        }

        bIsMoving = false;
    }
	
    // Move the leg IK target in a non linear way to add a lift off
    // https://www.weaverdev.io/blog/bonehead-procedural-animation
    // Fraction of the max distance from home we want to overshoot by
    float stepOvershootFraction = 5.0f;

    public float moveDuration = 0.15f;
	public float wantStepAtDistance = 0.1f;
	
    IEnumerator Move()
    {
        Vector3 startPoint = IKFoot.transform.position;
        Quaternion startRot = IKFoot.transform.rotation;

        // Currently removed rotation since not useful and CastedTransform was broken for position
        //Quaternion endRot = CastedTransform.rotation;

        // Directional vector from the foot to the home position
        Vector3 towardHome = (vGroundCastedTarget - IKFoot.transform.position);
        // Total distance to overshoot by   
        float overshootDistance = wantStepAtDistance * stepOvershootFraction;
        Vector3 overshootVector = towardHome * overshootDistance;
        // Since we don't ground the point in this simplified implementation,
        // we restrict the overshoot vector to be level with the ground
        // by projecting it on the world XZ plane.
        overshootVector = Vector3.ProjectOnPlane(overshootVector, Vector3.up);

        // Apply the overshoot
        Vector3 endPoint = vGroundCastedTarget + overshootVector;

        // We want to pass through the center point
        Vector3 centerPoint = (startPoint + endPoint) * 0.5f;
        // But also lift off, so we move it up by half the step distance (arbitrarily)
        centerPoint += transform.up * Vector3.Distance(startPoint, endPoint) * 0.5f;

        float timeElapsed = 0;
        do
        {
            timeElapsed += Time.deltaTime;
            float normalizedTime = timeElapsed / moveDuration;

            // Quadratic bezier curve
            IKFabric.Target.position =
              Vector3.Lerp(
                Vector3.Lerp(startPoint, centerPoint, normalizedTime),
                Vector3.Lerp(centerPoint, endPoint, normalizedTime),
                normalizedTime
              );

            //IKFabric.Target.rotation = Quaternion.Slerp(startRot, endRot, normalizedTime);

            yield return null;
        }
        while (timeElapsed < moveDuration);

        bIsMoving = false;
    }
    //
}
