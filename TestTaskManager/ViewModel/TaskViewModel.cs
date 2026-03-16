using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TestTaskManager.Core;
using TestTaskManager.Model;
using TestTaskManager.Model.Enums;
using TestTaskManager.Model.Repositories.Abstract;
using TestTaskManager.Model.Repositories.DataServices;
using TestTaskManager.ViewModel.Enums;

namespace TestTaskManager.ViewModel
{
    public class TaskViewModel : INotifyPropertyChanged
    {
        private readonly Window _window;
        private readonly IRepositoryService<UserTask> _repository;
        private UserTask _newTask;
        private UserTask _oldTask;

        private string _taskTitle = "Новая задача";

        public string TaskTitle
        {
            get => _taskTitle;
            set
            {
                _taskTitle = value;
            }
        }

        public UserTask NewTask
        {
            get => _newTask;
            set
            {
                _newTask = value;
                _oldTask = new UserTask() { Title = NewTask.Title, Description = NewTask.Description, Priority = NewTask.Priority, Status = NewTask.Status, DueDate = NewTask.DueDate, CreatedAt = NewTask.CreatedAt, Id = NewTask.Id };
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get => _newTask.Title;
            set
            {
                if (_newTask.Title != value)
                {
                    _newTask.Title = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Description
        { 
            get => _newTask.Description;
            set 
            {
                if (_newTask.Description != value)
                {
                    _newTask.Description = value;
                    OnPropertyChanged();
                }
            }
        }

        public Status Status
        {
            get => _newTask.Status;
            set
            {
                if (_newTask.Status != value)
                {
                    _newTask.Status = value;
                    OnPropertyChanged();
                }
            }
        }

        public Priority Priority
        {
            get => _newTask.Priority;
            set
            {
                if (_newTask.Priority != value)
                {
                    _newTask.Priority = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime DueDate
        {
            get => _newTask.DueDate;
            set
            {
                if (_newTask.DueDate != value)
                {
                    _newTask.DueDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime CreatedAt
        {
            get => _newTask.CreatedAt;
            set
            {
                if (_newTask.CreatedAt != value)
                {
                    _newTask.CreatedAt = value;
                    OnPropertyChanged();
                }
            }
        }


        public ICommand OperationCommand { get; }
        public ICommand CancelCommand { get; }

        public TaskViewModel(Window window, Operation operation)
        {
            _window = window;
            _repository = new ServiceTaskRebositoryJSON();
            if (operation == Operation.AddItems)
            {
                NewTask = new UserTask
                {
                    Id = Guid.NewGuid(),
                    Status = Status.Active,
                    Priority = Priority.Medium,
                    CreatedAt = DateTime.Now,
                    DueDate = DateTime.Now
                };
                OperationCommand = new RelayCommand(CreateTask, CanCreateTask);
                TaskTitle = "Создание новой задачи";
            }
            else
            {
                OperationCommand = new RelayCommand(EditTask, CanEditTask);
                TaskTitle = "Редактирование задачи";
            }

            CancelCommand = new RelayCommand(Cancel);
        }

        private bool CanCreateTask(object parameter)
        {
            return NewTask != null && !string.IsNullOrWhiteSpace(NewTask.Title);
        }

        private void CreateTask(object parameter)
        {
            if (NewTask != null && !string.IsNullOrWhiteSpace(NewTask.Title))
            {
                _repository.Create(NewTask);
                _window.DialogResult = true;
                _window.Close();
            }
        }

        private bool CanEditTask(object parameter)
        {
            return NewTask != null && !string.IsNullOrWhiteSpace(NewTask.Title); //И тут ещё дописать проверку
        }

        private void EditTask(object parameter)
        {
            if (NewTask != null && !string.IsNullOrWhiteSpace(NewTask.Title))
            {
                _repository.Update(NewTask);
                _window.DialogResult = true;
                _window.Close();
            }
        }

        private void Cancel(object parameter)
        {
            var result = MessageBox.Show(
                       $"Все изменения будут отменены, Вы уверены?",
                       "Подтверждение отмены изменений",
                       MessageBoxButton.YesNo,
                       MessageBoxImage.Question
                   );

            if (result == MessageBoxResult.Yes)
            {
                NewTask.Title = _oldTask.Title;
                NewTask.Description = _oldTask.Description;
                NewTask.Status = _oldTask.Status;
                NewTask.Priority = _oldTask.Priority;
                NewTask.DueDate = _oldTask.DueDate;
                NewTask.CreatedAt = _oldTask.CreatedAt;
                NewTask.Id = _oldTask.Id;
                _window.DialogResult = false;
                _window.Close();
            }
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}