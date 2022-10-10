using UnityEngine;

public class Spin : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(0.0f, 40.0f * Time.deltaTime, 0.0f);
    }
}