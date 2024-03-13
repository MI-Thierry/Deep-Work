using CommunityToolkit.Mvvm.ComponentModel;
using DeepWork.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace DeepWork.ViewModels.Pages;

public partial class TaskManagementViewModel : ObservableObject
{
	private string _selectedLongTaskName;
	private string _selectedShortTaskName;
	private readonly AccountManagementService _accountManager;

	public LongTaskViewModel SelectedLongTask { get; set; }
	public ShortTaskViewModel SelectedShortTask { get; set; }


	[ObservableProperty]
	private ObservableCollection<LongTaskViewModel> _longTasks;

	[ObservableProperty]
	private ObservableCollection<ShortTaskViewModel> _shortTasks;

	public TaskManagementViewModel()
	{
		_accountManager = App.GetService<AccountManagementService>();
		_longTasks = [];
		_shortTasks = [];

		foreach (var task in _accountManager.ActiveAccount.LongTasks)
			_longTasks.Add(task);

		if (_longTasks.Any())
			SelectLongTask(_longTasks.First());
	}

	public void SelectLongTask(LongTaskViewModel taskVm)
	{
		SelectedLongTask = _accountManager.GetLongTaskByName(taskVm.Name);
		_selectedLongTaskName = SelectedLongTask.Name;

		ShortTasks.Clear();
		foreach (var task in _accountManager.GetLongTaskByName(_selectedLongTaskName).RunningTasks)
			ShortTasks.Add(task);
	}

	public void SelectShortTask(ShortTaskViewModel taskVm)
	{
		SelectedShortTask = _accountManager.GetShortTaskByName(_selectedLongTaskName, taskVm.Name);
		_selectedShortTaskName = SelectedShortTask.Name;
	}

	public void AddLongTask(LongTaskViewModel task)
	{
		LongTasks.Add(task);
		_accountManager.AddLongTask(task);
	}

	public void EditSelectedLongTask(LongTaskViewModel editedTask)
	{
		_accountManager.EditLongTask(_selectedLongTaskName, editedTask);
		LongTaskViewModel taskToEdit = LongTasks.First(task => task.Name == _selectedLongTaskName);
		taskToEdit.Name = editedTask.Name;
		taskToEdit.StartDate = editedTask.StartDate;
		taskToEdit.EndDate = editedTask.EndDate;
		SelectLongTask(taskToEdit);
	}

	public void FinishLongTask(string name)
	{
		LongTasks.Remove(LongTasks.FirstOrDefault(item => item.Name == name));
		_accountManager.FinishLongTask(name);
	}

	public void AddShortTask(ShortTaskViewModel task)
	{
		ShortTasks.Add(task);
		LongTasks.First(t => t.Name == _selectedLongTaskName).TaskCount++;
		_accountManager.AddShortTask(_selectedLongTaskName, task);
	}

	public void EditShortTask(ShortTaskViewModel editedTask)
	{
		_accountManager.EditShortTask(_selectedLongTaskName, _selectedShortTaskName, editedTask);
		ShortTaskViewModel taskToEdit = ShortTasks.First(task => task.Name == _selectedShortTaskName);
		taskToEdit.Name = editedTask.Name;
		taskToEdit.Duration = editedTask.Duration;
		SelectShortTask(taskToEdit);
	}

	public void FinishShortTask(string name)
	{
		ShortTasks.Remove(ShortTasks.FirstOrDefault(item => item.Name == name));
		LongTasks.First(t => t.Name == _selectedLongTaskName).TaskCount--;
		_accountManager.FinishShortTask(_selectedLongTaskName, name);
	}
}
