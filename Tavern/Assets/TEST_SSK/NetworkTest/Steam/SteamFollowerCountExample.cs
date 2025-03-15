using UnityEngine;
using Steamworks;

public class FollowerCountExample : MonoBehaviour
{
    private CallResult<FriendsGetFollowerCount_t> followerCountCallResult;

    private void Start()
    {
        if (!SteamManager.Initialized)
        {
            Debug.LogError("Steam이 초기화되지 않았습니다.");
            return;
        }

        CSteamID targetSteamID = SteamUser.GetSteamID(); // 본인의 SteamID 또는 다른 유저 SteamID

        Debug.Log($"팔로워 수 요청 대상 SteamID: {targetSteamID.m_SteamID}");

        // API 호출
        SteamAPICall_t apiCall = SteamFriends.GetFollowerCount(targetSteamID);

        // 콜백 등록
        followerCountCallResult = CallResult<FriendsGetFollowerCount_t>.Create(OnFollowerCountReceived);
        followerCountCallResult.Set(apiCall);
    }

    private void OnFollowerCountReceived(FriendsGetFollowerCount_t result, bool ioFailure)
    {
        if (ioFailure)
        {
            Debug.LogError("팔로워 수 요청 실패: 네트워크 또는 API 오류");
            return;
        }

        Debug.Log($"팔로워 수: {result.m_nCount}");
    }

    private void Update()
    {
        // Steam API 업데이트 처리 (반드시 필요)
        if (SteamManager.Initialized)
        {
            SteamAPI.RunCallbacks();
        }
    }
}