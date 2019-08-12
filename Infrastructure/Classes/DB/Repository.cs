using Infrastructure.Classes.DB;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;


public class Repository<Entity>  where Entity : class,IEntity, new()
    {
        protected DataBaseContext Context;
        public Repository(DataBaseContext context)
        {
            this.Context = context;
        }

        public virtual Entity Get(long Id, int companyId)
        {
            var result = Context.Set<Entity>().AsNoTracking().Where(entity => entity.Id == Id).FirstOrDefault();
            return result;
        }
        public virtual Entity Get(long Id)
        {
            var result = Context.Set<Entity>().AsNoTracking().Where(entity => entity.Id == Id).FirstOrDefault();
            return result;
        }
        public IQueryable<Entity> GetAll()
        {
            IQueryable<Entity> dbQuery = Context.Set<Entity>();
            return dbQuery.AsNoTracking();
        }
        public IQueryable<Entity> Find(Expression<Func<Entity, bool>> predicate, params Expression<Func<Entity, object>>[] navigationProperties)
        {
            IQueryable<Entity> dbQuery = Context.Set<Entity>();

            foreach (Expression<Func<Entity, object>> navigationProperty in navigationProperties)
                dbQuery = dbQuery.AsNoTracking().Include<Entity, object>(navigationProperty);

            return dbQuery.Where(predicate);
        }
        public Entity SingleOrDefault(Expression<Func<Entity, bool>> predicate)
        {
            IQueryable<Entity> dbQuery = Context.Set<Entity>();

            return dbQuery.AsNoTracking().SingleOrDefault(predicate);
        }

        public Entity Add(Entity entity)
        {
            entity.CreatedAt = DateTime.Now;
            Context.Set<Entity>().Add(entity);

            SaveChanges();
            Context.Entry(entity).GetDatabaseValues();
            return entity;
        }

        public IEnumerable<IEntity> AddRange(IEnumerable<IEntity> entities)
        {
            foreach (IEntity entity in entities)
            {
                entity.CreatedAt = DateTime.Now;
            }

            Context.Set<IEntity>().AddRange(entities);
            SaveChanges();
            return entities;
        }
        public IEnumerable<Entity> AddRange(IEnumerable<Entity> entities, Expression<Action<IEnumerable<Entity>>> postAction)
        {
            postAction.Compile().Invoke(entities);

            foreach (Entity entity in entities)
            {
                entity.CreatedAt = DateTime.Now;
                entity.Id = Context.Set<Entity>().Count() > 0 ? Context.Set<Entity>().Max(z => z.Id) + 1 : 1;
                var result = Context.Set<Entity>().Add(entity).Entity;
                SaveChanges();

            }

            return entities;
        }
        public Entity Remove(Entity entity)
        {
            Context.Set<Entity>().Remove(entity);
            SaveChanges();
            return entity;
        }

        public IEnumerable<Entity> RemoveRange(IEnumerable<Entity> entities)
        {
            Context.Set<Entity>().RemoveRange(entities);
            SaveChanges();
            return entities;
        }

        public Entity Update(Entity entity)
        {
            entity.UpdateAt = DateTime.Now;
            Context.Set<Entity>().Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            SaveChanges();
            return entity;
        }

    public bool Any(Expression<Func<Entity, bool>> predicate) => Context.Set<Entity>().Any(predicate);

    public void SaveChanges()
        {
            Context.SaveChanges();
        }
    }
