
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

        

        private DbContextOptions<EntityData> options;

        public DataHandler(string connectionString)
        {
            #region Singleton
            if (Instance != null)
                Instance = null;
            Instance = this;
            #endregion

            _saveHandler = LazySingleton<SaveHandler>.Instance;

            Thread processingThread = new Thread(ProcessQueue);
            processingThread.Start();

            Enqueue(delegate { AllUsers = _saveHandler.LoadUsers();});
            Enqueue(delegate { AllNotes = _saveHandler.LoadNotes();});
            Enqueue(delegate { AllGroups = _saveHandler.LoadGroups();});

            SaveTimeout(15);
        }


        private async void SaveTimeout(int TimeoutSeconds)
        {
            while (true) {
                await Task.Delay(TimeoutSeconds * 1000);

                Enqueue(()=>_saveHandler.Save());
            }
        }

        public static void Create<T>(T obj)
        {
            if (obj == null)
                return;

            if (obj is Group) {
                var obje = obj as Group;
                Instance.AllGroups.TryAdd(obje.Id, obje);
                Enqueue(() => Instance._saveHandler.SaveObject(obje));
            }
            else if (obj is Note) {
                var obje = obj as Note;
                Instance.AllNotes.TryAdd(obje.Id, obje);
                Enqueue(() => Instance._saveHandler.SaveObject(obje));
            }
            else if (obj is User) {
                var obje = obj as User;
                Instance.AllUsers.TryAdd(obje.Id,obje);
                Enqueue(() => Instance._saveHandler.SaveObject(obje));
            }
        }

        public static void Delete<T>(T obj)
        {
            if(obj == null) return;

            if (obj is Group) {
                var obje = obj as Group;
                Instance.AllGroups.TryRemove(obje.Id, out _);
                Enqueue(() => Instance._saveHandler.RemoveObject(obje));
            }
            else if (obj is User) {
                var obje = obj as User;
                Instance.AllUsers.TryRemove(obje.Id, out _);
                Enqueue(() => Instance._saveHandler.RemoveObject(obje));
            }
            else if (obj is Note) {
                var obje = obj as Note;
                Instance.AllNotes.TryRemove(obje.Id, out _);
                Enqueue(() => Instance._saveHandler.RemoveObject(obje));
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
            if (objectList.TryGetValue(objectId, out var obj)) 
                return obj;
            
            throw new ObjectDoesNotExistException(objectId);
        }




        static Queue<Action> actionQueue = new Queue<Action>();
        static object queueLock = new object();

        static void ProcessQueue()
        {
            while (true) {
                Action action = Dequeue();
                if (action != null) {
                    action.Invoke();
                }
                else {
                    lock (queueLock) {
                        Monitor.Wait(queueLock);
                    }
                }
            }
        }

        static Action Dequeue()
        {
            lock (queueLock) {
                if (actionQueue.Count > 0) {
                    return actionQueue.Dequeue();
                }
                return null;
            }
        }

        static void Enqueue(Action action)
        {
            lock (queueLock) {
                actionQueue.Enqueue(action);
                Monitor.Pulse(queueLock);
            }
        }


    }
}
