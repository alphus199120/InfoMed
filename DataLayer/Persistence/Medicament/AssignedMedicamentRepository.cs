﻿namespace DataLayer.Persistence.Medicament
{
    using System;
    using System.Collections.Generic;    
    using System.Data;
    using System.Linq;

    using Domain.Medicament;

    public class AssignedMedicamentRepository : IAssignedMedicamentRepository
    {
        private readonly string ConnectionString;

        public AssignedMedicamentRepository(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public IEnumerable<AssignedMedicament> GetAll()
        {
            var context = new DomainContext(this.ConnectionString);
            return context.AssignedMedicaments.Include("Person").Include("Medicament");
        }

        public AssignedMedicament GetEntityById(Guid id)
        {
            throw new NotImplementedException("this method is not implemented");
        }

        public IEnumerable<AssignedMedicament> GetEntitiesByQuery(Func<AssignedMedicament, bool> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            using (var context = new DomainContext(this.ConnectionString))
            {
                return context.AssignedMedicaments.Include("Person").Include("Medicament").Where(query);
            }                                    
        }

        public AssignedMedicament CreateOrUpdateEntity(AssignedMedicament entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            using (var context = new DomainContext(this.ConnectionString))
            {                                
                if (this.GetEntitiesByQuery(v => v.PersonId == entity.PersonId && v.MedicamentId == entity.MedicamentId) == null)
                {
                    context.AssignedMedicaments.Add(entity);
                }
                else
                {
                    context.Entry(entity).State = EntityState.Modified;
                }

                context.SaveChanges();
            }

            return entity;
        }

        public void DeleteEntity(Guid id)
        {
            throw new NotImplementedException("this method is not implemented");
        }
    }
}