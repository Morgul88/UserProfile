using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public class CommentRepository(DataContext context) : BaseRepository<CommentEntity>(context)
{
    private readonly DataContext _context = context;
}
