using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDestroyOnStart : MonoBehaviour
{
    [Range(0f, 1f)][SerializeField] float random;

    private void Start()
    {
        if (Random.Range(0f,1f)<random)
        {
            Destroy(gameObject);
        }
    }
}
