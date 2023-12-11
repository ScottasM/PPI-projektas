using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using PPI_projektas.objects;
using PPI_projektas.objects.abstractions;

namespace PPI_projektas.Utils
{
    public class SaveHandler
    {

        private EntityData _context;
        public SaveHandler()
        {
            _context = new EntityData();
        }

        public void Save()
        {
            _context.SaveChangesAsync();
        }
        
        

        public ConcurrentDictionary<Guid, Note> LoadNotes()
        {
            var list = _context.Set<Note>().ToList();
            return new ConcurrentDictionary<Guid, Note>(list.Select(item => new KeyValuePair<Guid, Note>(((dynamic)item).Id, item)));
        }
        public ConcurrentDictionary<Guid, User> LoadUsers()
        {
            var list = _context.Users.Include(u => u.Groups).ToList();
            return new ConcurrentDictionary<Guid, User>(list.Select(item => new KeyValuePair<Guid, User>(item.Id, item)));
        }

        public ConcurrentDictionary<Guid, Group> LoadGroups()
        {
            var list = _context.Groups.Include(g => g.Members).ToList();
            return new ConcurrentDictionary<Guid, Group>(list.Select(item => new KeyValuePair<Guid, Group>(item.Id, item)));
        }

        public void SaveObject<T>(T obj) where T : Entity
        {
            _context.Set<T>().Add(obj);
            _context.SaveChangesAsync();
        }

        public void RemoveObject<T>(T obj) where T : Entity
        {
            _context.Set<T>().Remove(obj);
            _context.SaveChangesAsync();
        }
    }
}
