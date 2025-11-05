EXEC sp_msforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT ALL";
delete from T_AL_MatchPortfolioPosition

	INSERT INTO T_AL_MatchPortfolioPosition(Id, MatchPortfolioPosition, Description) VALUES(0,'None','Closing Positions will not be matched');
	INSERT INTO T_AL_MatchPortfolioPosition(Id, MatchPortfolioPosition, Description) VALUES(1,'CompletePortfolio','Closing Positions will be matched for entire portfolio');
	INSERT INTO T_AL_MatchPortfolioPosition(Id, MatchPortfolioPosition, Description) VALUES(2,'SelectedAccounts','Closing Positions will be matched for the selected accounts/masterfunds only');

EXEC sp_msforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL";