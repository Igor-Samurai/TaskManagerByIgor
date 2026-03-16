using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using TestTaskManager.Core;
using TestTaskManager.Model;
using TestTaskManager.Model.Enums;
using TestTaskManager.Model.Repositories.Abstract;
using TestTaskManager.Model.Repositories.DataServices;
using TestTaskManager.View;
using TestTaskManager.ViewModel.Enums;

namespace TestTaskManager.ViewModel
{
    public class UserTaskViewModel : INotifyPropertyChanged
    {
        private IRepositoryService<UserTask> serviceTaskRebositoryJSON = new ServiceTaskRebositoryJSON();

        public string SearchText { get; set; }

        private ObservableCollection<UserTask> _userTasks;
        public ObservableCollection<UserTask> UserTasks
        {
            get => _userTasks;
            set { _userTasks = value; OnPropertyChanged(); RefreshFilteredView(); }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    serviceTaskRebositoryJSON.Create(UserTasks[e.NewStartingIndex]);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    serviceTaskRebositoryJSON.Delete((UserTask)e.OldItems[0]);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    serviceTaskRebositoryJSON.Update(UserTasks[e.NewStartingIndex]);
                    break;
            }
        }

        private UserTask _selectedUserTask;
        public UserTask SelectedUserTask
        {
            get { return _selectedUserTask; }
            set
            {
                _selectedUserTask = value;
                OnPropertyChanged("SelectedUserTask");
            }
        }

        private SortOption _sortOption;
        public SortOption SortOption
        {
            get { return _sortOption; }
            set
            {
                _sortOption = value;
                RefreshFilteredView();
                OnPropertyChanged("SortOption");
            }
        }


        private RelayCommand filterStatusCommand;
        public RelayCommand FilterStatusCommand
        {
            get
            {
                return filterStatusCommand ??
                  (filterStatusCommand = new RelayCommand(obj =>
                  {
                      if (obj is string filterStatus)
                      {
                          switch (filterStatus)
                          {
                              case "Active":
                                  FilterStatus = Status.Active;
                                  break;
                              case "Completed":
                                  FilterStatus = Status.Completed;
                                  break;
                              default:
                                  FilterStatus = null;
                                  break;
                          }
                      }
                  }));
            }
        }

        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??= new RelayCommand(obj =>
                {
                    var addWindow = new TaskDialog();
                    addWindow.Title = "Добавить задачу";
                    var addViewModel = new TaskViewModel(addWindow, Operation.AddItems);

                    addWindow.DataContext = addViewModel;

                    if (addWindow.ShowDialog() == true)
                    {
                        UserTasks.Add(addViewModel.NewTask);
                        RefreshFilteredView();
                    }
                });
            }
        }

        private RelayCommand editCommand;
        public RelayCommand EditCommand
        {
            get
            {
                return editCommand ??
                  (editCommand = new RelayCommand(obj =>
                  {
                      if (obj is UserTask userTask)
                      {
                          var editWindow = new TaskDialog();
                          editWindow.Title = "Редактировать задачу";
                          var editViewModel = new TaskViewModel(editWindow, Operation.EditItems);
                          editWindow.DataContext = editViewModel;
                          editViewModel.NewTask = userTask;
                          editWindow.ShowDialog();
                          RefreshFilteredView();
                      }
                      else
                      {
                          MessageBox.Show("Выберите элемент для редактирования");
                      }
                  }));
            }
        }

        private RelayCommand removeCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                return removeCommand ??
                  (removeCommand = new RelayCommand(obj =>
                  {
                      if (obj is UserTask userTask)
                      {
                          var result = MessageBox.Show(
                       $"Удалить задачу '{userTask.Title}'?",
                       "Подтверждение удаления",
                       MessageBoxButton.YesNo,
                       MessageBoxImage.Question
                   );

                          if (result == MessageBoxResult.Yes)
                          {
                              UserTasks.Remove(userTask);
                              RefreshFilteredView();
                          }
                      }
                      else
                      {
                          MessageBox.Show("Выберите элемент для удаления");
                      }
                  },
                 (obj) => UserTasks.Count > 0));
            }
        }

        #region Фильтрация

        public ListCollectionView FilteredTasksView => CreateFilteredView();

        private Status? _filterStatus = null;
        public Status? FilterStatus
        {
            get => _filterStatus;
            set { _filterStatus = value; OnPropertyChanged(); FilteredTasksView.Refresh(); }
        }

        private string? _filterTitleOrDescription = null;
        public string? FilterTitleOrDescription
        {
            get => _filterTitleOrDescription;
            set { _filterTitleOrDescription = value; OnPropertyChanged(); FilteredTasksView.Refresh(); }
        }

        private ListCollectionView CreateFilteredView()
        {
            var view = CollectionViewSource.GetDefaultView(UserTasks) as ListCollectionView;
            view.Filter = FilterPredicate;
            view.SortDescriptions.Clear();
            if (SortOption == SortOption.DueDate)
            {
                view.SortDescriptions.Add(new SortDescription(nameof(UserTask.DueDate), ListSortDirection.Ascending));
            }
            else if (SortOption == SortOption.Priority)
            {
                view.SortDescriptions.Add(new SortDescription(nameof(UserTask.Priority), ListSortDirection.Descending));
            }
            else
            {
                view.SortDescriptions.Add(new SortDescription(nameof(UserTask.CreatedAt), ListSortDirection.Descending));
            }
            return view;
        }

        private bool FilterPredicate(object item)
        {
            if (item is not UserTask userTask) return false;

            bool status = false;
            if (FilterStatus is null)
            {
                status = true;
            }
            else if (userTask.Status == FilterStatus)
            {
                status = true;
            }

            bool titleOrDescription = false;

            if (string.IsNullOrEmpty(_filterTitleOrDescription))
            {
                titleOrDescription = true;
            }
            else if ((userTask.Title.Contains(_filterTitleOrDescription, StringComparison.OrdinalIgnoreCase)) ||
                            (userTask.Description?.Contains(_filterTitleOrDescription, StringComparison.OrdinalIgnoreCase) ?? false))
            {
                titleOrDescription = true;
            }

            return status && titleOrDescription;
        }

        #endregion


        private void RefreshFilteredView()
        {
            if (FilteredTasksView != null)
                FilteredTasksView.Refresh();
        }


        public UserTaskViewModel()
        {
            UserTasks = new (serviceTaskRebositoryJSON.GetAll());
            UserTasks.CollectionChanged += OnCollectionChanged;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
