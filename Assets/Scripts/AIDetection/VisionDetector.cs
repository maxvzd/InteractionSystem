using Constants;
using UnityEngine;

namespace AIDetection
{
    public static class VisionDetector
    {
        public static bool Detect(Detectee currentDetectee, Transform eyesTransform, float sightRadius, float halfFov, Transform currenTransform)
        {
            //TODO: do some benchmarking to test if it's better to do this with a collider trigger;
            Vector3 relativePos = currentDetectee.transform.position + Vector3.up * 0.3f - eyesTransform.position;
            if (relativePos.sqrMagnitude > sightRadius * sightRadius) return false;
            
            // detect if detectee is in fov
            Vector3 direction = relativePos.normalized;
            float dotProduct = Vector3.Dot(currenTransform.forward, direction);
            float fovRange = 1 - halfFov * (1f / 90f);

            if (dotProduct < fovRange) return false;
            Ray floorRay = new Ray(eyesTransform.position, direction * sightRadius);

            if (Physics.Raycast(floorRay, out RaycastHit floorHit, sightRadius))
            {
                if (floorHit.transform.gameObject.CompareTag(TagConstants.PlayerTag))
                {
                    Debug.DrawRay(floorRay.origin, floorRay.direction * relativePos.magnitude, Color.green);
                    return true;
                }
            }

            Vector3 headDirection = GetModifiedPositionDirection(
                currentDetectee.transform.position,
                currentDetectee.Height,
                0.3f,
                0f,
                0f,
                eyesTransform,
                currenTransform);

            if (FireRayWithInDirection(headDirection, eyesTransform, sightRadius))
            {
                Debug.DrawRay(eyesTransform.position, headDirection * relativePos.magnitude, Color.yellow);
                return true;
            }

            Vector3 rightDirection = GetModifiedPositionDirection(
                currentDetectee.transform.position,
                currentDetectee.Height / 2f,
                0f,
                currentDetectee.Width / 2f,
                0.1f,
                eyesTransform,
                currenTransform);

            if (FireRayWithInDirection(rightDirection, eyesTransform, sightRadius))
            {
                Debug.DrawRay(eyesTransform.position, rightDirection * relativePos.magnitude, Color.red);
                return true;
            }

            Vector3 leftDirection = GetModifiedPositionDirection(
                currentDetectee.transform.position,
                currentDetectee.Height / 2f,
                0f,
                -(currentDetectee.Width / 2f),
                0.1f,
                eyesTransform,
                currenTransform);

            if (FireRayWithInDirection(leftDirection, eyesTransform, sightRadius))
            {
                Debug.DrawRay(eyesTransform.position, leftDirection * relativePos.magnitude, Color.cyan);
                return true;
            }

            return false;
        }

        private static Vector3 GetModifiedPositionDirection(Vector3 currentDetecteePos, float upMagnitude, float upTolerance, float rightMagnitude, float rightTolerance, Transform eyesTransform, Transform currentTransform)
        {
            if (rightMagnitude < 0)
            {
                rightTolerance *= -1f;
            }

            Vector3 positionWithModifier = currentDetecteePos + currentTransform.up * (upMagnitude - upTolerance) + currentTransform.right * (rightMagnitude - rightTolerance);
            Vector3 relativePos = positionWithModifier - eyesTransform.position;
            return relativePos.normalized;
        }

        private static bool FireRayWithInDirection(Vector3 direction, Transform eyesTransform, float sightRadius)
        {
            Ray heightRay = new Ray(eyesTransform.position, direction * sightRadius);
            return Physics.Raycast(heightRay, out RaycastHit heightHit, sightRadius) && heightHit.transform.gameObject.CompareTag(TagConstants.PlayerTag);
        }
    }
}