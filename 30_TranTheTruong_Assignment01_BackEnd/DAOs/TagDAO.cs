using _30_TranTheTruong_Assignment01_BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace _30_TranTheTruong_Assignment01_BackEnd.DAOs
{
    public class TagDAO
    {
        private static TagDAO? _instance;
        private static readonly object _lock = new();

        private TagDAO() { }

        public static TagDAO Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new TagDAO();
                }
            }
        }

        public List<Tag> GetAll()
        {
            using var db = new FunewsManagementContext();
            return db.Tags.AsNoTracking().OrderBy(t => t.TagId).ToList();
        }
    }
}
