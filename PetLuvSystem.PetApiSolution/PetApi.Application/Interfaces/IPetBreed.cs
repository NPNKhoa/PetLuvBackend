﻿using PetApi.Domain.Entities;
using PetLuvSystem.SharedLibrary.Interfaces;
using PetLuvSystem.SharedLibrary.Responses;
using System.Linq.Expressions;

namespace PetApi.Application.Interfaces
{
    public interface IPetBreed : IGenericInterface<PetBreed>
    {
        public Task<PetBreed> FindById(Guid id, bool noTracking = false, bool include = false);
        public Task<Response> GetBreedMapping(bool showHidden = false);
        public Task<Response> GetByAsync(Expression<Func<PetBreed, bool>> predicate, bool isReturnList = false);
    }
}
