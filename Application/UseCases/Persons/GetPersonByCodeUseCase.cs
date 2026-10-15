using Domain;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Persons
{
    public class GetPersonByCodeUseCase
    {
         private readonly ICodeRepository<PersonEntity> _codeRepository;

        public GetPersonByCodeUseCase(ICodeRepository<PersonEntity> codeRepository)
        {
            _codeRepository = codeRepository;
        }

        public async Task<PersonEntity?> ExecuteAsync(string code)
        {
            var person = await _codeRepository.GetByCodeAsync(code);
            if (person == null)
            {
                throw new InvalidOperationException($"Person with code {code} not found.");
            }

            return person;
        }
    }
}
