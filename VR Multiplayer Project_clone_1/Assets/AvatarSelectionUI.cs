using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarSelectionUI : MonoBehaviour
{

    public static AvatarSelectionUI Singleton;

    public Button nextHead;
    public Button previousHead;
    public Button nextBody;
    public Button previousBody;
    public Slider skinSlider;
    public TMPro.TMP_InputField nameInputField;

    private NetworkAvatar currentNerworkAvatar;

    public void UpdateHeadIndex(int newIndex)
    {
        if (!currentNerworkAvatar)
            return;
        if (newIndex >= currentNerworkAvatar.headParts.Length)
            newIndex = 0;
        if (newIndex < 0)
            newIndex = currentNerworkAvatar.headParts.Length - 1;

        NetworkAvatar.NetworkAvatarData newData = currentNerworkAvatar.networkAvatarData.Value;
        newData.headIndex = newIndex;
        currentNerworkAvatar.networkAvatarData.Value = newData;

    }
    public void UpdateBodyIndex(int newIndex)
    {
        if (!currentNerworkAvatar)
            return;
        if (newIndex >= currentNerworkAvatar.bodyParts.Length)
            newIndex = 0;
        if (newIndex < 0)
            newIndex = currentNerworkAvatar.bodyParts.Length - 1;

        NetworkAvatar.NetworkAvatarData newData = currentNerworkAvatar.networkAvatarData.Value;
        newData.bodyIndex = newIndex;
        currentNerworkAvatar.networkAvatarData.Value = newData;

    }

    public void UpdateSkinValue(float value)
    {
        if (!currentNerworkAvatar)
            return;

        NetworkAvatar.NetworkAvatarData newData = currentNerworkAvatar.networkAvatarData.Value;
        newData.skinColor = value;
        currentNerworkAvatar.networkAvatarData.Value = newData;

    }
    public void UpdateNameValue(string value)
    {
        if (!currentNerworkAvatar)
            return;

        NetworkAvatar.NetworkAvatarData newData = currentNerworkAvatar.networkAvatarData.Value;
        newData.avatarName = value;
        currentNerworkAvatar.networkAvatarData.Value = newData;

    }

    public void Initialize(NetworkAvatar networkAvatar)
    {
        currentNerworkAvatar = networkAvatar;

        nameInputField.SetTextWithoutNotify(currentNerworkAvatar.networkAvatarData.Value.avatarName.ToString());
        skinSlider.SetValueWithoutNotify(currentNerworkAvatar.networkAvatarData.Value.skinColor);
    }

    // Start is called before the first frame update
    void Start()
    {
        Singleton = this;

        nextHead.onClick.AddListener(() => UpdateHeadIndex(currentNerworkAvatar.networkAvatarData.Value.headIndex + 1));
        previousHead.onClick.AddListener(() => UpdateHeadIndex(currentNerworkAvatar.networkAvatarData.Value.headIndex - 1));

        nextBody.onClick.AddListener(() => UpdateBodyIndex(currentNerworkAvatar.networkAvatarData.Value.headIndex + 1));
        previousBody.onClick.AddListener(() => UpdateBodyIndex(currentNerworkAvatar.networkAvatarData.Value.headIndex - 1));

        skinSlider.onValueChanged.AddListener(UpdateSkinValue);

        nameInputField.onEndEdit.AddListener(UpdateNameValue);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
