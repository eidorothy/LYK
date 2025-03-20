using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UI_Network : NetworkBehaviour  // 네트워크 프로퍼티에 접근할 수 있도록 상속
{
    [SerializeField] private Button _hostButton;
    [SerializeField] private Button _clientButton;
    [SerializeField] private TextMeshProUGUI _playerCountText;

    private NetworkVariable<int> _playerCount = new NetworkVariable<int>(0);

    void Awake()
    {
        _hostButton.onClick.AddListener(()=> {
            NetworkManager.Singleton.StartHost();
        });

        _clientButton.onClick.AddListener(()=> {
            NetworkManager.Singleton.StartClient();
        });
    }

    void Update()
    {
        _playerCountText.text = $"Players: {_playerCount.Value}";
        
        if (!IsServer)
        {
            return;
        }

        _playerCount.Value = NetworkManager.Singleton.ConnectedClients.Count;   // 서버가 수정
    }
}
