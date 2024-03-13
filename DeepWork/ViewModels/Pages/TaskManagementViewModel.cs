using CommunityToolkit.Mvvm.ComponentModel;
using DeepWork.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace DeepWork.ViewModels.Pages;

// Todo: Remove unnecessary things
public partial class TaskManagementViewModel : ObservableObject
{
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
		SelectedLongTask = _accountManager.GetLongTaskById(taskVm.Id);

		ShortTasks.Clear();
		foreach (var task in _accountManager.GetLongTaskById(SelectedLongTask.Id).RunningTasks)
			ShortTasks.Add(task);
	}

	public void AddLongTask(LongTaskViewModel task)
	{
		LongTasks.Add(_accountManager.AddLongTask(task));
	}

	public void EditSelectedLongTask(LongTaskViewModel editedTask)
	{
		_accountManager.EditLongTask(editedTask.Id, editedTask);
		LongTaskViewModel taskToEdit = LongTasks.First(task => task.Id == SelectedLongTask.Id);
		taskToEdit.Name = editedTask.Name;
		taskToEdit.StartDate = editedTask.StartDate;
		taskToEdit.EndDate = editedTask.EndDate;
		SelectLongTask(taskToEdit);
	}

	public void FinishLongTask(int id)
	{
		LongTasks.Remove(LongTasks.FirstOrDefault(task => task.Id == id));
		_accountManager.FinishLongTask(id);
	}

	public void DeleteLongTask(int id)
	{
		LongTasks.Remove(LongTasks.FirstOrDefault(task => task.Id == id));
		_accountManager.DeleteLongTask(id);
	}

	public void SelectShortTask(ShortTaskViewModel taskVm)
	{
		SelectedShortTask = _accountManager.GetShortTaskById(SelectedLongTask.Id, taskVm.Id);
	}

	public void AddShortTask(ShortTaskViewModel taskVm)
	{
		LongTasks.First(task => task.Id == SelectedLongTask.Id).TaskCount++;
		ShortTasks.Add(_accountManager.AddShortTask(SelectedLongTask.Id, taskVm));
	}

	public void EditShortTask(ShortTaskViewModel editedTask)
	{
		_accountManager.EditShortTask(SelectedLongTask.Id, SelectedShortTask.Id, editedTask);
		ShortTaskViewModel taskToEdit = ShortTasks.First(task => task.Id == editedTask.Id);
		taskToEdit.Name = editedTask.Name;
		taskToEdit.Duration = editedTask.Duration;
		SelectShortTask(taskToEdit);
	}

	public void FinishShortTask(int taskId)
	{
		ShortTasks.Remove(ShortTasks.FirstOrDefault(task => task.Id == taskId));
		LongTasks.First(task => task.Id == SelectedLongTask.Id).TaskCount--;
		_accountManager.FinishShortTask(SelectedLongTask.Id, taskId);
	}

	public void DeleteShortTask(int taskId)
	{
		ShortTasks.Remove(ShortTasks.FirstOrDefault(task => task.Id == taskId));
		LongTasks.First(task => task.Id == SelectedLongTask.Id).TaskCount--;
		_accountManager.DeleteShortTask(SelectedLongTask.Id, taskId);
	}
}
