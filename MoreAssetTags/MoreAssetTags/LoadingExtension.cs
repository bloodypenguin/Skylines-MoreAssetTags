using ICities;
using MoreAssetTags.Detours;
using MoreAssetTags.Redirection;

namespace MoreAssetTags
{
    public class LoadingExtension : LoadingExtensionBase
    {
        public override void OnCreated(ILoading loading)
        {
            base.OnCreated(loading);
            Redirector<SteamHelperDetour>.Deploy();
        }

        public override void OnReleased()
        {
            base.OnReleased();
            Redirector<SteamHelperDetour>.Revert();
        }
    }
}