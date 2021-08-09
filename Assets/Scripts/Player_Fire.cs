using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : Actor
{
    public int BulletCountInClip
    {
        get => currentWeapon.bulletCountInClip;   // 탄창에 총알 수
        set => currentWeapon.bulletCountInClip = value;   // 탄창에 총알 수
    }
    public int MaxBulletCountInClip => currentWeapon.maxBulletCountInClip;  // 탄창에 들어가는 최대 수
    public int AllBulletCount 
    { 
        get => currentWeapon.allBulletCount;
        set => currentWeapon.allBulletCount = value;
    }      // 가진 전체 총알 수
    public int MaxBulletCount => currentWeapon.maxBulletCount;
    public float ReloadTime => currentWeapon.reloadTime;

    public GameObject Bullet => currentWeapon.bullet;
    public Transform BulletPosition => currentWeapon.bulletPosition;


    float shootDelayEndTime;
    void Fire()
    {
        if (Input.GetMouseButton(0) && BulletCountInClip > 0)
        {
            isFiring = true;
            if (shootDelayEndTime < Time.time)
            {
                BulletCountInClip--;
                animator.SetTrigger("StartFire");
                AmmoUI.Instance.SetBulletCount(BulletCountInClip, MaxBulletCountInClip,
                    AllBulletCount + BulletCountInClip,
                    MaxBulletCount);
                shootDelayEndTime = Time.time + shootDelay;
                switch (currentWeapon.type) // 무기의 종류에 따라서 하는 동작을 다르게 함
                {
                    case WeaponInfo.WeaponType.Gun:
                        IncreaseRecoil();
                        currentWeapon.StartCoroutine(InstantiateBulletAndFlashBulletCo());
                        break;
                    case WeaponInfo.WeaponType.Melee:
                        // 근접일 때는 무기에 콜라이더를 활성화 하자.
                        currentWeapon.StartCoroutine(MeleeAttackCo());  // currenWeapon.StartCoroutine으로 무기가 바뀌면 코루틴을 꺼주자
                        break;
                }
            }
        }
        else
        {
            EndFiring();
        }
    }

    private IEnumerator MeleeAttackCo()
    {
        yield return new WaitForSeconds(currentWeapon.attackStartTime);
        currentWeapon.attackCollider.enabled = true;
        yield return new WaitForSeconds(currentWeapon.attackTime);
        currentWeapon.attackCollider.enabled = false;
    }

    private void EndFiring()
    {
        //animator.SetBool("Fire",false);
        DecreaseRecoil();
        isFiring = false;
    }

    GameObject bulletLight;
    public float bulletFlashTime = 0.001f;
    private IEnumerator InstantiateBulletAndFlashBulletCo()
    {
        yield return null;
        GameObject bulletGo = Instantiate(Bullet, BulletPosition.position, CalculateRecoil(transform.rotation));
        bulletGo.GetComponent<Bullet>().pushBackDistance = currentWeapon.pushBackDistance;
        //if()
        bulletLight.SetActive(true);
        yield return new WaitForSeconds(bulletFlashTime);
        bulletLight.SetActive(false);
    }

    float recoilValue = 0f;
    float recoilMaxValue = 1.5f;
    float recoilLerpValue = 0.1f;
    void IncreaseRecoil()
    {
        recoilValue = Mathf.Lerp(recoilValue, recoilMaxValue, recoilLerpValue);
    }
    void DecreaseRecoil()
    {
        recoilValue = Mathf.Lerp(recoilValue, 0, recoilLerpValue);
    }

    Vector3 recoil;
    Quaternion CalculateRecoil(Quaternion rotation)
    {
        recoil = new Vector3(Random.Range(-recoilValue, recoilValue), Random.Range(-recoilValue, recoilValue), 0);
        return Quaternion.Euler(rotation.eulerAngles + recoil);
    }

    [SerializeField] float shootDelay = 0.05f;
}