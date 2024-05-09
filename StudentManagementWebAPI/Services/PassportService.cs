using Microsoft.EntityFrameworkCore;
using StudentManagementWebAPI.Data;
using StudentManagementWebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagementWebAPI.Services
{
    public class PassportService : IPassportService
    {
        private readonly DataContext _context;

        public PassportService(DataContext context)
        {
            _context = context;
        }

        public async Task<Passport> GetPassportById(int passportId)
        {
            var passport = await _context.Passports.FindAsync(passportId);
            return passport ?? new Passport();
        }

        public async Task<IEnumerable<Passport>> GetPassports()
        {
            return await _context.Passports.ToListAsync();
        }

        public async Task<Passport> CreatePassport(Passport passport)
        {
            _context.Passports.Add(passport);
            await _context.SaveChangesAsync();
            return passport;
        }

        public async Task<bool> UpdatePassport(int passportId, Passport updatedPassport)
        {
            var existingPassport = await _context.Passports.FindAsync(passportId);
            if (existingPassport == null)
            {
                return false;
            }

            existingPassport.PassportNumber = updatedPassport.PassportNumber;
            existingPassport.ExpiryDate = updatedPassport.ExpiryDate;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePassport(int passportId)
        {
            var passport = await _context.Passports.FindAsync(passportId);
            if (passport == null)
            {
                return false;
            }

            _context.Passports.Remove(passport);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
