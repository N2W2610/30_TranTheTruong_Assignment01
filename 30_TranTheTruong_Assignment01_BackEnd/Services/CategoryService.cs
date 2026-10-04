using _30_TranTheTruong_Assignment01_BackEnd.Models;
using _30_TranTheTruong_Assignment01_BackEnd.Repositories;

namespace _30_TranTheTruong_Assignment01_BackEnd.Services
{
    public interface ICategoryService
    {
        List<Category> GetAll();
        Category? Get(short id);
        ServiceResult Create(Category category);
        ServiceResult Update(short id, Category category);
        ServiceResult Delete(short id);
    }

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;

        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public List<Category> GetAll() => _repo.GetAll();

        public Category? Get(short id) => _repo.GetById(id);

        public ServiceResult Create(Category category)
        {
            var check = Normalize(category);
            if (!check.Success) return check;

            _repo.Add(category);
            return ServiceResult.Ok();
        }

        public ServiceResult Update(short id, Category category)
        {
            if (_repo.GetById(id) == null) return ServiceResult.Missing("Category not found.");

            category.CategoryId = id;
            var check = Normalize(category);
            if (!check.Success) return check;

            return _repo.Update(category) ? ServiceResult.Ok() : ServiceResult.Missing("Category not found.");
        }

        public ServiceResult Delete(short id)
        {
            if (_repo.GetById(id) == null) return ServiceResult.Missing("Category not found.");

            if (_repo.IsUsedByNews(id))
                return ServiceResult.Fail("This category is used by news articles and cannot be deleted.");

            if (_repo.HasChildren(id))
                return ServiceResult.Fail("This category is the parent of other categories and cannot be deleted.");

            return _repo.Delete(id) ? ServiceResult.Ok() : ServiceResult.Missing("Category not found.");
        }

        private ServiceResult Normalize(Category category)
        {
            category.CategoryName = category.CategoryName!.Trim();
            category.CategoryDesciption = category.CategoryDesciption!.Trim();
            category.IsActive ??= true;

            if (category.ParentCategoryId.HasValue && _repo.GetById(category.ParentCategoryId.Value) == null)
                return ServiceResult.Fail("The selected parent category does not exist.");

            return ServiceResult.Ok();
        }
    }
}
