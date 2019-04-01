using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using GroundCloud.Contracts;
using LiteDB;

namespace GroundCloud.Impl
{
    public class GroundObservables:IGround
    {
        public GroundObservables()
        {

        }

        string connectionString = @"MyData.db";

        /// <summary>
        /// Insert the specified entity.
        /// </summary>
        /// <returns>The insert.</returns>
        /// <param name="entity">Entity.</param>
        /// <typeparam name="Entity">The 1st type parameter.</typeparam>
        public IObservable<Entity> Insert<Entity>(Entity entity)
        {
            return Observable.Create<Entity>((IObserver<Entity> observer) => {

                if (entity == null)
                {
                    observer.OnError(new ArgumentNullException(Constants.PARAM_ENTITY, Constants.PARAM_CANNOT_BE_NULL));
                }

                using (var db = new LiteDatabase(connectionString))
                {
                    // Get a collection (or create, if doesn't exist)
                    var entityCollection = db.GetCollection<Entity>(typeof(Entity).ToString());

                    int insertedID = entityCollection.Insert(entity);

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
            return Observable.Create<Entity>((IObserver<Entity> observer) => {

                if (entity == null)
                {
                    observer.OnError(new ArgumentNullException(Constants.PARAM_ENTITY, Constants.PARAM_CANNOT_BE_NULL));
                }

                using (var db = new LiteDatabase(connectionString))
                {
                    // Get a collection (or create, if doesn't exist)
                    var entityCollection = db.GetCollection<Entity>(typeof(Entity).ToString());
                    bool isUpdated= entityCollection.Update(entity);
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
            return Observable.Create<Entity>((IObserver<Entity> observer) => {

                if (entity == null)
                {
                    observer.OnError(new ArgumentNullException(Constants.PARAM_ENTITY, Constants.PARAM_CANNOT_BE_NULL));
                }

                using (var db = new LiteDatabase(connectionString))
                {
                    // Get a collection (or create, if doesn't exist)
                    var entityCollection = db.GetCollection<Entity>(typeof(Entity).ToString());

                    bool isUpserted = entityCollection.Upsert(entity);

                    if (isUpserted)
                    {
                        //TODO: updated entity need to pass
                        observer.OnNext(entity);
                        observer.OnCompleted();
                    }
                    else
                    {
                        observer.OnError(new ArgumentNullException(Constants.PARAM_ENTITY, Constants.UPSERT_FAILED));
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
            return Observable.Create<Entity>((IObserver<Entity> observer) => {

                if (string.IsNullOrEmpty(id))
                {
                    observer.OnError(new ArgumentNullException(Constants.PARAM_ID, Constants.PARAM_CANNOT_BE_NULL));
                }
                else
                {
                    using (var db = new LiteDatabase(connectionString))
                    {
                        // Get a collection (or create, if doesn't exist)
                        var entityCollection = db.GetCollection<Entity>(typeof(Entity).ToString());

                        var resultEntity = entityCollection.FindById(id);

                        if (resultEntity != null)
                        { 
                            bool isDeleted= entityCollection.Delete(id);
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
            return Observable.Create<IEnumerable<Entity>>((IObserver<IEnumerable<Entity>> observer) => {

                using (var db = new LiteDatabase(connectionString))
                {
                    // Get a collection (or create, if doesn't exist)
                    var entityCollection = db.GetCollection<Entity>(typeof(Entity).ToString());

                    var resEntityCollection = entityCollection.FindAll();

                    if (resEntityCollection != null)
                    {
                        observer.OnNext(resEntityCollection);
                        observer.OnCompleted();
                    }
                    else
                    {
                        observer.OnError(new ArgumentNullException(Constants.PARAM_ENTITY, Constants.NO_ENTITIES_FOUND));
                    }
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
            return Observable.Create<Entity>((IObserver<Entity> observer) => {

                if (string.IsNullOrEmpty(id))
                {
                    observer.OnError(new ArgumentNullException(Constants.PARAM_ID, Constants.PARAM_CANNOT_BE_NULL));
                }

                using (var db = new LiteDatabase(connectionString))
                {
                    // Get a collection (or create, if doesn't exist)
                    var entityCollection = db.GetCollection<Entity>(typeof(Entity).ToString());

                    var resEntity=  entityCollection.FindById(id);
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

                return Disposable.Empty;
            });
        }





    }
}
