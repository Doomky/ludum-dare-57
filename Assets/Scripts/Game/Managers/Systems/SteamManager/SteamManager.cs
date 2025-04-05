using Framework.Managers;

public class SteamManager : Manager<SteamManagerDefinition>
{
    public override void Load()
    {
        base.Load();

        if (this._definition.AppID != 0)
        {
            try
            {
                Steamworks.SteamClient.Init(this._definition.AppID);
            }
            catch (System.Exception e)
            {
                DebugHelper.LogError(this, e.Message);
            }
        }
    }

    public override void Unload()
    {
        base.Unload();
    }

    protected void Update()
    {
        Steamworks.SteamClient.RunCallbacks();
    }
}
