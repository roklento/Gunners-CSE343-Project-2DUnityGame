using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Grenade : NetworkBehaviour
{
    [SerializeField] private GameObject grenade;
    [SerializeField] private Transform grenadePos;
    [SerializeField] private float grenadeSpeed;
    private bool isReady = false;
    private float pressTime = 0;
    //[SerializeField] private GameObject explosionAnim;
    // Start is called before the first frame update
    private void Start()
    {
        //explosionAnim = GetComponent<Animator>();
    }
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
    }
    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.G) && isReady == false)
        {
            pressTime = Time.time;
        }
        if (Input.GetKeyUp(KeyCode.G))
        {
            pressTime = Time.time - pressTime;
            isReady = true;
        }
        if (Time.time >= pressTime && isReady == true)
        {
            var dir = transform.localRotation.y;
            RequestDroppingGrenadeServerRpc(dir,pressTime);
            isReady = false;
            pressTime = 0;
        }
        
    }

    [ServerRpc]
    private void RequestDroppingGrenadeServerRpc(float dir, float pressTime)
    {
        RequestDroppingGrenadeClientRpc(dir, pressTime);
    }

    [ClientRpc]
    private void RequestDroppingGrenadeClientRpc(float dir, float pressTime)
    {
        DroppingGrenade(dir, pressTime);
    }

    private void DroppingGrenade(float dir, float clientpPressTime)
    {
        int direction(float dir)
        {
            if (dir < 0f)
            {
                return -1;
            }
            else
                return 1;
        }
        GameObject newGrenade = Instantiate(grenade, grenadePos.position, Quaternion.identity);
        print(pressTime);
        newGrenade.GetComponent<Rigidbody2D>().velocity = new Vector2(grenadeSpeed * clientpPressTime * direction(dir) * Time.fixedDeltaTime, grenadeSpeed * clientpPressTime * Time.fixedDeltaTime);
        newGrenade.transform.localScale = new Vector2(newGrenade.transform.localScale.x * direction(dir), newGrenade.transform.localScale.y);
    }

    
}
