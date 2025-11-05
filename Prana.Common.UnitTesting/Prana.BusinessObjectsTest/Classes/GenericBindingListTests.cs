using Xunit;
using Prana.BusinessObjects;
using System.ComponentModel;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes
{
    public class GenericBindingListTests
    {
        private GenericBindingList<TestKeyable> _list;

        public GenericBindingListTests()
        {
            _list = new GenericBindingList<TestKeyable>();
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "GenericBindingList")]
        public void Add_Item_ShouldAddToListAndDictionary()
        {
            var item = new TestKeyable("Key1");
            _list.Add(item);

            Assert.Equal(1, _list.Count);
            Assert.True(_list.Contains(item));
            Assert.NotNull(_list.GetItem("Key1"));
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "GenericBindingList")]
        public void Add_NullItem_ShouldNotAddToList()
        {
            _list.Add(null);

            Assert.Equal(0, _list.Count);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "GenericBindingList")]
        public void GetList_ShouldReturnAllItems()
        {
            var item1 = new TestKeyable("Key1");
            var item2 = new TestKeyable("Key2");

            _list.Add(item1);
            _list.Add(item2);

            var items = _list.GetList();

            Assert.Equal(2, items.Count);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "GenericBindingList")]
        public void Remove_Item_ShouldRemoveFromListAndDictionary()
        {
            var item = new TestKeyable("Key1");
            _list.Add(item);
            _list.Remove(item);

            Assert.Equal(0, _list.Count);
            Assert.Null(_list.GetItem("Key1"));
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "GenericBindingList")]
        public void Remove_ItemByKey_ShouldRemoveFromListAndDictionary()
        {
            var item = new TestKeyable("Key1");
            _list.Add(item);
            _list.Remove("Key1");

            Assert.Equal(0, _list.Count);
            Assert.Null(_list.GetItem("Key1"));
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "GenericBindingList")]
        public void UpdateItem_ShouldReplaceExistingItem()
        {
            var oldItem = new TestKeyable("Key1");
            var newItem = new TestKeyable("Key1") { Value = "Updated" };

            _list.Add(oldItem);
            _list.UpdateItem(newItem);

            var retrievedItem = _list.GetItem("Key1");

            Assert.Equal("Updated", retrievedItem.Value);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "GenericBindingList")]
        public void Contains_Item_ShouldReturnTrueIfItemExists()
        {
            var item = new TestKeyable("Key1");
            _list.Add(item);

            Assert.True(_list.Contains(item));
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "GenericBindingList")]
        public void Contains_Item_ShouldReturnFalseIfItemDoesNotExist()
        {
            var item = new TestKeyable("Key1");

            Assert.False(_list.Contains(item));
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "GenericBindingList")]
        public void Clear_ShouldRemoveAllItems()
        {
            var item1 = new TestKeyable("Key1");
            var item2 = new TestKeyable("Key2");

            _list.Add(item1);
            _list.Add(item2);

            _list.Clear();

            Assert.Equal(0, _list.Count);
            Assert.Null(_list.GetItem("Key1"));
            Assert.Null(_list.GetItem("Key2"));
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "GenericBindingList")]
        public void RaisePropertyChanged_ShouldTriggerPropertyHasChanged()
        {
            var item = new TestKeyable("Key1");
            _list.Add(item);

            _list.Update(item);

            Assert.True(item.PropertyChangedCalled);
        }

        // Helper class implementing IKeyable and INotifyPropertyChangedCustom
        public class TestKeyable : IKeyable, INotifyPropertyChangedCustom
        {
            public string Key { get; set; }
            public string Value { get; set; }
            public bool PropertyChangedCalled { get; private set; }

            public TestKeyable(string key)
            {
                Key = key;
            }

            #pragma warning disable CS0067
            //Suppressing this warning as this event is not used in the current implementation but can't remove it as it's part of the interface contract.
            public event PropertyChangedEventHandler PropertyChanged;
            #pragma warning restore CS0067

            public string GetKey() => Key;

            public void Update(IKeyable item)
            {
                if (item is TestKeyable keyable)
                {
                    Value = keyable.Value;
                }
            }

            public void PropertyHasChanged()
            {
                PropertyChangedCalled = true;
            }
        }
    }
}
