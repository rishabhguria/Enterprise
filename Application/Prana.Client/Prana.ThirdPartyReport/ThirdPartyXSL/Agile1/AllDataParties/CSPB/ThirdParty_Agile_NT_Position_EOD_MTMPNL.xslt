<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>


	<xsl:template name="MonthName">
		<xsl:param name="Month"/>
		<xsl:choose>
			<xsl:when test="$Month=1">
				<xsl:value-of select="'JAN'"/>
			</xsl:when>
			<xsl:when test="$Month=2">
				<xsl:value-of select="'FEB'"/>
			</xsl:when>
			<xsl:when test="$Month=3">
				<xsl:value-of select="'MAR'"/>
			</xsl:when>
			<xsl:when test="$Month=4">
				<xsl:value-of select="'APR'"/>
			</xsl:when>
			<xsl:when test="$Month=5">
				<xsl:value-of select="'MAY'"/>
			</xsl:when>
			<xsl:when test="$Month=6">
				<xsl:value-of select="'JUN'"/>
			</xsl:when>
			<xsl:when test="$Month=7">
				<xsl:value-of select="'JUL'"/>
			</xsl:when>
			<xsl:when test="$Month=8">
				<xsl:value-of select="'AUG'"/>
			</xsl:when>
			<xsl:when test="$Month=9">
				<xsl:value-of select="'SEP'"/>
			</xsl:when>
			<xsl:when test="$Month=10">
				<xsl:value-of select="'OCT'"/>
			</xsl:when>
			<xsl:when test="$Month=11">
				<xsl:value-of select="'NOV'"/>
			</xsl:when>
			<xsl:when test="$Month=12">
				<xsl:value-of select="'DEC'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>


	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="/NewDataSet">
	<!--<xsl:template match="/ThirdPartyFlatFileDetailCollection">-->

		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<ThirdPartyFlatFileDetail>

				<RowHeader>
					<xsl:value-of select ="'false'"/>
				</RowHeader>

				<TaxlotState>
					<xsl:value-of select="TaxLotState"/>
				</TaxlotState>

				<AccountNumber>
					<xsl:value-of select="'Account Number'"/>
				</AccountNumber>

				<AccountName>
					<xsl:value-of select="'Account Name'"/>
				</AccountName>

				<Country>
					<xsl:value-of select="'Country'"/>
				</Country>

				<TradingCurrency>
					<xsl:value-of select="'Trading Currency'"/>
				</TradingCurrency>

				<SecuritySEDOL>
					<xsl:value-of select="'Security SEDOL'"/>
				</SecuritySEDOL>

				<SecurityISIN>
					<xsl:value-of select="'Security ISIN'"/>
				</SecurityISIN>

				<SecurityCINS>
					<xsl:value-of select="'Security CINS'"/>
				</SecurityCINS>

				<StockTicker>
					<xsl:value-of select="'Stock Ticker'"/>
				</StockTicker>

				<SecurityDescriptionLong1>
					<xsl:value-of select="'Security Description (Long1)'"/>
				</SecurityDescriptionLong1>

				<SegmentDescription>
					<xsl:value-of select="'Segment Description'"/>
				</SegmentDescription>

				<SharesParFull>
					<xsl:value-of select="'Shares/Par (Full)'"/>
				</SharesParFull>


				<Price>
					<xsl:value-of select="'Price'"/>
				</Price>


				<TradedMarketValue>
					<xsl:value-of select="'Traded Market Value'"/>
				</TradedMarketValue>

				<PriceBase>
					<xsl:value-of select="'Price Base'"/>
				</PriceBase>


				<TradedMarketValueBase>
					<xsl:value-of select="'Traded Market Value (Base)'"/>
				</TradedMarketValueBase>

				<CostValue>
					<xsl:value-of select="'Cost Value'"/>
				</CostValue>

				<CostBase>
					<xsl:value-of select="'Cost Base'"/>
				</CostBase>

				<ExchangeRate>
					<xsl:value-of select="'Exchange Rate'"/>
				</ExchangeRate>

				<MaturityDate>
					<xsl:value-of select="'Maturity Date'"/>
				</MaturityDate>

				<PositionDate>
					<xsl:value-of select="'Position Date'"/>
				</PositionDate>

				<MTM>
					<xsl:value-of select="'MTM'"/>
				</MTM>

				<EntityID>
					<xsl:value-of select="EntityID"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>


			<xsl:for-each select="ThirdPartyFlatFileDetail[AccountName='UCIT-GS' or AccountName='UCIT-NT']">
				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'false'"/>
					</RowHeader>

					<TaxlotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxlotState>


					<xsl:variable name="PB_NAME" select="' '"/>

					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name ="THIRDPARTY_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/ThirdParty_FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>


					<xsl:variable name="varAccountName">
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
								<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>




					<xsl:variable name = "PRANA_GUICODE_NAME">
						<xsl:value-of select="ISINSymbol"/>
					</xsl:variable>

					<xsl:variable name="PRANA_GUICODE_MAPPING">
						<xsl:value-of select ="document('../ReconMappingXml/ThirdParty_GuiAccountMapping.xml')/GuiAccountMapping/PB[@Name='NT']/GuiAccountData[@GuiIsinCode=$PRANA_GUICODE_NAME]/@GuiAccountCode"/>
					</xsl:variable>


					<xsl:variable name="TradeAtt">
						<xsl:choose>
							<xsl:when test="string-length(TradeAttribute2)=1">
								<xsl:value-of select="concat('0',TradeAttribute2)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="TradeAttribute2"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					
					<xsl:variable name ="varAccount1">
						<xsl:choose>
							
							
							<xsl:when test ="Asset='FXForward'">
								<xsl:value-of  select="concat('1750261','-',$PRANA_GUICODE_MAPPING)"/>
							</xsl:when>
							<xsl:when test ="Asset='EquitySwap'">
								<xsl:value-of  select="'6007884'"/>
							</xsl:when>
							
							<xsl:otherwise>
								<xsl:value-of  select="'1750261'"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<AccountNumber>
						<xsl:value-of select="$varAccount1"/>
					</AccountNumber>

					<xsl:variable name ="varAccount">
						<xsl:choose>
							<xsl:when test ="Asset='FXForward'">
								<xsl:value-of  select="concat('SC','/',ISINSymbol)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of  select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					
					<AccountName>
						<xsl:value-of select="$varAccount"/>
					</AccountName>

					<Country>
						<xsl:value-of select="substring(CurrencySymbol,1,2)"/>
					</Country>

					<TradingCurrency>
						<xsl:value-of select="CurrencySymbol"/>
					</TradingCurrency>

					<SecuritySEDOL>
						<xsl:value-of select="SEDOL"/>
					</SecuritySEDOL>

					<SecurityISIN>
						<xsl:value-of select="ISINSymbol"/>
					</SecurityISIN>

					<SecurityCINS>
						<xsl:value-of select="''"/>
					</SecurityCINS>

					<StockTicker>
						<xsl:value-of select="Symbol"/>
					</StockTicker>

					<SecurityDescriptionLong1>
						<xsl:value-of select="translate(CompanyName,',','/')"/>
					</SecurityDescriptionLong1>

					<SegmentDescription>
						<xsl:value-of select="Asset"/>
					</SegmentDescription>

					<SharesParFull>
						<xsl:choose>
							<xsl:when test ="number(Quantity)">
								<xsl:value-of select="Quantity"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</SharesParFull>

					<Price>
						<xsl:choose>
							<xsl:when test ="number(MarkPrice)">
								<xsl:value-of select="format-number(MarkPrice,'##.####')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</Price>

					<TradedMarketValue>
						<xsl:choose>
							<xsl:when test ="number(MarketValue)">
								<xsl:value-of select="format-number(MarketValue,'##.##')"/>	
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
						<!--<xsl:choose>
							<xsl:when test ="contains(Symbol,'CFD')">
								<xsl:value-of select="format-number(MarketValue - NetNotionalValue,'##.##') "/>
							</xsl:when>
							<xsl:when test ="number(MarketValue)">
								<xsl:value-of select="format-number(MarketValue,'##.##')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>-->
						
					</TradedMarketValue>
					
					
					<xsl:variable name="VarFxPrice">
						<xsl:choose>
							<xsl:when test="CurrencySymbol='GBP' or CurrencySymbol='AUD' or CurrencySymbol='EUR'">
								<xsl:value-of select="ForexRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="1 div ForexRate"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					
					<PriceBase>
						<xsl:choose>
							<xsl:when test ="number(MarkPriceBase)">
								<xsl:value-of select="format-number(MarkPriceBase,'##.####')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</PriceBase>					
					
					<TradedMarketValueBase>
						<xsl:choose>
							<xsl:when test ="number(MarketValueBase)">
								<xsl:value-of select="format-number(MarketValueBase,'##.##')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
						<!--<xsl:choose>
							<xsl:when test ="contains(Symbol,'CFD')">
								<xsl:value-of select="format-number(((MarketValue - NetNotionalValue) * EndingFXRate),'##.##') "/>
							</xsl:when>
							<xsl:when test ="number(MarketValue)">
								<xsl:value-of select="format-number(MarketValueBase,'##.##')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>-->
					</TradedMarketValueBase>

					<CostValue>
						<xsl:value-of select="format-number(NetNotionalValue,'##.##')"/>
					</CostValue>

					<CostBase>
						<xsl:value-of select="format-number(NetNotionalValueBase,'##.##')"/>
					</CostBase>

					<ExchangeRate>
						<!--<xsl:choose>
							<xsl:when test="number(FXRate_Taxlot)">
								<xsl:value-of select="FXRate_Taxlot"/>
							</xsl:when>
							<xsl:when test="number(ForexRate)">
								<xsl:value-of select="ForexRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'1'"/>
							</xsl:otherwise>
						</xsl:choose>-->
						<xsl:value-of select="format-number(FXRate,'##.########')"/>
					</ExchangeRate>
					
					<xsl:variable name ="varExpirationDate">
						<xsl:if test ="Asset = 'EquityOption' or Asset = 'FutureOption' or Asset = 'Future' or Asset = 'FixedIncome'">
							<xsl:value-of select ="ExpirationDate"/>
						</xsl:if>
					</xsl:variable>

					<xsl:variable name ="varMonth">
						<xsl:value-of select="substring-before(substring-after($varExpirationDate,'-'),'-')"/>
					</xsl:variable>
					<xsl:variable name ="varDay">
						<xsl:value-of select="substring-before(substring-after(substring-after($varExpirationDate,'-'),'-'),'T')"/>
					</xsl:variable>
					<xsl:variable name ="varYear">
						<xsl:value-of select="substring-before($varExpirationDate,'-')"/>
					</xsl:variable>
					
					<MaturityDate>
						<xsl:choose>
							<xsl:when test="Asset = 'EquityOption' or Asset = 'FutureOption' or Asset = 'Future' or Asset = 'FixedIncome'">
								<xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>

					</MaturityDate>

					<PositionDate>
						<xsl:value-of select="TradeDate"/>
					</PositionDate>

					<MTM>
						<xsl:choose>
							<xsl:when test ="number(MTMUnrealizedPNL)">
								<xsl:value-of select="format-number(MTMUnrealizedPNL,'##.##')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</MTM>

					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>

			</xsl:for-each>

		</ThirdPartyFlatFileDetailCollection>

	</xsl:template>

</xsl:stylesheet>