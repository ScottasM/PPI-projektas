using System.Collections.Concurrent;
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
            _context.SaveChanges();
        }

        public ConcurrentDictionary<Guid, T> LoadList<T>() where T: class
        {
            var list = _context.Set<T>().ToList();
            return new ConcurrentDictionary<Guid, T>(list.Select(item => new KeyValuePair<Guid, T>(((dynamic)item).Id, item)));
        }
        
        public void SaveObject<T>(T obj) where T : Entity
        {
            _context.Set<T>().Add(obj);
            _context.SaveChanges();
        }

        public void RemoveObject<T>(T obj) where T : Entity
        {
            _context.Set<T>().Remove(obj);
            _context.SaveChanges();
        }
    }
}
