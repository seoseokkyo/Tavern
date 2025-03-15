using UnityEngine;
using Steamworks;

public class FollowerCountExample : MonoBehaviour
{
    private CallResult<FriendsGetFollowerCount_t> followerCountCallResult;

    private void Start()
    {
        if (!SteamManager.Initialized)
        {
            Debug.LogError("Steam�� �ʱ�ȭ���� �ʾҽ��ϴ�.");
            return;
        }

        CSteamID targetSteamID = SteamUser.GetSteamID(); // ������ SteamID �Ǵ� �ٸ� ���� SteamID

        Debug.Log($"�ȷο� �� ��û ��� SteamID: {targetSteamID.m_SteamID}");

        // API ȣ��
        SteamAPICall_t apiCall = SteamFriends.GetFollowerCount(targetSteamID);

        // �ݹ� ���
        followerCountCallResult = CallResult<FriendsGetFollowerCount_t>.Create(OnFollowerCountReceived);
        followerCountCallResult.Set(apiCall);
    }

    private void OnFollowerCountReceived(FriendsGetFollowerCount_t result, bool ioFailure)
    {
        if (ioFailure)
        {
            Debug.LogError("�ȷο� �� ��û ����: ��Ʈ��ũ �Ǵ� API ����");
            return;
        }

        Debug.Log($"�ȷο� ��: {result.m_nCount}");
    }

    private void Update()
    {
        // Steam API ������Ʈ ó�� (�ݵ�� �ʿ�)
        if (SteamManager.Initialized)
        {
            SteamAPI.RunCallbacks();
        }
    }
}