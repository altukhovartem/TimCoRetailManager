﻿using AutoMapper;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDesktopUI.Library.API;
using TRMDesktopUI.Library.Helpers;
using TRMDesktopUI.Library.Models;
using TRMDesktopUI.Models;

namespace TRMDesktopUI.ViewModels
{
	public class SalesViewModel: Screen 
	{
		private IProductEndpoint _productEndpoint;
		private ISaleEndpoint _saleEndpoint;
		private IConfigHelper _configHelper;
		private IMapper _mapper;

		public SalesViewModel(IProductEndpoint productEndpoint, IConfigHelper configHelper, ISaleEndpoint saleEndpoint, IMapper mapper)
		{
			_productEndpoint = productEndpoint;
			_saleEndpoint = saleEndpoint;
			_configHelper = configHelper;
			_mapper = mapper;
		}

		protected override async void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);
			await LoadProducts();
		}

		private async Task LoadProducts()
		{
			var productList = await _productEndpoint.GetAll();
			var products = _mapper.Map<List<ProductDisplayModel>>(productList);
			Products = new BindingList<ProductDisplayModel>(products);
		}


		private BindingList<ProductDisplayModel> _product;

		public BindingList<ProductDisplayModel> Products
		{
			get { return _product; }
			set 
			{
				_product = value;
				NotifyOfPropertyChange(() => Products);
			}
		}

		private ProductDisplayModel _selectedProduct;

		public ProductDisplayModel SelectedProduct
		{
			get { return _selectedProduct; }
			set 
			{ 
				_selectedProduct = value;
				NotifyOfPropertyChange(() => SelectedProduct);
				NotifyOfPropertyChange(() => CanAddToCart);
			}
		}

		private async Task ResetSalesViewModel()
		{
			Cart = new BindingList<CartItemDisplayModel>();
			await LoadProducts();
			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
			NotifyOfPropertyChange(() => CanCheckOut);
		}

		private CartItemDisplayModel _selectedCartItem;

		public CartItemDisplayModel SelectedCartItem
		{
			get { return _selectedCartItem; }
			set
			{
				_selectedCartItem = value;
				NotifyOfPropertyChange(() => SelectedCartItem);
				NotifyOfPropertyChange(() => CanRemoveFromCart);
			}
		}


		private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();

		public BindingList<CartItemDisplayModel> Cart
		{
			get { return _cart; }
			set
			{
				_cart = value;
				NotifyOfPropertyChange(() => Cart);
			}
		}

		private int _itemQuantity = 1;

		public int ItemQuantity
		{
			get { return _itemQuantity; }
			set 
			{ 
				_itemQuantity = value;
				NotifyOfPropertyChange(() => ItemQuantity);
				NotifyOfPropertyChange(() => CanAddToCart);
			}
		}

		public string SubTotal 
		{ 
			get
			{
			
				return CalculateSubTotal().ToString("C");
			}
		}

		private decimal CalculateSubTotal()
		{
			decimal subTotal = 0;
			foreach (var item in Cart)
			{
				subTotal += item.Product.RetailPrice * item.QuantityInCart;
			}

			return subTotal;
		}

		public string Tax
		{
			get
			{
				return CalculateTax().ToString("C");
			}
		}

		private decimal CalculateTax()
		{
			decimal taxAmount = 0;
			decimal taxRate = _configHelper.GetTaxRate()/100;

			taxAmount = Cart
						.Where(x => x.Product.IsTaxable)
						.Sum(x => x.Product.RetailPrice * x.QuantityInCart * taxRate);

			return taxAmount;
		}

		public string Total
		{
			get
			{
				decimal total = CalculateSubTotal() + CalculateTax();
				return total.ToString("C");
			}
		}


		public bool CanAddToCart
		{
			get
			{
				bool output = false;
				// make sure someting is selected
				// make sure there is an item quantity

				if(ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity )
				{
					output = true;
				}

				return output;
			}
		}

		public void AddToCart()
		{
			CartItemDisplayModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
			if(existingItem != null)
			{
				existingItem.QuantityInCart += ItemQuantity;
			}
			else
			{
				CartItemDisplayModel item = new CartItemDisplayModel
				{
					Product = SelectedProduct,
					QuantityInCart = ItemQuantity
				};

				Cart.Add(item);
			}

			
			SelectedProduct.QuantityInStock -= ItemQuantity;
			ItemQuantity = 1;
			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
			NotifyOfPropertyChange(() => CanCheckOut);
		}

		public bool CanRemoveFromCart
		{
			get
			{
				bool output = false;
				if (SelectedCartItem != null && SelectedCartItem?.QuantityInCart > 0)
				{
					output = true;
				}

				return output;
			}
		}

		public void RemoveFromCart()
		{
			SelectedCartItem.Product.QuantityInStock += 1;
			if (SelectedCartItem.QuantityInCart > 1)
			{
				SelectedCartItem.QuantityInCart -= 1;
			}
			else
			{
				Cart.Remove(SelectedCartItem);
			}

			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
			NotifyOfPropertyChange(() => CanCheckOut);
			NotifyOfPropertyChange(() => CanAddToCart);
		}

		public bool CanCheckOut
		{
			get
			{
				bool output = false;
				if (Cart.Count > 0)
				{
					output = true;
				}

				return output;
			}
		}

		public async Task CheckOut()
		{
			//create a salemodel , post to API
			SaleModel sale = new SaleModel();
			foreach (var item in Cart)
			{
				sale.SaleDetails.Add(new SaleDetailModel
				{
					ProductId = item.Product.Id,
					Quantity = item.QuantityInCart
				});
			}

			await _saleEndpoint.PostSale(sale);
			await ResetSalesViewModel();
		}
	}
}
