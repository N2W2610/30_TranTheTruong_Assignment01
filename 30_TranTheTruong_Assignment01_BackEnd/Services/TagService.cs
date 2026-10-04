using _30_TranTheTruong_Assignment01_BackEnd.Models;
using _30_TranTheTruong_Assignment01_BackEnd.Repositories;

namespace _30_TranTheTruong_Assignment01_BackEnd.Services
{
    public interface ITagService
    {
        List<Tag> GetAll();
    }

    public class TagService : ITagService
    {
        private readonly ITagRepository _repo;

        public TagService(ITagRepository repo)
        {
            _repo = repo;
        }

        public List<Tag> GetAll() => _repo.GetAll();
    }
}
