using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    private readonly NetworkVariable<PlayerNetworkData> _netState = new NetworkVariable<PlayerNetworkData>(writePerm: NetworkVariableWritePermission.Owner);
    private Vector2 vec;
    private float _rotVel;
    [SerializeField] private float _cheapInterpolationTime = 0.1f;
   
    void Update()
    {
        if(IsOwner)
        {
            _netState.Value = new PlayerNetworkData()
            {
                Position = transform.position,
                Rotation = transform.rotation.eulerAngles
            };
        }
        else
        {
            transform.position = Vector2.SmoothDamp(transform.position, _netState.Value.Position, ref vec, _cheapInterpolationTime);
            transform.rotation = Quaternion.Euler(0, Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, _netState.Value.Rotation.y,ref _rotVel, _cheapInterpolationTime), 0);
        }
    }
    struct PlayerNetworkData : INetworkSerializable
    {
        private float _x, _y;
        private float _yRot;

        internal Vector2 Position
        {
            get => new Vector2(_x, _y);
            set
            {
                _x = value.x;
                _y = value.y;
            }
        }
        internal Vector2 Rotation
        {
            get => new Vector2(0, _yRot);
            set => _yRot = value.y;
        }
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _x);
            serializer.SerializeValue(ref _y);
        }

    }
}

