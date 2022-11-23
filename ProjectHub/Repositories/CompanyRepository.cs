using Microsoft.EntityFrameworkCore;
using ProjectHub.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHub.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly AppDbContext _appDbContext;

        public CompanyRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void AddCompany(Company company)
        {
            _appDbContext.Companies.Add(company);
            _appDbContext.SaveChanges();
        }

        public void DeleteCompany(int companyId)
        {
            Company company = _appDbContext.Companies.FirstOrDefault(c => c.CompanyId == companyId);
            _appDbContext.Companies.Remove(company);
            _appDbContext.SaveChanges();
        }

        public void DeleteCompany(Company company)
        {
            _appDbContext.Companies.Remove(company);
            _appDbContext.SaveChanges();
        }

        public void EditCompany(Company company)
        {
            _appDbContext.Companies.Update(company);
            _appDbContext.SaveChanges();
        }

        public IEnumerable<Company> GetAllCompanies()
        {
            return _appDbContext.Companies.Include(c => c.User);
        }

        public Company GetCompanyById(int companyId)
        {
            return _appDbContext.Companies
                .Include(c => c.User)
                .FirstOrDefault(c => c.CompanyId == companyId);
        }

        public Company GetCompanyByUserId(string userId)
        {
            return _appDbContext.Companies.FirstOrDefault(c => c.UserId == userId);
        }
    }
}
