using Application.DTOs.Visits;
using Domain;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Application.UseCases.Visits
{
    public class RegisterEntryUseCase
    {
        private readonly IRepository<VisitEntity, Guid> _repository;
        private readonly IVisitRepository<VisitEntity> _visitRepository;
        private readonly ICodeRepository<PersonEntity> _personRepository;

        public RegisterEntryUseCase(
        IRepository<VisitEntity, Guid> repository,
        IVisitRepository<VisitEntity> visitRepository,
        ICodeRepository<PersonEntity> personRepository)
        {
            _repository = repository;
            _visitRepository = visitRepository;
            _personRepository = personRepository;
        }

        public async Task<VisitEntity> ExecuteAsync(RegisterEntryDto dto)
        {
            Guid personId;

            if (dto.PersonId.HasValue)
            {
                personId = dto.PersonId.Value;
            }
            else if (!string.IsNullOrWhiteSpace(dto.Code))
            {
                var person = await _personRepository.GetByCodeAsync(dto.Code);
                if (person == null)
                {
                    throw new InvalidOperationException($"Person not found with code: {dto.Code}");
                }
                personId = person.Id;
            }
            else
            {
                throw new InvalidOperationException("Either PersonId or Code must be provided for entry registration.");
            }

            if (await _visitRepository.HasActiveVisitAsync(personId))
            {
                throw new InvalidOperationException("A visit is already active for this person.");
            }

            var visit = new VisitEntity(personId, dto.EntryTime);

            await _repository.AddAsync(visit);
            await _repository.SaveChangesAsync();

            return await _repository.GetByIdAsync(visit.Id) ??
                throw new InvalidOperationException("Error retrieving the created visit.");
        }
    }
}
