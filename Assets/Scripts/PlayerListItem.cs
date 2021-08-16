using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerListItem : MonoBehaviourPunCallbacks //다른 포톤 반응 받아들이기
{
    [SerializeField] TMP_Text playerItemText;
    Player player; // 포톤 리얼타임은 Player 객체를 선언할 수 있게 해줌.

    public void SetUp(Player _player)
    {
        player = _player;
        playerItemText.text = _player.NickName;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(player == otherPlayer) // 나간 플레이어가 본인일 경우
        {
            Destroy(gameObject); // 플레이어(이름표) 삭제
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
