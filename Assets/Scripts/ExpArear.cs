using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ExpArear : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI tmpro;
    [SerializeField] float speed;
    [SerializeField] float exp;
    [SerializeField] Transform[] points;
    float currentPresentage = 0;
    int currentPointIndex = 0;
    bool finish = false;
    Vector3 initScale;
    Color initColor;

    // Start is called before the first frame update
    void Start()
    {
        
        initScale = transform.localScale;
        initColor = tmpro.color;
    }

    // Update is called once per frame
    void Update()
    {
        tmpro.text = ((int)currentPresentage).ToString() + "%";
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (finish) return;
        if (collision.tag == TagsEnum.Player.ToString())
        {
            currentPresentage = Mathf.Min( currentPresentage + Time.deltaTime * speed,100);
            if (currentPresentage >= 100)
            {
                tmpro.DOFade(0, 0.5f).SetDelay(0.2f);
                gameObject.transform.DOScale(Vector2.zero, 0.5f).SetDelay(0.2f).SetEase(Ease.OutBack).OnComplete(() => { 
                    if(collision.TryGetComponent(out IExpGetter expGetter)){
                        expGetter.Get(exp);
                    }

                    currentPointIndex++;
                    if (currentPointIndex >= points.Length) currentPointIndex = 0;
                    
                    gameObject.transform.position = points[currentPointIndex].position;
                    gameObject.transform.localScale = initScale;
                    tmpro.color = initColor;
                    currentPresentage = 0;
                    finish = false;
                });
                finish = true;
                
                
            }
        }
    }
}
public interface IExpGetter
{
    public void Get(float exp);
}