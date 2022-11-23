using ProjectHub.Models;
using System.Collections.Generic;

namespace ProjectHub.Repositories
{
    public interface ICompanyRepository
    {
        IEnumerable<Company> GetAllCompanies();

        Company GetCompanyById(int companyId);

        Company GetCompanyByUserId(string userId);

        void AddCompany(Company company);

        void EditCompany(Company company);

        void DeleteCompany(int companyId);

        void DeleteCompany(Company company);
    }
}
