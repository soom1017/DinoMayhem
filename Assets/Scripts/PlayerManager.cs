using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO; // path를 사용하기 위함

public class PlayerManager : MonoBehaviour
{
    PhotonView PV; // 포톤뷰 선언

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if(PV.IsMine) // 내 포톤 네트워크이면
        {
            CreateController(); // 플레이어 컨트롤러를 붙여준다.
        }
    }

    void CreateController()
    {
        Debug.Log("instantiated PhotonView");

        // 포톤 프리펩에 있는 플레이어 매니저를 해당 위치와 각도에 만들어주기
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), Vector3.zero, Quaternion.identity);
    }
}