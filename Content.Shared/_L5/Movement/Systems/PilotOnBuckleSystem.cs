using Content.Shared._L5.Movement.Components;
using Content.Shared.Buckle.Components;
using Content.Shared.Movement.Components;
using Content.Shared.Movement.Systems;

namespace Content.Shared._L5.Movement.Systems
{
    public sealed class PilotOnBuckleSystem() : EntitySystem
    {
        [Dependency] private readonly SharedMoverController _moverController = default!;
        public override void Initialize()
        {
            // When strapping into an object, update entity associations.
            SubscribeLocalEvent<PilotOnBuckleComponent, StrappedEvent>(OnStrap);
            SubscribeLocalEvent<PilotOnBuckleComponent, UnstrappedEvent>(OnUnstrap);
        }

        /// <summary>
        /// When strapping into an object with an attach on sit component, update entity associations.
        /// </summary>
        private void OnStrap(Entity<PilotOnBuckleComponent> ent, ref StrappedEvent args)
        {
            var moverEntity = args.Buckle.Owner;
            var movedEntity = args.Strap.Owner;

            // We need the moved entity to be able to be moved and have a move speed to use.
            EnsureComp<InputMoverComponent>(movedEntity);
            EnsureComp<MovementSpeedModifierComponent>(movedEntity);

            _moverController.SetRelay(moverEntity, movedEntity);
        }

        /// <summary>
        /// When unstrapping from a moved entity, update entity associations.
        /// TODO: Not important for initial PR, but technically we'd want to do this on AttachOnSitComponent removal.
        /// </summary>
        private void OnUnstrap(Entity<PilotOnBuckleComponent> ent, ref UnstrappedEvent args)
        {
            var moverEntity = args.Buckle.Owner;
            var movedEntity = args.Strap.Owner;
            RemCompDeferred<RelayInputMoverComponent>(moverEntity);
            RemCompDeferred<MovementRelayTargetComponent>(movedEntity);
        }
    }
}
