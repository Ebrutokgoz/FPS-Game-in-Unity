using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public float coolDown = 0.1f;
    float lastFireTime = 0;
    public int defaultAmmo = 120;
    public int magSize = 30;
    public int currentAmmo;
    public int currentMagAmmo;
    public Camera camera;
    public int range;
    [Header("Gun Damage On Hit")]
    public int damage;
    public GameObject bloodPrefab;
    public GameObject decal;
    public GameObject magObject;
    public ParticleSystem muzzleParticle;
    int minAngle = -2;
    int maxAngle = 2;

    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = defaultAmmo - magSize;
        currentMagAmmo = magSize;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
        if(Input.GetMouseButton(0))
        {
            if (CanFire())
            {
                Fire();
            } 
        }
    }

    private void Reload()
    {
        if(currentAmmo == 0 && currentMagAmmo == magSize)
        {
            return;
        }
        if(currentAmmo < magSize)
        {
            currentMagAmmo = currentMagAmmo + currentAmmo;
            currentAmmo = 0;
        }
        else
        {
            currentAmmo -= magSize - currentMagAmmo;
            currentMagAmmo = magSize; 
        }
        GameObject newMagObject = Instantiate(magObject);
        newMagObject.transform.position = magObject.transform.position;
        newMagObject.AddComponent<Rigidbody>();

        
    }

    private bool CanFire()
    {
        if (currentMagAmmo > 0 && lastFireTime + coolDown < Time.time)
        {
            lastFireTime = Time.time + coolDown;
            return true;
        }
        
        return false;
    }

    private void Fire()
    {
        muzzleParticle.Play(true);
        currentMagAmmo -= 1;
        Debug.Log("Kalan mermi: " + currentMagAmmo);
        RaycastHit hit;
        if(Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, 10))
        {
            if(hit.transform.tag == "Zombie")
            {
                hit.transform.GetComponent<ZombieHealth>().Hit(damage);
                GenerateBloodEffect(hit);
            }
            else
            {
                GenerateHitEffect(hit);
            }
        }
        transform.localEulerAngles = new Vector3(Random.Range(minAngle, maxAngle), Random.Range(minAngle, maxAngle), Random.Range(minAngle, maxAngle));
    }

    private void GenerateHitEffect(RaycastHit hit)
    {
        //TODO:Mermi izi oluştur
        GameObject hitObject = Instantiate(decal, hit.point, Quaternion.identity);
        hitObject.transform.rotation = Quaternion.FromToRotation(decal.transform.forward * -1, hit.normal);
    }

    private void GenerateBloodEffect(RaycastHit hit)
    {
        GameObject bloodObject = Instantiate(bloodPrefab, hit.point, hit.transform.rotation);
    }

    
}
