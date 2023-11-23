
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

            Enqueue(delegate { AllUsers = _saveHandler.LoadList<User>();});
            Enqueue(delegate { AllNotes = _saveHandler.LoadList<Note>();});
            Enqueue(delegate { AllGroups = _saveHandler.LoadList<Group>();});

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
                Instance.AllGroups.Add(obje);
                Enqueue(() => Instance._saveHandler.SaveObject(obje));
            }
            else if (obj is User) {
                var obje = obj as User;
                Instance.AllUsers.Add(obje);
                Enqueue(() => Instance._saveHandler.SaveObject(obje));
            }
            else if (obj is Note) {
                var obje = obj as Note;
                Instance.AllNotes.Add(obje);
                Enqueue(() => Instance._saveHandler.SaveObject(obje));
            }
        }

        public static void Delete<T>(T obj)
        {
            if(obj == null) return;

            if (obj is Group) {
                var obje = obj as Group;
                Instance.AllGroups.Remove(obje);
                Enqueue(() => Instance._saveHandler.RemoveObject(obje));
            }
            else if (obj is User) {
                var obje = obj as User;
                Instance.AllUsers.Remove(obje);
                Enqueue(() => Instance._saveHandler.RemoveObject(obje));
            }
            else if (obj is Note) {
                var obje = obj as Note;
                Instance.AllNotes.Remove(obje);
                Enqueue(() => Instance._saveHandler.RemoveObject(obje));
            }

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
