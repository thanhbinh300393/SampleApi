using Sample.Common.Exceptions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Sample.Common.Entities
{
    public static class EntityExtension
    {
        public static object GetValuePrimaryKey<TEntity>(this TEntity entity, string primaryKey = null)
            where TEntity : class
        {
            if (entity == null) return null;

            if (string.IsNullOrWhiteSpace(primaryKey))
                primaryKey = GetPrimaryKey<TEntity>();

            if (string.IsNullOrWhiteSpace(primaryKey))
                throw new DevelopException($"Primary key of type {typeof(TEntity).Name} not found", new { entity, primaryKey });

            PropertyInfo propPrimaryKey = typeof(TEntity).GetProperties().FirstOrDefault(x => x.Name == primaryKey);

            if (propPrimaryKey == null)
                throw new DevelopException($"Primary key of type {typeof(TEntity).Name} not found", new { entity, primaryKey });

            return propPrimaryKey.GetValue(entity);
        }

        public static TEntity SetValuePrimaryKey<TEntity>(this TEntity entity, object value, string primaryKey = null)
            where TEntity : class
        {
            if (entity == null) return null;

            if (string.IsNullOrWhiteSpace(primaryKey))
                primaryKey = GetPrimaryKey<TEntity>();

            if (string.IsNullOrWhiteSpace(primaryKey))
                throw new DevelopException($"Primary key of type {typeof(TEntity).Name} not found", new { entity, value, primaryKey });

            PropertyInfo propPrimaryKey = typeof(TEntity).GetProperties().FirstOrDefault(x => x.Name == primaryKey);

            if (propPrimaryKey == null)
                throw new DevelopException($"Primary key of type {typeof(TEntity).Name} not found",
                    new { entity, value, primaryKey, propPrimaryKey });

            propPrimaryKey.SetValue(entity, Convert.ChangeType(value, propPrimaryKey.PropertyType));
            return entity;
        }

        public static PropertyInfo GetPropertyPrimaryKey<TEntity>()
        {
            PropertyInfo propPrimaryKey = null;
            propPrimaryKey = typeof(TEntity).GetProperties().FirstOrDefault(x => x.GetCustomAttribute<KeyAttribute>(true) != null);

            if (propPrimaryKey == null)
                propPrimaryKey = typeof(TEntity).GetProperties().FirstOrDefault(x => x.Name == "Id");
            return propPrimaryKey;
        }

        public static string GetPrimaryKey<TEntity>()
            where TEntity : class
        {
            return GetPropertyPrimaryKey<TEntity>()?.Name;
        }

        public static string GetTableName<TEntity>()
            where TEntity : class
        {
            var tableAttribute = typeof(TEntity)
                .GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.Schema.TableAttribute), true)
                .FirstOrDefault() as System.ComponentModel.DataAnnotations.Schema.TableAttribute;

            if (tableAttribute != null)
                return $"{tableAttribute.Schema ?? "dbo"}.{tableAttribute.Name}";
            return typeof(TEntity).Name;
        }

        public static string GetTableNameNoneSchema<TEntity>()
           where TEntity : class
        {
            return GetTableNameNoneSchema(typeof(TEntity));
        }

        public static string GetTableNameNoneSchema(Type tEntity)
        {
            var tableAttribute = tEntity
                .GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.Schema.TableAttribute), true)
                .FirstOrDefault() as System.ComponentModel.DataAnnotations.Schema.TableAttribute;

            if (tableAttribute != null)
                return $"{tableAttribute.Name}";
            return tEntity.Name;
        }
    }
}
