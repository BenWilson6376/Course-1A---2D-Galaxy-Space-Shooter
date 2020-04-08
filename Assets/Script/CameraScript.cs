using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float shakeMagnitude;
    public float shakeDuration;
    private Transform _originalTransform;
    public Transform originalTransform {
        get {
            return _originalTransform;
        }
    }

    private void Start()
    {
        _originalTransform = GetComponent<Transform>();
    }

    public void CallCameraShake()
    {
        StartCoroutine(CameraShake(shakeMagnitude, shakeDuration));
    }

    IEnumerator CameraShake(float magnitude, float duration)
    {
        float timeElapsed = 0f;
        Transform tempTransform = originalTransform;

        while (timeElapsed < duration) {
            timeElapsed += Time.deltaTime;
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            tempTransform.Translate(new Vector3(x, y, 0));
            //transform = tempTransform;
            yield return null;
        }
           gameObject.transform.position = new Vector3(0, 1, -10);
    }
}