using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer.material = new Material(spriteRenderer.material);
    }

    /// <summary>
    /// abs is absolute 0-1
    /// </summary>
    /// <param name="abs"></param>
    public void BarPosition(float abs)
    {
        spriteRenderer.material.SetFloat("_ClipUvRight", abs);
    }
}
