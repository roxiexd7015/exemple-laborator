using Laborator2.Domain.Models;
using Laborator2.Domain.Repositories;
using LanguageExt;
using Laborator2.Data.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using static Laborator2.Domain.Models.CartStates;
using static LanguageExt.Prelude;

namespace Laborator2.Data.Repositories
{
    public class OrderHeadersRepository : IOrderHeadersRepository
    {
        private readonly CartsContext dbContext;

        public OrderHeadersRepository(CartsContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public TryAsync<List<CalculatedFinalPrice>> TryGetExistingPrice() => async () => (await (
                          from oh in dbContext.OrderHeaders
                          join ol in dbContext.OrderLines on oh.OrderId equals ol.OrderId
                          select new { ol.OrderLineId /*ProductId*/, oh.Address, oh.Total })
                          .AsNoTracking()
                          .ToListAsync())
                          .Select(result => new CalculatedSudentGrade(
                                                    StudentRegistrationNumber: new(result.RegistrationNumber),
                                                    ExamGrade: new(result.Exam ?? 0m),
                                                    ActivityGrade: new(result.Activity ?? 0m),
                                                    FinalGrade: new(result.Final ?? 0m))
                          {
                              GradeId = result.GradeId
                          })
                          .ToList();

        public TryAsync<Unit> TrySaveGrades(PublishedExamGrades grades) => async () =>
        {
            var students = (await dbContext.Students.ToListAsync()).ToLookup(student => student.RegistrationNumber);
            var newGrades = grades.GradeList
                                    .Where(g => g.IsUpdated && g.GradeId == 0)
                                    .Select(g => new GradeDto()
                                    {
                                        StudentId = students[g.StudentRegistrationNumber.Value].Single().StudentId,
                                        Exam = g.ExamGrade.Value,
                                        Activity = g.ActivityGrade.Value,
                                        Final = g.FinalGrade.Value,
                                    });
            var updatedGrades = grades.GradeList.Where(g => g.IsUpdated && g.GradeId > 0)
                                    .Select(g => new GradeDto()
                                    {
                                        GradeId = g.GradeId,
                                        StudentId = students[g.StudentRegistrationNumber.Value].Single().StudentId,
                                        Exam = g.ExamGrade.Value,
                                        Activity = g.ActivityGrade.Value,
                                        Final = g.FinalGrade.Value,
                                    });

            dbContext.AddRange(newGrades);
            foreach (var entity in updatedGrades)
            {
                dbContext.Entry(entity).State = EntityState.Modified;
            }

            await dbContext.SaveChangesAsync();

            return unit;
        };
    }
}