using UnityEngine;
using System.Collections;

public class DissolveEffect : MonoBehaviour
{
    public float dissolveTime = 2f;
    private Material mat;

    private void Start()
    {
        mat = GetComponentInChildren<Renderer>().material;
    }

    public void StartDissolve()
    {
        StartCoroutine(DissolveRoutine());
    }

    private IEnumerator DissolveRoutine()
    {
        float t = 0f;

        while (t < dissolveTime)
        {
            t += Time.deltaTime;
            float amount = t / dissolveTime;
            mat.SetFloat("_DissolveAmount", amount);
            yield return null;
        }
    }
}
