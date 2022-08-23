using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Chat;
using Photon.Pun;
using ExitGames.Client.Photon;
using TMPro;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour, IChatClientListener
{

    private ChatClient chatClient;
    public TextMeshProUGUI connectionState;
    public TMP_InputField msgInput;
    public TextMeshProUGUI message;

    public GameObject messagePanel;

    private string worldchat;


    void IChatClientListener.DebugReturn(DebugLevel level, string message)
    {
        
    }

    void IChatClientListener.OnChatStateChange(ChatState state)
    {
        
    }

    void IChatClientListener.OnConnected()
    {
        
        chatClient.Subscribe(new string[] { worldchat });
        chatClient.SetOnlineStatus(ChatUserStatus.Online);
    }

    void IChatClientListener.OnDisconnected()
    {
        
    }

    void IChatClientListener.OnGetMessages(string channelName, string[] senders, object[] messages)
    {
       for(int i = 0; i < senders.Length; i++)
        {
            message.text += senders[i] + ":" + messages[i] + ",\n";           
        }
    }

    void IChatClientListener.OnPrivateMessage(string sender, object message, string channelName)
    {
        
    }

    void IChatClientListener.OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        
    }

    void IChatClientListener.OnSubscribed(string[] channels, bool[] results)
    {
        foreach (var channel in channels)
        {
            this.chatClient.PublishMessage(channel, "joined");
        }
        connectionState.text = "Connected";
    }

    void IChatClientListener.OnUnsubscribed(string[] channels)
    {
        
    }

    void IChatClientListener.OnUserSubscribed(string channel, string user)
    {
        
    }

    void IChatClientListener.OnUserUnsubscribed(string channel, string user)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
        Application.runInBackground = true;
        if (string.IsNullOrEmpty(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat))
        {
            Debug.LogError("No app id");
            return;
        }
        GetConnected();

        worldchat = "World";
    }

    // Update is called once per frame
    void Update()
    {
       
        if (chatClient != null)
        {
            chatClient.Service();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendMessages();
        }
    }

    public void GetConnected()
    {
        Debug.Log("Connecting");
        chatClient = new ChatClient(this);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new Photon.Chat.AuthenticationValues(Player.pName));
    }

    public void SendMessages()
    {
        chatClient.PublishMessage(worldchat, msgInput.text);
        msgInput.text = string.Empty;
    }
}
