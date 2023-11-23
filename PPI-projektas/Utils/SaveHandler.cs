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

        public void Save(Action onCompleted)
        {
            _context.SaveChanges();
            onCompleted?.Invoke();
        }

        public List<T> LoadList<T>() where T: class
        {
            return _context.Set<T>().ToList();
        }
        
        public void SaveObject<T>(T obj, Action onCompleted) where T : Entity
        {
            _context.Set<T>().Add(obj);
            _context.SaveChanges();
            onCompleted?.Invoke();
        }

        public void RemoveObject<T>(T obj, Action onCompleted) where T : Entity
        {
            _context.Set<T>().Remove(obj);
            _context.SaveChanges();
            onCompleted?.Invoke();
        }
    }
}
