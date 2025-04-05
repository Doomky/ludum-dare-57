using Framework.Managers;
using UnityEngine;

public class SteamManagerDefinition : ManagerDefinition
{
    [SerializeField]
    private uint _appID = 0;
    public uint AppID => this._appID;
}
