using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerColor : NetworkBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public List<Color> colors = new List<Color>();


   /* private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }*/

    public override void OnNetworkSpawn()
    {
        spriteRenderer.color = colors[(int)OwnerClientId];
    }
}
