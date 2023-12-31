using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Networking.Transport.Relay;
using System.Threading.Tasks;

public class RelayManager : MonoBehaviour
{

    public static RelayManager Instance;

    private UnityTransport transport;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        transport = GetComponent<UnityTransport>();
    }

    public async Task<string> CreateRelayGame(int maxPlayer)
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayer);

        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        Debug.Log("The join code is : " + joinCode);

        //RelayServerData serverData = new RelayServerData(allocation, "dtls");
        //transport.SetRelayServerData(serverData);

        transport.SetHostRelayData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port, allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData);

        NetworkManager.Singleton.StartHost();

        return joinCode;
    }

    public async void JoinRelayGame(string joinCode)
    {
        JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

        //RelayServerData serverData = new RelayServerData(allocation, "dtls");
        //transport.SetRelayServerData(serverData);

        transport.SetClientRelayData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port, allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData, allocation.HostConnectionData);

        NetworkManager.Singleton.StartClient();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
