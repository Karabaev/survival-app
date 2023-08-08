using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Karabaev.GameKit.Common;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Karabaev.GameKit.Entities
{
    [UsedImplicitly]
    public class EntitiesManager : ITickable
    {
        private readonly List<IEntity> _entities = new();

        public T CreateEntity<T>(IObjectResolver resolver) where T : IEntity
        {
            var entity = Activator.CreateInstance<T>();
            resolver.Inject(entity);
            _entities.Add(entity);
            return entity;
        }

        public void RemoveEntity(IEntity entity) => _entities.Remove(entity);

        void ITickable.Tick()
        {
            var now = GameTime.Now;
            var deltaTime = Time.deltaTime;

            foreach (var entity in _entities) {
                entity.Tick(deltaTime, now);
            }
        }
    }
}