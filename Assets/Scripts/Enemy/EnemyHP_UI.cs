using UnityEngine;
using UnityEngine.UI;

public class EnemyHP_UI : MonoBehaviour
{
    public Slider HP_bar;
    public Slider Ghost_Bar;

    public float lerpSpeed = 5f;

    private Enemy enemy;
    private float targetHP;

    Canvas targetCanvas;
    private Transform target;

    private void Start()
    {
        targetCanvas = GetComponent<Canvas>();
        targetCanvas.worldCamera = Camera.main;

        target = Camera.main.transform;

        enemy = GetComponentInParent<Enemy>();

        HP_bar.maxValue = enemy.maxHP;
        Ghost_Bar.maxValue = enemy.maxHP;

        HP_bar.value = enemy.maxHP;
        Ghost_Bar.value = enemy.maxHP;

        targetHP = enemy.maxHP;
    }

    private void Update()
    {
        targetHP = enemy.maxHP;
        HP_bar.value = targetHP;

        if (Ghost_Bar.value > targetHP)
        {
            Ghost_Bar.value = Mathf.Lerp(Ghost_Bar.value,targetHP,Time.deltaTime * lerpSpeed);
        }
        else
        {
            Ghost_Bar.value = targetHP;
        }
    }

    
    void LateUpdate()
    {
        transform.LookAt(target);
    }

}
