using Domain;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Visits
{
    public class GetActiveVisitsUseCase
    {
        private readonly IVisitRepository<VisitEntity> _visitRepository;

        public GetActiveVisitsUseCase(IVisitRepository<VisitEntity> visitRepository)
        {
            _visitRepository = visitRepository;
        }

        public async Task<IEnumerable<VisitEntity>> ExecuteAsync()
        {
            return await _visitRepository.GetActiveVisitsAsync();
        }
    }
}
