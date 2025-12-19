using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class AnimatedLine : MonoBehaviour
{
    public float travelTime = 0.1f;    
    public float trailDuration = 0.2f; 
    public float trailHeadRatio = 0.6f;
    
    private LineRenderer lr;
    private Vector3 startPos;
    private Vector3 endPos;
    private float elapsed;
    private bool hasFinishedTravel = false;

    public void Init(Vector3 start, Vector3 end)
    {
        startPos = start;
        endPos = end;
        elapsed = 0f;

        lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = true;
        lr.positionCount = 2;
        lr.SetPosition(0, startPos);
        lr.SetPosition(1, startPos);

        if (lr.material == null)
        {
            lr.material = new Material(Shader.Find("Standard")); 
        }
    }

    private void Update()
    {
        if (lr == null) return;

        elapsed += Time.deltaTime;

        float travelT = Mathf.Clamp01(elapsed / travelTime);

        Vector3 headPosition = Vector3.Lerp(startPos, endPos, travelT);
        lr.SetPosition(1, headPosition);

        float tailT = Mathf.Clamp01((elapsed - (travelTime * trailHeadRatio)) / (travelTime * (1 - trailHeadRatio)));
        
        Vector3 tailPosition = Vector3.Lerp(startPos, endPos, tailT);
        lr.SetPosition(0, tailPosition);

        if (travelT >= 1f && !hasFinishedTravel)
        {
            hasFinishedTravel = true;
            Destroy(gameObject, (travelTime * (1 - trailHeadRatio)) + 0.05f);
            this.enabled = false;
        }
    }
}