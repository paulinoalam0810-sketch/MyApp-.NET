using Application.DTOs.Visits;
using Domain;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Visits
{
    public class RegisterExitUseCase
    {
        private readonly IRepository<VisitEntity, Guid> _repository;
        private readonly IVisitRepository<VisitEntity> _visitRepository;

        public RegisterExitUseCase(IRepository<VisitEntity, Guid> repository,
         IVisitRepository<VisitEntity> visitRepository)
        {
            _repository = repository;
            _visitRepository = visitRepository;
        }

        public async Task<VisitEntity> ExecuteAsync(RegisterExitDto dto)
        {
            VisitEntity? visit;

            if (dto.VisitId.HasValue)
            {
                visit = await _repository.GetByIdAsync(dto.VisitId.Value);

                if (visit == null)
                {
                    throw new InvalidOperationException("Visit not found.");
                }
            }
            else if (!string.IsNullOrWhiteSpace(dto.Code))
            {
                visit = await _visitRepository.GetActiveVisitByPersonCodeAsync(dto.Code);

                if (visit == null)
                {
                    throw new InvalidOperationException("Visit not found.");
                }
            }
            else
            {
                throw new InvalidOperationException("Either VisitId or Code must be provided for registering an exit.");
            }

            visit.RegisterExit(dto.ExitTime);

            await _repository.UpdateAsync(visit);
            await _repository.SaveChangesAsync();

            return visit;
        }
    }
}
