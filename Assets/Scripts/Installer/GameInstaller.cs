using AirPlaneSystems;
using Data.Stats;
using Mechanics.Movement;
using UnityEngine;
using Zenject;

namespace Installer
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private Stats statsObj;

        [SerializeField] private AirPlaneController airPlaneController;

        public override void InstallBindings()
        {
            Container.Bind<IStats>()
                .To<Stats>()
                .FromInstance(statsObj);

            Container.Bind<IMovable>()
                .To<AirPlaneController>()
                .FromInstance(airPlaneController);
        }
    }
}