using System.Configuration;
using System.IO;
using System.Reflection;
using System.Windows;
using TestTaskManager.Model.Repositories.Abstract;
using TestTaskManager.Model.Repositories.JSON;

namespace TestTaskManager.Model.Repositories.DataServices
{
    public class ServiceTaskRebositoryJSON : IRepositoryService<UserTask>
    {
        private JSONService _jsonService;

        private const string _nameFile = "UserTasks.JSON";

        private Dictionary<Guid, UserTask> _tasks = new Dictionary<Guid, UserTask>();
        public ServiceTaskRebositoryJSON()
        {
            try
            {
                var app = (App)Application.Current;
                string path = $"{app.ProjectSettings.Database.ConnectionStringForJSON}\\{_nameFile}";
                if (!File.Exists(path))
                {
                    _jsonService = new JSONService($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\{_nameFile}");
                    _tasks = new Dictionary<Guid, UserTask>();
                }
                else
                {
                    _jsonService = new JSONService(path);
                    _tasks = _jsonService.Deserialize<UserTask>().ToDictionary((userTask) => userTask.Id);
                }
                
            }
            catch (Exception)
            {
                _tasks = new Dictionary<Guid, UserTask>();
            }
            
        }
        public IEnumerable<UserTask> GetAll()
        {
            return _tasks.Values;
        }

        public void Create(UserTask item)
        {
            if (_tasks.ContainsKey(item.Id) == false)
            {
                _tasks.Add(item.Id, item);
            }
            _jsonService.Serialize(_tasks.Values);
        }

        public void Delete(UserTask item)
        {
            if (_tasks.ContainsKey(item.Id))
            {
                _tasks.Remove(item.Id);
            }
            _jsonService.Serialize(_tasks.Values);
        }

        public void Update(UserTask item)
        {
            if (_tasks.ContainsKey(item.Id) == true)
            {
                _tasks[item.Id] = item;
            }
            _jsonService.Serialize(_tasks.Values);
        }
    }
}
