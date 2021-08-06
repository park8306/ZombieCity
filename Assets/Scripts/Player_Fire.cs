﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : Actor
{
    public GameObject bullet;
    public Transform bulletPosition;


    float shootDelayEndTime;
    void Fire()
    {
        if (Input.GetMouseButton(0))
        {
            isFiring = true;
            if (shootDelayEndTime < Time.time)
            {
                animator.SetTrigger("StartFire");
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
        Instantiate(bullet, bulletPosition.position, CalculateRecoil(transform.rotation));
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