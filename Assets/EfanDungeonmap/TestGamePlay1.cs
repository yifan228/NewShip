using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGamePlay1 : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text collisionCount;
    [SerializeField] TMPro.TMP_Text timertext;
    [SerializeField] Rigidbody2D ship;
    [SerializeField] Vector2 startPoint;
    int currentCollisionCount;
    float timer;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag(TagsEnum.FrontCollision.ToString()))
        {
            currentCollisionCount++;
            collisionCount.text ="Collide Wall Count : "+ (currentCollisionCount).ToString();
        }
    }

    private void Update()
    {
        timertext.text ="timer : "+ (timer += Time.deltaTime).ToString("0.00");
        Vector2 R = ship.position - startPoint;
        if (R.magnitude*Mathf.Cos(Vector2.Angle(R,Vector2.right))>=425 && R.magnitude * Mathf.Sin(Vector2.Angle(R, Vector2.right)) >= 425)
        {
            collisionCount.text = "YOU ESCAPE THIS DUNGEON!!!!";
        }
    }
}
