using _30_TranTheTruong_Assignment01_BackEnd.DAOs;
using _30_TranTheTruong_Assignment01_BackEnd.Models;

namespace _30_TranTheTruong_Assignment01_BackEnd.Repositories
{
    public class TagRepository : ITagRepository
    {
        public List<Tag> GetAll() => TagDAO.Instance.GetAll();
    }

}
