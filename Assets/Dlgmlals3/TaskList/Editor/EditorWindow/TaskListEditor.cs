using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Dlgmlals3.Tasks
{
	public class TaskListEditor : EditorWindow
	{
		VisualElement container;
		ObjectField savedTasksObjectField;
		Button loadTasksButton;

		TextField taskText;
		Button addTaskButton;
		ScrollView taskListScrollView;

		TaskListSO taskListSO;


		public const string path = "Assets/Dlgmlals3/TaskList/Editor/EditorWindow/";
		[MenuItem("Dlgmlals3/Task List")]

		public static void ShowWindow()
		{
			TaskListEditor window = GetWindow<TaskListEditor>();
			window.titleContent = new GUIContent("Task List");
		}

		public void CreateGUI()
		{
			container = rootVisualElement;
			VisualTreeAsset original = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path + "TaskListEditor.uxml");
			container.Add(original.Instantiate());

			StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(path + "TaskListEditor.uss");
			container.styleSheets.Add(styleSheet);

			savedTasksObjectField = container.Q<ObjectField>("savedTasksObjectField");
			savedTasksObjectField.objectType = typeof(TaskListSO);

			loadTasksButton = container.Q<Button>("loadTasksButton");
			loadTasksButton.clicked += LoadTasks;

			taskText = container.Q<TextField>("taskText");
			taskText.RegisterCallback<KeyDownEvent>(AddTask);

			addTaskButton = container.Q<Button>("addTaskButton");
			addTaskButton.clicked += AddTask;

			taskListScrollView = container.Q<ScrollView>("taskList");

			Debug.Log(taskText);
			Debug.Log(addTaskButton);
			Debug.Log(taskListScrollView);
		}

		void AddTask()
		{
			if (!string.IsNullOrEmpty(taskText.value))
			{
				taskListScrollView.Add(CreateTask(taskText.value));
				SaveTask(taskText.value);
				taskText.value = "";
				taskText.Focus();
			}
		}

		private Toggle CreateTask(string taskText)
		{
			Toggle taskItem = new Toggle();
			taskItem.text = taskText;
			return taskItem;
		}

		void AddTask(KeyDownEvent e)
		{
			if (Event.current.Equals(Event.KeyboardEvent("Return")))
			{
				AddTask();
			}
		}

		void LoadTasks()
		{
			taskListSO = (TaskListSO) savedTasksObjectField.value as TaskListSO;
			if (taskListSO != null)
			{
				taskListScrollView.Clear();
				List<string> tasks = taskListSO.GetTasks();
				foreach (string task in tasks)
				{
					taskListScrollView.Add(CreateTask(task));
				}
			}
		}

		void SaveTask(string task)
		{
			taskListSO.AddTask(task);
			EditorUtility.SetDirty(taskListSO);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

	}
}