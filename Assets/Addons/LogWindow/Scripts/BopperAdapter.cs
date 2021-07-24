using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using frame8.Logic.Misc.Other.Extensions;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using TMPro;
using UnityEngine.UI.ProceduralImage;
using UnityEngine.Events;

namespace Bopper
{

	// There are 2 important callbacks you need to implement, apart from Start(): CreateViewsHolder() and UpdateViewsHolder()

	public class BopperAdapter : OSA<BopperParams, BopperViewsHolder>
	{
		public NotificationList<Command> _data;
		public UnityAction<int> clickAction;
		public Player[] players;
		public CommandController commandController;

		/// <summary>
		/// Tracks current index so items can be highlighted/reset properly
		/// </summary>
		int currentIndex = 0;
		int lastIndex = -1;

		#region OSA implementation
		protected override void Start()
		{
			_data = commandController.commands;
			_data.listeners += OnDataChanged;

			BopperViewsHolder.players = players;
			// Calling this initializes internal data and prepares the adapter to handle item count changes
			base.Start();

		}
		const int START_INDEX = 0;
		const int END_INDEX = int.MaxValue; // so max visibile is END-START: 11-3 = 8

		/// <summary>
		/// Notify base adapter than content has changed.  
		/// 
		/// Note that the data index differs from the adapter index by START_INDEX
		/// </summary>
		/// <param name="index"></param>
		/// <param name="operation_count"></param>
		protected void OnDataChanged(int index, int operation_count)
        {

			if (index == 0 && operation_count == 0)
			{
				Debug.Log($"data.Count = {_data.Count}");
				ResetItems(Math.Max(0, Math.Min(END_INDEX, _data.Count) - START_INDEX));
				return;
			}

			int count = Math.Abs(operation_count); 
			Debug.Log($"BopperAdapter.OnDataChanged(i={index}, c={count}, oc={operation_count}) Data size={_data.Count}");

			if (index < START_INDEX)
			{
				count -= START_INDEX - index;
				index = START_INDEX;
			}
			if (index + count >= END_INDEX) // 1 + 1 = 2   ; >=1 so count 1 - index = 1
			{
                Debug.Log($"index({index}) + count({count}) >= END_INDEX({END_INDEX}), so setting count to {END_INDEX-index}");
				count = END_INDEX - index;
			}

			Debug.Log($"X                          (i={index}, c={count}, oc={operation_count}) Data size={_data.Count}");
			if (count > 0)
				Debug.Log($"We are about to notify OSA that data at index {index} count={count} has changed. The adujsted index is {index - START_INDEX}. The data is {_data[index]}");
			if (count > 0)
				if (operation_count > 0)
					InsertItems(index - START_INDEX, count);
				else 
					RemoveItems(index - START_INDEX, count);
		}


		protected Command GetViewData(int viewIndex)
		{
			return _data[viewIndex + START_INDEX];
		}

		protected override BopperViewsHolder CreateViewsHolder(int itemIndex)
		{
            Debug.Log($"BopperViewsHolder.CreateViewsHolder({itemIndex})");
			var type = GetViewData(itemIndex).GetType();

			RectTransform prefab;
			BopperViewsHolder vh;

			if (type == typeof(CommandPhase))
			{ 
				prefab = _Params.phasePrefab;
				vh = new BopperViewsHolderPhase();
			}
			else if (type == typeof(CommandSay))
            {
				prefab = _Params.chatPrefab;
				vh = new BopperViewsHolderChat();
			}
			else
			{
				prefab = _Params.commandPrefab;
				vh = new BopperViewsHolderCommand();
			}
            Debug.Log($"CreateVH: {itemIndex}");
			vh.Init(prefab, _Params.Content, itemIndex);
			vh.SetItemClickAction(clickAction);  // clicking the vh will call bopperAdapter.clickAction

			return vh;
		}



		// This is called anytime a previously invisible item become visible, or after it's created, 
		// or when anything that requires a refresh happens
		// Here you bind the data from the model to the item's views
		// *For the method's full description check the base implementation
		protected override void UpdateViewsHolder(BopperViewsHolder newOrRecycled)
		{
			// In this callback, "newOrRecycled.ItemIndex" is guaranteed to always reflect the
			// index of item that should be represented by this views holder. You'll use this index
			// to retrieve the model from your data set

			Debug.Assert(newOrRecycled != null, "UpdateViewsHolder passed null");
			Debug.Assert(newOrRecycled.message != null, "titleText cannot be null");

			Command item = GetViewData(newOrRecycled.ItemIndex);
			Debug.Assert(item != null, "Data is null");
			//Debug.Log($"newOrRecycled.UpdateViews({model.id},{currentIndex})");
			//newOrRecycled.UpdateViews(model, model.id == currentIndex);                                 //  TODO1
			newOrRecycled.UpdateViews(item, newOrRecycled.ItemIndex == currentIndex);                    //  TODO1

			newOrRecycled.MarkForRebuild();
			ScheduleComputeVisibilityTwinPass(true);
		}

		protected override float UpdateItemSizeOnTwinPass(BopperViewsHolder viewsHolder)
		{
			//viewsHolder.UpdateSize();
			//return viewsHolder.root.rect.height;
			if (viewsHolder.GetType() == typeof(BopperViewsHolderChat))
				return base.UpdateItemSizeOnTwinPass(viewsHolder);
			else
            {
				viewsHolder.UpdateSize();
				return viewsHolder.root.rect.height;
			}



		}

		/**
		 * This is where the fuckedup one-to-one subclass dependency is implemented
		 * 
		 * Who uses this POS method?
		 * A: it is defined and used by OSA
		 * 
		 * What and why does it exist?
		 * 
		 * From OSA: 
		 * 
		 * 		/// <summary> Self-explanatory. The default implementation returns true each time</summary>
		 *		/// <returns>If the provided views holder is compatible with the item with index <paramref name="indexOfItemThatWillBecomeVisible"/></returns>
		 * 
		 * OFC. "Self-explanatory".  If you can read the assholes mind!!!
		 * 
		 * It fucking calls the derived classes again. Spagehtii!
		 */

		protected override bool IsRecyclable(BopperViewsHolder potentiallyRecyclable, int indexOfItemThatWillBecomeVisible, double sizeOfItemThatWillBecomeVisible)
		{
			Command item = GetViewData(indexOfItemThatWillBecomeVisible);
			return potentiallyRecyclable.CanPresentModelType(item.GetType());
		}

        // This is the best place to clear an item's views in order to prepare it from being recycled, but this is not always needed, 
        // especially if the views' values are being overwritten anyway. Instead, this can be used to, for example, cancel an image 
        // download request, if it's still in progress when the item goes out of the viewport.
        // <newItemIndex> will be non-negative if this item will be recycled as opposed to just being disabled
        // *For the method's full description check the base implementation
        /*
		protected override void OnBeforeRecycleOrDisableViewsHolder(MyListItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
		{
			base.OnBeforeRecycleOrDisableViewsHolder(inRecycleBinOrVisible, newItemIndex);
		}
		*/

        // You only need to care about this if changing the item count by other means than ResetItems, 
        // case in which the existing items will not be re-created, but only their indices will change.
        // Even if you do this, you may still not need it if your item's views don't depend on the physical position 
        // in the content, but they depend exclusively to the data inside the model (this is the most common scenario).
        // In this particular case, we want the item's index to be displayed and also to not be stored inside the model,
        // so we update its title when its index changes. At this point, the Data list is already updated and 
        // shiftedViewsHolder.ItemIndex was correctly shifted so you can use it to retrieve the associated model
        // Also check the base implementation for complementary info
        /*
		protected override void OnItemIndexChangedDueInsertOrRemove(MyListItemViewsHolder shiftedViewsHolder, int oldIndex, bool wasInsert, int removeOrInsertIndex)
		{
			base.OnItemIndexChangedDueInsertOrRemove(shiftedViewsHolder, oldIndex, wasInsert, removeOrInsertIndex);

			shiftedViewsHolder.titleText.text = Data[shiftedViewsHolder.ItemIndex].title + " #" + shiftedViewsHolder.ItemIndex;
		}
		*/
        #endregion

        #region Selection
        public void SetCurrentTop()
		{
			SetCurrent(0);
		}

		public void SetCurrentEnd()
		{
			SetCurrent(int.MaxValue);
		}
		public void SetCurrentRelative(int n)
		{
			SetCurrent(currentIndex + n);
		}

		public void SetCurrent(int n)
		{
			lastIndex = currentIndex;
			currentIndex = Math.Max(0, Math.Min(GetItemsCount() - 1, n));
			//currentIndex = Math.Max(0, n);
			//Debug.Log($"CurrentIndex changes {lastIndex} => {currentIndex}");
		}


		public void UpdateSelection()
		{
			int count = GetItemsCount();
			//Debug.Log($"UpdateSelection() count={count} data.count={data.Count}");


			if (lastIndex != currentIndex && count > 0)
			{
				int makeVisibleIndex = Math.Min(currentIndex, count - 1);           // this does nothing
				double visibility = GetItemSignedVisibility(makeVisibleIndex);

				//Debug.Log($"c={currentIndex} l={lastIndex} v={makeVisibleIndex} vis={visibility}");

				// Make visible to force update
				if (visibility < 0)
					ScrollTo(makeVisibleIndex);
				else if (visibility > 0)
					ScrollTo(makeVisibleIndex, 1f, 1f);

				ForceUpdateViewsHolderIfVisible(currentIndex);

				// if at bottom of viewport, redraw to ensure new size is reflected in placement
				if (visibility > 0)
					ScrollTo(makeVisibleIndex, 1f, 1f);

				// update last selection if visible
				ForceUpdateViewsHolderIfVisible(lastIndex);
				//Debug.Log($"UpdateSelection({lastIndex} -> {currentIndex})");
			}
		}
        #endregion
    }


    #region ViewHolder
    abstract public class BopperViewsHolder : BaseItemViewsHolder
	{
		public TextMeshProUGUI message;
		public float height;
		public static Player[] players;
		public long id;
		public bool isSelected;
		protected Image selectionBackgroundImage;
		UnityAction<int> clickAction;


		public override void CollectViews()
		{
			base.CollectViews();
			height = root.rect.height;
			root.GetComponent<Button>()?.onClick.AddListener(OnClicked); // todo
			selectionBackgroundImage = root.GetComponent<Image>();

		}
		public virtual void UpdateViews(Command command, bool isSelected)
        {
            //Debug.Log($"UpdateViews({command.id}, Selected={isSelected}) m:{command.ToString()}");
			id = command.id;
			this.isSelected = isSelected;
		}

		public void UpdateSize()
		{
			root.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
		}
		public abstract bool CanPresentModelType(Type modelType);

		protected void OnClicked()
        {
			Debug.Log($"OnClicked(id:{id} ItemIndex:{ItemIndex})");
			clickAction?.Invoke(ItemIndex);
        }
		
		public void SetItemClickAction(UnityAction<int> clickAction)
        {
			this.clickAction = clickAction;
		}

	}

	public class BopperViewsHolderPhase : BopperViewsHolder
	{
		public override void CollectViews()
		{
			base.CollectViews();
			message = root.GetComponentInChildren<TextMeshProUGUI>();
		}
		public override void UpdateViews(Command command, bool isSelected)
        {
			base.UpdateViews(command, isSelected);
			Player player = players[command.player_id];
			message.text = command.ToString();
			message.color = player.darkColor;
			message.fontStyle = FontStyles.Normal;
			selectionBackgroundImage.color = isSelected ? player.lightColor : Color.white;
			
		}
		public override bool CanPresentModelType(Type modelType) { return modelType == typeof(CommandPhase); }

	}


	public class BopperViewsHolderChat : BopperViewsHolder
	{
		public ProceduralImage backgroundImage;
		public Image avatar;
		private ContentSizeFitter _contentSizeFitter;

		ContentSizeFitter ContentSizeFitter
		{
			get
			{
				if (_contentSizeFitter == null)
					_contentSizeFitter = root.GetComponent<ContentSizeFitter>();
				return _contentSizeFitter;
			}
		}
			

		public override void CollectViews()
		{
			base.CollectViews();

			if (ContentSizeFitter)
				ContentSizeFitter.enabled = false; // the content size fitter should not be enabled during normal lifecycle, only in the "Twin" pass frame

			root.GetComponentAtPath("HLG/MPImage/Text", out message);
			root.GetComponentAtPath("HLG/MPImage", out backgroundImage);
			root.GetComponentAtPath("HLG/IconContainer/Player Icon", out avatar);


			Debug.Assert(message != null);
			Debug.Assert(backgroundImage != null);
			Debug.Assert(avatar != null);
		}

		public override void MarkForRebuild()
		{
            //Debug.Log($"chat: MarkForRebuild {id}");
			base.MarkForRebuild();
			if (ContentSizeFitter)
				ContentSizeFitter.enabled = true;
			else
				throw new Exception("Fuck you");
		}
		public override void UnmarkForRebuild()
		{
			//Debug.Log($"chat: UnmarkForRebuild {id}");
			if (ContentSizeFitter)
				ContentSizeFitter.enabled = false;
			base.UnmarkForRebuild();
		}


		public override void UpdateViews(Command command, bool isSelected)
		{
			base.UpdateViews(command, isSelected);
			Player player = players[command.player_id];

			message.text = command.ToString();
			message.color = player.darkColor;
			message.fontStyle = FontStyles.Bold;
			backgroundImage.color = player.lightColor;
			//backgroundImage.OutlineColor = player.darkColor;
			avatar.sprite = player.avatar;

			selectionBackgroundImage.color = isSelected ? players[0].lightColor : Color.white;

		}
		public override bool CanPresentModelType(Type modelType) { return modelType == typeof(CommandSay); }
	}



    public class BopperViewsHolderCommand : BopperViewsHolder
	{
		public ProceduralImage backgroundImage;
		public ProceduralImage borderImage;
		public Image avatar;

		public override void CollectViews()
		{
			base.CollectViews();
			root.GetComponentAtPath("Background", out backgroundImage);
			root.GetComponentAtPath("Background/Text", out message);
			root.GetComponentAtPath("Background/Border", out borderImage);
			root.GetComponentAtPath("IconContainer/Player Icon", out avatar);
		}

		public override void UpdateViews(Command command, bool isSelected)
		{
			base.UpdateViews(command, isSelected);
			Player player = players[command.player_id];

			message.text = command.ToString();
			message.color = player.darkColor;
			message.fontStyle = FontStyles.Normal;
			//backgroundImage.color = isSelected ? player.lightColor : Color.white;
			selectionBackgroundImage.color = isSelected ? player.lightColor : Color.white;
			borderImage.color = player.darkColor;
			avatar.sprite = player.avatar;
		}
		public override bool CanPresentModelType(Type modelType) { return modelType != typeof(CommandSay) && modelType != typeof(CommandPhase); }
	}

    #endregion

    [Serializable]
	public class Player
	{
		public string name;
		public Color darkColor;
		public Color lightColor;
		public Sprite avatar;
	}

	/// <summary>Contains the 3 prefabs associated with the 3 views holders</summary>
	[Serializable] // serializable, so it can be shown in inspector
	public class BopperParams : BaseParams
	{
		public RectTransform commandPrefab;
		public RectTransform chatPrefab;
		public RectTransform phasePrefab;

		public override void InitIfNeeded(IOSA iAdapter)
		{
			base.InitIfNeeded(iAdapter);

			AssertValidWidthHeight(commandPrefab);
			AssertValidWidthHeight(chatPrefab);
			AssertValidWidthHeight(phasePrefab);
		}
	}
}
