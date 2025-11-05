Insert INTO T_WatchList_TabNames (TabName, IsPermanent, UserID)
SELECT 'Watchlist 1', 1, CU.UserID FROM T_CompanyUser CU
WHERE NOT EXISTS (SELECT WT.TabId FROM T_WatchList_TabNames WT WHERE WT.UserID = CU.UserID AND WT.IsPermanent = 1)
