 using System;
 using System.Threading.Tasks;
 using Microsoft.EntityFrameworkCore;

 using ISTS.Domain.Sessions;
 using ISTS.Infrastructure.Model;

 namespace ISTS.Infrastructure.Repository
 {
     public class SessionRepository : ISessionRepository
     {
         private readonly IstsContext _context;
         
         public SessionRepository(IstsContext context)
         {
             _context = context;
         }
         
         public Task<Session> GetAsync(Guid id)
         {
             var entity = _context.Sessions
                .SingleAsync(x => x.Id == id);

            return entity;
         }
     }
 }