using StudentManagementWebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagementWebAPI.Services
{
    public interface IPassportService
    {
        Task<Passport> GetPassportById(int passportId);
        Task<IEnumerable<Passport>> GetPassports();
        Task<Passport> CreatePassport(Passport passport);
        Task<bool> UpdatePassport(int passportId, Passport updatedPassport);
        Task<bool> DeletePassport(int passportId);
    }
}
