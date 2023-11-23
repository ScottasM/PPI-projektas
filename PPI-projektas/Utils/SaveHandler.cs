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


        enum FileState
        {
            Ready,
            Saving,
            Reading
        }

        private FileState _state = FileState.Ready;

        public void Save()
        {
            if( _state == FileState.Saving ) {
                return;
            }
            _state = FileState.Saving;
            _context.SaveChanges();
            _state = FileState.Ready;
        }

        public List<T> LoadList<T>() where T: class
        {
            return _context.Set<T>().ToList();
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
