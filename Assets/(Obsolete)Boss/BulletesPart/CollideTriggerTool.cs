using System;
using System.Collections.Generic;
using UnityEngine;

public class CollideTriggerTool : MonoBehaviour
{
    Action OntriggerCallback;
    TagsEnum tagEnum;
    public void Initialize(Action OntriggerCallback,TagsEnum targetnameEnum)
    {
        this.OntriggerCallback = OntriggerCallback;
        tagEnum = targetnameEnum;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(tagEnum.ToString()))
        {
            OntriggerCallback();
        }
    }
}
