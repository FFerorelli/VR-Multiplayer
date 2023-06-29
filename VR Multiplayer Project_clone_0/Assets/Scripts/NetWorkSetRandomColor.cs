using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetWorkSetRandomColor : NetworkBehaviour
{
    public List<Renderer> renderers;

    private NetworkVariable<Color> networkColor = new NetworkVariable<Color>(Color.blue, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public void SetRenderersColor(Color newColor)
    {
        foreach (var item in renderers)
        {
            item.material.color = newColor;
        }
    }

    public override void OnNetworkSpawn()
    {
        networkColor.OnValueChanged += (x, y) => SetRenderersColor(y);

        if (IsOwner)
        {
            networkColor.Value = Random.ColorHSV(0,1,1,1);

            SetRenderersColor(networkColor.Value);
        }
    }
    public override void OnNetworkDespawn()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        networkColor.OnValueChanged -= (x, y) => SetRenderersColor(y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
