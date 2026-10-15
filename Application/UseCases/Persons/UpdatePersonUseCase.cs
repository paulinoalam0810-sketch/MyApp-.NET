using Domain;
using Domain.Abstractions;
using Application.DTOs.Persons;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Persons
{
    public class UpdatePersonUseCase
    {
        private readonly IRepository<PersonEntity, Guid> _repository;
        public UpdatePersonUseCase(IRepository<PersonEntity, Guid> repository)
        {
            _repository = repository;
        }
        public async Task<PersonEntity> ExecuteAsync(Guid id, UpdatePersonDto dto)
        {
            var person = await _repository.GetByIdAsync(dto.Id);
            if (person == null)
            {
                throw new InvalidOperationException($"Person with ID {dto.Id} not found.");
            }

            person.UpdatePersonalInfo(dto.FirstName, dto.LastName, dto.Email, dto.PhoneNumber);

            await _repository.UpdateAsync(person);
            await _repository.SaveChangesAsync();

            return person;
        }
    }
}
