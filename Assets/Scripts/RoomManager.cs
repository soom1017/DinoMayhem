using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.IO; // Path를 사용하기 위함.
using System;

public class RoomManager : MonoBehaviourPunCallbacks //다른 포톤 반응 받아들이기
{
    public static RoomManager Instance;

    private void Awake()
    {
        if(Instance) // 다른 룸매니저 존재 확인
        {
            Destroy(gameObject); // 있으면 파괴
            return;
        }
        DontDestroyOnLoad(gameObject); // 룸매니저가 본인 혼자면 그대로
        Instance = this;
    }

    public override void OnEnable()
    {
        // 활성화되면 씬 매니저의 OnSceneLoaded에 체인을 건다.
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        // 비활성화되면 씬 매니저의 체인을 지운다.
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode load)
    {
        if(scene.buildIndex == 1)
        {
            // 포톤 프리펩에 있는 플레이어 매니저를 해당 위치와 각도에 만들어주기
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
        }
    }
}
