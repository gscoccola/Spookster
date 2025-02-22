using System.Collections;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] float offset = 10f;
    [SerializeField] float delay = 2f;

    public void LookDown()
    {
        StopAllCoroutines();
        StartCoroutine(MoveTranslate(-1));
    }

    public void LookUp()
    {
        StopAllCoroutines();
        StartCoroutine(MoveTranslate(1));
    }

    public void ResetCamera()
    {
        StopAllCoroutines();
        transform.localPosition = new Vector3(0f, 0f, 0f);
    }

    IEnumerator MoveTranslate(int direction)
    {
        yield return new WaitForSeconds(delay);
        transform.localPosition = new Vector3(0f, direction* offset, 0f);
    }
}
