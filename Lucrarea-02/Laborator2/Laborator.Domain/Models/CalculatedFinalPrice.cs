using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laborator2.Domain.Models
{
    public record CalculatedFinalPrice(Client Client, Grade ExamGrade, Grade ActivityGrade, Grade FinalGrade);
}