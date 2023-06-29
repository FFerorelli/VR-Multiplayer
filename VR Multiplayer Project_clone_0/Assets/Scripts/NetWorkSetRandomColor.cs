using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

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
        networkColor.OnValueChanged -= (x, y) => SetRenderersColor(y);
    }
    // Start is called before the first frame update
    void Start()
    {
        networkColor.OnValueChanged -= (x, y) => SetRenderersColor(y);
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            RequestUpdateColorServerRpc(new ServerRpcParams());
        }
    }

    // Client Rpc works when used by the server to the client
    [ClientRpc]
    public void UpdateColorClientRpc(ClientRpcParams clientParameters)
    {
        if (IsOwner)
        {
            networkColor.Value = Random.ColorHSV(0, 1, 1, 1);
        }
    }
    // Server Rpc works when used by the client
    [ServerRpc(RequireOwnership = false)]
    public void RequestUpdateColorServerRpc(ServerRpcParams paramenters)
    {
        Debug.Log("Sent by " + paramenters.Receive.SenderClientId);

        ClientRpcParams clientParameters = new ClientRpcParams();
        clientParameters.Send.TargetClientIds = new List<ulong>() { 0 };
        UpdateColorClientRpc(clientParameters);
    }
}
