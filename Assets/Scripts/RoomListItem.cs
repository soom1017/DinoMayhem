using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text roomItemText;

    RoomInfo info; // 포톤 리얼타임의 방정보 기능

    public void SetUp(RoomInfo _info)
    {
        info = _info;
        roomItemText.text = _info.Name;
    }

    public void OnClick()
    {
        Launcher.instance.JoinRoom(info);
    }
}
