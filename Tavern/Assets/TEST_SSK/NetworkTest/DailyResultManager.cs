using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Photon.Realtime;


// 일일결산 데이터
[Serializable]
public class UsedItemSpecification
{
    public string ItemName;
    public int Count;

    public UsedItemSpecification(string ItemName, int Count)
    {
        this.ItemName = ItemName;
        this.Count = Count;
    }
}

[Serializable]
public class DailyTotalResult
{
    public int ResultDay = 0;
    public List<UsedItemSpecification> UsedItemSpecifications = new List<UsedItemSpecification>();
    public float UsedItemPrice = 0.0f;
}

public class DailyResultManager : MonoBehaviourPun
{
    // 각 일자별 결산 데이터를 갖고있는 리스트
    private List<DailyTotalResult> DailyTotalResults = new();

    // 하루동안 사용한 아이템 기록
    private Dictionary<string, int> CurrentBusinessUsedItem = new();

    protected static DailyResultManager d_instance;
    public static DailyResultManager Instance
    {
        get
        {
            if (d_instance == null)
            {
                var CreatedCheck = FindFirstObjectByType<DailyResultManager>();

                if (null != CreatedCheck)
                {
                    d_instance = CreatedCheck;

                    return d_instance;
                }
                else
                {
                    return new GameObject("DailyResultManager").AddComponent<DailyResultManager>();
                }
            }
            else
            {
                return d_instance;
            }
        }
    }

    private void Awake()
    {
        if (d_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        d_instance = this;
    }

    private void Start()
    {

    }

    public void UsedItem(string ItemName, int Count)
    {
        if (!CurrentBusinessUsedItem.ContainsKey(ItemName))
        {
            CurrentBusinessUsedItem.Add(ItemName, Count);
        }
        else
        {
            CurrentBusinessUsedItem[ItemName] += Count;
        }

        Debug.Log($"Total Used : {ItemName}/{Count}");
    }

    public void CalculateResult(int Day)
    {
        if(GetDailyTotalResultAt(Day, out DailyTotalResult Result))
        {
            // 이미 계산한 일자인 경우
            return;
        }

        Result.ResultDay = Day;

        var UsedItemList = CurrentBusinessUsedItem.Keys;

        foreach (var item in UsedItemList)
        {
            var UsedItemData = ItemManager.Instance.GetItemDataByName(item);

            int UsedItemNum = CurrentBusinessUsedItem[item];

            Result.UsedItemSpecifications.Add(new UsedItemSpecification(item, UsedItemNum));

            Result.UsedItemPrice += UsedItemData.fOptionValue1 * UsedItemNum;
        }

        DailyTotalResults.Add(Result);
        SendDailyTotalResult(Result);
    }

    public bool GetDailyTotalResultAt(int Day, out DailyTotalResult Result)
    {
        Result = new();

        // 조회 일자에 대한 데이터가 아직 만들어지지 않은 경우
        if (0 > Day || Day >= DailyTotalResults.Count)
        {
            return false;
        }

        Result = DailyTotalResults[Day];

        return true;
    }

    public void ClearDailyUsedItem()
    {
        CurrentBusinessUsedItem.Clear();
    }

    //<< 단일 Result 송수신
    public void SendDailyTotalResult(DailyTotalResult result)
    {
        byte[] data = SerializeDailyTotalResult(result);

        photonView.RPC("ReceiveDailyTotalResult", RpcTarget.All, data);
    }

    [PunRPC]
    private void ReceiveDailyTotalResult(byte[] data)
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            return;
        }

        DailyTotalResult result = DeserializeDailyTotalResult(data);

        DailyTotalResults.Add(result);

        Debug.Log($"Received DailyTotalResult - ResultDay: {result.ResultDay}, UsedItemPrice: {result.UsedItemPrice}");

        foreach (var item in result.UsedItemSpecifications)
        {
            Debug.Log($"Item: {item.ItemName}, Value: {item.Count}");
        }
    }

    private byte[] SerializeDailyTotalResult(DailyTotalResult result)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, result);
            return ms.ToArray();
        }
    }

    private DailyTotalResult DeserializeDailyTotalResult(byte[] data)
    {
        using (MemoryStream ms = new MemoryStream(data))
        {
            BinaryFormatter bf = new BinaryFormatter();
            return (DailyTotalResult)bf.Deserialize(ms);
        }
    }
    //<<

    //<< 데이터 전체 송수신
    public void ReqSyncResultData()
    {
        DailyTotalResults.Clear();

        photonView.RPC("ClientToServerSyncDailyResultData", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer);
    }

    [PunRPC]
    public void ClientToServerSyncDailyResultData(Player TargetPlayer)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        DailyTotalResult[] Results = DailyTotalResults.ToArray();

        byte[] serializedData = SerializeDailyTotalResults(Results);
        photonView.RPC("ReceiveDailyTotalResults", TargetPlayer, serializedData);
    }

    [PunRPC]
    public void ReceiveDailyTotalResults(byte[] data)
    {
        DailyTotalResult[] Results = DeserializeDailyTotalResults(data);

        DailyTotalResults = new List<DailyTotalResult>(Results);

        Debug.Log("DailyTotalResults received. Count: " + DailyTotalResults.Count);
    }

    private byte[] SerializeDailyTotalResults(DailyTotalResult[] results)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, results);
            return ms.ToArray();
        }
    }

    private DailyTotalResult[] DeserializeDailyTotalResults(byte[] data)
    {
        using (MemoryStream ms = new MemoryStream(data))
        {
            BinaryFormatter bf = new BinaryFormatter();
            return (DailyTotalResult[])bf.Deserialize(ms);
        }
    }
}
