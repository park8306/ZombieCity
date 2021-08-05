using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DropItemType
{
    Gold,
    Point,
    Item,
}
public class DroppedItem : MonoBehaviour
{
    public enum GetMethodType
    {
        TriggerEnter,
        KeyDown,
    }
    public GetMethodType getMethod;
    public KeyCode keyCode = KeyCode.E;
    public DropItemType type;   // 인스펙터에서 지정
    public int amount;
    public int itemId;
    //public Color textColor = Color.white;

    bool alreadyDone = false;   // 이미 트리거가 작동했다면 사용하기 위한 변수
    private void Awake()
    {
        enabled = false;    // update가 작동안해도 trigger가 작동한다
    }
    private void OnTriggerEnter(Collider other)
    {
        if (alreadyDone)    // 이미 작동했다면 나가자
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            switch (getMethod)
            {
                case GetMethodType.TriggerEnter:
                    ItemAcquisition();
                    break;
                case GetMethodType.KeyDown:
                    enabled = true; // update문을 사용하겠다는 의미
                    break;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enabled = false;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            enabled = false;
            ItemAcquisition();
        }
    }

    private void ItemAcquisition()  // 아이템 획득
    {
        alreadyDone = true; // 트리거가 작동했다고 표시
        switch (type)
        {
            case DropItemType.Gold:
                StageManager.Instance.AddGold(amount);
                break;
        }
        Destroy(transform.parent.gameObject);   // 부모의 오브젝트를 파괴
    }
}
