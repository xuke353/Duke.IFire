using System;
using IFire.Framework.Abstractions;
using IFire.Framework.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace IFire.Data.EFCore {

    public class EntityAuditingHelper {

        internal static void SetCreationAuditProperties(EntityEntry entityEntry, string username, int? userId) {
            CheckAndSetId(entityEntry);
            var entityAsObj = entityEntry.Entity;
            if (entityAsObj is not CreationEntity entityWithCreationTime) {
                return;
            }
            if (entityWithCreationTime.CreateTime.GetValueOrDefault() == default) {
                entityWithCreationTime.CreateTime = DateTime.Now;
            }

            if (!userId.HasValue || userId == 0) {
                return;
            }

            if (entityWithCreationTime.CreatorId != 0) {
                return;
            }

            entityWithCreationTime.CreatorId = userId.Value;
            entityWithCreationTime.CreatorName = username;
        }

        private static void CheckAndSetId(EntityEntry entry) {
            if (entry.Entity is IEntity<Guid> entity && entity.Id == Guid.Empty) {
                var idPropertyEntry = entry.Property("Id");

                if (idPropertyEntry != null && idPropertyEntry.Metadata.ValueGenerated == ValueGenerated.Never) {
                    entity.Id = Guid.NewGuid();
                }
            }
        }

        internal static void SetModificationAuditProperties(EntityEntry entityEntry, string username, int? userId) {
            var entityAsObj = entityEntry.Entity;
            if (entityAsObj is not AuditEntity entityWithAudit) {
                return;
            }
            entityWithAudit.ModifyTime = DateTime.Now;

            if (!userId.HasValue || userId == 0) {
                entityWithAudit.ModifierId = null;
                entityWithAudit.ModifierName = null;
                return;
            }
            entityWithAudit.ModifierId = userId;
            entityWithAudit.ModifierName = username;
        }

        internal static void SetDeletionAuditProperties(EntityEntry entityEntry, string username, int? userId) {
            CancelDeletionForSoftDelete(entityEntry);
            var entity = entityEntry.Entity;
            //var softDelete = entity as ISoftDelete;
            //if (softDelete == null) {
            //    return;
            //}
            if (entity is not ISoftDelete softDelete) {
                return;
            }

            softDelete.DeletionTime = DateTime.Now;
            softDelete.DeleterId = userId;
            softDelete.DeleterName = username;
        }

        private static void CancelDeletionForSoftDelete(EntityEntry entry) {
            if (!(entry.Entity is ISoftDelete)) {
                return;
            }

            entry.Reload();
            entry.State = EntityState.Modified;
            ((ISoftDelete)entry.Entity).IsDeleted = true;
        }
    }
}
