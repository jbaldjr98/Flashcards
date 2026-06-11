using DataInterface;
using DomainInterface;
using Flashcards.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class SubjectService: GenericService<Subject>, ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;

        public SubjectService( ISubjectRepository subjectRepository) :base (subjectRepository) { 
            _subjectRepository = subjectRepository;
        }

        public async Task CreateNewSubject(Subject newSubject)
        {
            var isUniqueName = _subjectRepository.GetSubjectByName(newSubject.Name);
            if (isUniqueName == null)
            {
                await _subjectRepository.AddAsync(newSubject);

            }
        }
    }
}
