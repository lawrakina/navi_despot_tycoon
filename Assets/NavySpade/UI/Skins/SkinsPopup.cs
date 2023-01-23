using System.Collections.Generic;
using Core.Meta.Skins;
using Core.UI.Economic;
using Core.UI.Popups.Abstract;
using NavySpade.Meta.Runtime.Skins.Configuration;
using NavySpade.Modules.Meta.Runtime.Skins.Configuration;
using NavySpade.UI.Popups.Abstract;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace Core.UI.Skins
{
    public class SkinsPopup : Popup
    {
        [SerializeField] private SkinElement _selected;
        [SerializeField] private GameObject _pagePrefab;
        [SerializeField] private SkinElement _skinsPrefab;
        [SerializeField] private SkinSelectionButtonPresentor _buttonView;

        [SerializeField] private PaginationManager _pagination;
        [SerializeField] private Toggle _pageTogglePrefab;

        [SerializeField] private Transform _pageParent;
        [SerializeField] private HorizontalScrollSnap _scrollSnap;

        [SerializeField] private PricePresentor _pricePresentor;

        [SerializeField] private int _skinsCountInPage = 4;

        private GameObject[] _pages;
        private SkinElement[] _skins;
        private int _selectedSkinIndex;

        private List<SkinData> _skinDatas;

        private int PagesCount => (_skinDatas.Count + _skinsCountInPage - 1) / _skinsCountInPage;
        private int CurrentPage => Mathf.FloorToInt((float) SkinsManager.SelectedSkinIndex / PagesCount);

        public override void OnAwake()
        {
        }

        public override void OnStart()
        {
            GeneratePages();
            UpdateScrollSnap();
            UpdateToggleGroup();
            
            _selected.UpdateView(SkinsManager.SelectedSkin);
        }

        private void GeneratePages()
        {
            _skinDatas = SkinsConfig.Instance.GetAllSkins();
            var skinIndex = 0;
            var pagesCount = PagesCount;
            _pages = new GameObject[pagesCount];
            _skins = new SkinElement[_skinDatas.Count];

            for (var pageIndex = 0; pageIndex < _pages.Length; pageIndex++)
            {
                var currentPage = _pages[pageIndex] = Instantiate(_pagePrefab, _pageParent);

                for (int i = 0; i < _skinsCountInPage; i++)
                {
                    if (skinIndex >= _skinDatas.Count)
                        break;

                    var skin = Instantiate(_skinsPrefab, currentPage.transform);
                    var data = _skinDatas[skinIndex];
                    var currentIndex = skinIndex;
                    
                    skin.InitView(() => ClickToSkinIndex(currentIndex));
                    skin.UpdateView(data);
                    _skins[skinIndex] = skin;

                    skinIndex++;
                }
            }
        }

        private void ClickToSkinIndex(int index)
        {
            var data = SkinsConfig.Instance.GetSkin(index);

            _selectedSkinIndex = index;
            _selected.UpdateView(data);

            _pricePresentor.UpdateView(data.ShopDefaultPrice);
            _buttonView.UpdateView(index);
        }

        public void BuyButton()
        {
            var data = SkinsConfig.Instance.GetSkin(_selectedSkinIndex);

            if (!data.ShopDefaultPrice.IsCanBuy()) 
                return;
            
            
            data.ShopDefaultPrice.Buy();
            SkinsManager.OpenSkin(_selectedSkinIndex);
            _buttonView.UpdateView(_selectedSkinIndex);
        }

        private void UpdateScrollSnap()
        {
            var currentPage = CurrentPage;
            _scrollSnap.ChildObjects = _pages;
            _scrollSnap.CurrentPage = currentPage;
            _scrollSnap.GoToScreen(currentPage);
        }

        private void UpdateToggleGroup()
        {
            var childs = new Queue<GameObject>();
            foreach (Transform child in _pagination.transform)
            {
                childs.Enqueue(child.gameObject);
            }

            while (childs.Count > 0)
            {
                var elemnt = childs.Dequeue();
                DestroyImmediate(elemnt);
            }

            for (var i = 0; i < PagesCount; i++)
            {
                Instantiate(_pageTogglePrefab, _pagination.transform);
            }
            
            _pagination.ResetPaginationChildren();
        }
    }
}