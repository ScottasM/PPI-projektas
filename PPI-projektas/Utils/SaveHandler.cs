﻿using System.Text.Json;
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
