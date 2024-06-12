using GameFramework;
using GameFramework.Singleton;
using System.Collections.Concurrent;
using System.Linq;
using UnityGameFramework;

namespace Client
{
	public class FormManager : Singleton<FormManager>
	{
		private class FormData : IReference
		{
			public UIType ResourceType { get; set; }
			public UIBaseForm Form { get; set; }
			public void Clear()
			{
				ResourceType = UIType.None;
				Form = null;
			}

			public static FormData Create(UIBaseForm form)
			{
				var data = ReferencePool.Acquire<FormData>();
				data.Form = form;
				data.ResourceType = form.ResourceType;
				return data;
			}
		}
		public FormManager() { }

		private readonly ConcurrentDictionary<UIType, FormData> _forms = new();
		private readonly ConcurrentQueue<UIBaseForm> _showForms = new();
		private readonly ConcurrentQueue<FormData> _waitRemoveForms = new();

		public int LowDepthOffset { get; private set; } = 0;
		public int MidDepthOffset { get; private set; } = 0;
		public int HighDepthOffset { get; private set; } = 0;

		private readonly ConcurrentStack<UIBaseForm> _lowFormStack = new();
		private readonly ConcurrentStack<UIBaseForm> _midFormStack = new();
		private readonly ConcurrentStack<UIBaseForm> _highFormStack = new();
		private readonly ConcurrentStack<UIBaseForm> _cacheFormStack = new();
		public override int Priority => 0;
		public override void Init()
		{
			
		}

		public override void Release()
		{
			LowDepthOffset = 0;
			MidDepthOffset = 0;
			HighDepthOffset = 0;
			_forms.Clear();
			_showForms.Clear();
			_waitRemoveForms.Clear();
		}

		public override void Update(float elapseSeconds, float realElapseSeconds)
		{
			while (_waitRemoveForms.Count > 0)
			{
				if (_waitRemoveForms.TryDequeue(out var data))
				{
					ReferencePool.Release(data);
				}
			}
			while (_showForms.Count > 0)
			{
				if (_showForms.TryDequeue(out var form))
				{
					if (!form.IsDestroyed)
					{
						form.Show();
					}
				}
			}
			foreach (var formData in _forms.Values)
			{
				if (formData.Form != null && !formData.Form.IsDestroyed)
				{
					formData.Form.Update(elapseSeconds, realElapseSeconds);
				}
			}
		}

		public void AddForm(UIBaseForm form)
		{
			_forms.TryAdd(form.ResourceType, FormData.Create(form));
			_showForms.Enqueue(form);
			var resourceDataInfo = ResourceConfig.Get(form.ResourceType);
			switch (resourceDataInfo.UILayer)
			{
				case UILayer.LowUI:
					_lowFormStack.Push(form);
					LowDepthOffset = form.AddDepth(LowDepthOffset) + 1;
					break;
				case UILayer.MidUI:
					_midFormStack.Push(form);
					MidDepthOffset = form.AddDepth(MidDepthOffset) + 1;
					break;
				case UILayer.HighUI:
					_highFormStack.Push(form);
					HighDepthOffset = form.AddDepth(HighDepthOffset) + 1;
					break;
			}
		}

		public void ChangeDepth(UIType resourceType, int depthOffset)
		{
			var resourceDataInfo = ResourceConfig.Get(resourceType);
			switch (resourceDataInfo.UILayer)
			{
				case UILayer.LowUI:
				{
					while (_lowFormStack.Count > 0)
					{
						if (_lowFormStack.TryPop(out var form))
						{
							_cacheFormStack.Push(form);
							if (form.ResourceType == resourceType)
							{
								var depth = form.AddDepth(depthOffset);

								if (depth >= LowDepthOffset)
								{
									LowDepthOffset = depth + 1;
								}
								break;
							}
						}
					}
					var list = _lowFormStack.ToList();
					list.AddRange(_cacheFormStack.ToList());
					list.Sort();
					_lowFormStack.Clear();
					_cacheFormStack.Clear();
					foreach (var f in list)
					{
						_lowFormStack.Push(f);
					}
					break;
				}	
				case UILayer.MidUI:
				{
					while (_midFormStack.Count > 0)
					{
						if (_midFormStack.TryPop(out var form))
						{
							_cacheFormStack.Push(form);
							if (form.ResourceType == resourceType)
							{
								var depth = form.AddDepth(depthOffset);

								if (depth >= MidDepthOffset)
								{
									MidDepthOffset = depth + 1;
								}
								break;
							}
						}
					}
					var list = _midFormStack.ToList();
					list.AddRange(_cacheFormStack.ToList());
					list.Sort();
					_midFormStack.Clear();
					_cacheFormStack.Clear();
					foreach (var f in list)
					{
						_midFormStack.Push(f);
					}
					break;
				}
				case UILayer.HighUI:
				{
					while (_highFormStack.Count > 0)
					{
						if (_highFormStack.TryPop(out var form))
						{
							_cacheFormStack.Push(form);
							if (form.ResourceType == resourceType)
							{
								var depth = form.AddDepth(depthOffset);

								if (depth >= HighDepthOffset)
								{
									HighDepthOffset = depth + 1;
								}
								break;
							}
						}
					}
					var list = _highFormStack.ToList();
					list.AddRange(_cacheFormStack.ToList());
					list.Sort();
					_highFormStack.Clear();
					_cacheFormStack.Clear();
					foreach (var f in list)
					{
						_highFormStack.Push(f);
					}
					break;
				}
			}
		}

		public bool HasForm(UIType resourceType)
		{
			return _forms.ContainsKey(resourceType);
		}

		public T GetForm<T>(UIType resourceType) where T : UIBaseForm
		{
			if (_forms.TryGetValue(resourceType, out var form))
			{
				return form as T;
			}
			return null;
		}

		public void RemoveForm(UIType resourceType)
		{
			if (_forms.TryRemove(resourceType, out var formData))
			{
				_waitRemoveForms.Enqueue(formData);
				var resourceDataInfo = ResourceConfig.Get(resourceType);
				switch (resourceDataInfo.UILayer)
				{
					case UILayer.LowUI:
					{
						while (_lowFormStack.Count > 0)
						{
							if (_lowFormStack.TryPop(out var form))
							{
								if (form.ResourceType == resourceType)
								{
									if (_cacheFormStack.Count == 0)
									{
										LowDepthOffset = form.DepthOffset;
									}
									break;
								}
								else
								{
									_cacheFormStack.Push(form);
								}
							}
						}
						while (_cacheFormStack.Count > 0)
						{
							if (_cacheFormStack.TryPop(out var cacheForm))
							{
								_lowFormStack.Push(cacheForm);
							}
						}
						break;
					}
					case UILayer.MidUI:
					{
						while (_midFormStack.Count > 0)
						{
							if (_midFormStack.TryPop(out var form))
							{
								if (form.ResourceType == resourceType)
								{
									if (_cacheFormStack.Count == 0)
									{
										MidDepthOffset = form.DepthOffset;
									}
									break;
								}
								else
								{
									_cacheFormStack.Push(form);
								}
							}
						}
						while (_cacheFormStack.Count > 0)
						{
							if (_cacheFormStack.TryPop(out var cacheForm))
							{
								_midFormStack.Push(cacheForm);
							}
						}
						break;
					}
					case UILayer.HighUI:
					{
						while (_highFormStack.Count > 0)
						{
							if (_highFormStack.TryPop(out var form))
							{
								if (form.ResourceType == resourceType)
								{
									if (_cacheFormStack.Count == 0)
									{
										HighDepthOffset = form.DepthOffset;
									}
									break;
								}
								else
								{
									_cacheFormStack.Push(form);
								}
							}
						}
						while (_cacheFormStack.Count > 0)
						{
							if (_cacheFormStack.TryPop(out var cacheForm))
							{
								_highFormStack.Push(cacheForm);
							}
						}
						break;
					}
				}
			}
		}
	
		public void CloseAllForm()
		{
			foreach (var formData in _forms)
			{
				formData.Value.Form.Destroy();
			}
		}
	}
}
