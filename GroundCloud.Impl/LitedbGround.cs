using GroundCloud.Contracts;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;

namespace GroundCloud.Impl
{
    public class LitedbGround : IGround
    {

        string connectionString = null;

        public LitedbGround(string connectionString = null)
        {
            this.connectionString = connectionString ?? $@"filename={Assembly.GetCallingAssembly().GetName().Name}.db;mode=Exclusive";
        }

        /// <summary>
        /// Insert the specified entity.
        /// </summary>
        /// <returns>The insert.</returns>
        /// <param name="entity">Entity.</param>
        /// <typeparam name="Entity">The 1st type parameter.</typeparam>
        public IObservable<Entity> Insert<Entity>(Entity entity)
        {
            return Observable.Create<Entity>((IObserver<Entity> observer) =>
            {

                if (entity == null)
                {
                    observer.OnError(new ArgumentNullException(Constants.PARAM_ENTITY, Constants.PARAM_CANNOT_BE_NULL));
                }
                else
                {
                    try
                    {
                        using (var db = new LiteDatabase(connectionString))
                        {
                            // Get a collection (or create, if doesn't exist)
                            var entityCollection = db.GetCollection<Entity>(typeof(Entity).Name);

                            var insertedID = entityCollection.Insert(entity);

                            if (insertedID == 0)
                            {
                                observer.OnError(new ArgumentNullException(Constants.PARAM_ENTITY, Constants.INSERT_FAILED));
                            }
                            else
                            {

                                var insertedEntity = entityCollection.FindById(insertedID);
                                if (insertedEntity != null)
                                {
                                    observer.OnNext(insertedEntity);
                                    observer.OnCompleted();
                                }
                                else
                                {
                                    observer.OnError(new ArgumentNullException(Constants.PARAM_ENTITY, Constants.NOT_ABLE_GET_INSERTED_ENTITY));
                                }
                            }
                        }
                    }
                    catch (LiteException ex)
                    {
                        observer.OnError(ex);
                    }
                }
                return Disposable.Empty;
            });
        }

        /// <summary>
        /// Update the specified entity.
        /// </summary>
        /// <returns>The update.</returns>
        /// <param name="entity">Entity.</param>
        /// <typeparam name="Entity">The 1st type parameter.</typeparam>
        public IObservable<Entity> Update<Entity>(Entity entity)
        {
            return Observable.Create<Entity>((IObserver<Entity> observer) =>
            {

                if (entity == null)
                {
                    observer.OnError(new ArgumentNullException(Constants.PARAM_ENTITY, Constants.PARAM_CANNOT_BE_NULL));
                }
                else
                {
                    try
                    {
                        using (var db = new LiteDatabase(connectionString))
                        {
                            // Get a collection (or create, if doesn't exist)
                            var entityCollection = db.GetCollection<Entity>(typeof(Entity).Name);
                            // var result = entityCollection.FindAll();
                            bool isUpdated = entityCollection.Update(entity);
                            if (isUpdated)
                            {
                                //TODO: find updated entity and send it OnNext
                                observer.OnNext(entity);
                                observer.OnCompleted();
                            }
                            else
                            {
                                observer.OnError(new ArgumentNullException(Constants.PARAM_ENTITY, Constants.UPDATE_FAILED));
                            }
                        }
                    }
                    catch (LiteException ex)
                    {
                        observer.OnError(ex);
                    }
                }
                return Disposable.Empty;
            });
        }

        /// <summary>
        /// Upsert the specified entity.
        /// </summary>
        /// <returns>The upsert.</returns>
        /// <param name="entity">Entity.</param>
        /// <typeparam name="Entity">The 1st type parameter.</typeparam>
        public IObservable<Entity> Upsert<Entity>(Entity entity)
        {
            return Observable.Create<Entity>((IObserver<Entity> observer) =>
            {

                if (entity == null)
                {
                    observer.OnError(new ArgumentNullException(Constants.PARAM_ENTITY, Constants.PARAM_CANNOT_BE_NULL));
                }
                else
                {
                    try
                    {
                        using (var db = new LiteDatabase(connectionString))
                        {
                            // Get a collection (or create, if doesn't exist)
                            var entityCollection = db.GetCollection<Entity>(typeof(Entity).Name);

                            bool isUpserted = entityCollection.Upsert(entity);

                            observer.OnNext(entity);
                            observer.OnCompleted();

                        }
                    }
                    catch (LiteException ex)
                    {
                        observer.OnError(ex);
                    }
                }

                return Disposable.Empty;
            });
        }

        /// <summary>
        /// Delete the specified entity.
        /// </summary>
        /// <returns>The delete.</returns>
        /// <param name="entity">Entity.</param>
        /// <typeparam name="Entity">The 1st type parameter.</typeparam>
        public IObservable<Entity> Delete<Entity>(string id)
        {
            return Observable.Create<Entity>((IObserver<Entity> observer) =>
            {

                if (string.IsNullOrEmpty(id))
                {
                    observer.OnError(new ArgumentNullException(Constants.PARAM_ID, Constants.PARAM_CANNOT_BE_NULL));
                }
                else
                {
                    try
                    {
                        using (var db = new LiteDatabase(connectionString))
                        {
                            // Get a collection (or create, if doesn't exist)
                            var entityCollection = db.GetCollection<Entity>(typeof(Entity).Name);

                            var resultEntity = entityCollection.FindById(Convert.ToInt32(id));

                            if (resultEntity != null)
                            {

                                bool isDeleted = entityCollection.Delete(Convert.ToInt32(id));
                                if (isDeleted)
                                {
                                    observer.OnNext(resultEntity);
                                    observer.OnCompleted();
                                }
                                else
                                {
                                    observer.OnError(new ArgumentNullException(Constants.PARAM_ENTITY, Constants.DELETE_FAILED));
                                }
                            }
                            else
                            {
                                observer.OnError(new ArgumentNullException(Constants.PARAM_ENTITY, Constants.DELETE_ENTITY_NOTFOUND));
                            }
                        }
                    }
                    catch (LiteException ex)
                    {
                        observer.OnError(ex);
                    }
                }

                return Disposable.Empty;
            });
        }

        /// <summary>
        /// Fetchs all.
        /// </summary>
        /// <returns>The all.</returns>
        /// <typeparam name="Entity">The 1st type parameter.</typeparam>
        public IObservable<IEnumerable<Entity>> FetchAll<Entity>()
        {
            return Observable.Create<IEnumerable<Entity>>((IObserver<IEnumerable<Entity>> observer) =>
            {

                try
                {
                    using (var db = new LiteDatabase(connectionString))
                    {
                        // Get a collection (or create, if doesn't exist)
                        var entityCollection = db.GetCollection<Entity>(typeof(Entity).Name);

                        var resEntityCollection = entityCollection.FindAll();

                        if (resEntityCollection != null)
                        {
                            observer.OnNext(resEntityCollection);
                            observer.OnCompleted();
                        }
                        else
                        {
                            //observer.OnError(new ArgumentNullException(Constants.PARAM_ENTITY, Constants.NO_ENTITIES_FOUND));
                            observer.OnError(new Exception(Constants.NO_ENTITIES_FOUND));
                        }
                    }
                }
                catch (LiteException ex)
                {
                    observer.OnError(ex);
                }
                return Disposable.Empty;
            });
        }

        /// <summary>
        /// Fetchs the by identifier.
        /// </summary>
        /// <returns>The by identifier.</returns>
        /// <param name="id">Identifier.</param>
        /// <typeparam name="Entity">The 1st type parameter.</typeparam>
        public IObservable<Entity> FetchById<Entity>(string id)
        {
            return Observable.Create<Entity>((IObserver<Entity> observer) =>
            {

                if (string.IsNullOrEmpty(id))
                {
                    observer.OnError(new ArgumentNullException(Constants.PARAM_ID, Constants.PARAM_CANNOT_BE_NULL));
                }
                else
                {
                    try
                    {
                        using (var db = new LiteDatabase(connectionString))
                        {
                            // Get a collection (or create, if doesn't exist)
                            var entityCollection = db.GetCollection<Entity>(typeof(Entity).Name);


                            var resEntity = entityCollection.FindById(Convert.ToInt32(id));
                            if (resEntity != null)
                            {
                                observer.OnNext(resEntity);
                                observer.OnCompleted();
                            }
                            else
                            {
                                observer.OnError(new ArgumentNullException(Constants.PARAM_ENTITY, Constants.NO_ENTITY_FOUND));
                            }
                        }
                    }
                    catch (LiteException ex)
                    {
                        observer.OnError(ex);
                    }

                }
                return Disposable.Empty;
            });
        }

        /// <summary>
        /// Drop Collection.
        /// </summary>
        /// <returns>true - If Collection Is Dropped Else false</returns>
        /// <param name="name">Name Of Collection.</param>
        public IObservable<bool> Drop(string name)
        {
            return Observable.Create<bool>((IObserver<bool> observer) =>
            {

                if (string.IsNullOrEmpty(name))
                {
                    observer.OnError(new ArgumentNullException(Constants.PARAM_ID, Constants.PARAM_CANNOT_BE_NULL));
                }
                else
                {
                    try
                    {
                        using (var db = new LiteDatabase(connectionString))
                        {
                            var entityCollection = db.DropCollection(name);
                            observer.OnNext(entityCollection);
                            observer.OnCompleted();
                        }
                    }
                    catch (LiteException ex)
                    {
                        observer.OnError(ex);
                    }
                }
                return Disposable.Empty;
            });
        }
    }
}