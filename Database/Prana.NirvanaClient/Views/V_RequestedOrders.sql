  
CREATE VIEW [dbo].[V_RequestedOrders] AS  
SELECT     O.ParentClOrderID, LastSub.CLOrderID, O.Symbol, O.OrderSidetagValue, dbo.T_Side.Side AS OrderSideName, O.TradingAccountID,   
                      CTA.TradingShortName AS TradingAccountName, S.UserID, S.OrderTypeTagValue, dbo.T_OrderType.OrderTypes AS OrderTypeName, S.Quantity,   
                      S.Price, S.ExecutionInst, S.TimeInForce, S.HandlingInst, S.CounterPartyID, dbo.T_CounterParty.ShortName AS CounterPartyName, S.VenueID,   
                      dbo.T_Venue.VenueName, S.FundID, S.StrategyID, S.OpenClose, O.ListID, O.WaveID, dbo.T_Asset.AssetID, dbo.T_Asset.AssetName,   
                      dbo.T_UnderLying.UnderLyingID, dbo.T_UnderLying.UnderLyingName, dbo.T_Exchange.ExchangeID,   
                      dbo.T_Exchange.DisplayName AS ExchangeName, dbo.T_Currency.CurrencyName, O.AUECID, S.InsertionTime, S.MsgType, S.NirvanaMsgType,   
                      S.ClientOrderID, S.ParentClientOrderID, S.NirvanaSeqNumber, S.OrderID, S.OrigClOrderID, S.AlgoStrategyID,   
                      S.AlgoParameters, S.AUECLocalDate, S.SettlementDate, S.Text, O.CurrencyID  
FROM         dbo.T_Sub AS S INNER JOIN  
                          (SELECT DISTINCT O.ParentClOrderID, MAX(S.ClOrderID) AS CLOrderID  
                            FROM          dbo.T_Order AS O INNER JOIN  
                                                   dbo.T_Sub AS S ON O.ParentClOrderID = S.ParentClOrderID  
                            GROUP BY O.ParentClOrderID) AS LastSub ON S.ClOrderID = LastSub.CLOrderID INNER JOIN  
                      dbo.T_Order AS O ON O.ParentClOrderID = LastSub.ParentClOrderID INNER JOIN  
                      dbo.T_AUEC ON dbo.T_AUEC.AUECID = O.AUECID INNER JOIN  
                      dbo.T_Asset ON dbo.T_Asset.AssetID = dbo.T_AUEC.AssetID INNER JOIN  
                      dbo.T_UnderLying ON dbo.T_UnderLying.UnderLyingID = dbo.T_AUEC.UnderLyingID INNER JOIN  
                      dbo.T_Exchange ON dbo.T_Exchange.ExchangeID = dbo.T_AUEC.ExchangeID INNER JOIN  
                      dbo.T_Currency ON O.CurrencyID = dbo.T_Currency.CurrencyID INNER JOIN  
                      dbo.T_Side ON dbo.T_Side.SideTagValue = O.OrderSidetagValue INNER JOIN  
                      dbo.T_CompanyTradingAccounts AS CTA ON O.TradingAccountID = CTA.CompanyTradingAccountsID INNER JOIN  
                      dbo.T_Venue ON S.VenueID = dbo.T_Venue.VenueID INNER JOIN  
                      dbo.T_CounterParty ON S.CounterPartyID = dbo.T_CounterParty.CounterPartyID INNER JOIN  
                      dbo.T_OrderType ON S.OrderTypeTagValue = dbo.T_OrderType.OrderTypeTagValue  
WHERE     (S.NirvanaMsgType <> 3)  
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[71] 4[4] 2[23] 3) )"
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
         Begin Table = "S"
            Begin Extent = 
               Top = 74
               Left = 291
               Bottom = 401
               Right = 469
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "LastSub"
            Begin Extent = 
               Top = 30
               Left = 343
               Bottom = 108
               Right = 503
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "O"
            Begin Extent = 
               Top = 30
               Left = 19
               Bottom = 138
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T_AUEC"
            Begin Extent = 
               Top = 32
               Left = 541
               Bottom = 140
               Right = 778
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T_Asset"
            Begin Extent = 
               Top = 166
               Left = 610
               Bottom = 259
               Right = 761
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T_UnderLying"
            Begin Extent = 
               Top = 247
               Left = 607
               Bottom = 355
               Right = 768
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T_Exchange"
            Begin Extent = 
               Top = 142
               Left = 612
               Bottom = 250
               Right = 811
            End
            DisplayFlags = 280
   ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'V_RequestedOrders';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'         TopColumn = 0
         End
         Begin Table = "T_Currency"
            Begin Extent = 
               Top = 347
               Left = 635
               Bottom = 440
               Right = 793
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T_Side"
            Begin Extent = 
               Top = 413
               Left = 15
               Bottom = 506
               Right = 166
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CTA"
            Begin Extent = 
               Top = 492
               Left = 227
               Bottom = 600
               Right = 443
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T_Venue"
            Begin Extent = 
               Top = 504
               Left = 38
               Bottom = 612
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T_CounterParty"
            Begin Extent = 
               Top = 600
               Left = 227
               Bottom = 708
               Right = 439
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T_OrderType"
            Begin Extent = 
               Top = 612
               Left = 38
               Bottom = 705
               Right = 214
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'V_RequestedOrders';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'V_RequestedOrders';

