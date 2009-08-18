﻿namespace MapinfoWrapper.DataAccess.RowOperations.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a basic row in a Mapinfo table.
    /// <para>When creating strong typed entities you will need to inherit from this class or 
    /// <see cref="MappableEntity"/></para>
    /// </summary>
    public class BaseEntity
    {
        public enum EntityState
        {
            /// <summary>
            /// The entity is in a new state and has not been added to a table.
            /// </summary>
            New = 0,
            /// <summary>
            /// The entity is in a deleted state, if the entity is in this state it is
            /// considered to be out of date.
            /// </summary>
            Deleted = 2,
            /// <summary>
            /// The entity is in a modifed state.
            /// </summary>
            Modifed = 4,
        }

        private int rowid;

        public BaseEntity()
        {
            this.State = EntityState.New;       
        }

        /// <summary>
        /// Gets the row id for the current entity.
        /// <para>Returns 0 if the entity has deleted or is a new entity.</para>
        /// </summary>
        public int RowId
        {
            get
            {
                if (this.State == EntityState.Deleted || 
                    this.State == EntityState.New)
                {
                    return 0;
                }
                return this.rowid;
            }
            internal set
            {
                this.rowid = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="Table"/> that this record is associated with.  Returns null if the
        /// record is not inserted into a table
        /// </summary>
        [MapinfoIgnore]
        public Table Table { get; internal set; }

        /// <summary>
        /// Gets a <see cref="EntityState"/> representing the current state of the entity.
        /// </summary>
        [MapinfoIgnore]
        public EntityState State { get; internal set;}

        /// <summary>
        /// Gets and sets the <see cref="List{T}"/> that is used as this entities backing store.
        /// </summary>
        [MapinfoIgnore]
        internal List<ColumnMapping> BackingStore { get; set; }

        [MapinfoIgnore]
        public object this[string columnName]
        {
            get
            {
                return this.BackingStore.First(col => col.ColumnName.ToLower() == columnName.ToLower()).Data;
            }
        }
    }

    /// <summary>
    /// Attribute used to mark a property to be ignored when being loaded with data.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property , Inherited = false, AllowMultiple = true)]
    internal sealed class MapinfoIgnore : Attribute
    {
        public MapinfoIgnore() { }
    }
}
