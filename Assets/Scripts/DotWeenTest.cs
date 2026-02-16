using DG.Tweening;
using UnityEngine;

public class DotWeenTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.DOMove(transform.position + 10f * Vector3.left, 10f);
    }

}
