
﻿using PPI_projektas.objects;
using Microsoft.EntityFrameworkCore;
using System;

﻿using PPI_projektas.Exceptions;

using PPI_projektas.objects.abstractions;

namespace PPI_projektas.Utils
{
    public class DataHandler
    {

        public static DataHandler Instance;

        public List<User> AllUsers = new();
        public List<Group> AllGroups = new();
        public List<Note> AllNotes = new();

        private SaveHandler _saveHandler;

        

        private DbContextOptions<EntityData> options;
        enum FileState
        {
            Ready,
            Saving,
            Reading
        }

        private FileState _state = FileState.Ready;
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
            _state = FileState.Saving;

            // assign loaded guids to actual objects
          

            SaveTimeout(15);
        }


        private async void SaveTimeout(int TimeoutSeconds)
        {
            while (true) {
                await Task.Delay(TimeoutSeconds * 1000);
                _state = FileState.Saving;
                _saveHandler.Save(() => ContextActionDone());
            }
        }

        public static void Create<T>(T obj) 
        {
            if (obj == null)
                return;
            Instance._state = FileState.Saving;
            if (obj is Group) {
                var obje = obj as Group;
                Instance.AllGroups.Add(obje);
                Instance._saveHandler.SaveObject(obje,() => Instance.ContextActionDone());
            }
            else if (obj is User) {
                var obje = obj as User;
                Instance.AllUsers.Add(obje);
                Instance._saveHandler.SaveObject(obje, () => Instance.ContextActionDone());
            }
            else if (obj is Note) {
                var obje = obj as Note;
                Instance.AllNotes.Add(obje);
                Instance._saveHandler.SaveObject(obje, () => Instance.ContextActionDone());
            }
            else Instance._state = FileState.Ready;
        }

        public static void Delete<T>(T obj)
        {
            if(obj == null) return;

            Instance._state = FileState.Saving;
            if (obj is Group) {
                var obje = obj as Group;
                Instance.AllGroups.Remove(obje);
                Instance._saveHandler.RemoveObject(obje, () => Instance.ContextActionDone());
            }
            else if (obj is User) {
                var obje = obj as User;
                Instance.AllUsers.Remove(obje);
                Instance._saveHandler.RemoveObject(obje, () => Instance.ContextActionDone());
            }
            else if (obj is Note) {
                var obje = obj as Note;
                Instance.AllNotes.Remove(obje);
                Instance._saveHandler.RemoveObject(obje, () => Instance.ContextActionDone());
            }
            else Instance._state = FileState.Ready;
        }

        public static bool userExists(string username)
        {
            return Instance.AllUsers.Any(inst => inst.GetUsername() == username);
        }

        public static User? userExistsObject(string username)
        {
            return Instance.AllUsers.Find(inst => inst.GetUsername() == username);
        }
        
        public static T FindObjectById<T>(Guid objectId, List<T> objectList) where T : Entity
        {
            var obj = objectList.Find(obj => obj.Id == objectId);
            if (obj == null) throw new ObjectDoesNotExistException(objectId);

            return obj;
        }

        public void ContextActionDone()
        {
            _state = FileState.Ready;
        }
    }
}
