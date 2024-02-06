﻿using Infrastructure.Contexts;
using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public abstract class BaseRepository<TEntity> where TEntity : class
{
    private readonly DataContext _context;

    protected BaseRepository(DataContext context)
    {
        _context = context;
    }

    public virtual TEntity Create(TEntity entity)
    {
        try
        {
            
            _context.Set<TEntity>().Add(entity);
            _context.SaveChanges();
            return entity;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public virtual IEnumerable<TEntity> GetAll()
    {
        try
        {
            return _context.Set<TEntity>().ToList();
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public virtual TEntity GetOne(Expression<Func<TEntity,bool>> predicate) 
    {
        try
        {
            var result = _context.Set<TEntity>().FirstOrDefault(predicate);
            if(result != null)
            {
                return result;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }



    public virtual TEntity Update(Expression<Func<TEntity, bool>> predicate, TEntity entity)
    {
        try
        {
            var entityToUpdate = _context.Set<TEntity>().FirstOrDefault(predicate);
            if (entityToUpdate != null!)
            {
                _context.Entry(entityToUpdate).CurrentValues.SetValues(entity);
                _context.SaveChanges();

                return entityToUpdate;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public virtual bool Delete(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var entity = _context.Set<TEntity>().FirstOrDefault(predicate);
            if (entity != null)
            {
                
                _context.Set<TEntity>().Remove(entity);
                _context.SaveChanges();

                return true;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return false!;
    }

    public virtual bool DeleteByEntity(TEntity entity)
    {
        try
        {
            
            if (entity != null)
            {

                _context.Set<TEntity>().Remove(entity);
                _context.SaveChanges();

                return true;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return false!;
    }

    public virtual bool Exists(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
           return _context.Set<TEntity>().Any(predicate);
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return false!;
    }
}