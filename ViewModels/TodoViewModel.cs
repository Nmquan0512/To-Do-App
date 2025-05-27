using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ToDoApp.Models;
using ToDoApp.Data;
using ToDoApp.Commands;

namespace ToDoApp.ViewModels;

public class TodoViewModel : INotifyPropertyChanged
{
    private readonly AppDbContext _context;

    public ObservableCollection<TodoItem> Tasks { get; set; }
    public ICommand AddCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand UpdateCommand { get; }

    private TodoItem _selectedTask;
    public TodoItem SelectedTask
    {
        get => _selectedTask;
        set
        {
            _selectedTask = value;
            OnPropertyChanged(nameof(SelectedTask));
        }
    }

    public TodoViewModel()
    {
        _context = new AppDbContext();
        _context.Database.EnsureCreated();

        Tasks = new ObservableCollection<TodoItem>(_context.TodoItems.ToList());
        SelectedTask = new TodoItem { Deadline = DateTime.Now };

        AddCommand = new RelayCommand(_ =>
        {
            if (string.IsNullOrWhiteSpace(SelectedTask.Title))
            {
                MessageBox.Show("Vui lòng nhập tiêu đề công việc.", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _context.TodoItems.Add(SelectedTask);
            _context.SaveChanges();
            Tasks.Add(SelectedTask);
            SelectedTask = new TodoItem { Deadline = DateTime.Now };
        });

        DeleteCommand = new RelayCommand(_ =>
        {
            if (SelectedTask != null && Tasks.Contains(SelectedTask))
            {
                _context.TodoItems.Remove(SelectedTask);
                _context.SaveChanges();
                Tasks.Remove(SelectedTask);
                SelectedTask = new TodoItem { Deadline = DateTime.Now };
            }
        });

        UpdateCommand = new RelayCommand(_ =>
        {
            if (SelectedTask != null)
            {
                if (string.IsNullOrWhiteSpace(SelectedTask.Title))
                {
                    MessageBox.Show("Tiêu đề không được để trống.", "Lỗi cập nhật", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _context.TodoItems.Update(SelectedTask);
                _context.SaveChanges();
                MessageBox.Show("Cập nhật thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        });
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
