using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkAvatar : MonoBehaviour
{
    public GameObject[] headParts;
    public GameObject[] bodyParts;
    public Renderer[] skinParts;
    public Gradient skinGradient;
    public TMPro.TextMeshPro playerName;

    public NetworkAvatarData networkAvatarData;

    public struct NetworkAvatarData
    {
        public int headIndex;
        public int bodyIndex;
        public float skinColor;
        public string avatarName;
    }

    public NetworkAvatarData GenereteRandom()
    {
        int randomHeadIndex = Random.Range(0, headParts.Length);
        int randomBodyIndex = Random.Range(0, bodyParts.Length);
        float randomSkinColor = Random.Range((float)0, (float)1);

        string avatarName = "Random Avatar";

        NetworkAvatarData randomData = new NetworkAvatarData
        {
            headIndex = randomHeadIndex,
            bodyIndex = randomBodyIndex,
            skinColor = randomSkinColor,
            avatarName = avatarName
        };

        return randomData;
    }

    public void UpdateAvatarFromData(NetworkAvatarData newData)
    {
        for (int i = 0; i < headParts.Length; i++)
        {
            headParts[i].SetActive(i == newData.headIndex);
        }

        for (int i = 0; i < bodyParts.Length; i++)
        {
            bodyParts[i].SetActive(i == newData.bodyIndex);
        }

        foreach (var item in skinParts)
        {
            item.material.color = skinGradient.Evaluate(newData.skinColor);
        }

        playerName.text = newData.avatarName.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        networkAvatarData = GenereteRandom();
        UpdateAvatarFromData(networkAvatarData);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
