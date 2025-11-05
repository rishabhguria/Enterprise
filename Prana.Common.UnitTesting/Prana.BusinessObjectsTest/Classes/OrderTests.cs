using Prana.BusinessObjects;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes
{
    public class OrderTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "Order")]
        public void CanCreateSubOrders_ValidConditions_ReturnsTrue()
        {
            // Arrange
            var order = new Order
            {
                OrderStatusTagValue = FIXConstants.ORDSTATUS_New,
                Quantity = 100,
                UnsentQty = 50
            };
            var subOrder = new Order
            {
                Quantity = 30
            };

            // Act
            var result = order.CanCreateSubOrders(subOrder);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "Order")]
        public void CanCreateSubOrders_InvalidStatus_ReturnsFalse()
        {
            // Arrange
            var order = new Order
            {
                OrderStatusTagValue = FIXConstants.ORDSTATUS_Cancelled
            };
            var subOrder = new Order();
            subOrder.Quantity = 30;

            // Act
            var result = order.CanCreateSubOrders(subOrder);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "Order")]
        public void CreateSubOrder_ValidConditions_ReturnsTrue()
        {
            // Arrange
            var parentOrder = new Order
            {
                Quantity = 100,
                UnsentQty = 50
            };
            var subOrder = new Order
            {
                Quantity = 30
            };

            // Act
            var result = parentOrder.CreateSubOrder(subOrder);

            // Assert
            Assert.True(result);
            Assert.Equal(parentOrder, subOrder.Parent);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "Order")]
        public void CreateSubOrder_InvalidConditions_ReturnsFalse()
        {
            // Arrange
            var parentOrder = new Order
            {
                Quantity = 10 // Not enough qty
            };
            var subOrder = new Order
            {
                Quantity = 30
            };

            // Act
            var result = parentOrder.CreateSubOrder(subOrder);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "Order")]
        public void GetSubOrder_ValidClientOrderID_ReturnsSubOrder()
        {
            // Arrange
            var order = new Order
            {
                Quantity = 100,
                UnsentQty = 50
            };
            var subOrder = new Order { ClientOrderID = "123" };
            order.CreateSubOrder(subOrder);

            // Act
            var result = order.GetSubOrder("123");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("123", result.ClientOrderID);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "Order")]
        public void GetSubOrder_InvalidClientOrderID_ReturnsNull()
        {
            // Arrange
            var order = new Order
            {
                Quantity = 100,
                UnsentQty = 50
            };

            // Act
            var result = order.GetSubOrder("invalidID");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "Order")]
        public void AddFill_ValidFill_UpdatesOrderFields()
        {
            // Arrange
            var order = new Order();
            var orderFill = new OrderFill
            {
                LeavesQty = 10,
                MsgType = FIXConstants.MSGOrderCancelReject,
                Price = 100,
                CumQty = 20,
                AvgPrice = 95,
                OrderStatusTagValue = FIXConstants.ORDSTATUS_New
            };

            // Act
            order.AddFill(orderFill);

            // Assert
            Assert.Equal(10, order.LeavesQty);
            Assert.Equal(100, order.Price);
            Assert.Equal(20, order.CumQty);
            Assert.Equal(95, order.AvgPrice);
            Assert.Equal(FIXConstants.ORDSTATUS_New, order.OrderStatusTagValue);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "Order")]
        public void AddFill_OrderStatusRejected_UpdatesFieldsCorrectly()
        {
            // Arrange
            var order = new Order();
            var orderFill = new OrderFill
            {
                LeavesQty = 0,
                MsgType = FIXConstants.MSGOrderCancelReject,
                Price = 50,
                CumQty = 30,
                AvgPrice = 40,
                OrderStatusTagValue = FIXConstants.ORDSTATUS_Rejected,
                LastShares = 10,
                Quantity = 100,
                Text = "Order rejected"
            };

            // Act
            order.AddFill(orderFill);

            // Assert
            Assert.Equal(0, order.LeavesQty);
            Assert.Equal(50, order.Price);
            Assert.Equal(30, order.CumQty);
            Assert.Equal(40, order.AvgPrice);
            Assert.Equal(FIXConstants.ORDSTATUS_Rejected, order.OrderStatusTagValue);
            Assert.Equal(10, order.LastShares);
            Assert.Equal(100, order.Quantity);
            Assert.Equal("Order rejected", order.Text);
            Assert.Equal(70, order.UnsentQty);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "Order")]
        public void AddFill_OrderStatusNew_UpdatesSendQtyAndOrderID()
        {
            // Arrange
            var order = new Order();
            var orderFill = new OrderFill
            {
                LeavesQty = 20,
                MsgType = FIXConstants.MSGOrderCancelReject,
                OrderID = "ORD123",
                OrderStatusTagValue = FIXConstants.ORDSTATUS_New,
                CumQty = 20,
                Price = 200
            };

            // Act
            order.AddFill(orderFill);

            // Assert
            Assert.Equal("ORD123", order.OrderID);
            Assert.Equal(0, order.SendQty);
            Assert.Equal(20, order.CumQty);
            Assert.Equal(20, order.LeavesQty);
            Assert.Equal(200, order.Price);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "Order")]
        public void AddFill_MsgTypeCancelReject_SetsClOrderID()
        {
            // Arrange
            var order = new Order();
            var orderFill = new OrderFill
            {
                MsgType = FIXConstants.MSGOrderCancelReject,
                OrigClOrderID = "OriginalOrder123",
                OrderTypeTagValue = FIXConstants.ORDTYPE_Limit,
                LeavesQty = 15
            };

            // Act
            order.AddFill(orderFill);

            // Assert
            Assert.Equal("OriginalOrder123", order.ClOrderID);
            Assert.Equal(FIXConstants.ORDTYPE_Limit, order.OrderTypeTagValue);
            Assert.Equal(15, order.LeavesQty);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "Order")]
        public void AddFill_OrderStatusPendingReplace_UpdatesUnsentQty()
        {
            // Arrange
            var order = new Order();
            var orderFill = new OrderFill
            {
                Quantity = 100,
                LeavesQty = 30,
                CumQty = 50,
                OrderStatusTagValue = FIXConstants.ORDSTATUS_PendingReplace
            };

            // Act
            order.AddFill(orderFill);

            // Assert
            Assert.Equal(30, order.LeavesQty);
            Assert.Equal(50, order.CumQty);
            Assert.Equal(20, order.UnsentQty);
        }
    }
}
