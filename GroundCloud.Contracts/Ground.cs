using System;
using System.Collections.Generic;

namespace GroundCloud.Contracts
{
    /// <summary>
    /// Ground Interface Defining Local Caching Operations
    /// </summary>
    public interface IGround
    {
        /// <summary>
        /// Create Entity In Local Cache
        /// </summary>
        /// <param name="entity">Entity To Be Created</param>
        /// <typeparam name="Entity">Type Of Entity</typeparam>
        /// <returns>Created Entity</returns>
        IObservable<Entity> Insert<Entity>(Entity entity);
        /// <summary>
        /// Update Entity In Local Cache
        /// </summary>
        /// <param name="entity">Entity To Be Updated</param>
        /// <typeparam name="Entity">Type Of Entity</typeparam>
        /// <returns>Updated Entity</returns>
        IObservable<Entity> Update<Entity>(Entity entity);
        /// <summary>
        /// Update Or Insert Entity In Local Cache
        /// </summary>
        /// <param name="entity">Entity To Be Upserted</param>
        /// <typeparam name="Entity">Type Of Entity</typeparam>
        /// <returns>Upserted Entity</returns>
        IObservable<Entity> Upsert<Entity>(Entity entity);
        /// <summary>
        /// Delete Entity From Local Cache
        /// </summary>
        /// <param name="entity">Entity To Be Deleted</param>
        /// <typeparam name="Entity">Type Of Entity</typeparam>
        /// <returns>Deleted Entity</returns>
        IObservable<Entity> Delete<Entity>(Entity entity);
        /// <summary>
        /// Fetch All Entities From Local Cache
        /// </summary>
        /// <typeparam name="Entity">Type Of Entity</typeparam>
        /// <returns>All Entities</returns>
        IObservable<IEnumerable<Entity>> FetchAll<Entity>();
        /// <summary>
        /// Fetch Entity By Entity Id From Local Cache
        /// </summary>
        /// <param name="id">Primary Id Of Entity To Fetch</param>
        /// <typeparam name="Entity">Type Of Entity</typeparam>
        /// <returns>Entity Matching Primary Id</returns>
        IObservable<Entity> FetchById<Entity>(string id);
    }
}