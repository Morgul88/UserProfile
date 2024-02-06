using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class UserProfileRepository(DataContext context) : BaseRepository<UserProfileEntity>(context)
{
    private readonly DataContext _context = context;

    public override IEnumerable<UserProfileEntity> GetAll()
    {
        try
        {
            return _context.UserProfiles.Include(x => x.User).Include(x => x.Adress).Include(x => x.Comment).Include(x => x.Role).ToList();
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public override UserProfileEntity GetOne(Expression<Func<UserProfileEntity, bool>> predicate)
    {
        try
        {
            return _context.UserProfiles.Include(x => x.User).Include(x => x.Adress).Include(x => x.Comment).Include(x => x.Role).FirstOrDefault(predicate)!;

        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    //public override bool Delete(Expression<Func<UserProfileEntity, bool>> predicate)
    //{
    //    try
    //    {
    //        var entity = _context.Set<UserProfileEntity>().Include(x => x.Adress).Include(x => x.Role).Include(x => x.User).FirstOrDefault(predicate);
    //        if (entity != null)
    //        {

    //            _context.Set<UserProfileEntity>().Remove(entity);
    //            _context.SaveChanges();

    //            return true;
    //        }
    //    }
    //    catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
    //    return false!;
    //}

    //public virtual bool DeleteBaseVersion(Expression<Func<UserProfileEntity, bool>> predicate)
    //{
    //    try
    //    {
    //        var entity = _context.Set<UserProfileEntity>().FirstOrDefault(predicate);
    //        if (entity != null)
    //        {

    //            _context.Set<UserProfileEntity>().Remove(entity);
    //            _context.SaveChanges();

    //            return true;
    //        }
    //    }
    //    catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
    //    return false!;
    //}

}
