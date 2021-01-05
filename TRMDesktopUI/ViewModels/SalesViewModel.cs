using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDesktopUI.ViewModels
{
	public class SalesViewModel: Screen 
	{
		private BindingList<string> _product;

		public BindingList<string> Products
		{
			get { return _product; }
			set 
			{
				_product = value;
				NotifyOfPropertyChange(() => Products);
			}
		}

		private BindingList<string> _cart;

		public BindingList<string> Cart
		{
			get { return _cart; }
			set
			{
				_cart = value;
				NotifyOfPropertyChange(() => Cart);
			}
		}

		private int _itemQuantity;

		public int ItemQuantity
		{
			get { return _itemQuantity; }
			set 
			{ 
				_itemQuantity = value;
				NotifyOfPropertyChange(() => ItemQuantity);
			}
		}

		public string SubTotal 
		{ 
			get
			{
				// TODO - replace with calculation
				return "$0.00";
			}
		}

		public string Tax
		{
			get
			{
				// TODO - replace with calculation
				return "$0.00";
			}
		}

		public string Total
		{
			get
			{
				// TODO - replace with calculation
				return "$0.00";
			}
		}


		public bool CanAddToCart
		{
			get
			{
				bool output = false;
				// make sure someting is selected
				// make sure there is an item quantity

				return output;
			}
		}

		public void AddToCart()
		{

		}

		public bool CanRemoveFromCart
		{
			get
			{
				bool output = false;
				// make sure someting is selected
				// make sure there is an item quantity

				return output;
			}
		}

		public void RemoveFromCart()
		{

		}

		public bool CanCheckOut
		{
			get
			{
				bool output = false;
				// make sure someting is selected
				// make sure there is an item quantity

				return output;
			}
		}

		public void CheckOut()
		{

		}
	}
}
