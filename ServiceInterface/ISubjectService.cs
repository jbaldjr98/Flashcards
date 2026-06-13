using Flashcards.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
    public interface ISubjectService : IGenericService<Subject> 
    {
        Task<Subject> CreateNewSubject(Subject newSubject);
    }
}
