using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public int hp = 100;
    [HideInInspector] public int maxHp; // 최대 HP
    public GameObject bloodParticle;

    public float bloodEffectYPosition = 1.3f;
    protected Animator animator;

    protected void Awake()
    {
        maxHp = hp; // maxHp의 값을 초기화
    }
    protected void CreateBloodEffect()
    {
        var pos = transform.position;
        pos.y = bloodEffectYPosition;
        Instantiate(bloodParticle, pos, Quaternion.identity);
    }
    // 다른 곳에서도 사용할 수 있도록 static으로 선언
    static public void CreateTextEffect(int number, Vector3 position, Color color)
    {
        CreateTextEffect(number.ToNumber(), "TextEffect", position, color);
    }
    static public void CreateTextEffect(string str, string prefabname, Vector3 position, Color color, Transform parent= null)
    {
        GameObject memoryGo = (GameObject)Resources.Load(prefabname);
        GameObject go = Instantiate(memoryGo, position, Camera.main.transform.rotation);
        if(parent)
        {
            go.transform.parent = parent;
        }
        TextMeshPro textMeshPro = go.GetComponent<TextMeshPro>();
        textMeshPro.text = str;
        textMeshPro.color = color;
    }
    public Color damageColor = Color.white;
    protected void TakeHit(int damage)
    {
        hp -= damage;
        CreateBloodEffect();// 피 이펙트 생성
        CreateTextEffect(damage,transform.position, damageColor);
    }
}
