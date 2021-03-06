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
            return context.AssignedMedicaments.Include("Medicament").Include("AssignedMedicamentMeasurings").Include("MedicamentApplicationWay").Include("PersonConsultation");
        }

        public AssignedMedicament GetEntityById(Guid id)
        {
            using (var context = new DomainContext(this.ConnectionString))
            {
                return context.AssignedMedicaments.Include("Medicament").Include("AssignedMedicamentMeasurings").Include("MedicamentApplicationWay").Include("PersonConsultation")
                    .FirstOrDefault(v => v.Id == id);
            }
        }

        public IEnumerable<AssignedMedicament> GetEntitiesByQuery(Func<AssignedMedicament, bool> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            using (var context = new DomainContext(this.ConnectionString))
            {
                return context.AssignedMedicaments.Include("Medicament").Include("AssignedMedicamentMeasurings").Include("MedicamentApplicationWay").Include("PersonConsultation")
                    .Where(query).ToList();
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
                if (this.GetEntityById(entity.Id) == null)
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
            using (var context = new DomainContext(this.ConnectionString))
            {
                var assignedMedicament = context.AssignedMedicaments.FirstOrDefault(v => v.Id == id);
                if (assignedMedicament == null)
                {
                    return;
                }

                context.AssignedMedicaments.Remove(assignedMedicament);
                context.SaveChanges();
            }
        }
    }
}
