using ProjectHub.Models;
using System.Collections.Generic;

namespace ProjectHub.Repositories
{
    public interface IProfessorRepository
    {
        IEnumerable<Professor> GetAllProfessors();

        Professor GetProfessorById(int professorId);

        Professor GetProfessorByUserId(string userId);

        void AddProfessor(Professor professor);

        void EditProfessor(Professor professor);

        void DeleteProfessor(int professorId);

        void DeleteProfessor(Professor professor);
    }
}
