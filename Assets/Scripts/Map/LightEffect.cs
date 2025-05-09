using UnityEngine;

public class LightBeamEffect : MonoBehaviour
{
    public float pulseSpeed = 2f;
    public float pulseHeight = 0.1f;
    private Vector3 startScale;

    void Start()
    {
        startScale = transform.localScale;
    }

    void Update()
    {
        float scaleY = startScale.y + Mathf.Sin(Time.time * pulseSpeed) * pulseHeight;
        transform.localScale = new Vector3(startScale.x, scaleY, startScale.z);
    }
}
