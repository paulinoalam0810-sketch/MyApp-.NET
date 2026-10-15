using Domain;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Visits
{
    public class GetVisitsByPersonUseCase
    {
        private readonly IVisitRepository<VisitEntity> _visitRepository;

        public GetVisitsByPersonUseCase(IVisitRepository<VisitEntity> visitRepository)
        {
            _visitRepository = visitRepository;
        }

        public async Task<IEnumerable<VisitEntity>> ExecuteAsync(Guid personId)
        {
            return await _visitRepository.GetVisitsByPersonIdAsync(personId);
        }
    }
}
