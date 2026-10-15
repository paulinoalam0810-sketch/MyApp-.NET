using Domain;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Visits
{
    public class GetAllVisitsUseCase
    {
        private readonly IRepository<VisitEntity, Guid> _repository;

        public GetAllVisitsUseCase(IRepository<VisitEntity, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<VisitEntity>> ExecuteAsync()
        {
            return await _repository.GetAllAsync();
        }
    }
}
