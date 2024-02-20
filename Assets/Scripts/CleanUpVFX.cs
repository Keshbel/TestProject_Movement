using System.Collections;

using UnityEngine;
using UnityEngine.VFX;

public class CleanUpVFX : MonoBehaviour
{
    private VisualEffect _vfx;
    
    private void Start()
    {
        _vfx = GetComponent<VisualEffect>();

        StartCoroutine(CleanUpRoutine());
    }

    //destroying with delay
    private IEnumerator CleanUpRoutine()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForFixedUpdate();
        }

        while (_vfx.aliveParticleCount != 0)
        {
            yield return new WaitForFixedUpdate();
        }

        Destroy(gameObject);
    }
}
