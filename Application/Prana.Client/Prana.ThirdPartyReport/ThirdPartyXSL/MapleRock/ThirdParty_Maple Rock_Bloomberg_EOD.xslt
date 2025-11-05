<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" version="1.0" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/NewDataSet">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<ThirdPartyFlatFileDetail>
					<!-- for system internal use -->
					<RowHeader>
						<xsl:value-of select="'true'"/>
					</RowHeader>
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>
					<xsl:variable name="PB_NAME" select="'BAML'"/>
					<xsl:variable name="PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>
					<xsl:variable name="THIRDPARTY_FUND_CODE">
						<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PRANA_FUND_NAME]/@PranaFund"/>
					</xsl:variable>
					<PortfolioName>
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
								<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</PortfolioName>
					<Security>
						<!--<xsl:choose>
							<xsl:when test="contains(BBCode,'Curncy')">
								<xsl:value-of select="BBCode"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="concat(substring-before(BBCode,' '),' ',substring-before(substring-after(BBCode,' '),' '))"/>
							</xsl:otherwise>
						</xsl:choose>-->
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="OSISymbol"/>
							</xsl:when>
							<xsl:when test="BBCode!='*'">
								<xsl:value-of select="BBCode"/>
							</xsl:when>
							<xsl:when test ="Asset = 'Equity' and CurrencySymbol = 'USD'">
								<xsl:value-of select ="concat(Symbol,' ','US',' ','EQUITY')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="BBCode"/>
							</xsl:otherwise>
						</xsl:choose>

					</Security>
					<Sedol>
						<xsl:value-of select="SEDOL"/>
					</Sedol>
					<Cusip>
						<xsl:value-of select="CUSIP"/>
					</Cusip>
					<ISIN>
						<xsl:value-of select="ISIN"/>
					</ISIN>
					<SecurityName>
						<xsl:value-of select="translate(FullSecurityName,',','')"/>
					</SecurityName>
					<Position>
						<xsl:value-of select="Qty"/>
					</Position>
					<Weight>
						<xsl:value-of select="''"/>
					</Weight>
					<MktPx>
						<xsl:value-of select="''"/>
					</MktPx>

					<CostPrice>
						<xsl:value-of select="AvgPrice"/>
					</CostPrice>
					<xsl:variable name="TradeDate" select="substring-before(TradeDate,'T')"/>

					<AsOfDate>
						<xsl:value-of select="concat(substring-before(substring-after($TradeDate,'-'),'-'),'/',substring-after(substring-after($TradeDate,'-'),'-'),'/',substring-before($TradeDate,'-'))"/>
					</AsOfDate>

					<NewClassification>
						<xsl:value-of select="Asset"/>
					</NewClassification>
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>
				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>