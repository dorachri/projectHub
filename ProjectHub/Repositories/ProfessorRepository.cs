using Microsoft.EntityFrameworkCore;
using ProjectHub.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHub.Repositories
{
    public class ProfessorRepository : IProfessorRepository
    {
        private readonly AppDbContext _appDbContext;

        public ProfessorRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void AddProfessor(Professor professor)
        {
            _appDbContext.Professors.Add(professor);
            _appDbContext.SaveChanges();
        }

        public void DeleteProfessor(int professorId)
        {
            Professor professor = _appDbContext.Professors.FirstOrDefault(p => p.ProfessorId == professorId);
            _appDbContext.Professors.Remove(professor);
            _appDbContext.SaveChanges();
        }

        public void DeleteProfessor(Professor professor)
        {
            _appDbContext.Professors.Remove(professor);
            _appDbContext.SaveChanges();
        }

        public void EditProfessor(Professor professor)
        {
            _appDbContext.Professors.Update(professor);
            _appDbContext.SaveChanges();
        }

        public IEnumerable<Professor> GetAllProfessors()
        {
            return _appDbContext.Professors.Include(p => p.User);
        }

        public Professor GetProfessorById(int professorId)
        {
            return _appDbContext.Professors
                .Include(p => p.User)
                .FirstOrDefault(p => p.ProfessorId == professorId);
        }

        public Professor GetProfessorByUserId(string userId)
        {
            return _appDbContext.Professors.FirstOrDefault(p => p.UserId == userId);
        }
    }
}
