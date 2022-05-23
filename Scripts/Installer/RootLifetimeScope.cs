using Kameffee.AudioPlayer;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Unity1week202205.Installer
{
    public class RootLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            var audioPlayer = AudioPlayer.Instance as AudioPlayer;
            audioPlayer.InitializeBgm(() => Resources.Load<BgmBundle>("AudioBundles/BgmBundle"));
            audioPlayer.InitializeSe(() => Resources.Load<SeBundle>("AudioBundles/SeBundle"));

            builder.RegisterInstance(AudioPlayer.Instance).As<IAudioPlayer>();
            
            builder.RegisterBuildCallback(resolver =>
            {
                resolver.Resolve<IAudioPlayer>().Bgm.SetVolume(0.5f);
                resolver.Resolve<IAudioPlayer>().Se.SetVolume(0.5f);
            });
        }
    }
}
