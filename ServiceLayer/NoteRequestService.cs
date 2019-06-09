using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DomainModel.Entities;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;
using Database.Persistance.Tenants;
using Framework.BusinessRule;

namespace ServiceLayer
{
    public class NoteRequestService : MasterFileService<NoteRequest>, INoteRequestService
    {
        private readonly INoteRequestRepository _noteRequestRepository;

        public NoteRequestService(ITenantPersistenceService tenantPersistenceService, INoteRequestRepository noteRequestRepository,
            IBusinessRuleSet<NoteRequest> businessRuleSet = null)
            : base(noteRequestRepository, noteRequestRepository, tenantPersistenceService, businessRuleSet)
        {
            _noteRequestRepository = noteRequestRepository;
        }

    }
}
