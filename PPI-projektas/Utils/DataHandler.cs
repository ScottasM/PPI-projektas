
﻿using PPI_projektas.objects;
using Microsoft.EntityFrameworkCore;
 using System.Collections.Concurrent;
 using PPI_projektas.Exceptions;

using PPI_projektas.objects.abstractions;

namespace PPI_projektas.Utils
{
    public class DataHandler
    {

        public static DataHandler Instance;

        public ConcurrentDictionary<Guid, User> AllUsers = new();
        public ConcurrentDictionary<Guid, Group> AllGroups = new();
        public ConcurrentDictionary<Guid, Note> AllNotes = new();

        private SaveHandler _saveHandler;

        enum FileState
        {
            Ready,
            Saving,
            Reading
        }

        private FileState _state = FileState.Ready;

        private DbContextOptions<EntityData> options;

        public DataHandler(string connectionString)
        {
            #region Singleton
            if (Instance != null)
                Instance = null;
            Instance = this;
            #endregion

            _saveHandler = LazySingleton<SaveHandler>.Instance;



            _state = FileState.Reading;

            AllUsers = _saveHandler.LoadList<User>();
            AllNotes = _saveHandler.LoadList<Note>();
            AllGroups = _saveHandler.LoadList<Group>();


            // assign loaded guids to actual objects
          
            _state = FileState.Ready;

            SaveTimeout(15);
        }


        private async void SaveTimeout(int TimeoutSeconds)
        {
            while (true) {
                await Task.Delay(TimeoutSeconds * 1000);

                if(_state != FileState.Ready) { // dont save if we're reading from the files
                    continue;
                }

                _state = FileState.Saving;

                _saveHandler.Save();

                _state = FileState.Ready;
            }
        }

        public static void Create<T>(T obj)
        {
            if (obj == null)
                return;

            if (obj is Group) {
                var obje = obj as Group;
                Instance.AllGroups.TryAdd(obje.Id, obje);
                Instance._saveHandler.SaveObject(obje);
            }
            else if (obj is User) {
                var obje = obj as User;
                Instance.AllUsers.TryAdd(obje.Id, obje);
                Instance._saveHandler.SaveObject(obje);
            }
            else if (obj is Note) {
                var obje = obj as Note;
                Instance.AllNotes.TryAdd(obje.Id, obje);
                Instance._saveHandler.SaveObject(obje);
            }
        }

        public static void Delete<T>(T obj)
        {
            if(obj == null) return;

            if (obj is Group) {
                var obje = obj as Group;
                Instance.AllGroups.TryRemove(obje.Id, out _);
                Instance._saveHandler.RemoveObject(obje);
            }
            else if (obj is User) {
                var obje = obj as User;
                Instance.AllUsers.TryRemove(obje.Id, out _);
                Instance._saveHandler.RemoveObject(obje);
            }
            else if (obj is Note) {
                var obje = obj as Note;
                Instance.AllNotes.TryRemove(obje.Id, out _);
                Instance._saveHandler.RemoveObject(obje);
            }
        }

        public static bool userExists(string username)
        {
            return Instance.AllUsers.Values.Any(inst => inst.GetUsername() == username);
        }

        public static User? userExistsObject(string username)
        {
            return Instance.AllUsers.Values.FirstOrDefault(inst => inst.GetUsername() == username);
        }
        
        public static T FindObjectById<T>(Guid objectId, ConcurrentDictionary<Guid, T> objectList) where T : Entity
        {
            if (objectList.TryGetValue(objectId, out var obj)) throw new ObjectDoesNotExistException(objectId);

            return obj;
        }
    }
}
