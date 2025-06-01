using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleItem : MonoBehaviour,SetActiveWithDistance
{
    [SerializeField] GameObject container;
    private float showDistance = 75;
    protected Transform target = null;
    [SerializeField] bool isArbitrary;
    [SerializeField] int appealProbability;
    private bool firstOrderActive;


    public void Initialize(Transform target)
    {
        this.target = target;
        if (!isArbitrary)
        {
            firstOrderActive = true;
        }
        else
        {
            int num = UnityEngine.Random.Range(0, 100);
            if (num<appealProbability)
            {
                firstOrderActive = true;
            }
            else
            {
                container.SetActive(false);
                firstOrderActive = false;
            }
        }

    }
    public void UpdateDistance()
    {
        if (Vector2.Distance(target.position,transform.position)>= showDistance)
        {
            if (container.activeInHierarchy)
            {
                MainControllerSetActive(false);
            }
        }
        else
        {
            if (!container.activeInHierarchy)
            {
                MainControllerSetActive(true);
            }
        }
    }

    private void MainControllerSetActive(bool t)
    {
        if (firstOrderActive == false)
        {
            return;
        }
        container.SetActive(t);
    }

    private void Update()
    {
        Updating();
    }

    protected virtual void Updating()
    {

    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
