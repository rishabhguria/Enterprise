CREATE VIEW dbo.V_TradedOrders  
AS  
SELECT     dbo.T_TradedOrders.CLOrderID, dbo.T_TradedOrders.OrderSidetagValue, dbo.T_Side.Side AS OrderSideName, dbo.T_TradedOrders.Symbol,   
                      dbo.T_TradedOrders.OrderTypeTagValue, dbo.T_OrderType.OrderTypes AS OrderTypeName, dbo.T_TradedOrders.CounterPartyID,   
                      dbo.T_CounterParty.ShortName AS CounterPartyName, dbo.T_TradedOrders.VenueID, dbo.T_Venue.VenueName,   
                      dbo.T_TradedOrders.TradingAccountID, dbo.T_CompanyTradingAccounts.TradingShortName AS TradingAccountName, dbo.T_Asset.AssetID,   
                      dbo.T_Asset.AssetName, dbo.T_UnderLying.UnderLyingID, dbo.T_UnderLying.UnderLyingName, dbo.T_Exchange.ExchangeID,   
                      dbo.T_Exchange.DisplayName AS ExchangeName, dbo.T_Currency.CurrencyID, dbo.T_Currency.CurrencyName, dbo.T_TradedOrders.FundID,   
                      dbo.T_TradedOrders.StrategyID, dbo.T_TradedOrders.AUECID, dbo.T_TradedOrders.AveragePrice, dbo.T_TradedOrders.Quantity,   
                      dbo.T_TradedOrders.CumQty, dbo.T_TradedOrders.Price, dbo.T_TradedOrders.NirvanaSeqNumber, dbo.T_TradedOrders.InsertionTime,   
                      dbo.T_TradedOrders.AvgPrice, dbo.T_TradedOrders.MsgType, dbo.T_TradedOrders.OrderStatus, dbo.T_TradedOrders.ExecType,   
                      dbo.T_TradedOrders.LeavesQty, dbo.T_TradedOrders.LastPx, dbo.T_TradedOrders.LastShares, dbo.T_Sub.OriginalUserID, 
                      dbo.T_TradedOrders.OrigClOrderID, dbo.T_TradedOrders.ParentClOrderID, dbo.T_TradedOrders.ClientOrderID,   
                      dbo.T_TradedOrders.ParentClientOrderID, dbo.T_TradedOrders.OrderStatusTagValue, dbo.T_TradedOrders.TransactTime,   
                      dbo.T_TradedOrders.OpenClose, dbo.T_TradedOrders.NirvanaMsgType, dbo.T_TradedOrders.OrderID, dbo.T_TradedOrders.SettlementDate,   
                      dbo.T_TradedOrders.AlgoStrategyID, dbo.T_TradedOrders.AlgoParameters, dbo.T_TradedOrders.ExecutionInst, dbo.T_TradedOrders.TimeInForce,   
                      dbo.T_TradedOrders.HandlingInst, dbo.T_TradedOrders.AUECLocalDate, dbo.T_TradedOrders.Text, dbo.T_TradedOrders.Expr1,   
                      dbo.T_TradedOrders.AvgFxRateForTrade, dbo.T_TradedOrders.SenderSubID,dbo.T_TradedOrders.ProcessDate  ,dbo.T_TradedOrders.ModifiedUserID,dbo.T_Sub.UserID as CurrentUser,dbo.T_Sub.StagedOrderID
FROM         dbo.T_UnderLying INNER JOIN  
                      dbo.T_AUEC ON dbo.T_UnderLying.UnderLyingID = dbo.T_AUEC.UnderLyingID INNER JOIN  
                      dbo.T_Asset ON dbo.T_AUEC.AssetID = dbo.T_Asset.AssetID INNER JOIN  
                      dbo.T_Currency ON dbo.T_AUEC.BaseCurrencyID = dbo.T_Currency.CurrencyID INNER JOIN  
                      dbo.T_Exchange ON dbo.T_AUEC.ExchangeID = dbo.T_Exchange.ExchangeID INNER JOIN  
                      dbo.T_TradedOrders INNER JOIN  
                      dbo.T_CounterParty ON dbo.T_TradedOrders.CounterPartyID = dbo.T_CounterParty.CounterPartyID INNER JOIN  
                      dbo.T_Venue ON dbo.T_TradedOrders.VenueID = dbo.T_Venue.VenueID ON dbo.T_AUEC.AUECID = dbo.T_TradedOrders.AUECID INNER JOIN  
                      dbo.T_CompanyTradingAccounts ON dbo.T_TradedOrders.TradingAccountID = dbo.T_CompanyTradingAccounts.CompanyTradingAccountsID INNER JOIN  
                      dbo.T_Side ON dbo.T_TradedOrders.OrderSidetagValue = dbo.T_Side.SideTagValue INNER JOIN  
                      dbo.T_OrderType ON dbo.T_TradedOrders.OrderTypeTagValue = dbo.T_OrderType.OrderTypeTagValue  INNER JOIN
					  dbo.T_Sub ON dbo.T_TradedOrders.CLOrderID = dbo.T_Sub.ClOrderID
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "T_UnderLying"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 121
               Right = 200
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T_AUEC"
            Begin Extent = 
               Top = 6
               Left = 238
               Bottom = 121
               Right = 476
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T_Asset"
            Begin Extent = 
               Top = 126
               Left = 38
               Bottom = 226
               Right = 190
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T_Currency"
            Begin Extent = 
               Top = 126
               Left = 228
               Bottom = 226
               Right = 387
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T_Exchange"
            Begin Extent = 
               Top = 228
               Left = 38
               Bottom = 343
               Right = 238
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T_TradedOrders"
            Begin Extent = 
               Top = 348
               Left = 38
               Bottom = 463
               Right = 265
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T_CounterParty"
            Begin Extent = 
               Top = 468
               Left = 38
               Bottom = 583
               Right = 251
            End
         ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'V_TradedOrders';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'   DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T_Venue"
            Begin Extent = 
               Top = 228
               Left = 276
               Bottom = 343
               Right = 428
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T_CompanyTradingAccounts"
            Begin Extent = 
               Top = 588
               Left = 38
               Bottom = 703
               Right = 255
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T_Side"
            Begin Extent = 
               Top = 348
               Left = 303
               Bottom = 463
               Right = 455
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T_OrderType"
            Begin Extent = 
               Top = 468
               Left = 289
               Bottom = 568
               Right = 466
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'V_TradedOrders';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'V_TradedOrders';

